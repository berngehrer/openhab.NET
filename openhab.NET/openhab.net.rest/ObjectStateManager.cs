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
        IDataSource<T> _elementSource;
        List<T> _elementContainer = new List<T>();
        
        OpenhabContext<T> _context;
        ClientBackgroundWorker _worker;
        
        
        public ObjectStateManager(OpenhabContext<T> context) 
        {
            _context = context;
            _context.Observer.Subscribe(this);

            _elementSource = context.CreateDataSource();

            _worker = context.ClientFactory.CreateWorker();
            _worker?.Start(ElementSyncWorker);
        }

        public bool HasBackgroundWorker => _worker != null;
        

        void ElementSyncWorker(OpenhabClient client)
        {
            using (var source = _context.CreateDataSource(client))
            {
                var elements = source.GetAll().WaitSynchronously(_worker.Token);
                ProcessAll(elements, _worker.Token);
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


        IEnumerable<T> ProcessAll(IEnumerable<T> collection, CancellationToken token)
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

        T ProcessElement(T element)
        {
            // TODO: Add Semaphore
            var storeElement = _elementContainer.GetOrAdd(element, x => x.Name == element?.Name);
            if (!storeElement.IsEqual(element))
            {
                _context.Observer.Notify(this, element);
            }
            return storeElement;
        }

        public void Dispose()
        {
            _elementContainer.Clear();
            _elementSource.Dispose();
            _worker?.Cancel(false);
        }

        public async void OnNotify(IOpenhabElement element)
        {
            await _elementSource.UpdateState((T)element);
        }
    }
}
