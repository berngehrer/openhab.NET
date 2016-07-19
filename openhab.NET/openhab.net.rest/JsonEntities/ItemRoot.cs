using Newtonsoft.Json;
using System.Collections.Generic;

namespace openhab.net.rest.JsonEntities
{
    [JsonObject]
    public class ItemRootObject
    {
        [JsonProperty("item")]
        public List<ItemObject> Items { get; set; }
    }

    [JsonConverter(typeof(ItemConverter))]
    public class ItemObject
    {
        public const string ItemTypeName = "type";

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonIgnore]
        public ItemType ItemType { get; set; }
         
        public override string ToString() => $"{ItemType} | {Name}";
    }
}
