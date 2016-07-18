using System.Net.Http;
using System.Threading;

namespace openhab.net.rest
{ 
    internal class MessageHandler
    {
        public MessageHandler()
        {
        }
        public MessageHandler(RestEndpoint endpoint, string relativePath = "")
        {
            Endpoint = endpoint;
        }

        public string RelativePath { get; set; }
        public RestEndpoint Endpoint { get; set; }
        public CancellationToken? CancelToken { get; set; }
        public MIMEType MimeType { get; set; } = MIMEType.Json;
        public HttpMethod Method { get; set; } = HttpMethod.Get;

        public string MimeString => $"{MimeType.GetValue()}";
        public string RelativeAddress => $"{Endpoint.GetValue()}/{RelativePath}";
    }
}
