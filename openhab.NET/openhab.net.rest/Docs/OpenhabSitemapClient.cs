//using Newtonsoft.Json;
//using openhab.net.rest.Http;
//using openhab.net.rest.Json;
//using openhab.net.rest.Sitemaps;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace openhab.net.rest
//{
//    public class OpenhabSitemapClient : OpenhabClient<OpenhabSitemap>, IOpenhabClient<OpenhabSitemap>
//    {
//        public OpenhabSitemapClient(string host, int port = 8080, bool pooling = false)
//            : base(new ClientSettings(host, port), pooling)
//        {
//        }

//        public OpenhabSitemapClient(ClientSettings settings, bool pooling = false)
//            : base(settings, pooling)
//        {
//        }


//        public async Task<OpenhabSitemap> GetByNameAsync(string name)
//        {
//            var message = new MessageHandler
//            {
//                RelativePath = name,
//                CancelToken = base.CancelToken,
//                Collection = SiteCollection.Sitemaps
//            };

//            var json = await RestProxy.ReadAsString(message);
//            var result = JsonConvert.DeserializeObject<SitemapObject>(json);
//            return ConvertSitemap(result);
//        }

//        public async Task<IEnumerable<OpenhabSitemap>> GetAllAsync()
//        {
//            var message = new MessageHandler
//            {
//                CancelToken = base.CancelToken,
//                Collection = SiteCollection.Sitemaps
//            };

//            var json = await RestProxy.ReadAsString(message);
//            var result = JsonConvert.DeserializeObject<SitemapRootObject>(json);
//            return result.Sitemaps.Select(ConvertSitemap);
//        }


//        internal OpenhabSitemap ConvertSitemap(SitemapObject item)
//        {
//            return new OpenhabSitemap(item);
//        }
//    }
//}
