﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest.Http
{
    internal sealed class HttpClientProxy : IDisposable
    {
        HttpClient _innerClient;
        PoolingSession _pooling;
        
        public HttpClientProxy(OpenhabSettings settings)
            : this(settings, PoolingSession.False)
        {
        }

        public HttpClientProxy(OpenhabSettings settings, PoolingSession pooling)
        {
            _pooling = pooling;
            _innerClient = CreateClient(settings);
        }
        

        public async Task<string> ReadAsString(MessageHandler message)
        {
            using (var response = await GetResponse(message))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public Task<bool> SendMessage(MessageHandler message)
        {
            // TODO: Async problem!
            var task = GetResponse(message);
            task.Wait();
            using (var response = task.Result) 
            {
                return Task.FromResult(response.IsSuccessStatusCode);
            }
        }

        Task<HttpResponseMessage> GetResponse(MessageHandler message)
        {
            var request = CreateRequest(message);

            if (message.CancelToken.HasValue) {
                return _innerClient.SendAsync(request, message.CancelToken.Value);
            } else {
                return _innerClient.SendAsync(request);
            }
        }

        HttpClient CreateClient(OpenhabSettings settings)
        {
            var httpHandler = new HttpClientHandler
            {
                Proxy = settings.Proxy,
                AllowAutoRedirect = false,
                Credentials = settings.Credential
            };
            
            var client = new HttpClient(httpHandler, true); 
            client.BaseAddress = new Uri(settings.BaseAddress);
            if (_pooling.UsePooling) {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            }
            return client;
        }

        HttpRequestMessage CreateRequest(MessageHandler message)
        {
            var header = new MediaTypeWithQualityHeaderValue(message.MimeString);
            var request = new HttpRequestMessage(message.Method, message.RelativeAddress);
            
            request.Headers.Accept.Add(header);
            if (_pooling.UsePooling) {
                request.Headers.Add("X-Atmosphere-Transport", "long-polling");
                request.Headers.Add("X-Atmosphere-tracking-id", _pooling.ToString());
            }
            if (!string.IsNullOrEmpty(message.Content)) {
                request.Content = new StringContent(message.Content, Encoding.UTF8, message.MimeString);
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
