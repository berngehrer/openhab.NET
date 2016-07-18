using openhab.net.rest.ComponentModel;

namespace openhab.net.rest
{
    /// <summary>
    /// Represents the set of link types.
    /// Result of links when accessing api root.
    /// </summary>
    public enum RestEndpoint
    {
        [FieldValue("/rest/sitemaps")]
        Sitemaps,
        [FieldValue("/rest/sitemaps")]
        Pages,
        [FieldValue("/rest/items")]
        Groups,
        [FieldValue("/rest/items")]
        Items
    }
}
