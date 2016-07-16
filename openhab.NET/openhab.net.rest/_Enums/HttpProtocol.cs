using openhab.net.rest.ComponentModel;

namespace openhab.net.rest
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
