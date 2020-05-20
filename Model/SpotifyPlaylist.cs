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

    public class SpotifyPlaylist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}