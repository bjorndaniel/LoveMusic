using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoveMusic
{

    public abstract class LastFmTracksResult
    {
        public LastFmTracksList LastFmTracksList { get; set; }
    }

    public class LastFmLovedTracksResult : LastFmTracksResult
    {
        [JsonPropertyName("lovedtracks")]
        public new LastFmTracksList LastFmTracksList { get; set; }
    }

    public class LastFmTopTracksResult : LastFmTracksResult
    {
        [JsonPropertyName("toptracks")]
        public new LastFmTracksList LastFmTracksList { get; set; }
    }
    public class LastFmRecentTracksResult : LastFmTracksResult
    {
        [JsonPropertyName("recenttracks")]
        public new LastFmTracksList LastFmTracksList { get; set; }
    }

    public class LastFmTracksList
    {
        [JsonPropertyName("@attr")]
        public LastFmAttributes Attributes { get; set; }

        [JsonPropertyName("track")]
        public List<LastFmTrack> Tracks { get; set; }
    }

    public class LastFmAttributes
    {
        [JsonPropertyName("page")]
        public long Page { get; set; }

        [JsonPropertyName("total")]
        public long TotalTracks { get; set; }

        [JsonPropertyName("totalPages")]
        public long TotalPages { get; set; }
    }

    public class LastFmTrack
    {
        [JsonPropertyName("artist")]
        public LastFmArtist Artist { get; set; }

        [JsonPropertyName("image")]
        public LastFmImage[] Image { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string ImageString => Image?[2]?.Text?.ToString() ?? string.Empty;
    }

    public class LastFmArtist
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }

    public partial class LastFmImage
    {
        [JsonPropertyName("#text")]
        public Uri Text { get; set; }
    }
}