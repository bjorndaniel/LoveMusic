namespace LoveMusic;

public class SpotifyTrackSearchResult
{
    [JsonPropertyName("tracks")]
    public SpotifyTracks SpotifyTracks { get; set; } = new SpotifyTracks();
}

public class SpotifyTracks
{
    [JsonPropertyName("items")]
    public IEnumerable<SpotifyTrack> Tracks { get; set; } = Enumerable.Empty<SpotifyTrack>();

    [JsonPropertyName("total")]
    public long Total { get; set; }
}
public class SpotifyTrack
{
    [JsonPropertyName("artists")]
    public IEnumerable<SpotifyArtist> Artists { get; set; } = Enumerable.Empty<SpotifyArtist>();

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;

    [JsonIgnore]
    public bool NotFound { get; set; }
}
public class SpotifyArtist
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class SpotifyContext
{
    [JsonPropertyName("context")]
    public PlayerContext Context { get; set; } = new PlayerContext();

    [JsonPropertyName("duration")]
    public double Duration { get; set; }

    [JsonPropertyName("position")]
    public double Position { get; set; }

    [JsonPropertyName("paused")]
    public bool? Paused { get; set; }

    [JsonPropertyName("track_window")]
    public TrackContext TrackWindow { get; set; } = new TrackContext();
    public string Track => TrackWindow?.CurrentTrack?.Name ?? string.Empty;
    public string Artist => TrackWindow?.CurrentTrack?.Artist ?? string.Empty;
    public bool HasUri => !string.IsNullOrEmpty(Context?.Uri);
    public bool HasImage => TrackWindow?.CurrentTrack?.album?.images?.OrderByDescending(_ => _.Height).FirstOrDefault()?.Url != null;
    public string Image => TrackWindow?.CurrentTrack?.album?.images?.OrderByDescending(_ => _.Height).FirstOrDefault()?.Url ?? string.Empty;
    public bool IsPlayingList(string id)
    {
        if(!string.IsNullOrEmpty(Context?.Uri) && Context.Uri.Contains(id))
        {
            return true;
        }
        return false;
    }
    public SpotifyPlaylist FindPlaylist(List<SpotifyPlaylist> lists) =>
        lists.FirstOrDefault(_ => Context.Uri?.ToLower().Contains(_?.Id?.ToLower() ?? "") ?? false) ?? new();
}

public class PlayerContext
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;
}

public class TrackContext
{
    [JsonPropertyName("current_track")]
    public Track CurrentTrack { get; set; } = new Track();
}

public class Album
{
    public string name { get; set; } = string.Empty;
    public List<SpotifyImage> images { get; set; } = new List<SpotifyImage>();
}
public class Track
{
    public Album album { get; set; } = new Album();
    public List<Artist> artists { get; set; } = new List<Artist>();
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    public string Artist => artists?.FirstOrDefault()?.name ?? string.Empty;
}

public class Artist
{
    public string name { get; set; } = string.Empty;
}