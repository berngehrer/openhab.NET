using openhab.net.rest.Core;
using openhab.net.rest.Http;
using openhab.net.rest.Json;
using System;
using System.Linq;
using System.Collections.Generic;

namespace openhab.net.rest
{
    internal class ObjectStateManager<T> : List<T>, IDisposable where T : IOpenhabElement
    {
        SiteCollection _collection;
        ClientBackgroundWorker _worker;

        public ObjectStateManager(SiteCollection collection, ClientBackgroundWorker worker = null)
        {
            _collection = collection;
            (_worker = worker)?.Start(UpdateWorkerMethod);
        }
         
        public bool HasBackgroundWorker => _worker != null;


        void UpdateWorkerMethod(OpenhabClient client)
        {
            var message = new MessageHandler { CancelToken = _worker.Token, Collection = _collection };
            var request = client.SendRequest<ItemRootObject>(message);

            request.Wait(_worker.Token);
            if (request.IsCompleted && !request.IsFaulted)
            {
                request.Result.Items.ForEach(e =>
                {
                    if (_worker.IsCancellationRequested) {
                        return;
                    }
                    //var item = ToComparableItem(e);
                    //if (HasChanged(item))
                    //{
                    //    FireRefreshed(item);
                    //}
                });
            }
        }
        
        bool HasChanged(T element)
        {
            // TODO!!!
            var existing = this.FirstOrDefault(x => x.Name.Equals(element.Name, StringComparison.CurrentCultureIgnoreCase));
            if (existing == null)
            {
                Add(element);
                return true;
            }
            else if (existing.Value == element.Value)
            {
                bool hasChanged = existing.Value != element.Value;
                //if (hasChanged)
                //{
                //    existing.Value = element.Value;
                //    return true;
                //}
            }
            return false;
        }

        public void Dispose()
        {
            _worker?.Cancel(false);
            Clear();
        }
    }
}
