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
            const string url = @"http://192.168.178.69:8080/rest/items";
            //const string url = @"http://192.168.178.69:8080/rest/items/gLight";
            //const string url = @"http://192.168.178.69:8080/rest/items/MQTT_TVLED_POW";
            //const string url = @"http://192.168.178.69:8080/rest/items/MQTT_TVLED_POW/state";

            Task.Run(async () =>
            {
                using (var client = new RequestClient())
                {
                    var json = await client.GetJson(url, longPooling: true);
                    Assert.IsFalse(string.IsNullOrEmpty(json));
                }
            })
            .GetAwaiter().GetResult();
        }


        // - As String
        // - As Date
        // - As Float
        // - As Int
        // - As Bool
    }
}
