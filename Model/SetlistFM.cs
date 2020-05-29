using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoveMusic
{

    public class SetlistResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("itemsPerPage")]
        public int PerPage { get; set; }

        [JsonPropertyName("setlist")]
        public List<Setlist> SetLists { get; set; } = new List<Setlist>();

    }

    public class Setlist
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("eventDate")]
        public string EventDate { get; set; }

        [JsonPropertyName("artist")]
        public SetlistArtist Artist { get; set; }

        [JsonPropertyName("venue")]
        public SetlistVenue Venue { get; set; }

        [JsonPropertyName("tour")]
        public SetlistTour Tour { get; set; }

        [JsonPropertyName("sets")]
        public SetlistSets Sets { get; set; }
    }

    public class SetlistSets
    {
        [JsonPropertyName("set")]
        public List<SetlistSet> Sets { get; set; } = new List<SetlistSet>();
    }

    public class SetlistSet
    {
        [JsonPropertyName("encore")]
        public int? Encore { get; set; }

        [JsonPropertyName("song")]
        public List<SetlistSong> Songs { get; set; }
    }

    public class SetlistCover
    {

    }

    public class SetlistSong
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("tape")]
        public bool Tape { get; set; }
    }

    public class SetlistArtist
    {
        [JsonPropertyName("mbid")]
        public string Mbid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class SetlistVenue
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("city")]
        public SetlistCity City { get; set; }

    }

    public class SetlistTour
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class SetlistCountry
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class SetlistCity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public SetlistCountry Country { get; set; }
    }

}