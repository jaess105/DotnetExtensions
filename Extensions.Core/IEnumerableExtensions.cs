namespace Extensions.Core;

// ReSharper disable once InconsistentNaming
public static class IEnumerableExtensions
{
    /// <summary>
    /// Enumerates a sequence and returns each element paired with its index.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="self">The source sequence to enumerate.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of tuples, where each tuple contains the index and the element.
    /// </returns>
    public static IEnumerable<(int Index, T Element)> Enumerate<T>(this IEnumerable<T> self)
        => self.Select((el, i) => (i, el));
}