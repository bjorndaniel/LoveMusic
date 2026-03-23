namespace LoveMusic;
public class LastFmCount
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
}

public abstract class LastFmTracksResult
{
    public LastFmTracksList LastFmTracksList { get; set; } = new LastFmTracksList();
}

public class LastFmLovedTracksResult : LastFmTracksResult
{
    [JsonPropertyName("lovedtracks")]
    public new LastFmTracksList LastFmTracksList { get; set; } = new LastFmTracksList();
}

public class LastFmTopTracksResult : LastFmTracksResult
{
    [JsonPropertyName("toptracks")]
    public new LastFmTracksList LastFmTracksList { get; set; } = new LastFmTracksList();
}

public class LastFmRecentTracksResult : LastFmTracksResult
{
    [JsonPropertyName("recenttracks")]
    public new LastFmTracksList LastFmTracksList { get; set; } = new LastFmTracksList();
}

public class LastFmTracksList
{
    [JsonPropertyName("@attr")]
    public LastFmAttributes Attributes { get; set; } = new LastFmAttributes();

    [JsonPropertyName("track")]
    public List<LastFmTrack> Tracks { get; set; } = new List<LastFmTrack>();
}

///Seems like System.Text.Json can't handle int64 correctly as of now
public class LastFmAttributes
{
    [JsonPropertyName("page")]
    public string PageString { get; set; } = string.Empty;

    public int Page => int.TryParse(PageString, out var t) ? t : 0;

    [JsonPropertyName("total")]
    public string Total { get; set; } = string.Empty;

    public int TotalTracks => int.TryParse(Total, out var t) ? t : 0;

    [JsonPropertyName("totalPages")]
    public string TotalPagesString { get; set; } = string.Empty;

    public int TotalPages => int.TryParse(TotalPagesString, out var t) ? t : 0;
}

public class LastFmTrack
{
    [JsonPropertyName("artist")]
    public LastFmArtist Artist { get; set; } = new LastFmArtist();

    [JsonPropertyName("image")]
    public LastFmImage[] Image { get; set; } = Array.Empty<LastFmImage>();

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public string ImageString => Image?.Length > 2 && Image[2]?.Text != null ? Image[2].Text.ToString() : string.Empty;
}

public class LastFmArtist
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public partial class LastFmImage
{
    [JsonPropertyName("#text")]
    public Uri Text { get; set; } = new Uri("http://localhost");
}