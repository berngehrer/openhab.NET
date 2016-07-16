using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;

namespace openhab.net.rest.JsonEntities
{
    internal class ItemConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var item = new OpenhabItem();
            var jObject = JObject.Load(reader);
            serializer.Populate(jObject.CreateReader(), item);
            item.ItemType = ParseType(jObject);
            return item;
        }

        ItemType ParseType(JObject obj)
        {
            var key = obj[OpenhabItem.ItemTypeName]?.ToString();
            if (key != null && TypeStrings.ContainsKey(key))
            {
                return TypeStrings[key];
            }
            return ItemType.Unknown;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(OpenhabItem);


        static ConcurrentDictionary<string, ItemType> TypeStrings =
            new ConcurrentDictionary<string, ItemType>(FieldValueExtension.ToDictionary<ItemType>());
    }
}
