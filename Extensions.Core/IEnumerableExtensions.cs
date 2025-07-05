namespace Extensions.Core;

// ReSharper disable once InconsistentNaming
public static class IEnumerableExtensions
{
    public static IEnumerable<(int i, T el)> Enumerate<T>(this IEnumerable<T> self)
        => self.Select((el, i) => (i, el));
}