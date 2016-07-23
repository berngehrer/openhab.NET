using openhab.net.rest.Http;
using openhab.net.rest.Items;
using openhab.net.rest.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public class ItemContext : OpenhabContext<OpenhabItem>
    {
        public ItemContext(string host, int port = 8080)
            : this(host, port, null)
        {
        }

        public ItemContext(string host, int port = 8080, UpdateStrategy stategy = null)
            : this(new ClientSettings(host, port), stategy)
        {
        }

        public ItemContext(ClientSettings settings, UpdateStrategy strategy = null) 
            : base(settings, strategy)
        {
        }


        internal override async Task<IEnumerable<OpenhabItem>> GetAll(OpenhabClient client, MessageHandler message)
        {
            var result = await client.SendRequest<ItemRootObject>(message);
            // Add to statemanager
            return result.Items.Select(ConvertItem);
        }

        public override async Task<IEnumerable<OpenhabItem>> GetAll()
        {
            CancelSource = new CancellationTokenSource();
            var message = new MessageHandler
            {
                Collection = SiteCollection.Items,
                CancelToken = CancelSource?.Token
            };
            return await GetAll(Connection, message);
        }
        
        public override async Task<OpenhabItem> GetByName(string name)
        {
            CancelSource = new CancellationTokenSource();
            var message = new MessageHandler
            {
                RelativePath = name,
                Collection = SiteCollection.Items,
                CancelToken = CancelSource?.Token
            };
            // Add to statemanager
            return ConvertItem(await Connection.SendRequest<ItemObject>(message));
        }
        
        OpenhabItem ConvertItem(ItemObject item)
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

        internal override SiteCollection Collection => SiteCollection.Items;
    }
}
