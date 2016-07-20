using openhab.net.rest.Core;

namespace openhab.net.rest.Http
{
    /// <summary>
    /// Represents the set of link types.
    /// Result of links when accessing api root.
    /// </summary>
    internal enum SiteCollection
    {
        [FieldValue("/rest/sitemaps")]
        Sitemaps,
        [FieldValue("/rest/items")]
        Items
    }
}
