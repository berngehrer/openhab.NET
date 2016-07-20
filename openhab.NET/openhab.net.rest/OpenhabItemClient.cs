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
            : base(host, port, pooling)
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
            return ConvertToItems(json).FirstOrDefault();
        }

        public async Task<IEnumerable<OpenhabItem>> GetAllAsync()
        {
            var message = new MessageHandler
            {
                CancelToken = base.CancelToken,
                Collection = SiteCollection.Items
            };
            
            var json = await RestProxy.ReadAsString(message);
            return ConvertToItems(json);
        }


        protected IEnumerable<OpenhabItem> ConvertToItems(string json)
        {
            var result = JsonConvert.DeserializeObject<ItemRootObject>(json);

            foreach (var item in result.Items)
            {
                switch (item.ItemType)
                {
                    case ItemType.Call:
                        yield return new CallItem(item);
                        break;
                    case ItemType.Color:
                        yield return new ColorItem(item);
                        break;
                    case ItemType.Contact:
                        yield return new ContactItem(item);
                        break;
                    case ItemType.DateTime:
                        yield return new DateTimeItem(item);
                        break;
                    case ItemType.Dimmer:
                        yield return new DimmerItem(item);
                        break;
                    case ItemType.Group:
                        yield return new GroupItem(item);
                        break;
                    case ItemType.Location:
                        yield return new LocationItem(item);
                        break;
                    case ItemType.Number:
                        yield return new NumberItem(item);
                        break;
                    case ItemType.Rollershutter:
                        yield return new RollershutterItem(item);
                        break;
                    case ItemType.String:
                        yield return new StringItem(item);
                        break;
                    case ItemType.Switch:
                        yield return new SwitchItem(item);
                        break;
                }
            }
        }
    }
}
