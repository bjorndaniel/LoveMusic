using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoveMusic
{
    public class CreateSpotifyPlaylist
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("public")]
        public bool Public { get; set; }

    }
}

public class SpotifyPlaylist
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}

public class SpotifyPlayUri
{
    public string context_uri { get; set; }
}

public class SpotifyImage
{
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public class SpotifyPlaylistResult
{
    [JsonPropertyName("items")]
    public List<SpotifyPlaylist> Playlists { get; set; } = new List<SpotifyPlaylist>();
    [JsonPropertyName("total")]
    public int Total { get; set; }
}