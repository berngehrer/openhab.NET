using Newtonsoft.Json;
using openhab.net.rest.Core;
using openhab.net.rest.Http;
using System;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal class OpenhabClient : IDisposable
    {
        HttpClientProxy _proxy;
        
        internal OpenhabClient(ClientSettings settings, bool pooling = false)
        {
            Settings = settings;

            var poolingClass = PoolingSession.False;
            if (pooling) {
                poolingClass = new PoolingSession(IdProvider.GetNext());
            }
            _proxy = new HttpClientProxy(settings, poolingClass);
        }

        public ClientSettings Settings { get; }
        
        internal Task<bool> SendCommand()
        {
            // todo
            return Task.FromResult(true);
        }

        internal async Task<T> SendRequest<T>(MessageHandler message) where T : class, new()
        {
            var json = await _proxy.ReadAsString(message);
            return JsonConvert.DeserializeObject<T>(json);
        }


        public void Dispose()
        {
            _proxy.Dispose();
        }
    }
}
