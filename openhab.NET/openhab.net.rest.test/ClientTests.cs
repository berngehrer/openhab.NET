using Microsoft.VisualStudio.TestTools.UnitTesting;
using openhab.net.rest.Http;
using System.Linq;
using System.Threading.Tasks;

namespace openhab.net.rest.test
{
    [TestClass]
    public class ClientTests
    {
        const string TestSitemap = "default";
        const string TestItem = "MQTT_TVLED_POW";

        public ClientTests()
        {
            int port = 8080;
            string host = "192.168.178.69";
            Settings = new ClientSettings(host, port);
        }

        ClientSettings Settings { get; }


        [TestMethod]
        public void TestOpenhabItemClient()
        { 
            Task.Run(async () =>
            {
                using (var context = new ItemContext(Settings))
                {
                    var items = await context.GetAll();
                    Assert.IsNotNull(items);
                    Assert.IsTrue(items.Any());
                }
            })
            .GetAwaiter().GetResult();
        }
         
        [TestMethod]
        public void TestOpenhabItemSingleClient()
        {
            Task.Run(async () =>
            {
                var strategy = new UpdateStrategy(realtime: true);
                using (var context = new ItemContext(Settings, strategy))
                {
                    var item = await context.GetByName(TestItem);
                    Assert.IsNotNull(item);
                }
            })
            .GetAwaiter().GetResult();
        }

        //[TestMethod]
        //public void TestOpenhabSitemapClient()
        //{
        //    Task.Run(async () =>
        //    {
        //        using (var client = new OpenhabSitemapClient(Settings))
        //        {
        //            var sitemaps = await client.GetAllAsync();
        //            Assert.IsNotNull(sitemaps);
        //            Assert.IsTrue(sitemaps.Any());
        //        }
        //    })
        //    .GetAwaiter().GetResult();
        //}

        //[TestMethod]
        //public void TestOpenhabSitemapSingleClient()
        //{
        //    Task.Run(async () =>
        //    {
        //        using (var client = new OpenhabSitemapClient(Settings))
        //        {
        //            var sitemap = await client.GetByNameAsync(TestSitemap);
        //            Assert.IsNotNull(sitemap);
        //        }
        //    })
        //    .GetAwaiter().GetResult();
        //}

        [TestMethod]
        public void TestHttpClientProxy()
        {
            Task.Run(async () =>
            {
                var message = new MessageHandler(SiteCollection.Items);

                using (var client = new HttpClientProxy(Settings))
                {
                    var json = await client.ReadAsString(message);
                    Assert.IsFalse(string.IsNullOrEmpty(json));
                }
            })
            .GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestHttpClientProxyPlain()
        {
            Task.Run(async () =>
            {
                var message = new MessageHandler
                {
                    Collection = SiteCollection.Items,
                    MimeType = MIMEType.PlainText,
                    RelativePath = $"{TestItem}/state"
                };

                using (var client = new HttpClientProxy(Settings))
                {
                    var json = await client.ReadAsString(message);
                    Assert.IsFalse(string.IsNullOrEmpty(json));
                }
            })
            .GetAwaiter().GetResult();
        }

        //[TestMethod, Timeout(TestTimeout.Infinite)]
        //public void TestHttpClientProxyPooling()
        //{
        //    Task.Run(async () =>
        //    {
        //        var pooling = new PoolingSession(1234);
        //        var message = new MessageHandler(SiteCollection.Items, relativePath: TestItem);

        //        using (var client = new HttpClientProxy(Settings, pooling))
        //        {
        //            var json = await client.ReadAsString(message);
        //            Assert.IsFalse(string.IsNullOrEmpty(json));
        //        }
        //    })
        //    .GetAwaiter().GetResult();
        //}
    }
}
