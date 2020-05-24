using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace LoveMusic
{
    public class SpotifyService
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStore;

        private List<SpotifyTrack> _addedTracks;

        public SpotifyService(HttpClient client, ILocalStorageService localStore)
        {
            _client = client;
            _localStore = localStore;
        }

        public event Func<object, MessageEventArgs, Task> RequestRefresh;

        public event Func<object, MessageEventArgs, Task> CreationDone;

        public async Task<SpotifyPlaylist> CreatePlaylist(string name, List<LastFmTrack> tracks)
        {
            var notFound = 0;
            var processed = 0;
            var toPost = new List<SpotifyTrack>();
            _addedTracks = new List<SpotifyTrack>();
            var token = await _localStore.GetItemAsync<string>("SpotifyToken");
            var user = await _localStore.GetItemAsync<SpotifyUser>("SpotifyUser");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var playlist = new CreateSpotifyPlaylist
            {
                Name = name
            };
            var result = await _client.PostAsJsonAsync($"https://api.spotify.com/v1/users/{user.Id}/playlists", playlist);
            if (!result.IsSuccessStatusCode)
            {
                RequestRefresh?.Invoke(this, new MessageEventArgs { Messages = new List<string> { "Could not create playlist." } });
                return null;
            }
            var created = JsonConvert.DeserializeObject<SpotifyPlaylist>(await result.Content.ReadAsStringAsync());
            foreach (var lfmTrack in tracks)
            {
                var track = await SearchTrack(lfmTrack.Artist.Name, lfmTrack.Name);
                if (track.NotFound)
                {
                    notFound++;
                }
                else
                {
                    toPost.Add(track);
                    _addedTracks.Add(track);
                }
                processed++;
                if (toPost.Count == 10)
                {
                    var query = string.Join(',', toPost.Select(_ => _.Uri?.ToString()));
                    await _client.PostAsync($"https://api.spotify.com/v1/playlists/{created.Id}/tracks?uris={query}", new StringContent(""));
                    toPost.Clear();
                    RequestRefresh?.Invoke(this, new MessageEventArgs
                    {
                        Messages = new List<string> { $"Added {processed} of {tracks.Count}. {notFound} not found." },
                            Type = UIUpdateType.Processing
                    });
                }
            }
            if (toPost.Any())
            {
                var query = string.Join(',', toPost.Select(_ => _.Uri?.ToString()));
                await _client.PostAsync($"https://api.spotify.com/v1/playlists/{created.Id}/tracks?uris={query}", new StringContent(""));
            }
            CreationDone?.Invoke(this, new MessageEventArgs
            {
                Type = UIUpdateType.Done,
                    Messages = new List<string>
                    {
                        $"Your playlist {name} was created.",
                        $"{tracks.Count - notFound} added.",
                        $"{notFound} tracks could not be found on Spotify.",
                    }
            });
            return created;
        }

        public async Task<SpotifyTrack> SearchTrack(string artist, string track)
        {
            try
            {
                var sResult = await _client.GetStringAsync($"https://api.spotify.com/v1/search?q={artist} {track}&type=track&limit=3");
                var spotifyTracks = JsonConvert.DeserializeObject<SpotifyTrackSearchResult>(sResult);
                if (spotifyTracks.SpotifyTracks.Total > 0)
                {
                    return spotifyTracks.SpotifyTracks.Tracks.First();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new SpotifyTrack
            {
                Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = artist } },
                    Name = $"{track} NOT FOUND!!!!!",
                    NotFound = true
            };
        }

        public async Task Play(string playlistUri)
        {
            var token = await _localStore.GetItemAsync<string>("SpotifyToken");
            var player = await _localStore.GetItemAsync<string>("SpotifyPlayerId");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var json = new SpotifyPlayUri
            {
                context_uri = playlistUri
            };
            await _client.PutAsJsonAsync($"https://api.spotify.com/v1/me/player/play?device_id={player}", json);
        }

        public async Task<List<SpotifyPlaylistImage>> GetImages(string playlistId)
        {
            var token = await _localStore.GetItemAsync<string>("SpotifyToken");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await _client.GetStringAsync($"https://api.spotify.com/v1/playlists/{playlistId}/images");
            return JsonConvert.DeserializeObject<List<SpotifyPlaylistImage>>(result);
        }

        public async Task UpdatePlayer(SpotifyPlayerTask task)
        {
            var token = await _localStore.GetItemAsync<string>("SpotifyToken");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            switch (task)
            {
                case SpotifyPlayerTask.next:
                case SpotifyPlayerTask.previous:
                    await _client.PostAsync($"https://api.spotify.com/v1/me/player/{task}", null);
                    break;
                default:
                    await _client.PutAsync($"https://api.spotify.com/v1/me/player/{task}", null);
                    break;
            }
        }
    }
}