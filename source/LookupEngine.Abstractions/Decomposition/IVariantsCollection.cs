namespace LookupEngine.Abstractions.Decomposition;

/// <summary>
///     Represents a collection of variants
/// </summary>
public interface IVariantsCollection
{
    /// <summary>
    ///     Consume variants and evaluate values
    /// </summary>
    /// <returns>The evaluated variant</returns>
    IVariant Consume();
}