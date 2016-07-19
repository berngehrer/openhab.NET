using Newtonsoft.Json;
using openhab.net.rest.Items;
using openhab.net.rest.JsonEntities;
using System.Collections.Generic;

namespace openhab.net.rest
{
    class ItemModel
    {
        public IEnumerable<OpenhabItem> Test()
        {
            var result = JsonConvert.DeserializeObject<ItemRootObject>("");

            foreach (var item in result.Items)
            {
                switch (item.ItemType)
                {
                    case ItemType.Number:
                        yield return new NumberItem(item);
                        break;
                    case ItemType.String:
                        yield return new StringItem(item);
                        break;
                }
            }
        }
    }
}
