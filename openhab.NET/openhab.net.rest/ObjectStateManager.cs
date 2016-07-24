using openhab.net.rest.Core;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal class ObjectStateManager<T> : ObservableCollection<T>, IDisposable where T : IOpenhabElement
    {
        OpenhabContext<T> _context;
        ClientBackgroundWorker _worker;
        
        public ObjectStateManager(OpenhabContext<T> context) 
        {
            _context = context;
            _worker = CreateWorker();
            _worker?.Start(UpdateWorkerMethod);
        }

        // TODO
        public DataSource.IDataSource<T> ElementSource { get; }

        ClientBackgroundWorker CreateWorker()
        {
            var workerClient = _context.ClientFactory.Create();
            if (workerClient != null) {
                var delay = _context.ClientFactory.Strategy.Interval.Milliseconds;
                return new ClientBackgroundWorker(workerClient, delay);
            }
            return null;
        }

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
                await SendStatus((T)sender);
            }
        }

        void UpdateWorkerMethod(OpenhabClient client)
        {
            // TODO !
            var elements = ElementSource.GetAll().WaitSave(_worker.Token);
            elements?.ForEach(element =>
            {
                if (!_worker.IsCancellationRequested)
                {
                    // TODO: Abgleich mit interner Collection
                    SendStatus(element).WaitSave(_worker.Token);
                }
                else return;
            });
        }

        async Task SendStatus(T element)
        {
            //if (!element.HasChanged) {
            //    return;
            //}

            // TODO: DataSourceFactory?
            using (var client = _context.ClientFactory.Create(withStrategy: false))
            using (var source = new DataSource.ItemSource(client))
            {
                bool success = await source.UpdateState(element);
                if (success)
                {
                    _context.FireRefreshed(element);
                }
            }
        }

        public void Dispose()
        {
            _worker?.Cancel(false);
            Clear();
        }
    }
}
