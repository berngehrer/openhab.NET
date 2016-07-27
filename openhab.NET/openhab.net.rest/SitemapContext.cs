using openhab.net.rest.Sitemaps;

namespace openhab.net.rest
{
    public class SitemapContext : OpenhabContext<OpenhabSitemap>
    {
        public SitemapContext(string host, int port = 8080)
            : this(new OpenhabSettings(host, port))
        {
        }

        public SitemapContext(OpenhabSettings settings)
            : base(settings, UpdateStrategy.Default)
        {
        }


        internal override IDataSource<OpenhabSitemap> CreateDataSource(OpenhabClient client = null)
        {
            if (client == null)  {
                return new SitemapSourceAdapter(ItemObserver, ClientFactory.CreateClient());
            } else {
                return new SitemapSourceAdapter(ItemObserver, client, disposeClient: false);
            }
        }
    }
}
