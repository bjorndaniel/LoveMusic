namespace LoveMusic.Api;

public class LastFMFunctions
{
    private readonly ILogger<LastFMFunctions> _logger;
    private static readonly HttpClient _httpClient = new HttpClient();

    public LastFMFunctions(ILogger<LastFMFunctions> logger)
    {
        _logger = logger;
    }

    [Function("count")]
    public async Task<IActionResult> Count([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lastfm/count")] HttpRequest req)
    {
        _logger.LogInformation("Processing last.fm count");
        var lastFmUser = req.Query["user"].FirstOrDefault();
        var method = req.Query["method"].FirstOrDefault();
        if(!string.IsNullOrWhiteSpace(lastFmUser) && !string.IsNullOrWhiteSpace(method))
        {
            var url = Environment.GetEnvironmentVariable("LastFmUrl");
            var key = Environment.GetEnvironmentVariable("LastFmKey");
            url = $"{url}?method={method}&user={lastFmUser}&api_key={key}&format=json&limit=1&page=1";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(data);
                int count = GetCount(method, json.RootElement);
                return new OkObjectResult(new { count });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from Last.fm");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }
        else
        {
            return new BadRequestObjectResult("Username and method are required");
        }

        static int GetCount(string method, JsonElement json)
        {
            switch(method)
            {
                case "user.gettoptracks":
                    return int.Parse(json.GetProperty("toptracks").GetProperty("@attr").GetProperty("total").GetString()!);
                case "user.getrecenttracks":
                    return int.Parse(json.GetProperty("recenttracks").GetProperty("@attr").GetProperty("total").GetString()!);
                case "user.getlovedtracks":
                    return int.Parse(json.GetProperty("lovedtracks").GetProperty("@attr").GetProperty("total").GetString()!);
                default:
                    return 0;
            }
        }
    }

    [Function("tracks")]
    public async Task<IActionResult> Tracks([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lastfm/tracks")] HttpRequest req)
    {
        _logger.LogInformation("Processing last.fm tracks");
        var lastFmUser = req.Query["user"].FirstOrDefault();
        var method = req.Query["method"].FirstOrDefault();
        var perpage = req.Query["perpage"].FirstOrDefault();
        var page = req.Query["page"].FirstOrDefault();

        if(!string.IsNullOrWhiteSpace(lastFmUser) && !string.IsNullOrWhiteSpace(method) && !string.IsNullOrWhiteSpace(perpage) && !string.IsNullOrWhiteSpace(page))
        {
            var url = Environment.GetEnvironmentVariable("LastFmUrl");
            var key = Environment.GetEnvironmentVariable("LastFmKey");
            url = $"{url}?method={method}&user={lastFmUser}&api_key={key}&format=json&limit={perpage}&page={page}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(data);
                return new OkObjectResult(json.RootElement);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from Last.fm");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }
        else
        {
            return new BadRequestObjectResult("Username and method are required");
        }
    }
}
