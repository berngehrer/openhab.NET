using Microsoft.VisualStudio.TestTools.UnitTesting;
using openhab.net.rest.Channels;
using System.Threading.Tasks;

namespace openhab.net.rest.test
{
    [TestClass]
    public class PoolingClientTest
    {
        const string TestItem = "MQTT_TVLED_POW";

        public PoolingClientTest()
        {
            Settings = new ClientSettings("192.168.178.69");
        }

        ClientSettings Settings { get; }


        [TestMethod]
        public void TestHttpClientProxy()
        {
            Task.Run(async () =>
            {
                var message = new MessageHandler(RestEndpoint.Items);
                
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
                    Endpoint = RestEndpoint.Items,
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

        [TestMethod, Timeout(TestTimeout.Infinite)]
        public void TestHttpClientProxyPooling()
        {
            Task.Run(async () =>
            {
                var pooling = new PoolingSession(1234);
                var message = new MessageHandler(RestEndpoint.Items, relativePath: TestItem);

                using (var client = new HttpClientProxy(Settings, pooling))
                {
                    var json = await client.ReadAsString(message);
                    Assert.IsFalse(string.IsNullOrEmpty(json));
                }
            })
            .GetAwaiter().GetResult();
        }
    }
}
