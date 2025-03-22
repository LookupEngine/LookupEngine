using JetBrains.Annotations;

namespace LookupEngine.Options;

/// <summary>
///     Object decomposition options for in-context decomposition
/// </summary>
/// <typeparam name="TContext">The type of execution context</typeparam>
[PublicAPI]
public class DecomposeOptions<TContext> : DecomposeOptions
{
    /// <summary>
    ///     Context where decomposition performs.
    /// </summary>
    /// <remarks>A context can be any object that provides additional metadata to resolve the value of the object members.</remarks>
    public required TContext Context { get; set; }
}