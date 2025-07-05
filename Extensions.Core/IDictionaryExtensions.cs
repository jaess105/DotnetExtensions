namespace Extensions.Core;

/// <summary>
/// Provides extension methods for working with <see cref="IDictionary{TKey, TValue}"/>, simplifying
/// common patterns like get-or-insert and collection aggregation.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IDictionaryExtensions
{
    /// <summary>
    /// Retrieves the value associated with the specified <paramref name="key"/> if it exists;
    /// otherwise, computes a new value using the <paramref name="default"/> factory,
    /// adds it to the dictionary, and returns it.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary values.</typeparam>
    /// <param name="self">The dictionary to operate on.</param>
    /// <param name="key">The key to look up.</param>
    /// <param name="default">A function that generates the default value if the key is not found.</param>
    /// <returns>The existing or newly inserted value associated with the key.</returns>
    public static TValue GetOrInsert<TKey, TValue>(
        this IDictionary<TKey, TValue> self,
        TKey key,
        Func<TValue> @default)
    {
        if (self.TryGetValue(key, out TValue? value)) { return value; }

        value = @default();
        self[key] = value;

        return value;
    }

    /// <summary>
    /// Adds the specified <paramref name="value"/> to a collection at the given <paramref name="key"/>.
    /// If the key does not exist, a new collection of type <typeparamref name="TCol"/> is created and added to the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary keys.</typeparam>
    /// <typeparam name="TCol">The collection type used as the dictionary value, which must implement <see cref="ICollection{TValue}"/> and have a public parameterless constructor.</typeparam>
    /// <typeparam name="TValue">The type of the values contained in the collection.</typeparam>
    /// <param name="self">The dictionary to operate on.</param>
    /// <param name="key">The key whose collection should receive the value.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <returns>The collection associated with the key after the value has been added.</returns>
    public static TCol AddToOrInsert<TKey, TCol, TValue>(
        this IDictionary<TKey, TCol> self,
        TKey key,
        TValue value)
        where TCol : ICollection<TValue>, new()
    {
        return self.AddToOrInsert(key, value, () => []);
    }

    /// <summary>
    /// Adds the specified <paramref name="value"/> to a collection at the given <paramref name="key"/>.
    /// If the key does not exist, a new collection is created using the specified <paramref name="factory"/> function.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary keys.</typeparam>
    /// <typeparam name="TCol">The collection type used as the dictionary value, which must implement <see cref="ICollection{TValue}"/>.</typeparam>
    /// <typeparam name="TValue">The type of the values contained in the collection.</typeparam>
    /// <param name="self">The dictionary to operate on.</param>
    /// <param name="key">The key whose collection should receive the value.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <param name="factory">A function that creates a new collection instance when the key is not present.</param>
    /// <returns>The collection associated with the key after the value has been added.</returns>
    public static TCol AddToOrInsert<TKey, TCol, TValue>(
        this IDictionary<TKey, TCol> self,
        TKey key,
        TValue value,
        Func<TCol> factory)
        where TCol : ICollection<TValue>
    {
        if (!self.TryGetValue(key, out TCol? col))
        {
            col = factory();
            self[key] = col;
        }

        col.Add(value);

        return col;
    }
}