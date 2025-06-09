namespace LoveMusic;

public class SpotifyDevice
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

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
    public List<string> device_ids { get; set; } = new List<string>();
    public bool play { get; set; }
}