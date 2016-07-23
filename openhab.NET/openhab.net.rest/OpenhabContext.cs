using openhab.net.rest.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public abstract class OpenhabContext<T> : IDisposable where T : IOpenhabElement
    {
        // TODO
        ObjectStateManager<T> _objectManager;
        
        public event ContextRefreshedHandler<T> Refreshed;
        

        protected OpenhabContext(ClientSettings settings, UpdateStrategy strategy)
        {
            _connection = new OpenhabClient(settings);
            _objectManager = CreateStateManager(strategy ?? UpdateStrategy.Default);
        }

        ObjectStateManager<T> CreateStateManager(UpdateStrategy strategy)
        {
            OpenhabClient client;
            ClientBackgroundWorker worker = null;

            // Timed update
            if (strategy.Interval != TimeSpan.Zero) {
                client = new OpenhabClient(_connection.Settings);
            }
            // Permanent update by server push
            else if (strategy.Realtime) {
                client = new OpenhabClient(_connection.Settings, pooling: true);
            }
            // No background update
            else client = null;

            if (client != null) {
                worker = new ClientBackgroundWorker(client, strategy.Interval.Milliseconds);
            }
            return new ObjectStateManager<T>(Collection, worker);
        }

        OpenhabClient _connection;
        internal OpenhabClient Connection => _connection;
        internal CancellationTokenSource CancelSource { get; set; }
        internal abstract Http.SiteCollection Collection { get; }

        public abstract Task<IEnumerable<T>> GetAll();
        public abstract Task<T> GetByName(string name);
        
        public Task<bool> Commit()
        {
            // TODO
            return Connection.SendCommand();
        }
                
        // Replace?
        protected void FireRefreshed(T element)
        {
            Refreshed?.Invoke(this, new ContextRefreshedEventArgs<T>(element));
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
    }
}
