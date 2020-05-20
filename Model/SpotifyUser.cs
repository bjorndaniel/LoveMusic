using Newtonsoft.Json;

namespace LoveMusic
{
    public class SpotifyUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

}