﻿using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.Decomposition;
#if NETCOREAPP
using System.Text.Json.Serialization;
#endif

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

/// <summary>
///     Represents a decomposed member value
/// </summary>
[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {RawValue}")]
public sealed class DecomposedValue
{
    /// <summary>
    ///     The raw, non-evaluated value
    /// </summary>
#if NETCOREAPP
    [JsonIgnore]
#endif
    public required object? RawValue { get; init; }

    /// <summary>
    ///     The value name
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     The value type name
    /// </summary>
    public required string TypeName { get; init; }

    /// <summary>
    ///     The value type full name
    /// </summary>
    public required string TypeFullName { get; init; }

    /// <summary>
    ///     The description of the evaluation context
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    ///     Descriptor for value description
    /// </summary>
#if NETCOREAPP
    [JsonIgnore]
#endif
    public Descriptor? Descriptor { get; init; }
}