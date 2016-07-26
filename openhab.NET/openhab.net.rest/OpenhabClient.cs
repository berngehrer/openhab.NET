using Newtonsoft.Json;
using openhab.net.rest.Core;
using openhab.net.rest.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal class OpenhabClient : IDisposable
    {
        HttpClientProxy _proxy;
        SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public OpenhabClient(ClientSettings settings, bool pooling = false)
        {
            Settings = settings;

            var poolingClass = PoolingSession.False;
            if (pooling) {
                poolingClass = new PoolingSession(IdProvider.GetNext());
            }
            _proxy = new HttpClientProxy(settings, poolingClass);
        }

        public ClientSettings Settings { get; }


        public async Task<bool> SendCommand(MessageHandler message)
        {
            return await _proxy.SendMessage(message);
            //return SendWithSemaphore(
            //    () => _proxy.SendMessage(message),
            //    message.GetToken()
            //);
        }

        public async Task<T> SendRequest<T>(MessageHandler message) where T : class, new()
        {
            var json = await _proxy.ReadAsString(message);
            return JsonConvert.DeserializeObject<T>(json);

            //return SendWithSemaphore(async () =>
            //{
            //    var json = await _proxy.ReadAsString(message);
            //    return JsonConvert.DeserializeObject<T>(json);
            //},
            //message.GetToken());
        }

        async Task<T> SendWithSemaphore<T>(Func<Task<T>> task, CancellationToken token)
        {
            await _semaphore.WaitAsync(token);
            return await task().ContinueWith(t =>
            {
                _semaphore.Release();
                return t.GetAwaiter().GetResult();
            },
            TaskContinuationOptions.ExecuteSynchronously);
        }

        public void Dispose()
        {
            _proxy.Dispose();
            _semaphore.Dispose();
        }
    }
}
