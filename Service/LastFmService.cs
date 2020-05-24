using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LoveMusic
{
    public class LastFmService
    {

        private readonly HttpClient _client;
        private readonly string _apiKey;
        private string LastFmUrl => "https://ws.audioscrobbler.com/2.0/?method={0}&user={1}&api_key={2}&format=json&limit={3}&page={4}";

        public LastFmService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiKey = config["Api"];
        }

        public event Func<object, MessageEventArgs, Task> RequestRefresh;

        public async Task<List<LastFmTrack>> GetTracksList(PlaylistType type, string lastFmUser, int nrToGet)
        {
            var returnValue = new List<LastFmTrack>();
            var perPage = nrToGet < 100 ? nrToGet : 100;
            var pageNr = 1;
            var method = Extensions.GetAttributeNameProperty<PlaylistType, LastFmMethod>(type.ToString());
            var url = string.Format(LastFmUrl, method, lastFmUser, _apiKey, perPage, pageNr);
            var result = await _client.GetStringAsync(url);
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
                url = string.Format(LastFmUrl, method, lastFmUser, _apiKey, perPage, pageNr);
                result = await _client.GetStringAsync(url);
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
            var url = string.Format(LastFmUrl, method, lastFmUser, _apiKey, 1, 1);
            var result = await _client.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                return GetTracksResult(type, await result.Content.ReadAsStringAsync()).Attributes.TotalTracks;
            }
            return 0;
        }

        private LastFmTracksList GetTracksResult(PlaylistType type, string result)
        {
            return type
            switch
            {
                PlaylistType.TopTracks => JsonConvert.DeserializeObject<LastFmTopTracksResult>(result).LastFmTracksList,
                    PlaylistType.RecentTracks => JsonConvert.DeserializeObject<LastFmRecentTracksResult>(result).LastFmTracksList,
                    _ => JsonConvert.DeserializeObject<LastFmLovedTracksResult>(result).LastFmTracksList,
            };
        }
    }
}