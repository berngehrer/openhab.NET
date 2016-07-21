using Newtonsoft.Json;
using System.Collections.Generic;

namespace openhab.net.rest.Json
{
    [JsonObject]
    internal class SitemapRootObject
    {
        [JsonProperty("sitemap")]
        public List<SitemapObject> Sitemaps { get; set; }
    }
     
    internal class SitemapObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("homepage")]
        public SitemapHomepage Homepage { get; set; }

        public override string ToString() => Label;
    }

    internal class SitemapHomepage
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
