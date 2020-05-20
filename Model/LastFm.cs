using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoveMusic
{

    public abstract class LastFmTracksResult
    {
        public LastFmTracksList LastFmTracksList { get; set; }
    }

    public class LastFmLovedTracksResult : LastFmTracksResult
    {
        [JsonProperty("lovedtracks")]
        public new LastFmTracksList LastFmTracksList { get; set; }
    }

    public class LastFmTopTracksResult : LastFmTracksResult
    {
        [JsonProperty("toptracks")]
        public new LastFmTracksList LastFmTracksList { get; set; }
    }
    public class LastFmRecentTracksResult : LastFmTracksResult
    {
        [JsonProperty("recenttracks")]
        public new LastFmTracksList LastFmTracksList { get; set; }
    }

    public class LastFmTracksList
    {
        [JsonProperty("@attr")]
        public LastFmAttributes Attributes { get; set; }

        [JsonProperty("track")]
        public List<LastFmTrack> Tracks { get; set; }
    }

    public class LastFmAttributes
    {
        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("total")]
        public long TotalTracks { get; set; }

        [JsonProperty("totalPages")]
        public long TotalPages { get; set; }
    }

    public class LastFmTrack
    {
        [JsonProperty("artist")]
        public LastFmArtist Artist { get; set; }

        [JsonProperty("image")]
        public LastFmImage[] Image { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string ImageString => Image?[2]?.Text?.ToString() ?? string.Empty;
    }

    public class LastFmArtist
    {
        [JsonProperty("name")]
        public string Name { get; set; }

    }

    public partial class LastFmImage
    {
        [JsonProperty("#text")]
        public Uri Text { get; set; }
    }
}