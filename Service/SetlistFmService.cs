using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LoveMusic
{
    public class SetlistFmService
    {
        private readonly HttpClient _client;
        private readonly string _url = "https://api.setlist.fm/rest/1.0";
        private readonly string _apiKey;

        public SetlistFmService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiKey = config["Api"];
        }

        public async Task<IEnumerable<Setlist>> Search(string searchText)
        {
            var _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}