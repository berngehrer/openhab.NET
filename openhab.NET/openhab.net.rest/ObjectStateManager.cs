using openhab.net.rest.Core;
using openhab.net.rest.Http;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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

        ClientBackgroundWorker CreateWorker()
        {
            var clientFactory = new ContextClientFactory<T>(_context);
            var workerClient = clientFactory.Create();
            if (workerClient != null) {
                return new ClientBackgroundWorker(workerClient, _context.Strategy.Interval.Milliseconds);
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
            var message = new MessageHandler
            {
                CancelToken = _worker.Token,
                Collection = _context.Collection
            };

            _context.GetAll(client, message)
                    .WaitSave(_worker.Token)
                   ?.ForEach(element =>
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
            if (!element.HasChanged) {
                return;
            }

            var message = new MessageHandler
            {
                Method = HttpMethod.Put,
                MimeType = MIMEType.PlainText,
                Content = element.ToString(),
                Collection = _context.Collection,
                RelativePath = $"{element.Name}/state"
            };
             
            var sendTask = _context.Connection.SendStatus(message);
            await sendTask;
            if (sendTask.IsSuccess() && sendTask.Result == true)
            {
                element.Reset();
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
