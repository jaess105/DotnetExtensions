using System.Diagnostics.CodeAnalysis;

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

    /// <summary>
    /// Determines whether an <see cref="IEnumerable{T}"/> is <c>null</c> or contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="enumerable">The sequence to check.</param>
    /// <returns><c>true</c> if the sequence is <c>null</c> or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
        => enumerable is null || !enumerable.Any();

    /// <summary>
    /// Determines whether a <see cref="string"/> is <c>null</c>, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns><c>true</c> if the string is <c>null</c>, empty, or white-space; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
        => str is null || string.IsNullOrWhiteSpace(str);

    /// <summary>
    /// Determines whether an <see cref="IEnumerable{T}"/> is not <c>null</c> and contains at least one element.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="enumerable">The sequence to check.</param>
    /// <returns><c>true</c> if the sequence is not <c>null</c> and not empty; otherwise, <c>false</c>.</returns>
    public static bool IsNotNullOrEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? enumerable)
        => !enumerable.IsNullOrEmpty();
}