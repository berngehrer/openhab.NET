using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal sealed class RequestClient : IDisposable
    {
        public async Task<string> GetJson(string url, bool longPooling = false)
        {
            using (var client = CreateClient(longPooling))
            using (var response = await client.GetAsync(url))
            using (var body = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(body))
            {
                return reader.ReadToEnd();
            }
        }

        HttpClient CreateClient(bool longPooling)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (longPooling) {
                client.DefaultRequestHeaders.Add("X-Atmosphere-Transport", "long-polling");
                client.DefaultRequestHeaders.Add("X-Atmosphere-tracking-id", "1234");
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            }
            return client;
        }

        public void Dispose()
        {
        }
    }
}
