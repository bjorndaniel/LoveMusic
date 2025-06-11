namespace LoveMusic.Api;

public class SetlistFMFunctions
{
    private readonly ILogger<SetlistFMFunctions> _logger;
    private static readonly HttpClient _httpClient = new HttpClient();

    public SetlistFMFunctions(ILogger<SetlistFMFunctions> logger)
    {
        _logger = logger;
    }

    [Function("search")]
    public async Task<IActionResult> Search([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "setlistfm/search")] HttpRequest req)
    {
        _logger.LogInformation("Processing setlist.fm search");
        if(req.Query["artist"].FirstOrDefault() is not null)
        {
            var key = Environment.GetEnvironmentVariable("SetlistFmKey");
            var url = Environment.GetEnvironmentVariable("SetlistFmUrl");
            var path = $"{url}/rest/1.0/search/setlists?artistName={HttpUtility.UrlEncode(req.Query["artist"])}&cityName={HttpUtility.UrlEncode(req.Query["city"])}&p={req.Query["page"]}";
            if(string.IsNullOrWhiteSpace(req.Query["venue"].FirstOrDefault()) is false)
            {
                path = $"{path}&venueName={HttpUtility.UrlEncode(req.Query["venue"])}";
            }
            _logger.LogInformation(path);
            try
            {
                if(string.IsNullOrWhiteSpace(key) is false)
                {
                    _httpClient.DefaultRequestHeaders.Add("x-api-key", key);
                }
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.GetAsync(path);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(data);
                return new OkObjectResult(json.RootElement);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from Setlist.fm");
                return new ObjectResult(ex.Message) { StatusCode = 500 };
            }
        }
        else
        {
            return new BadRequestObjectResult("Searchtext is required");
        }
    }
}
