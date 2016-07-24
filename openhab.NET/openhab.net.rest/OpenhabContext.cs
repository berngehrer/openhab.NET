using openhab.net.rest.DataSource;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public abstract class OpenhabContext<T> : IDisposable where T : IOpenhabElement
    {
        ObjectStateManager<T> _objectManager;
        CancellationTokenSource _cancelSource;

        public event ContextRefreshedHandler<T> Refreshed;


        protected OpenhabContext(ClientSettings settings, UpdateStrategy strategy)
        {
            ClientFactory = new ContextClientFactory(settings, strategy);
            _objectManager = new ObjectStateManager<T>(this);
        }


        internal ContextClientFactory ClientFactory { get; }

        internal ObjectStateManager<T> ObjectManager => _objectManager;
        
        public bool IsSyncronized => ObjectManager.HasBackgroundWorker;


        public async Task<IEnumerable<T>> GetAll()
        {
            _cancelSource = new CancellationTokenSource();
            return await ObjectManager.GetAll(_cancelSource.Token);
        }

        public async Task<T> GetByName(string name)
        {
            _cancelSource = new CancellationTokenSource();
            return await ObjectManager.GetByName(name, _cancelSource.Token);
        }

        internal void FireRefreshed(IOpenhabElement element)
        {
            if (element?.GetType().Is<T>() ?? false) {
                Refreshed?.Invoke(this, new ContextRefreshedEventArgs<T>((T)element));
            }
        }
        
        public void Cancel()
        {
            if (!_cancelSource?.IsCancellationRequested ?? false)
            {
                _cancelSource.Cancel(false);
            }
        }

        public void Dispose()
        {
            _cancelSource?.Cancel(false);
            _objectManager.Dispose();
        }

        internal abstract IDataSource<T> CreateDataSource(OpenhabClient client = null);
    }
}
