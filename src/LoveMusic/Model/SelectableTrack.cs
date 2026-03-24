namespace LoveMusic;

public class SelectableTrack
{
    public string Artist { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public SpotifyTrack? SpotifyResult { get; set; }
    public bool Selected { get; set; } = true;
    public bool NotFound => SpotifyResult?.NotFound ?? false;
    public List<SpotifyTrack> Alternatives { get; set; } = [];
    public bool ShowAlternatives { get; set; }
    public string SearchQuery { get; set; } = string.Empty;
}
