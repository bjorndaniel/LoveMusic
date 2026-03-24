namespace LoveMusic;

public class SpotifyService
{
    private readonly HttpClient _client;

    private readonly string _baseUrl = "https://api.spotify.com/v1";

    private readonly ILocalStorageService _localStore;

    private List<SpotifyTrack> _addedTracks = [];

    public SpotifyService(HttpClient client, ILocalStorageService localStore)
    {
        _client = client;
        _localStore = localStore;
    }

    public event Func<object, MessageEventArgs, Task>? RequestRefresh;

    public event Func<object, MessageEventArgs, Task>? CreationDone;

    public async Task<SpotifyPlaylist?> CreatePlaylist(string name, List<LastFmTrack> tracks)
    {
        var notFound = 0;
        var processed = 0;
        var toPost = new List<SpotifyTrack>();
        _addedTracks = new List<SpotifyTrack>();
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        var user = await _localStore.GetItemAsync<SpotifyUser>(Constants.SpotifyUserKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var playlist = new CreateSpotifyPlaylist
        {
            Name = name
        };
        var result = await _client.PostAsJsonAsync($"{_baseUrl}/users/{user.Id}/playlists", playlist);
        if(!result.IsSuccessStatusCode)
        {
            RequestRefresh?.Invoke(this, new MessageEventArgs { Messages = new List<string> { "Could not create playlist." } });
            return null;
        }
        var created = JsonSerializer.Deserialize<SpotifyPlaylist>(await result.Content.ReadAsStringAsync());
        foreach(var lfmTrack in tracks)
        {
            var track = await SearchTrack(lfmTrack.Artist.Name, lfmTrack.Name);
            if(track.NotFound)
            {
                notFound++;
            }
            else
            {
                toPost.Add(track);
                _addedTracks.Add(track);
            }
            processed++;
            if(toPost.Count == 10)
            {
                var query = string.Join(',', toPost.Select(_ => _.Uri?.ToString()));
                await _client.PostAsync($"{_baseUrl}/playlists/{created?.Id}/tracks?uris={query}", new StringContent(""));
                toPost.Clear();
                RequestRefresh?.Invoke(this, new MessageEventArgs
                {
                    Messages = new List<string> { $"Added {processed} of {tracks.Count}. {notFound} not found." },
                    Type = UIUpdateType.Processing
                });
            }
        }
        if(toPost.Any())
        {
            var query = string.Join(',', toPost.Select(_ => _.Uri?.ToString()));
            await _client.PostAsync($"{_baseUrl}/playlists/{created?.Id}/tracks?uris={query}", new StringContent(""));
        }
        CreationDone?.Invoke(this, new MessageEventArgs
        {
            Type = UIUpdateType.Done,
            Messages =
                [
                    $"Your playlist {name} was created.",
                    $"{tracks.Count - notFound} added.",
                    $"{notFound} tracks could not be found on Spotify.",
                ]
        });
        return created;
    }

    public async Task<SpotifyPlaylist?> CreatePlaylistSFM(string name, Setlist setlist)
    {
        var notFound = 0;
        var processed = 0;
        var toPost = new List<SpotifyTrack>();
        _addedTracks = new List<SpotifyTrack>();
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        var user = await _localStore.GetItemAsync<SpotifyUser>(Constants.SpotifyUserKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var playlist = new CreateSpotifyPlaylist
        {
            Name = name
        };
        var result = await _client.PostAsJsonAsync($"{_baseUrl}/users/{user.Id}/playlists", playlist);
        if(!result.IsSuccessStatusCode)
        {
            RequestRefresh?.Invoke(this, new MessageEventArgs { Messages = new List<string> { "Could not create playlist." } });
            return null;
        }
        var created = JsonSerializer.Deserialize<SpotifyPlaylist>(await result.Content.ReadAsStringAsync());
        foreach(var song in setlist.Songs)
        {
            var track = await SearchTrack(setlist.Artist.Name, song.Name);
            if(track.NotFound)
            {
                notFound++;
            }
            else
            {
                toPost.Add(track);
                _addedTracks.Add(track);
            }
            processed++;
            if(toPost.Count == 10)
            {
                var query = string.Join(',', toPost.Select(_ => _.Uri?.ToString()));
                await _client.PostAsync($"{_baseUrl}/playlists/{created?.Id}/tracks?uris={query}", new StringContent(""));
                toPost.Clear();
                RequestRefresh?.Invoke(this, new MessageEventArgs
                {
                    Messages = [$"Added {processed} of {setlist.Songs.Count}. {notFound} not found."],
                    Type = UIUpdateType.Processing
                });
            }
        }
        if(toPost.Any())
        {
            var query = string.Join(',', toPost.Select(_ => _.Uri?.ToString()));
            await _client.PostAsync($"{_baseUrl}/playlists/{created?.Id}/tracks?uris={query}", new StringContent(""));
        }
        CreationDone?.Invoke(this, new MessageEventArgs
        {
            Type = UIUpdateType.Done,
            Messages = new List<string>
                 {
                     $"Your playlist {name} was created.",
                     $"{setlist.Songs.Count - notFound} added.",
                     $"{notFound} tracks could not be found on Spotify.",
                 }
        });
        return created;
    }

    public async Task<List<SelectableTrack>> PreviewTracks(List<SelectableTrack> tracks)
    {
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var processed = 0;
        foreach (var track in tracks)
        {
            track.SpotifyResult = await SearchTrack(track.Artist, track.Name);
            track.Selected = !track.NotFound;
            processed++;
            if (processed % 5 == 0 || processed == tracks.Count)
            {
                RequestRefresh?.Invoke(this, new MessageEventArgs
                {
                    Messages = [$"Searching {processed} of {tracks.Count}..."],
                    Type = UIUpdateType.Processing
                });
            }
        }
        return tracks;
    }

    public async Task<SpotifyPlaylist?> CreatePlaylistFromSelected(string name, List<SelectableTrack> selected)
    {
        var toAdd = selected.Where(_ => _.Selected && !_.NotFound).ToList();
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        var user = await _localStore.GetItemAsync<SpotifyUser>(Constants.SpotifyUserKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await _client.PostAsJsonAsync($"{_baseUrl}/users/{user.Id}/playlists", new CreateSpotifyPlaylist { Name = name });
        if (!result.IsSuccessStatusCode)
        {
            RequestRefresh?.Invoke(this, new MessageEventArgs { Messages = ["Could not create playlist."] });
            return null;
        }
        var created = JsonSerializer.Deserialize<SpotifyPlaylist>(await result.Content.ReadAsStringAsync());
        var batch = new List<SpotifyTrack>();
        var processed = 0;
        foreach (var track in toAdd)
        {
            batch.Add(track.SpotifyResult!);
            processed++;
            if (batch.Count == 10)
            {
                var query = string.Join(',', batch.Select(_ => _.Uri));
                await _client.PostAsync($"{_baseUrl}/playlists/{created?.Id}/tracks?uris={query}", new StringContent(""));
                batch.Clear();
                RequestRefresh?.Invoke(this, new MessageEventArgs
                {
                    Messages = [$"Added {processed} of {toAdd.Count}."],
                    Type = UIUpdateType.Processing
                });
            }
        }
        if (batch.Any())
        {
            var query = string.Join(',', batch.Select(_ => _.Uri));
            await _client.PostAsync($"{_baseUrl}/playlists/{created?.Id}/tracks?uris={query}", new StringContent(""));
        }
        var notFound = selected.Count(_ => _.NotFound || !_.Selected);
        CreationDone?.Invoke(this, new MessageEventArgs
        {
            Type = UIUpdateType.Done,
            Messages =
            [
                $"Your playlist {name} was created.",
                $"{toAdd.Count} added.",
                $"{notFound} tracks skipped or not found on Spotify.",
            ]
        });
        return created;
    }

    public async Task<SpotifyTrack> SearchTrack(string artist, string track)
    {
        try
        {
            var sResult = await _client.GetStringAsync($"{_baseUrl}/search?q={artist} {track}&type=track&limit=10");
            var spotifyTracks = JsonSerializer.Deserialize<SpotifyTrackSearchResult>(sResult);
            var tracks = spotifyTracks?.SpotifyTracks.Tracks.ToList() ?? [];
            if (tracks.Count > 0)
            {
                var nonLive = tracks.FirstOrDefault(t => !t.Name.Contains("live", StringComparison.OrdinalIgnoreCase));
                return nonLive ?? tracks.First();
            }
        }
        catch(Exception e)
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

    public async Task<List<SpotifyTrack>> SearchAlternatives(string query)
    {
        try
        {
            var sResult = await _client.GetStringAsync($"{_baseUrl}/search?q={Uri.EscapeDataString(query)}&type=track&limit=5");
            var spotifyTracks = JsonSerializer.Deserialize<SpotifyTrackSearchResult>(sResult);
            return spotifyTracks?.SpotifyTracks.Tracks.ToList() ?? [];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return [];
        }
    }

    public async Task Play(string playlistUri)
    {
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        var player = await _localStore.GetItemAsync<string>(Constants.SpotifyPlayerIdKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var json = new SpotifyPlayUri
        {
            context_uri = playlistUri
        };
        await _client.PutAsJsonAsync($"{_baseUrl}/me/player/play?device_id={player}", json);
    }

    public async Task<List<SpotifyImage>> GetImages(string playlistId)
    {
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await _client.GetAsync($"{_baseUrl}/playlists/{playlistId}/images");
        if(result.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<List<SpotifyImage>>(await result.Content.ReadAsStringAsync()) ?? [];
        }
        return [];

    }

    public async Task UpdatePlayer(SpotifyPlayerTask task)
    {
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        var deviceId = await _localStore.GetItemAsync<string>(Constants.SpotifyPlayerIdKey);
        var devices = await GetDevices();
        if(devices.FirstOrDefault(_ => _.Id == deviceId && _.IsActive) == null)
        {
            await SetPlayerActive(deviceId, task == SpotifyPlayerTask.play);
        }
        var context = await _localStore.GetItemAsync<SpotifyContext>(Constants.SpotifyContextKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        switch(task)
        {
            case SpotifyPlayerTask.next:
            case SpotifyPlayerTask.previous:
                await _client.PostAsync($"{_baseUrl}/me/player/{task}", null);
                break;
            case SpotifyPlayerTask.pause:
                await _client.PutAsync($"{_baseUrl}/me/player/{task}", null);
                break;
            default:
                if(context == null || (!context.Paused ?? true))
                {
                    await _client.PutAsync($"{_baseUrl}/me/player/{task}", null);
                }
                else
                {
                    await _client.PutAsync($"{_baseUrl}/me/player/{task}", null);
                }
                break;
        }
    }

    public async Task<List<SpotifyDevice>> GetDevices()
    {
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await _client.GetAsync($"{_baseUrl}/me/player/devices");
        if(result.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<DeviceResult>(await result.Content.ReadAsStringAsync())?.Devices ?? [];
        }
        return new List<SpotifyDevice>();
    }

    public async Task SetPlayerActive(string id, bool play)
    {
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        await _client.PutAsJsonAsync($"{_baseUrl}/me/player", new ActivateDevice { device_ids = new List<string> { id }, play = play });
    }

    public async Task<bool> GetUsersPlaylists()
    {
        var token = await _localStore.GetItemAsync<string>(Constants.SpotifyTokenKey);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var firstResult = await _client.GetAsync($"{_baseUrl}/me/playlists?limit=50");
        if(firstResult.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<SpotifyPlaylistResult>(await firstResult.Content.ReadAsStringAsync());
            var total = result?.Total;
            var lists = result?.Playlists;
            while(lists?.Count < total)
            {
                result = await _client.GetFromJsonAsync<SpotifyPlaylistResult>($"{_baseUrl}/me/playlists?limit=50&offset={lists.Count}");
                lists.AddRange(result?.Playlists ?? []);
            }
            await _localStore.SetItemAsync(Constants.SpotifyPlaylistsKey, lists);
            return true;
        }
        return false;
    }
}