namespace LoveMusic;

public class LastFmService
{
    private readonly HttpClient _client;
    private readonly string _apiUrl;

    public LastFmService(HttpClient client, IConfiguration config)
    {
        _client = client;
        _apiUrl = config["FunctionsApi"] ?? string.Empty;
    }

    public event Func<object, MessageEventArgs, Task>? RequestRefresh;

    public async Task<List<LastFmTrack>> GetTracksList(PlaylistType type, string lastFmUser, int nrToGet)
    {
        var returnValue = new List<LastFmTrack>();
        var perPage = nrToGet < 100 ? nrToGet : 100;
        var pageNr = 1;
        var method = Extensions.GetAttributeNameProperty<PlaylistType, LastFmMethod>(type.ToString());
        var result = await _client.GetStringAsync($"{_apiUrl}/lastfm/tracks?user={lastFmUser}&method={method}&perpage={perPage}&page={pageNr}");
        var lfmResult = GetTracksResult(type, result);
        var totalTracks = lfmResult.Attributes.TotalTracks;
        nrToGet = (int)totalTracks < nrToGet ? (int)totalTracks : nrToGet;
        returnValue.AddRange(lfmResult.Tracks);
        while(returnValue.Count < nrToGet)
        {
            if((nrToGet - returnValue.Count) < perPage)
            {
                perPage = nrToGet - returnValue.Count;
            }
            pageNr++;
            result = await _client.GetStringAsync($"{_apiUrl}/lastfm/tracks?user={lastFmUser}&method={method}&perpage={perPage}&page={pageNr}");
            returnValue.AddRange(GetTracksResult(type, result).Tracks);
            if(returnValue.Count % 50 == 0)
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
        try
        {
            var method = Extensions.GetAttributeNameProperty<PlaylistType, LastFmMethod>(type.ToString());
            var result = await _client.GetAsync($"{_apiUrl}/lastfm/count?user={lastFmUser}&method={method}");
            if(result.IsSuccessStatusCode)
            {
                var model = JsonSerializer.Deserialize<LastFmCount>(await result.Content.ReadAsStringAsync());
                return model?.Count ?? 0;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error fetching track count: {ex.Message}");
        }
        return 0;
    }

    private LastFmTracksList GetTracksResult(PlaylistType type, string result) => type
        switch
    {
        PlaylistType.TopTracks => JsonSerializer.Deserialize<LastFmTopTracksResult>(result)?.LastFmTracksList ?? new(),
        PlaylistType.RecentTracks => JsonSerializer.Deserialize<LastFmRecentTracksResult>(result)?.LastFmTracksList ?? new(),
        _ => JsonSerializer.Deserialize<LastFmLovedTracksResult>(result)?.LastFmTracksList ?? new()
    };
}