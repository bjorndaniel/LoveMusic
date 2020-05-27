using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoveMusic
{
    public class SpotifyDevice
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_restricted")]
        public bool IsRestricted { get; set; }
    }

    public class DeviceResult
    {
        [JsonPropertyName("devices")]
        public List<SpotifyDevice> Devices { get; set; } = new List<SpotifyDevice>();
    }

    public class ActivateDevice
    {
        public List<string> device_ids { get; set; }
        public bool play { get; set; }
    }
}