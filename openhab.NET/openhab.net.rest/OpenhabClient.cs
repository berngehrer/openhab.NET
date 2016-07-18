//using Newtonsoft.Json;
//using openhab.net.rest.Contracts;
//using openhab.net.rest.JsonEntities;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace openhab.net.rest
//{
//    public class OpenhabClient : IClient
//    {
//        public OpenhabClient(string host, int port = 8080)
//            : this(new ClientSettings(host, port))
//        { 
//        }

//        public OpenhabClient(ClientSettings handler)
//        {
//            Handler = handler;
//            Endpoints = new EndpointCollection(this);
//        }

//        EndpointCollection Endpoints { get; }

//        public ClientSettings Handler { get; }

//        string _baseAddress;
//        public string BaseAddress => _baseAddress ?? (_baseAddress = Handler?.BuildUp());


//        public async Task<IEnumerable<OpenhabItem>> GetResultAsync(RestEndpoint endpoint)
//        {
//            if (endpoint == RestEndpoint.Sitemaps) {
//                throw new System.NotImplementedException();
//            }

//            using (var client = new RequestClient())
//            {
//                var json = await client.GetJson(Endpoints[endpoint]);
//                var result = JsonConvert.DeserializeObject<ItemRootObject>(json);
//                return result.Items;
//            }
//        }
//    }
//}
