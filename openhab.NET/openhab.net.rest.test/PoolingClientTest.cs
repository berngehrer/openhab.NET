using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace openhab.net.rest.test
{
    [TestClass]
    public class PoolingClientTest
    {
        [TestMethod, Timeout(TestTimeout.Infinite)]
        public void TestRequestClientPooling()
        {
            //const string url = @"http://192.168.178.69:8080/rest/items";
            //const string url = @"http://192.168.178.69:8080/rest/items/gLight";
            //const string url = @"http://192.168.178.69:8080/rest/items/MQTT_TVLED_POW";
            //const string url = @"http://192.168.178.69:8080/rest/items/MQTT_TVLED_POW/state";

            Task.Run(async () =>
            {
                var settings = new ClientSettings("192.168.178.69");
                var message = new MessageHandler(RestEndpoint.Items);

                using (var client = new HttpClientProxy(settings))
                {
                    var json = await client.ReadAsString(message);
                    Assert.IsFalse(string.IsNullOrEmpty(json));
                }
            })
            .GetAwaiter().GetResult();
        }


        
    }
}
