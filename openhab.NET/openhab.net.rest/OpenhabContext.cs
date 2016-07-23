using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public abstract class OpenhabContext<T> : IDisposable where T : IOpenhabElement
    {
        OpenhabClient _connection;
        ObjectStateManager<T> _objectManager;

        public event ContextRefreshedHandler<T> Refreshed;
        

        protected OpenhabContext(ClientSettings settings, UpdateStrategy strategy)
        {
            Strategy = strategy;

            _connection = new OpenhabClient(settings);
            _objectManager = new ObjectStateManager<T>(this);
        }
        
        internal OpenhabClient Connection => _connection;
        //internal ObjectStateManager<T> ObjectManager => _objectManager;
        internal UpdateStrategy Strategy { get; private set; }
        internal CancellationTokenSource CancelSource { get; set; }
        internal abstract Http.SiteCollection Collection { get; }


        internal void FireRefreshed(IOpenhabElement element)
        {
            if (element?.GetType().Is<T>() ?? false) {
                Refreshed?.Invoke(this, new ContextRefreshedEventArgs<T>((T)element));
            }
        }

        public void Cancel()
        {
            if (!CancelSource?.IsCancellationRequested ?? false)
            {
                CancelSource.Cancel(false);
            }
        }

        public void Dispose()
        {
            CancelSource?.Cancel(false);
            _objectManager.Dispose();
            _connection.Dispose();
        }

        // TODO: bessere Lösung
        internal abstract Task<IEnumerable<T>> GetAll(OpenhabClient client, Http.MessageHandler message);
        public abstract Task<IEnumerable<T>> GetAll();
        public abstract Task<T> GetByName(string name);
    }
}
