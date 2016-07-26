using openhab.net.rest.Core;
using openhab.net.rest.DataSource;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public abstract class OpenhabContext<T> : IElementObservable, IDisposable where T : IOpenhabElement
    {
        ObjectStateManager<T> _objectManager;

        public event ContextRefreshedHandler<T> Refreshed;
        
        protected OpenhabContext(ClientSettings settings, UpdateStrategy strategy)
        {
            Observer.Subscribe(this);

            ClientFactory = new ContextClientFactory(settings, strategy);
            _objectManager = new ObjectStateManager<T>(this);
        }


        internal ContextClientFactory ClientFactory { get; }
        internal ObjectStateManager<T> ObjectManager => _objectManager;
        internal IElementObserver Observer { get; } = new ElementObserver();        
        public bool IsSyncronized => ObjectManager.HasBackgroundWorker;


        public async Task<IEnumerable<T>> GetAll(CancellationToken? token = null)
        {
            return await ObjectManager.GetAll(token ?? CancellationToken.None);
        }

        public async Task<T> GetByName(string name, CancellationToken? token = null)
        {
            return await ObjectManager.GetByName(name, token ?? CancellationToken.None);
        }
                

        public void OnNotify(IOpenhabElement element)
        {
            if (element?.GetType().Is<T>() ?? false)
            {
                var args = new ContextRefreshedEventArgs<T>((T)element);
                Refreshed?.Invoke(this, args);
            }
        }

        public void Dispose()
        {
            _objectManager.Dispose();
        }

        internal abstract IDataSource<T> CreateDataSource(OpenhabClient client = null);
    }
}
