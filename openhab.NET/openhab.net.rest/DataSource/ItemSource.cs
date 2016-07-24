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
    internal class ItemSource : IDataSource<OpenhabItem>
    {
        bool _disposeClient;
        OpenhabClient _client;

        public ItemSource(OpenhabClient client, bool dispose = false)
        {
            _client = client;
            _disposeClient = dispose;
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
                Method = HttpMethod.Put,
                MimeType = MIMEType.PlainText,
                Content = element.ToString(),
                Collection = TargetCollection,
                RelativePath = $"{element.Name}/state"
            };
            return await UpdateState(message);
        }

        public async Task<bool> UpdateState(MessageHandler message)
        {
            var sendTask = _client.SendCommand(message);
            await sendTask;
            return sendTask.IsSuccess() && sendTask.Result == true;
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
