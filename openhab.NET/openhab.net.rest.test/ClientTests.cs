//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Linq;
//using System.Threading.Tasks;

//const string url = @"http://192.168.178.69:8080/rest/items";
//const string url = @"http://192.168.178.69:8080/rest/items/gLight";
//const string url = @"http://192.168.178.69:8080/rest/items/MQTT_TVLED_POW";
//const string url = @"http://192.168.178.69:8080/rest/items/MQTT_TVLED_POW/state";

//namespace openhab.net.rest.test
//{
//    [TestClass]
//    public class ClientTests
//    {
//        [TestMethod]
//        public void TestOpenhabClientHandler()
//        {
//            int port = 8080;
//            string host = "localhost";
//            string user = "username";
//            string pwd = "password";            

//            var expected = $"http://{host}:{port}{ClientSettings.ApiPath}";
//            var handler = new ClientSettings(host, port);
//            Assert.AreEqual(handler.BuildUp(), expected);

//            expected = $"https://{host}:{port}{ClientSettings.ApiPath}";
//            handler.Protocol = HttpProtocol.HTTPS;
//            Assert.AreEqual(handler.BuildUp(), expected);

//            expected = $"https://{user}:{pwd}@{host}:{port}{ClientSettings.ApiPath}";
//            handler.Credential = new System.Net.NetworkCredential(user, pwd);
//            Assert.AreEqual(handler.BuildUp(), expected);
//        }

//        [TestMethod]
//        public void TestOpenhabClient()
//        {
//            string host = "192.168.178.69";

//            var client = new OpenhabClient(host);
//            Task.Run(async () =>
//            {
//                var items = await client.GetResultAsync(RestEndpoint.Items);
//                Assert.IsNotNull(items);
//                Assert.IsTrue(items.Any());
//            })
//            .GetAwaiter().GetResult();
//        }

//        [TestMethod]
//        public void TestRequestClient()
//        {
//            int port = 8080;
//            string host = "192.168.178.69";
//            var handler = new ClientSettings(host, port);

//            Task.Run(async () =>
//            {
//                using (var client = new RequestClient())
//                {
//                    var json = await client.GetJson(handler.BuildUp());
//                    Assert.IsFalse(string.IsNullOrEmpty(json));
//                }
//            })
//            .GetAwaiter().GetResult();
//        }
//    }
//}
