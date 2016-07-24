using openhab.net.rest.Core;
using openhab.net.rest.DataSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal class ObjectStateManager<T> : ObservableCollection<T>, IDisposable where T : IOpenhabElement
    {
        OpenhabContext<T> _context;
        ClientBackgroundWorker _worker;
        
        public ObjectStateManager(OpenhabContext<T> context) 
        {
            ElementSource = context.CreateDataSource();

            _context = context;
            _worker = CreateWorker();
            _worker?.Start(UpdateWorkerMethod);
        }
        
        ClientBackgroundWorker CreateWorker()
        {
            var workerClient = _context.ClientFactory.Create();
            if (workerClient != null) {
                var delay = _context.ClientFactory.Strategy.Interval.Milliseconds;
                return new ClientBackgroundWorker(workerClient, delay);
            }
            return null;
        }

        public IDataSource<T> ElementSource { get; }

        public bool HasBackgroundWorker => _worker != null;


        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.OldItems.OfType<INotifyPropertyChanged>()) {
                item.PropertyChanged -= ValueChanged;
            }
            foreach (var item in e.NewItems.OfType<INotifyPropertyChanged>()) {
                item.PropertyChanged += ValueChanged;
            }
        }

        async void ValueChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender.GetType().Is<T>()) {
                await RaiseStatus((T)sender);
            }
        }

        void UpdateWorkerMethod(OpenhabClient client)
        {
            using (var source = _context.CreateDataSource(client))
            {
                var elements = source.GetAll().WaitSave(_worker.Token);
                elements?.ForEach(element =>
                {
                    if (!_worker.IsCancellationRequested) {
                        ProcessElement(element).WaitSave(_worker.Token);
                    }
                    else return;
                });
            }
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken token)
        {
            var elements = await ElementSource.GetAll(token);
            elements?.ForEach(element => 
            {
                if (!token.IsCancellationRequested) {
                    ProcessElement(element).WaitSave(token);
                }
                else return;
            });
            return elements;
        }

        public async Task<T> GetByName(string name, CancellationToken token)
        {
            var element = await ElementSource.GetByName(name, token);
            await ProcessElement(element);
            return element;
        }        

        // TODO !!!
        async Task<bool> ProcessElement(T element)
        {
            var existing = this.FirstOrDefault(x => x.Name.Equals(element.Name));
            if (existing == null)
            {
                Add(element);
                return true;
            }
            return false;
        }

        async Task RaiseStatus(T element)
        {
            bool success = await ElementSource.UpdateState(element);
            if (success) {
                _context.FireRefreshed(element);
            }
        }

        public void Dispose()
        {  
            _worker?.Cancel(false);
            Clear();
        }
    }
}
