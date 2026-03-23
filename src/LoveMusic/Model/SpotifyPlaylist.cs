namespace LoveMusic;

public class CreateSpotifyPlaylist
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("public")]
    public bool Public { get; set; }
}

public class SpotifyPlaylist
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;
}

public class SpotifyPlayUri
{
    public string context_uri { get; set; } = string.Empty;
}

public class SpotifyImage
{
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public class SpotifyPlaylistResult
{
    [JsonPropertyName("items")]
    public List<SpotifyPlaylist> Playlists { get; set; } = new List<SpotifyPlaylist>();
    [JsonPropertyName("total")]
    public int Total { get; set; }
}