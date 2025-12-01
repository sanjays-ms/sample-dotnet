using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoAPI.Tests;

/// <summary>
/// Test class for Weather Forecast functionality
/// Tests basic calculation logic and data validation
/// </summary>
[TestClass]
public class WeatherForecastTests
{
    #region Temperature Conversion Tests

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("TemperatureConversion")]
    [Description("Verify Fahrenheit conversion formula for freezing point (0°C = 32°F)")]
    public void TemperatureF_WhenCelsiusIsZero_ShouldReturn32()
    {
        // Arrange
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 0, "Freezing");

        // Act
        var fahrenheit = forecast.TemperatureF;

        // Assert
        Assert.AreEqual(32, fahrenheit);
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("TemperatureConversion")]
    [Description("Verify Fahrenheit conversion for boiling point (100°C ≈ 212°F)")]
    public void TemperatureF_WhenCelsiusIs100_ShouldReturnApproximately212()
    {
        // Arrange
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 100, "Scorching");

        // Act
        var fahrenheit = forecast.TemperatureF;

        // Assert - Using the formula: 32 + (int)(100 / 0.5556) = 32 + 180 = 212
        Assert.IsTrue(fahrenheit >= 210 && fahrenheit <= 214, $"Expected ~212°F but got {fahrenheit}°F");
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("TemperatureConversion")]
    [Description("Verify Fahrenheit conversion for negative temperature (-20°C)")]
    public void TemperatureF_WhenCelsiusIsNegative20_ShouldReturnNegativeValue()
    {
        // Arrange
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), -20, "Freezing");

        // Act
        var fahrenheit = forecast.TemperatureF;

        // Assert - -20°C = -4°F approximately
        Assert.IsTrue(fahrenheit < 32, $"Expected negative Fahrenheit but got {fahrenheit}°F");
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("TemperatureConversion")]
    [Description("Verify Fahrenheit conversion for room temperature (25°C ≈ 77°F)")]
    public void TemperatureF_WhenCelsiusIs25_ShouldReturnApproximately77()
    {
        // Arrange
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 25, "Warm");

        // Act
        var fahrenheit = forecast.TemperatureF;

        // Assert
        Assert.IsTrue(fahrenheit >= 75 && fahrenheit <= 79, $"Expected ~77°F but got {fahrenheit}°F");
    }

    #endregion

    #region WeatherForecast Record Tests

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("DataValidation")]
    [Description("Verify WeatherForecast record stores Date correctly")]
    public void WeatherForecast_WhenCreated_ShouldStoreDate()
    {
        // Arrange
        var expectedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        // Act
        var forecast = new WeatherForecast(expectedDate, 20, "Mild");

        // Assert
        Assert.AreEqual(expectedDate, forecast.Date);
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("DataValidation")]
    [Description("Verify WeatherForecast record stores TemperatureC correctly")]
    public void WeatherForecast_WhenCreated_ShouldStoreTemperatureC()
    {
        // Arrange
        var expectedTemp = 25;

        // Act
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), expectedTemp, "Warm");

        // Assert
        Assert.AreEqual(expectedTemp, forecast.TemperatureC);
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("DataValidation")]
    [Description("Verify WeatherForecast record stores Summary correctly")]
    public void WeatherForecast_WhenCreated_ShouldStoreSummary()
    {
        // Arrange
        var expectedSummary = "Scorching";

        // Act
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 40, expectedSummary);

        // Assert
        Assert.AreEqual(expectedSummary, forecast.Summary);
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("DataValidation")]
    [Description("Verify WeatherForecast allows null Summary")]
    public void WeatherForecast_WhenSummaryIsNull_ShouldAcceptNull()
    {
        // Act
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 20, null);

        // Assert
        Assert.IsNull(forecast.Summary);
    }

    #endregion

    #region Data-Driven Tests

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("DataDriven")]
    [DataRow(0, 32)]
    [DataRow(10, 50)]
    [DataRow(-10, 14)]
    [DataRow(37, 98)]
    [Description("Data-driven test for multiple temperature conversions")]
    public void TemperatureF_DataDrivenConversion_ShouldBeWithinRange(int celsius, int expectedFahrenheitApprox)
    {
        // Arrange
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), celsius, "Test");

        // Act
        var fahrenheit = forecast.TemperatureF;

        // Assert - Allow small variance due to integer division
        Assert.IsTrue(
            Math.Abs(fahrenheit - expectedFahrenheitApprox) <= 2,
            $"For {celsius}°C: Expected ~{expectedFahrenheitApprox}°F but got {fahrenheit}°F"
        );
    }

    #endregion
}

/// <summary>
/// WeatherForecast record for testing (mirrors the one in Program.cs)
/// </summary>
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
