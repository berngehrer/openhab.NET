using openhab.net.rest.Core;
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
            _context.ItemObserver.Subscribe(this);

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

        async void ElementSyncWorker(OpenhabClient client)
        {
            using (var source = _context.CreateDataSource(client))
            {
                var result = await _elementSource.GetAll(_worker.Token);
                ProcessAll(result, _worker.Token);
            }
        }

        IEnumerable<T> ProcessAll(IEnumerable<T> collection, CancellationToken token)
        {
            lock (_lockObj)
            {
                var returnList = new List<T>();
                if (collection != null) {
                    foreach (var item in collection)
                    {
                        if (!token.IsCancellationRequested) {
                            returnList.Add(ProcessElement(item));
                        }
                        else break;
                    }
                }
                return returnList;
            }
        }

        T ProcessElement(T element)
        {
            lock (_lockObj)
            {
                var storeElement = _elementContainer.GetOrAdd(element, x => x.Name == element?.Name);
                if (!storeElement.IsEqual(element)) {
                    _context.ItemObserver.Notify(this, element);
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
