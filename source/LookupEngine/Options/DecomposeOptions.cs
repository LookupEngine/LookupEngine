using System.Collections;
using JetBrains.Annotations;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Descriptors;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

/// <summary>
///     Object decomposition options
/// </summary>
[PublicAPI]
public class DecomposeOptions
{
    private Func<object?, Type?, Descriptor>? _typeResolver;

    /// <summary>
    ///     Decompose object root
    /// </summary>
    public bool IncludeRoot { get; set; }

    /// <summary>
    ///     Decompose object fields
    /// </summary>
    public bool IncludeFields { get; set; }

    /// <summary>
    ///     Decompose object properties
    /// </summary>
    public bool IncludeEvents { get; set; }

    /// <summary>
    ///     Include unsupported members in the decomposition
    /// </summary>
    public bool IncludeUnsupported { get; set; }

    /// <summary>
    ///     Decompose private members
    /// </summary>
    public bool IncludePrivateMembers { get; set; }

    /// <summary>
    ///     Decompose static members
    /// </summary>
    public bool IncludeStaticMembers { get; set; }

    /// <summary>
    ///     Enable object extensions
    /// </summary>
    public bool EnableExtensions { get; set; }

    /// <summary>
    ///     Enable member redirection
    /// </summary>
    public bool EnableRedirection { get; set; }

    /// <summary>
    ///     Map for resolving unsupported members
    /// </summary>
    public Func<object?, Type?, Descriptor> TypeResolver
    {
        get { return _typeResolver ??= DefaultResolveMap; }
        set => _typeResolver = value;
    }

    /// <summary>
    ///     The default map for resolving system types
    /// </summary>
    public static DecomposeOptions Default => new();

    private static Descriptor DefaultResolveMap(object? obj, Type? type)
    {
        return obj switch
        {
            bool value when type is null || type == typeof(bool) => new BooleanDescriptor(value),
            string value when type is null || type == typeof(string) => new StringDescriptor(value),
            IEnumerable value => new EnumerableDescriptor(value),
            Exception value when type is null || type == typeof(Exception) => new ExceptionDescriptor(value),
            _ => new ObjectDescriptor(obj)
        };
    }
}