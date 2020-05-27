
using System.Text.Json.Serialization;

namespace LoveMusic
{
    public class SpotifyUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

}