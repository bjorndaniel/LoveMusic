using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LoveMusic
{
    public class SetlistFmService
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl;

        public SetlistFmService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiUrl = config["FunctionsApi"];
        }
        public async Task<SetlistResponse> Search(string artist, string city, string venue, int page)
        {
            var result = await _client.GetAsync($"{_apiUrl}/setlistfm/search?artist={artist}&city={city}&venue={venue}&page={page}").ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                var setlists = JsonSerializer.Deserialize<SetlistResponse>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                return setlists;
            }

            return new SetlistResponse { Total = 0 };
        }
    }
}