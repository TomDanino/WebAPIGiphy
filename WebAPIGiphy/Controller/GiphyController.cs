using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPIGiphy
{
    public class GiphyController : ApiController
    {
        [HttpGet]
        public async Task<IEnumerable<string>> Fetch(string input)
        {
            IEnumerable<string> urls;

            CacheObject result = Cache.Retrieve(input);
            if (result != null && result.urls != null && result.urls.Any() && (DateTime.Now - result.lastUpdate < TimeSpan.FromSeconds(Consts.CACHE_SEARCH_TIMEOUT_HOURS)))
            {
                urls = result.urls;
            }
            else
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var GetURL = await client.GetAsync(String.Format("{0}?q={1}&api_key={2}", Consts.URL_SEARCH, input, Consts.API_KEY));
                        var Content = await GetURL.Content.ReadAsStringAsync();
                        RootObject c = JsonConvert.DeserializeObject<RootObject>(Content);
                        urls = c.data.Select(x => x.url).ToList();
                        Cache.Insert(input, urls);
                    }
                }
                catch (Exception ex)
                {
                    //write to log
                    throw ex;
                }
            }

            return urls;
        }
    }
}
