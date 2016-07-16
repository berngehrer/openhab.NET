using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace openhab.net.rest.test
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void TestOpenhabClientHandler()
        {
            int port = 8080;
            string host = "localhost";
            string user = "username";
            string pwd = "password";            

            var expected = $"http://{host}:{port}{OpenhabClientHandler.ApiPath}";
            var handler = new OpenhabClientHandler(host, port);
            Assert.AreEqual(handler.BuildUp(), expected);

            expected = $"https://{host}:{port}{OpenhabClientHandler.ApiPath}";
            handler.Protocol = HttpProtocol.HTTPS;
            Assert.AreEqual(handler.BuildUp(), expected);

            expected = $"https://{user}:{pwd}@{host}:{port}{OpenhabClientHandler.ApiPath}";
            handler.Credentials = new System.Net.NetworkCredential(user, pwd);
            Assert.AreEqual(handler.BuildUp(), expected);
        }

        [TestMethod]
        public void TestOpenhabClient()
        {
            string host = "192.168.178.69";

            var client = new OpenhabClient(host);
            Task.Run(async () =>
            {
                var items = await client.GetResultAsync(EndpointType.Items);
                Assert.IsNotNull(items);
                Assert.IsTrue(items.Any());
            })
            .GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestRequestClient()
        {
            int port = 8080;
            string host = "192.168.178.69";
            var handler = new OpenhabClientHandler(host, port);

            Task.Run(async () =>
            {
                using (var client = new RequestClient())
                {
                    var json = await client.GetJson(handler.BuildUp());
                    Assert.IsFalse(string.IsNullOrEmpty(json));
                }
            })
            .GetAwaiter().GetResult();
        }
    }
}
