using openhab.net.rest.Core;

namespace openhab.net.rest.Http
{
    /// <summary>
    /// Protocols which are support by openhab
    /// </summary>
    public enum HttpProtocol
    {
        [FieldValue("http")]
        HTTP,
        [FieldValue("https")]
        HTTPS
    }
}
