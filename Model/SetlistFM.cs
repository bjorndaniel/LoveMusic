namespace LoveMusic;

public class SetlistResponse
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("itemsPerPage")]
    public int PerPage { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("setlist")]
    public List<Setlist> SetLists { get; set; } = new List<Setlist>();
}

public class Setlist
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("eventDate")]
    public string EventDate { get; set; } = string.Empty;

    [JsonPropertyName("artist")]
    public SetlistArtist Artist { get; set; } = new SetlistArtist();

    [JsonPropertyName("venue")]
    public SetlistVenue Venue { get; set; } = new SetlistVenue();

    [JsonPropertyName("tour")]
    public SetlistTour Tour { get; set; } = new SetlistTour();

    [JsonPropertyName("sets")]
    public SetlistSets Sets { get; set; } = new SetlistSets();

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    public List<SetlistSong> Songs => Sets.Sets.SelectMany(_ => _.Songs).ToList();
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
    public List<SetlistSong> Songs { get; set; } = new List<SetlistSong>();
}

public class SetlistCover
{
}

public class SetlistSong
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("tape")]
    public bool Tape { get; set; }
}

public class SetlistArtist
{
    [JsonPropertyName("mbid")]
    public string Mbid { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public class SetlistVenue
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("city")]
    public SetlistCity City { get; set; } = new SetlistCity();
}

public class SetlistTour
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class SetlistCountry
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class SetlistCity
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public SetlistCountry Country { get; set; } = new SetlistCountry();
}