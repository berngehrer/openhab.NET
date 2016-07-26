using openhab.net.rest.Core;
using openhab.net.rest.DataSource;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal class ObjectStateManager<T> : IElementObservable, IDisposable where T : IOpenhabElement
    {
        object _lockObj = new object();

        BackgroundClient _worker;
        OpenhabContext<T> _context;

        IDataSource<T> _elementSource;
        List<T> _elementContainer = new List<T>();
        

        public ObjectStateManager(OpenhabContext<T> context) 
        {
            _context = context;
            _context.Observer.Subscribe(this);

            _elementSource = context.CreateDataSource();

            _worker = context.ClientFactory.CreateWorker();
            _worker?.Start(ElementSyncWorker);
        }

        public bool HasBackgroundWorker => _worker != null;


        public async void OnNotify(IOpenhabElement element)
        {
            if (element.GetType().Is<T>()) {
                await _elementSource.UpdateState((T)element);
            }
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken token)
        {
            var elements = await _elementSource.GetAll(token);
            return ProcessAll(elements, token);
        }

        public async Task<T> GetByName(string name, CancellationToken token)
        {
            var element = await _elementSource.GetByName(name, token);
            return ProcessElement(element);
        }

        void ElementSyncWorker(OpenhabClient client)
        {
            using (var source = _context.CreateDataSource(client))
            {
                var task = _elementSource.GetAll(_worker.Token);
                task.RunSynchronously();
                ProcessAll(task.Result, _worker.Token);
            }
        }

        IEnumerable<T> ProcessAll(IEnumerable<T> collection, CancellationToken token)
        {
            lock (_lockObj)
            {
                if (collection != null) {
                    foreach (var item in collection)
                    {
                        if (!token.IsCancellationRequested) {
                            yield return ProcessElement(item);
                        }
                        else break;
                    }
                }
            }
        }

        T ProcessElement(T element)
        {
            lock (_lockObj)
            {
                var storeElement = _elementContainer.GetOrAdd(element, x => x.Name == element?.Name);
                if (!storeElement.IsEqual(element)) {
                    _context.Observer.Notify(this, element);
                }
                return storeElement;
            }
        }

        public void Dispose()
        {
            _elementContainer.Clear();
            _elementSource.Dispose();
            _worker?.Cancel(false);
        }
    }
}
