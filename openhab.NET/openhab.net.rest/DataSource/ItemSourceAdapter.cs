using openhab.net.rest.Core;
using openhab.net.rest.Http;
using openhab.net.rest.Items;
using openhab.net.rest.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest.DataSource
{
    internal class ItemSourceAdapter : IDataSource<OpenhabItem>
    {
        bool _disposeClient;
        OpenhabClient _client;
        IElementObserver _observer;

        public ItemSourceAdapter(IElementObserver observer, OpenhabClient client, bool disposeClient = true)
        {
            _client = client;
            _observer = observer;
            _disposeClient = disposeClient;
        }

        public SiteCollection TargetCollection => SiteCollection.Items;


        public async Task<IEnumerable<OpenhabItem>> GetAll(CancellationToken? token = null)
        {
            var message = new MessageHandler
            {
                Collection = TargetCollection,
                CancelToken = token
            };
            return await GetAll(message);
        }

        public async Task<IEnumerable<OpenhabItem>> GetAll(MessageHandler message)
        {
            var result = await _client.SendRequest<ItemRootObject>(message);
            return result.Items.Select(ConvertItem);
        }

        public async Task<OpenhabItem> GetByName(string name, CancellationToken? token = null)
        {
            var message = new MessageHandler
            {
                RelativePath = name,
                CancelToken = token,
                Collection = TargetCollection,
            };
            return await GetByName(message);
        }

        public async Task<OpenhabItem> GetByName(MessageHandler message)
        {
            var result = await _client.SendRequest<ItemObject>(message);
            return ConvertItem(result);
        }

        public async Task<bool> UpdateState(OpenhabItem element, CancellationToken? token = null)
        {
            var message = new MessageHandler
            {
                CancelToken = token,
                Method = HttpMethod.Post,
                MimeType = MIMEType.PlainText,
                Content = element.ToString(),
                Collection = TargetCollection,
                RelativePath = element.Name
            };
            return await UpdateState(message);
        }

        public async Task<bool> UpdateState(MessageHandler message)
        {
            return await _client.SendCommand(message);
        }

        public void Dispose()
        {
            if (_disposeClient) {
                _client.Dispose();
            }
        }


        OpenhabItem ConvertItem(ItemObject item)
        {
            switch (item.ItemType)
            {
                case ItemType.Call:
                    return new CallItem(item, _observer);
                case ItemType.Color:
                    return new ColorItem(item, _observer);
                case ItemType.Contact:
                    return new ContactItem(item, _observer);
                case ItemType.DateTime:
                    return new DateTimeItem(item, _observer);
                case ItemType.Dimmer:
                    return new DimmerItem(item, _observer);
                case ItemType.Group:
                    return new GroupItem(item, _observer);
                case ItemType.Location:
                    return new LocationItem(item, _observer);
                case ItemType.Number:
                    return new NumberItem(item, _observer);
                case ItemType.Rollershutter:
                    return new RollershutterItem(item, _observer);
                case ItemType.String:
                    return new StringItem(item, _observer);
                case ItemType.Switch:
                    return new SwitchItem(item, _observer);
            }
            return null;
        }
    }
}
