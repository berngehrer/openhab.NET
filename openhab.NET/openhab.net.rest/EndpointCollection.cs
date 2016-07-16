using openhab.net.rest.Contracts;
using System.Collections.Generic;

namespace openhab.net.rest
{
    internal class EndpointCollection : Dictionary<EndpointType,string>
    {
        public EndpointCollection(IClient client)
        {
            Add(EndpointType.Sitemaps, $"{client.BaseAddress}/{EndpointType.Sitemaps.GetValue()}");
            Add(EndpointType.Items,    $"{client.BaseAddress}/{EndpointType.Items.GetValue()}");
        }
    }
}
