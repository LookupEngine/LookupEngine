// Copyright (c) Lookup Foundation and Contributors
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// THIS PROGRAM IS PROVIDED "AS IS" AND WITH ALL FAULTS.
// NO IMPLIED WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE IS PROVIDED.
// THERE IS NO GUARANTEE THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.

using JetBrains.Annotations;
using LookupEngine.Abstractions;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Exceptions;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

/// <summary>
///     Provides functionality to work with the internal structure of an object
/// </summary>
[PublicAPI]
public partial class LookupComposer
{
    private readonly DecomposeOptions _options;

    private int _depth;
    private object _input;
    private Type? _memberDeclaringType;
    private Descriptor? _memberDeclaringDescriptor;
    private DecomposedObject? _decomposedObject;
    private List<DecomposedMember>? _decomposedMembers;

    /// <summary>
    ///     Initialize a new composer instance
    /// </summary>
    private protected LookupComposer(object value, DecomposeOptions options)
    {
        _input = value;
        _options = options;
    }

    /// <summary>
    ///     Decomposed members of the input object
    /// </summary>
    internal List<DecomposedMember> DecomposedMembers
    {
        get
        {
            if (_decomposedMembers is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(DecomposedMembers));
            }

            return _decomposedMembers;
        }
        set => _decomposedMembers = value;
    }

    /// <summary>
    ///     The object type for the current hierarchy depth
    /// </summary>
    internal Type MemberDeclaringType
    {
        get
        {
            if (_memberDeclaringType is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(MemberDeclaringType));
            }

            return _memberDeclaringType;
        }
        set => _memberDeclaringType = value;
    }

    /// <summary>
    ///     The object descriptor for the current hierarchy depth
    /// </summary>
    internal Descriptor MemberDeclaringDescriptor
    {
        get
        {
            if (_memberDeclaringDescriptor is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(MemberDeclaringDescriptor));
            }

            return _memberDeclaringDescriptor;
        }
        set => _memberDeclaringDescriptor = value;
    }
}