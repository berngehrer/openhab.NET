using openhab.net.rest.Core;
using openhab.net.rest.Http;
using System;
using System.Threading;

namespace openhab.net.rest
{
    public abstract class OpenhabClient<T> : IDisposable where T : IOpenhabElement
    {
        protected OpenhabClient(ClientSettings settings, bool pooling = false)
        {
            Settings = settings;

            var poolingClass = PoolingSession.False;
            if (pooling) {
                poolingClass = new PoolingSession(IdProvider.GetNext());
            }
            _proxy = new HttpClientProxy(settings, poolingClass);
        }

        public ClientSettings Settings { get; }

        internal CancellationToken? CancelToken { get; }

        HttpClientProxy _proxy;
        internal HttpClientProxy RestProxy => _proxy;
        

        public void Dispose()
        {
            _proxy.Dispose();
        }
    }
}
