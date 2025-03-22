namespace LookupEngine.Abstractions.Decomposition;

/// <summary>
///     Represents a collection of variants
/// </summary>
/// <typeparam name="T">The variant type</typeparam>
public interface IVariantsCollection<in T> : IVariantsCollection
{
    /// <summary>
    ///     Adds a new variant
    /// </summary>
    /// <param name="result">The evaluated value</param>
    /// <returns>The variant collection with a new value</returns>
    IVariantsCollection<T> Add(T? result);

    /// <summary>
    ///     Adds a new variant with description
    /// </summary>
    /// <param name="result">The evaluated value</param>
    /// <param name="description">The description of the evaluation context</param>
    /// <returns>The variant collection with a new value</returns>
    IVariantsCollection<T> Add(T? result, string description);
}