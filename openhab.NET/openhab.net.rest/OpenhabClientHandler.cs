using System.Net;

namespace openhab.net.rest
{
    public class OpenhabClientHandler
    {
        public const string ApiPath = "/rest";

        public OpenhabClientHandler(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public int Port { get; set; }
        public string Host { get; set; }
        public IWebProxy Proxy { get; set; }
        public HttpProtocol Protocol { get; set; }
        public NetworkCredential Credentials { get; set; }

        public string BuildUp()
        {
            string credentials = string.Empty;
            if (Credentials != null) {
                credentials = $"{Credentials.UserName}:{Credentials.Password}@";
            }
            string protocol = $"{Protocol.GetValue()}://";
            string address = $"{Host}:{Port}";
            return $"{protocol}{credentials}{address}{ApiPath}";
        }
    }
}
