using openhab.net.rest.Core;

namespace openhab.net.rest.Http
{
    internal enum MIMEType
    {
        [FieldValue("text/plain")]
        PlainText,
        [FieldValue("application/json")]
        Json
    }
}
