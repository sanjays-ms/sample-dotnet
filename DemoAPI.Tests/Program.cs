using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

// Simple console app to verify NuGet packages work
Console.WriteLine("=== NuGet Package Source Mapping Test ===");

// Test Newtonsoft.Json
var testData = new
{
    Message = "Newtonsoft.Json is working!",
    Timestamp = DateTime.UtcNow,
    Version = "13.0.4"
};

string json = JsonConvert.SerializeObject(testData, Formatting.Indented);
Console.WriteLine(json);

// Test Microsoft.Extensions.Logging
using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Microsoft.Extensions.Logging is working!");

Console.WriteLine("=== All packages loaded successfully ===");

public partial class Program { }
