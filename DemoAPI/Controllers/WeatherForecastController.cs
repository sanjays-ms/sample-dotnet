using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DemoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("json-test")]
        public IActionResult TestNewtonsoftJson()
        {
            // Demonstrate Microsoft.Extensions.Logging functionality
            _logger.LogInformation("Testing Newtonsoft.Json and Microsoft.Extensions.Logging packages");

            // Demonstrate Newtonsoft.Json functionality
            var testData = new
            {
                Message = "Newtonsoft.Json is working!",
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
            parsedJson["AdditionalInfo"] = "This endpoint tests NuGet package restore";

            _logger.LogInformation("Successfully tested both NuGet packages");

            return Ok(parsedJson);
        }
    }
}
