using System.Net.Http;
using System.Threading;

namespace openhab.net.rest.Http
{ 
    internal class MessageHandler
    {
        public MessageHandler()
        {
        }
        public MessageHandler(SiteCollection collection, string relativePath = "")
        {
            Collection = collection;
        }

        public string RelativePath { get; set; }
        public SiteCollection Collection { get; set; }
        public CancellationToken? CancelToken { get; set; }
        public MIMEType MimeType { get; set; } = MIMEType.Json;
        public HttpMethod Method { get; set; } = HttpMethod.Get;

        public string MimeString => $"{MimeType.GetValue()}";
        public string RelativeAddress => $"{Collection.GetValue()}/{RelativePath}";
    }
}
