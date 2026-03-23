var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Register only the custom CORS middleware (no AddCors)
//builder.UseMiddleware<CorsMiddleware>();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();

// Custom CORS middleware for Azure Functions Isolated
public class CorsMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();
        if(httpContext != null)
        {
            var origin = httpContext.Request.Headers["Origin"].FirstOrDefault();
            if(origin == "https://localhost:5003")
            {
                httpContext.Response.Headers["Access-Control-Allow-Origin"] = origin;
                httpContext.Response.Headers["Access-Control-Allow-Headers"] = "*";
                httpContext.Response.Headers["Access-Control-Allow-Methods"] = "*";
            }
        }
        await next(context);
    }
}
