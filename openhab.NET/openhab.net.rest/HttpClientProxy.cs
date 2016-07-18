using openhab.net.rest.Channels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal sealed class HttpClientProxy : IDisposable
    {
        HttpClient _innerClient;
        
        public HttpClientProxy(ClientSettings settings)
            : this(settings, PoolingSession.False)
        {
        }

        public HttpClientProxy(ClientSettings settings, PoolingSession pooling)
        {
            Pooling = pooling;
            BaseAddress = new Uri(settings.BaseAddress);

            _innerClient = CreateClient(settings);
        }

        
        public Uri BaseAddress { get; }
        public PoolingSession Pooling { get; }
        

        public async Task<string> ReadAsString(MessageHandler message)
        {
            var request = CreateRequest(message);

            Task<HttpResponseMessage> responseTask;
            if (message.CancelToken.HasValue) {
                responseTask = _innerClient.SendAsync(request, message.CancelToken.Value);
            } else {
                responseTask = _innerClient.SendAsync(request);
            }

            using (var response = await responseTask)
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }


        HttpClient CreateClient(ClientSettings settings)
        {
            var httpHandler = new HttpClientHandler
            {
                Proxy = settings.Proxy,
                AllowAutoRedirect = false,
                Credentials = settings.Credential
            };

            var client = new HttpClient(httpHandler, true);
            if (Pooling.UsePooling) {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            }
            return client;
        }

        HttpRequestMessage CreateRequest(MessageHandler message)
        {
            var url = new Uri(BaseAddress, message.RelativeAddress);
            var header = new MediaTypeWithQualityHeaderValue(message.MimeString);
            var request = new HttpRequestMessage(message.Method, url);

            request.Headers.Accept.Add(header);
            if (Pooling.UsePooling) {
                request.Headers.Add("X-Atmosphere-Transport", "long-polling");
                request.Headers.Add("X-Atmosphere-tracking-id", Pooling.ToString());
            }
            return request;
        }


        public void Dispose()
        {
            _innerClient?.CancelPendingRequests();
            _innerClient?.Dispose();
        }
    }
}
