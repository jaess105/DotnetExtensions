using EnumerateTuple = (int Index, string Element);

namespace Extensions.Core.Tests;

// ReSharper disable once InconsistentNaming
[TestFixture]
public class IEnumerableExtensionsTests
{
    #region Enumerate

    [Test]
    public void Enumerate_ReturnsIndexElementPairs()
    {
        string[] input = ["a", "b", "c"];
        EnumerateTuple[] expected =
        [
            (0, "a"),
            (1, "b"),
            (2, "c")
        ];

        EnumerateTuple[] result = input.Enumerate().ToArray();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Enumerate_EmptyInput_ReturnsEmpty()
    {
        IEnumerable<string> input = [];

        IEnumerable<EnumerateTuple> result = input.Enumerate();

        Assert.That(result, Is.Empty);
    }

    #endregion

    private static IEnumerable<IEnumerable<int>?> GetEmptyEnumerables()
    {
        yield return null;
        yield return new List<int>();
        yield return [];
    }

    #region IsNullOrEmpty (IEnumerable)

    [TestCase(null)]
    [TestCaseSource(nameof(GetEmptyEnumerables))]
    public void IsNullOrEmpty_IEnumerable_ReturnsTrue(IEnumerable<int>? input)
    {
        Assert.That(input.IsNullOrEmpty(), Is.True);
    }

    [Test]
    public void IsNullOrEmpty_IEnumerable_WithElements_ReturnsFalse()
    {
        int[] input = [1, 2];

        bool result = input.IsNullOrEmpty();

        Assert.That(result, Is.False);
    }

    #endregion

    #region IsNotNullOrEmpty (IEnumerable)

    [TestCase(null)]
    [TestCaseSource(nameof(GetEmptyEnumerables))]
    public void IsNotNullOrEmpty_IEnumerable_ReturnsFalse(IEnumerable<int>? input)
    {
        Assert.That(input.IsNotNullOrEmpty(), Is.False);
    }

    [Test]
    public void IsNotNullOrEmpty_IEnumerable_WithElements_ReturnsTrue()
    {
        int[] input = [10];

        bool result = input.IsNotNullOrEmpty();

        Assert.That(result, Is.True);
    }

    #endregion

    #region IsNullOrEmpty (string)

    [TestCase(null, ExpectedResult = true)]
    [TestCase("", ExpectedResult = true)]
    [TestCase("   ", ExpectedResult = true)]
    [TestCase("Hello", ExpectedResult = false)]
    [TestCase("  Hello  ", ExpectedResult = false)]
    public bool IsNullOrEmpty_String_Tests(string? input)
    {
        return input.IsNullOrEmpty();
    }

    #endregion
}