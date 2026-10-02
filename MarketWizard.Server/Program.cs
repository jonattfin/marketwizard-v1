using MarketWizard.Server;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var repo = new Repository();

var api = app.MapGroup("/api");
api.MapGet("weatherforecast", repo.GetWeatherForecast) .WithName("GetWeatherForecast");

api.MapGet("indices", repo.GetIndices).WithName("GetIndices");
api.MapGet("top-news", repo.GetTopNews) .WithName("GetTopNews");
api.MapGet("sector-performance", repo.GetSectorPerformance) .WithName("GetSectorPerformance");

api.MapGet("top-gainers", repo.GetTopGainers) .WithName("GetTopGainers");
api.MapGet("top-losers", repo.GetTopLosers) .WithName("GetTopLosers");

api.MapGet("top-industries", repo.GetTopIndustries) .WithName("GetTopIndustries");
api.MapGet("worst-industries", repo.GetWorstIndustries) .WithName("GetWorstIndustries");

app.MapDefaultEndpoints();

app.UseFileServer();

await app.RunAsync();