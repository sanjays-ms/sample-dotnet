using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var app = builder.Build();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/nuget-test", (ILogger<Program> logger) =>
{
    // Demonstrate Microsoft.Extensions.Logging functionality
    logger.LogInformation("Testing Newtonsoft.Json and Microsoft.Extensions.Logging packages");

    // Demonstrate Newtonsoft.Json functionality
    var testData = new
    {
        Message = "Both NuGet packages are working correctly!",
        Timestamp = DateTime.UtcNow,
        TargetFramework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
        Packages = new
        {
            NewtonsoftJson = "13.0.3",
            MicrosoftExtensionsLogging = "10.0.0"
        }
    };

    // Serialize to JSON string
    string jsonString = JsonConvert.SerializeObject(testData, Formatting.Indented);

    // Parse back to JObject to demonstrate JSON manipulation
    JObject parsedJson = JObject.Parse(jsonString);
    parsedJson["AdditionalInfo"] = "This endpoint tests NuGet package restore for Azure Pipelines";

    logger.LogInformation("Successfully tested both NuGet packages");

    return Results.Ok(parsedJson);
})
.WithName("NuGetPackageTest");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
