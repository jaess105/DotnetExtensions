// ReSharper disable InconsistentNaming

namespace Extensions.Core.Tests;

// ReSharper disable once InconsistentNaming
[TestFixture]
public class IDictionaryExtensionsTests
{
    #region GetOrInsert

    [Test]
    public void GetOrInsert_ExistingKey_ReturnsExistingValue()
    {
        Dictionary<string, int> dict = new()
        {
            ["a"] = 42
        };

        int result = dict.GetOrInsert("a", () => 99);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(42));
            Assert.That(dict, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void GetOrInsert_MissingKey_InsertsAndReturnsDefault()
    {
        Dictionary<string, int> dict = [];

        int result = dict.GetOrInsert("missing", () => 5);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(5));
            Assert.That(dict, Has.Count.EqualTo(1));
        });
        Assert.That(dict["missing"], Is.EqualTo(5));
    }

    [Test]
    public void GetOrInsert_DefaultFactoryCalledOnlyOnce()
    {
        var dict = new Dictionary<string, int>();
        var callCount = 0;

        dict.GetOrInsert("x", () =>
        {
            callCount++;
            return 100;
        });

        Assert.That(callCount, Is.EqualTo(1));
    }

    #endregion

    #region AddToOrInsert with new()

    private static readonly string[] AddToOrInsert_WithNewCollection_CreatesAndAdds_Expected = ["red"];

    [Test]
    public void AddToOrInsert_WithNewCollection_CreatesAndAdds()
    {
        Dictionary<string, List<string>> dict = [];

        List<string> result = dict.AddToOrInsert("colors", "red");

        Assert.That(dict.ContainsKey("colors"), Is.True);
        CollectionAssert.AreEquivalent(AddToOrInsert_WithNewCollection_CreatesAndAdds_Expected, result);
    }


    private static readonly string[] AddToOrInsert_WithNewCollection_AddsToExistingList_Expected = ["apple", "banana"];

    [Test]
    public void AddToOrInsert_WithNewCollection_AddsToExistingList()
    {
        Dictionary<string, List<string>> dict = new()
        {
            ["fruits"] = ["apple"]
        };

        List<string> result = dict.AddToOrInsert("fruits", "banana");

        CollectionAssert.AreEquivalent(AddToOrInsert_WithNewCollection_AddsToExistingList_Expected, result);
    }

    #endregion

    #region AddToOrInsert with factory

    [Test]
    public void AddToOrInsert_WithFactory_UsesCustomCollectionType()
    {
        Dictionary<int, HashSet<int>> dict = [];

        HashSet<int> result = dict.AddToOrInsert(1, 42, () => []);

        Assert.That(dict.ContainsKey(1), Is.True);
        CollectionAssert.Contains(result, 42);
        Assert.That(result, Is.InstanceOf<HashSet<int>>());
    }

    private static readonly int[] AddToOrInsert_WithFactory_DoesNotCallFactoryIfExists_Expected = [10, 20];

    [Test]
    public void AddToOrInsert_WithFactory_DoesNotCallFactoryIfExists()
    {
        Dictionary<int, List<int>> dict = new()
        {
            [1] = [10]
        };

        var factoryCalled = false;

        List<int> result = dict.AddToOrInsert(1, 20, () =>
        {
            factoryCalled = true;
            return [];
        });

        Assert.That(factoryCalled, Is.False);
        CollectionAssert.AreEquivalent(AddToOrInsert_WithFactory_DoesNotCallFactoryIfExists_Expected, result);
    }

    #endregion
}