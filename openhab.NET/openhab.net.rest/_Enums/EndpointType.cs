using openhab.net.rest.ComponentModel;

namespace openhab.net.rest
{
    /// <summary>
    /// Represents the set of link types.
    /// Result of links when accessing api root.
    /// </summary>
    public enum EndpointType
    {
        [FieldValue("sitemaps")]
        Sitemaps,
        [FieldValue("items")]
        Items
    }
}
