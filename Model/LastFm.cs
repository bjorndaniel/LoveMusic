using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoveMusic
{

    public class LastFmCount
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

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

    ///Seems like System.Text.Json can't handle int64 correctly as of now
    public class LastFmAttributes
    {
        [JsonPropertyName("page")]
        public string PageString { get; set; }

        public int Page => int.TryParse(PageString, out var t) ? t : 0;

        [JsonPropertyName("total")]
        public string Total { get; set; }

        public int TotalTracks => int.TryParse(Total, out var t) ? t : 0;

        [JsonPropertyName("totalPages")]
        public string TotalPagesString { get; set; }

        public int TotalPages => int.TryParse(TotalPagesString, out var t) ? t : 0;
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