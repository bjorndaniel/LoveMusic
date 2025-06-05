var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("app");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SpotifyService>();
builder.Services.AddScoped<LastFmService>();
builder.Services.AddScoped<SetlistFmService>();
builder.Services.AddBlazoredToast();
await builder.Build().RunAsync();
