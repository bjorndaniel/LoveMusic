using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoveMusic
{
    public class SpotifyTrackSearchResult
    {
        [JsonProperty("tracks")]
        public SpotifyTracks SpotifyTracks { get; set; }
    }

    public class SpotifyTracks
    {
        [JsonProperty("items")]
        public IEnumerable<SpotifyTrack> Tracks { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }
    public class SpotifyTrack
    {
        [JsonProperty("artists")]
        public IEnumerable<SpotifyArtist> Artists { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonIgnore]
        public bool NotFound { get; set; }
    }
    public class SpotifyArtist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class SpotifyContext
    {
        public long duration { get; set; }
        public long position { get; set; }
        public bool paused { get; set; }

        public TrackContext track_window { get; set; }
    }

    public class TrackContext
    {
        public Track current_track { get; set; }
    }

    public class Album
    {
        public string name { get; set; }
    }
    public class Track
    {
        public Album album { get; set; }
        public List<Artist> artists { get; set; } = new List<Artist>();
        public string name { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
    }
}