using openhab.net.rest.ComponentModel;

namespace openhab.net.rest
{
    internal enum TransferProtocol
    {
        [FieldValue("http")]
        HTTP,
        [FieldValue("https")]
        HTTPS
    }
}
