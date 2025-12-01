using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoAPI.Tests;

/// <summary>
/// Test class demonstrating various VSTest features for testing VSTest@3 task
/// Includes parallel tests, async tests, timeouts, and test lifecycle
/// </summary>
[TestClass]
public class VSTestFeaturesTests
{
    private static int _classInitializeCount = 0;
    private int _testInitializeCount = 0;

    #region Test Lifecycle Methods

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _classInitializeCount++;
        context.WriteLine($"ClassInitialize executed. Count: {_classInitializeCount}");
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        // Cleanup resources used by the class
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _testInitializeCount++;
    }

    [TestCleanup]
    public void TestCleanup()
    {
        // Cleanup after each test
    }

    #endregion

    #region Basic Tests

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("BasicTest")]
    [Description("Simple passing test")]
    public void SimpleTest_ShouldPass()
    {
        Assert.IsTrue(true);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("BasicTest")]
    [Priority(1)]
    [Description("Test with Priority attribute")]
    public void PriorityTest_WithPriority1_ShouldPass()
    {
        Assert.AreEqual(1, 1);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("BasicTest")]
    [Owner("TestAuthor")]
    [Description("Test with Owner attribute")]
    public void OwnerTest_WithOwnerAttribute_ShouldPass()
    {
        Assert.IsNotNull("test");
    }

    #endregion

    #region Async Tests

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("AsyncTest")]
    [Description("Async test method that completes successfully")]
    public async Task AsyncTest_WhenAwaited_ShouldComplete()
    {
        // Arrange
        var expectedResult = 42;

        // Act
        var result = await Task.FromResult(expectedResult);

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("AsyncTest")]
    [Description("Async test with delay")]
    public async Task AsyncTest_WithDelay_ShouldComplete()
    {
        // Act
        await Task.Delay(100);

        // Assert
        Assert.IsTrue(true);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("AsyncTest")]
    [Description("Async test calling async method")]
    public async Task AsyncTest_CallingAsyncMethod_ShouldReturnExpectedValue()
    {
        // Arrange & Act
        var result = await GetValueAsync();

        // Assert
        Assert.AreEqual("async result", result);
    }

    private static async Task<string> GetValueAsync()
    {
        await Task.Delay(50);
        return "async result";
    }

    #endregion

    #region Timeout Tests

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("TimeoutTest")]
    [Timeout(5000)] // 5 second timeout
    [Description("Test with timeout that should pass")]
    public void TimeoutTest_CompletesQuickly_ShouldPass()
    {
        Thread.Sleep(100); // Sleep for 100ms - well under timeout
        Assert.IsTrue(true);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("TimeoutTest")]
    [Timeout(TestTimeout.Infinite)]
    [Description("Test with infinite timeout")]
    public void TimeoutTest_InfiniteTimeout_ShouldPass()
    {
        Assert.IsTrue(true);
    }

    #endregion

    #region Collection Assert Tests

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("CollectionTest")]
    [Description("CollectionAssert.AreEqual for ordered comparison")]
    public void CollectionAssert_AreEqual_ShouldCompareOrdered()
    {
        // Arrange
        var expected = new[] { 1, 2, 3 };
        var actual = new[] { 1, 2, 3 };

        // Assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("CollectionTest")]
    [Description("CollectionAssert.AreEquivalent for unordered comparison")]
    public void CollectionAssert_AreEquivalent_ShouldIgnoreOrder()
    {
        // Arrange
        var expected = new[] { 3, 1, 2 };
        var actual = new[] { 1, 2, 3 };

        // Assert
        CollectionAssert.AreEquivalent(expected, actual);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("CollectionTest")]
    [Description("CollectionAssert.Contains for membership")]
    public void CollectionAssert_Contains_ShouldFindElement()
    {
        // Arrange
        var collection = new[] { "Freezing", "Bracing", "Chilly", "Cool" };

        // Assert
        CollectionAssert.Contains(collection, "Chilly");
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("CollectionTest")]
    [Description("CollectionAssert.AllItemsAreNotNull")]
    public void CollectionAssert_AllItemsAreNotNull_ShouldPass()
    {
        // Arrange
        var collection = new[] { "One", "Two", "Three" };

        // Assert
        CollectionAssert.AllItemsAreNotNull(collection);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("CollectionTest")]
    [Description("CollectionAssert.AllItemsAreUnique")]
    public void CollectionAssert_AllItemsAreUnique_ShouldPass()
    {
        // Arrange
        var collection = new[] { 1, 2, 3, 4, 5 };

        // Assert
        CollectionAssert.AllItemsAreUnique(collection);
    }

    #endregion

    #region String Assert Tests

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("StringTest")]
    [Description("StringAssert.Contains")]
    public void StringAssert_Contains_ShouldFindSubstring()
    {
        // Arrange
        var fullString = "Weather Forecast API";

        // Assert
        StringAssert.Contains(fullString, "Forecast");
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("StringTest")]
    [Description("StringAssert.StartsWith")]
    public void StringAssert_StartsWith_ShouldMatchPrefix()
    {
        // Arrange
        var text = "net8.0;net9.0;net10.0";

        // Assert
        StringAssert.StartsWith(text, "net8.0");
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("StringTest")]
    [Description("StringAssert.EndsWith")]
    public void StringAssert_EndsWith_ShouldMatchSuffix()
    {
        // Arrange
        var text = "net8.0;net9.0;net10.0";

        // Assert
        StringAssert.EndsWith(text, "net10.0");
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("StringTest")]
    [Description("StringAssert.Matches with regex")]
    public void StringAssert_Matches_ShouldMatchRegex()
    {
        // Arrange
        var version = "net10.0";
        var pattern = new System.Text.RegularExpressions.Regex(@"^net\d+\.\d+$");

        // Assert
        StringAssert.Matches(version, pattern);
    }

    #endregion

    #region Exception Tests

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("ExceptionTest")]
    [ExpectedException(typeof(ArgumentNullException))]
    [Description("Test expecting ArgumentNullException")]
    public void ExceptionTest_WhenNullArgument_ShouldThrowArgumentNullException()
    {
        // Act - This should throw ArgumentNullException
        ThrowIfNull(null!);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("ExceptionTest")]
    [Description("Test using Assert.ThrowsException")]
    public void ExceptionTest_UsingAssertThrows_ShouldCatchException()
    {
        // Act & Assert
        var exception = Assert.ThrowsException<DivideByZeroException>(() =>
        {
            int x = 10;
            int y = 0;
            int result = x / y;
        });

        Assert.IsNotNull(exception);
    }

    private static void ThrowIfNull(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
    }

    #endregion

    #region Test Context Tests

    public TestContext? TestContext { get; set; }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("TestContext")]
    [Description("Test using TestContext for logging")]
    public void TestContext_WriteLine_ShouldLogMessage()
    {
        // Arrange & Act
        TestContext?.WriteLine("This message should appear in test output");
        TestContext?.WriteLine($"Test Name: {TestContext?.TestName}");

        // Assert
        Assert.IsNotNull(TestContext);
    }

    [TestMethod]
    [TestCategory("VSTestFeatures")]
    [TestCategory("TestContext")]
    [Description("Test accessing TestContext properties")]
    public void TestContext_Properties_ShouldBeAccessible()
    {
        // Assert
        Assert.IsNotNull(TestContext?.TestName);
        TestContext?.WriteLine($"Running test: {TestContext.TestName}");
        TestContext?.WriteLine($"Fully qualified name: {TestContext.FullyQualifiedTestClassName}");
    }

    #endregion
}
