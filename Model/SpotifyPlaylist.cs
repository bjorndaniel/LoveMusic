using Newtonsoft.Json;

namespace LoveMusic
{
    public class CreateSpotifyPlaylist
    {
        [JsonProperty("name ")]
        public string Name { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

    }
}

public class SpotifyPlaylist
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }
}

public class SpotifyPlayUri
{
    public string context_uri { get; set; }
}

public class SpotifyPlaylistImage
{
    [JsonProperty("height")]
    public int Height { get; set; }

    [JsonProperty("width")]
    public int Width { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}