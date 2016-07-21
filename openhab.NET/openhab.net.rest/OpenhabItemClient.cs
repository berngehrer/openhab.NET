using Newtonsoft.Json;
using openhab.net.rest.Http;
using openhab.net.rest.Items;
using openhab.net.rest.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public class OpenhabItemClient : OpenhabClient<OpenhabItem>, IOpenhabClient<OpenhabItem>
    {
        public OpenhabItemClient(string host, int port = 8080, bool pooling = false)
            : base(new ClientSettings(host, port), pooling)
        {
        }

        public OpenhabItemClient(ClientSettings settings, bool pooling = false)
            : base(settings, pooling)
        {
        }
        

        public async Task<OpenhabItem> GetByNameAsync(string name)
        {
            var message = new MessageHandler
            {
                RelativePath = name,
                CancelToken = base.CancelToken,
                Collection = SiteCollection.Items
            };

            var json = await RestProxy.ReadAsString(message);
            var item = JsonConvert.DeserializeObject<ItemObject>(json);
            return ConvertItem(item);
        }

        public async Task<IEnumerable<OpenhabItem>> GetAllAsync()
        {
            var message = new MessageHandler
            {
                CancelToken = base.CancelToken,
                Collection = SiteCollection.Items
            };
            
            var json = await RestProxy.ReadAsString(message);
            var result = JsonConvert.DeserializeObject<ItemRootObject>(json);
            return result.Items.Select(ConvertItem);
        }


        internal OpenhabItem ConvertItem(ItemObject item)
        {
            switch (item.ItemType)
            {
                case ItemType.Call:
                    return new CallItem(item);
                case ItemType.Color:
                    return new ColorItem(item);
                case ItemType.Contact:
                    return new ContactItem(item);
                case ItemType.DateTime:
                    return new DateTimeItem(item);
                case ItemType.Dimmer:
                    return new DimmerItem(item);
                case ItemType.Group:
                    return new GroupItem(item);
                case ItemType.Location:
                    return new LocationItem(item);
                case ItemType.Number:
                    return new NumberItem(item);
                case ItemType.Rollershutter:
                    return new RollershutterItem(item);
                case ItemType.String:
                    return new StringItem(item);
                case ItemType.Switch:
                    return new SwitchItem(item);
            }
            return null;
        }
    }
}
