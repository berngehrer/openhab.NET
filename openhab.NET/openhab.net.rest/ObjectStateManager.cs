using openhab.net.rest.Core;
using openhab.net.rest.DataSource;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal class ObjectStateManager<T> : IDisposable where T : IOpenhabElement
    {
        IDataSource<T> _source;
        OpenhabContext<T> _context;
        ClientBackgroundWorker _worker;
        
        public ObjectStateManager(OpenhabContext<T> context) 
        {
            _context = context;
            _source = context.CreateDataSource();
            _worker = context.ClientFactory.CreateWorker();
            _worker?.Start(ElementSyncWorker);

            Elements = new NotifyCollection<T>(async (element) => await RaiseUpdate(element));
        }

        NotifyCollection<T> Elements { get; }
        IDataSource<T> ElementSource => _source;
        public bool HasBackgroundWorker => _worker != null;
        

        void ElementSyncWorker(OpenhabClient client)
        {
            using (var source = _context.CreateDataSource(client))
            {
                var elements = source.GetAll().WaitSave(_worker.Token);
                ProcessAll(elements, _worker.Token);
            }
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken token)
        {
            var elements = await ElementSource.GetAll(token);
            return ProcessAll(elements, token, false);
        }

        public async Task<T> GetByName(string name, CancellationToken token)
        {
            var element = await ElementSource.GetByName(name, token);
            return ProcessElement(element, false);
        }


        IEnumerable<T> ProcessAll(IEnumerable<T> collection, CancellationToken token, bool notify = true)
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

        T ProcessElement(T element, bool notify = true)
        {
            // TODO: Add Semaphore
            var storeElement = Elements.AddOrGet(element);
            if (storeElement.ShadowUpdate(element) && notify)
            {
                _context.FireRefreshed(element);
            }
            return storeElement;
        }

        async Task RaiseUpdate(T element)
        {
            bool success = await ElementSource.UpdateState(element);
            if (success) {
                _context.FireRefreshed(element);
            }
        }

        public void Dispose()
        {
            Elements.Clear();
            _source.Dispose();
            _worker?.Cancel(false);
        }
    }
}
