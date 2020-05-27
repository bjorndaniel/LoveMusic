using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace LoveMusic
{
    public class SpotifyTrackSearchResult
    {
        [JsonPropertyName("tracks")]
        public SpotifyTracks SpotifyTracks { get; set; }
    }

    public class SpotifyTracks
    {
        [JsonPropertyName("items")]
        public IEnumerable<SpotifyTrack> Tracks { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }
    }
    public class SpotifyTrack
    {
        [JsonPropertyName("artists")]
        public IEnumerable<SpotifyArtist> Artists { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonIgnore]
        public bool NotFound { get; set; }
    }
    public class SpotifyArtist
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class SpotifyContext
    {
        [JsonPropertyName("context")]
        public PlayerContext Context { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("position")]
        public double Position { get; set; }

        [JsonPropertyName("paused")]
        public bool? Paused { get; set; }

        [JsonPropertyName("track_window")]
        public TrackContext TrackWindow { get; set; }
        public string Track => TrackWindow?.CurrentTrack?.Name;
        public string Artist => TrackWindow?.CurrentTrack?.Artist;
        public bool HasUri => !string.IsNullOrEmpty(Context?.Uri);
        public bool HasImage => TrackWindow?.CurrentTrack?.album?.images?.OrderByDescending(_ => _.Height).FirstOrDefault()?.Url != null;
        public string Image => TrackWindow?.CurrentTrack?.album?.images?.OrderByDescending(_ => _.Height).FirstOrDefault()?.Url;
        public bool IsPlayingList(string id)
        {
            if (!string.IsNullOrEmpty(Context?.Uri) && Context.Uri.Contains(id))
            {
                return true;
            }
            return false;
        }
        public SpotifyPlaylist FindPlaylist(List<SpotifyPlaylist> lists) =>
            lists.FirstOrDefault(_ => Context.Uri?.ToLower().Contains(_?.Id?.ToLower()) ?? false);
    }

    public class PlayerContext
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public class TrackContext
    {
        [JsonPropertyName("current_track")]
        public Track CurrentTrack { get; set; }
    }

    public class Album
    {
        public string name { get; set; }
        public List<SpotifyImage> images { get; set; } = new List<SpotifyImage>();
    }
    public class Track
    {
        public Album album { get; set; }
        public List<Artist> artists { get; set; } = new List<Artist>();
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public string Artist => artists?.FirstOrDefault()?.name;

    }

    public class Artist
    {
        public string name { get; set; }
    }
}