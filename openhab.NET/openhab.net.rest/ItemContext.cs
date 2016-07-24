using openhab.net.rest.DataSource;
using openhab.net.rest.Items;

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
            : base(settings, strategy ?? UpdateStrategy.Default)
        {
        }


        internal override IDataSource<OpenhabItem> CreateDataSource(OpenhabClient client = null)
        {
            if (client == null) {
                client = ClientFactory.Create(withStrategy: false);
                return new ItemSource(client, true);
            }
            else {
                return new ItemSource(client);
            }
        }
    }
}
