using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LoveMusic
{
    public class LastFmService
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl;

        public LastFmService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiUrl = config["FunctionsApi"];
        }

        public event Func<object, MessageEventArgs, Task> RequestRefresh;

        public async Task<List<LastFmTrack>> GetTracksList(PlaylistType type, string lastFmUser, int nrToGet)
        {
            var returnValue = new List<LastFmTrack>();
            var perPage = nrToGet < 100 ? nrToGet : 100;
            var pageNr = 1;
            var method = Extensions.GetAttributeNameProperty<PlaylistType, LastFmMethod>(type.ToString());
            var result = await _client.GetStringAsync($"{_apiUrl}/lastfm/tracks?user={lastFmUser}&method={method}&perpage={perPage}&page={pageNr}");
            var lfmResult = GetTracksResult(type, result);
            var totalTracks = lfmResult.Attributes.TotalTracks;
            nrToGet = (int) totalTracks < nrToGet ? (int) totalTracks : nrToGet;
            returnValue.AddRange(lfmResult.Tracks);
            while (returnValue.Count < nrToGet)
            {
                if ((nrToGet - returnValue.Count) < perPage)
                {
                    perPage = nrToGet - returnValue.Count;
                }
                pageNr++;
                result = await _client.GetStringAsync($"{_apiUrl}/lastfm/tracks?user={lastFmUser}&method={method}&perpage={perPage}&page={pageNr}");
                returnValue.AddRange(GetTracksResult(type, result).Tracks);
                if (returnValue.Count % 50 == 0)
                {
                    RequestRefresh?.Invoke(this, new MessageEventArgs
                    {
                        Messages = new List<string> { $"{returnValue.Count} of {nrToGet} fetched." },
                            Type = UIUpdateType.Processing
                    });
                }

            }
            return returnValue;
        }

        public async Task<long> GetTrackCount(PlaylistType type, string lastFmUser)
        {
            var method = Extensions.GetAttributeNameProperty<PlaylistType, LastFmMethod>(type.ToString());
            var result = await _client.GetAsync($"{_apiUrl}/lastfm/count?user={lastFmUser}&method={method}");
            if (result.IsSuccessStatusCode)
            {
                var model = JsonSerializer.Deserialize<LastFmCount>(await result.Content.ReadAsStringAsync());
                return model.Count;
            }
            return 0;
        }

        private LastFmTracksList GetTracksResult(PlaylistType type, string result)
        {
            return type
            switch
            {
                PlaylistType.TopTracks => JsonSerializer.Deserialize<LastFmTopTracksResult>(result).LastFmTracksList,
                    PlaylistType.RecentTracks => JsonSerializer.Deserialize<LastFmRecentTracksResult>(result).LastFmTracksList,
                    _ => JsonSerializer.Deserialize<LastFmLovedTracksResult>(result).LastFmTracksList,
            };
        }
    }
}