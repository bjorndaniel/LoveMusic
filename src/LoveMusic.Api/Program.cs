var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Register only the custom CORS middleware (no AddCors)
builder.UseMiddleware<CorsMiddleware>();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();

// Custom CORS middleware for Azure Functions Isolated
public class CorsMiddleware : IFunctionsWorkerMiddleware
{
    private static readonly HashSet<string> AllowedOrigins = new()
    {
        "https://lovemusic.bjorndaniel.se",
        "https://localhost:5003"
        // Add preview URLs if needed
        // "https://<your-app-name>.<hash>.azurestaticapps.net"
    };

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();
        if(httpContext != null)
        {
            var origin = httpContext.Request.Headers["Origin"].FirstOrDefault();
            if(string.IsNullOrEmpty(origin) || AllowedOrigins.Contains(origin) is false)
            {
                httpContext.Response.StatusCode = 404;
                await httpContext.Response.CompleteAsync();
                return;
            }
            httpContext.Response.Headers["Access-Control-Allow-Origin"] = origin;
            httpContext.Response.Headers["Access-Control-Allow-Headers"] = "*";
            httpContext.Response.Headers["Access-Control-Allow-Methods"] = "*";
        }
        await next(context);
    }
}
