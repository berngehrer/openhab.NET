using openhab.net.rest.Contracts;
using System.Collections.Generic;

namespace openhab.net.rest
{
    internal class EndpointCollection : Dictionary<RestEndpoint,string>
    {
        public EndpointCollection(IClient client)
        {
            Add(RestEndpoint.Sitemaps, $"{client.BaseAddress}/{RestEndpoint.Sitemaps.GetValue()}");
            Add(RestEndpoint.Items,    $"{client.BaseAddress}/{RestEndpoint.Items.GetValue()}");
        }
    }
}
