using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DemoAPI.Tests;

/// <summary>
/// Test class for JSON operations using Newtonsoft.Json
/// Tests serialization, deserialization, and JSON manipulation
/// </summary>
[TestClass]
public class JsonOperationsTests
{
    #region Serialization Tests

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("Serialization")]
    [Description("Verify object can be serialized to JSON string")]
    public void SerializeObject_WhenValidObject_ShouldReturnJsonString()
    {
        // Arrange
        var testData = new { Name = "Test", Value = 123 };

        // Act
        string json = JsonConvert.SerializeObject(testData);

        // Assert
        Assert.IsNotNull(json);
        Assert.IsTrue(json.Contains("Test"));
        Assert.IsTrue(json.Contains("123"));
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("Serialization")]
    [Description("Verify object can be serialized with indented formatting")]
    public void SerializeObject_WithIndentedFormatting_ShouldContainNewlines()
    {
        // Arrange
        var testData = new { Name = "Test", Value = 123 };

        // Act
        string json = JsonConvert.SerializeObject(testData, Formatting.Indented);

        // Assert
        Assert.IsTrue(json.Contains(Environment.NewLine) || json.Contains("\n"));
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("Serialization")]
    [Description("Verify complex nested object can be serialized")]
    public void SerializeObject_WithNestedObject_ShouldSerializeAll()
    {
        // Arrange
        var testData = new
        {
            Message = "Test",
            Packages = new
            {
                NewtonsoftJson = "13.0.3",
                MicrosoftExtensionsLogging = "10.0.0"
            }
        };

        // Act
        string json = JsonConvert.SerializeObject(testData);

        // Assert
        Assert.IsTrue(json.Contains("13.0.3"));
        Assert.IsTrue(json.Contains("10.0.0"));
    }

    #endregion

    #region Deserialization Tests

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("Deserialization")]
    [Description("Verify JSON string can be deserialized to dynamic object")]
    public void DeserializeObject_WhenValidJson_ShouldReturnObject()
    {
        // Arrange
        string json = @"{""Name"":""Test"",""Value"":123}";

        // Act
        dynamic? result = JsonConvert.DeserializeObject(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", (string)result!.Name);
        Assert.AreEqual(123, (int)result.Value);
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("Deserialization")]
    [Description("Verify JSON can be deserialized to typed object")]
    public void DeserializeObject_ToTypedObject_ShouldMapProperties()
    {
        // Arrange
        string json = @"{""Name"":""TestName"",""Age"":30}";

        // Act
        var result = JsonConvert.DeserializeObject<TestPerson>(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("TestName", result.Name);
        Assert.AreEqual(30, result.Age);
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("Deserialization")]
    [ExpectedException(typeof(JsonReaderException))]
    [Description("Verify invalid JSON throws JsonReaderException")]
    public void DeserializeObject_WhenInvalidJson_ShouldThrowException()
    {
        // Arrange
        string invalidJson = @"{Name:Test}"; // Missing quotes

        // Act & Assert - Should throw JsonReaderException
        JsonConvert.DeserializeObject(invalidJson);
    }

    #endregion

    #region JObject Tests

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("JObject")]
    [Description("Verify JObject.Parse can parse valid JSON")]
    public void JObjectParse_WhenValidJson_ShouldReturnJObject()
    {
        // Arrange
        string json = @"{""Message"":""Hello"",""Count"":5}";

        // Act
        JObject result = JObject.Parse(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Hello", result["Message"]?.ToString());
        Assert.AreEqual(5, result["Count"]?.Value<int>());
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("JObject")]
    [Description("Verify JObject allows adding new properties")]
    public void JObject_WhenAddingProperty_ShouldContainNewProperty()
    {
        // Arrange
        string json = @"{""Original"":""Value""}";
        JObject jObject = JObject.Parse(json);

        // Act
        jObject["NewProperty"] = "NewValue";

        // Assert
        Assert.AreEqual("NewValue", jObject["NewProperty"]?.ToString());
        Assert.AreEqual("Value", jObject["Original"]?.ToString());
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("JObject")]
    [Description("Verify JObject allows modifying existing properties")]
    public void JObject_WhenModifyingProperty_ShouldUpdateValue()
    {
        // Arrange
        string json = @"{""Status"":""Pending""}";
        JObject jObject = JObject.Parse(json);

        // Act
        jObject["Status"] = "Complete";

        // Assert
        Assert.AreEqual("Complete", jObject["Status"]?.ToString());
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("JObject")]
    [Description("Verify JObject can be converted back to string")]
    public void JObject_ToString_ShouldReturnValidJson()
    {
        // Arrange
        JObject jObject = new JObject
        {
            ["Name"] = "Test",
            ["Value"] = 100
        };

        // Act
        string json = jObject.ToString();

        // Assert
        Assert.IsTrue(json.Contains("Test"));
        Assert.IsTrue(json.Contains("100"));
    }

    #endregion

    #region Edge Cases

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("EdgeCase")]
    [Description("Verify empty object serialization")]
    public void SerializeObject_WhenEmptyObject_ShouldReturnEmptyJson()
    {
        // Act
        string json = JsonConvert.SerializeObject(new { });

        // Assert
        Assert.AreEqual("{}", json);
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("EdgeCase")]
    [Description("Verify null handling in serialization")]
    public void SerializeObject_WithNullProperty_ShouldIncludeNull()
    {
        // Arrange
        var testData = new { Name = (string?)null, Value = 123 };

        // Act
        string json = JsonConvert.SerializeObject(testData);

        // Assert
        Assert.IsTrue(json.Contains("null"));
    }

    [TestMethod]
    [TestCategory("Unit")]
    [TestCategory("EdgeCase")]
    [Description("Verify array serialization")]
    public void SerializeObject_WhenArray_ShouldSerializeToJsonArray()
    {
        // Arrange
        var testArray = new[] { "Freezing", "Bracing", "Chilly" };

        // Act
        string json = JsonConvert.SerializeObject(testArray);

        // Assert
        Assert.IsTrue(json.StartsWith("["));
        Assert.IsTrue(json.EndsWith("]"));
        Assert.IsTrue(json.Contains("Freezing"));
    }

    #endregion
}

/// <summary>
/// Test helper class for typed deserialization
/// </summary>
public class TestPerson
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
