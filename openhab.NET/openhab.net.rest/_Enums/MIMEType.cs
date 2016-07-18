using openhab.net.rest.ComponentModel;

namespace openhab.net.rest
{
    internal enum MIMEType
    {
        [FieldValue("text/plain")]
        PlainText,
        [FieldValue("application/json")]
        Json
    }
}
