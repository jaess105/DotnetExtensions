namespace Extensions.Core;

// ReSharper disable once InconsistentNaming
public static class IEnumerableExtensions
{
    public static IEnumerable<(int Index, T Element)> Enumerate<T>(this IEnumerable<T> self)
        => self.Select((el, i) => (i, el));
}