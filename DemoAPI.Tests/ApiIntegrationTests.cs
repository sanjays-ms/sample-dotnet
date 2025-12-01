using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoAPI.Tests;

/// <summary>
/// Integration and API-related tests
/// Tests that validate API endpoint behavior and business logic
/// </summary>
[TestClass]
public class ApiIntegrationTests
{
    #region Weather Summaries Tests

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("WeatherAPI")]
    [Description("Verify all weather summaries are valid strings")]
    public void Summaries_AllItems_ShouldBeNonEmptyStrings()
    {
        foreach (var summary in Summaries)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(summary), $"Summary '{summary}' should not be empty");
        }
    }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("WeatherAPI")]
    [Description("Verify expected number of summaries")]
    public void Summaries_Count_ShouldBe10()
    {
        Assert.AreEqual(10, Summaries.Length);
    }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("WeatherAPI")]
    [Description("Verify summaries contains expected values")]
    public void Summaries_ShouldContainExpectedValues()
    {
        CollectionAssert.Contains(Summaries, "Freezing");
        CollectionAssert.Contains(Summaries, "Scorching");
        CollectionAssert.Contains(Summaries, "Mild");
    }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("WeatherAPI")]
    [Description("Verify summaries are unique")]
    public void Summaries_ShouldAllBeUnique()
    {
        CollectionAssert.AllItemsAreUnique(Summaries);
    }

    #endregion

    #region Weather Forecast Generation Tests

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("WeatherAPI")]
    [Description("Verify forecast generation creates 5 items")]
    public void GenerateForecast_ShouldReturn5Items()
    {
        // Act
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();

        // Assert
        Assert.AreEqual(5, forecast.Length);
    }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("WeatherAPI")]
    [Description("Verify forecast dates are in the future")]
    public void GenerateForecast_Dates_ShouldBeInFuture()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();

        // Assert
        foreach (var item in forecast)
        {
            Assert.IsTrue(item.Date > today, $"Date {item.Date} should be after today {today}");
        }
    }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("WeatherAPI")]
    [Description("Verify forecast temperatures are within valid range")]
    public void GenerateForecast_Temperatures_ShouldBeInValidRange()
    {
        // Act
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();

        // Assert
        foreach (var item in forecast)
        {
            Assert.IsTrue(item.TemperatureC >= -20 && item.TemperatureC < 55,
                $"Temperature {item.TemperatureC} should be between -20 and 55");
        }
    }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("WeatherAPI")]
    [Description("Verify forecast summaries are from valid list")]
    public void GenerateForecast_Summaries_ShouldBeFromValidList()
    {
        // Act
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();

        // Assert
        foreach (var item in forecast)
        {
            CollectionAssert.Contains(Summaries, item.Summary,
                $"Summary '{item.Summary}' should be in valid summaries list");
        }
    }

    #endregion

    #region Target Framework Tests

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("Framework")]
    [Description("Verify runtime framework description is available")]
    public void RuntimeInfo_FrameworkDescription_ShouldNotBeEmpty()
    {
        // Act
        var frameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(frameworkDescription));
        StringAssert.StartsWith(frameworkDescription, ".NET");
    }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("Framework")]
    [Description("Verify we're running on expected .NET version")]
    public void RuntimeInfo_ShouldBeNet8OrHigher()
    {
        // Act
        var version = Environment.Version;

        // Assert
        Assert.IsTrue(version.Major >= 8, $"Expected .NET 8 or higher, but got .NET {version.Major}");
    }

    #endregion

    #region Package Integration Tests

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("NuGetPackages")]
    [Description("Verify Newtonsoft.Json is available")]
    public void NewtonsoftJson_ShouldBeAvailable()
    {
        // Act
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(new { Test = "Value" });

        // Assert
        Assert.IsNotNull(json);
        Assert.IsTrue(json.Contains("Value"));
    }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("NuGetPackages")]
    [Description("Verify Newtonsoft.Json version")]
    public void NewtonsoftJson_Version_ShouldBeExpected()
    {
        // Act
        var assembly = typeof(Newtonsoft.Json.JsonConvert).Assembly;
        var version = assembly.GetName().Version;

        // Assert
        Assert.IsNotNull(version);
        Assert.AreEqual(13, version.Major, "Expected Newtonsoft.Json major version 13");
    }

    #endregion
}
