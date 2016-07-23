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


        public Task<bool> SendCommand(MessageHandler message)
        {
            // TODO
            return SendWithSemaphore(
                () => Task.FromResult(true), 
                message.GetToken()
            );
        }

        public Task<bool> SendStatus(MessageHandler message)
        {
            return SendWithSemaphore(
                () => _proxy.SendMessage(message),
                message.GetToken()
            );
        }

        public Task<T> SendRequest<T>(MessageHandler message) where T : class, new()
        {
            return SendWithSemaphore(async () =>
            {
                var json = await _proxy.ReadAsString(message);
                return JsonConvert.DeserializeObject<T>(json);
            },
            message.GetToken());
        }

        async Task<T> SendWithSemaphore<T>(Func<Task<T>> task, CancellationToken token)
        {
            await _semaphore.WaitAsync(token);
            return await task().ContinueWith(t =>
            {
                _semaphore.Release();
                return t.GetResultSave();
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
