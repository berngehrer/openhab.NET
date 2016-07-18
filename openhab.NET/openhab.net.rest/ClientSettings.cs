using System.Net;

namespace openhab.net.rest
{
    public class ClientSettings 
    {
        public ClientSettings(string host, int port = 8080)
        {
            Host = host;
            Port = port;
            Protocol = HttpProtocol.HTTP;
        }
        
        public int Port { get; set; }
        public string Host { get; set; }
        public IWebProxy Proxy { get; set; }
        public HttpProtocol Protocol { get; set; }
        public NetworkCredential Credential { get; set; }

        public string BaseAddress => $"{Protocol.GetValue()}://{Host}:{Port}";
    }
}
