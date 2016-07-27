using openhab.net.rest.Core;
using openhab.net.rest.Http;
using openhab.net.rest.Json;
using openhab.net.rest.Sitemaps;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal class SitemapSourceAdapter : IDataSource<OpenhabSitemap>
    {
        bool _disposeClient;
        OpenhabClient _client;
        IElementObserver _observer;

        public SitemapSourceAdapter(IElementObserver observer, OpenhabClient client, bool disposeClient = true)
        {
            _client = client;
            _observer = observer;
            _disposeClient = disposeClient;
        }

        public SiteCollection TargetCollection => SiteCollection.Sitemaps;


        public async Task<IEnumerable<OpenhabSitemap>> GetAll(CancellationToken? token = null)
        {
            var message = new MessageHandler
            {
                Collection = TargetCollection,
                CancelToken = token
            };
            return await GetAll(message);
        }

        public async Task<IEnumerable<OpenhabSitemap>> GetAll(MessageHandler message)
        {
            var result = await _client.SendRequest<SitemapRootObject>(message);
            return result.Sitemaps.Select(ConvertSitemap);
        }

        public async Task<OpenhabSitemap> GetByName(string name, CancellationToken? token = null)
        {
            var message = new MessageHandler
            {
                RelativePath = name,
                CancelToken = token,
                Collection = TargetCollection,
            };
            return await GetByName(message);
        }

        public async Task<OpenhabSitemap> GetByName(MessageHandler message)
        {
            var result = await _client.SendRequest<SitemapObject>(message);
            return ConvertSitemap(result);
        }

        public Task<bool> UpdateState(OpenhabSitemap element, CancellationToken? token = null)
        {
            return Task.FromResult(false);
        }

        public Task<bool> UpdateState(MessageHandler message)
        {
            return Task.FromResult(false);
        }

        public void Dispose()
        {
            if (_disposeClient) {
                _client.Dispose();
            }
        }


        OpenhabSitemap ConvertSitemap(SitemapObject item)
        {
            return new OpenhabSitemap(item, _observer);
        }
    }
}
