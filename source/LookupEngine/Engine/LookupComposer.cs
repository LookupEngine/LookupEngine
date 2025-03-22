// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using JetBrains.Annotations;
using LookupEngine.Abstractions;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public partial class LookupComposer
{
    /// <summary>
    ///     Decompose an object into its internal components and evaluate their values
    /// </summary>
    /// <param name="value">The object to decompose</param>
    /// <param name="options">The decomposition options</param>
    /// <returns>Decomposed object and its structure</returns>
    [Pure]
    public static DecomposedObject Decompose(object? value, DecomposeOptions? options = null)
    {
        if (value is null) return CreateNullableDecomposition();

        options ??= DecomposeOptions.Default;
        return value switch
        {
            Type type => new LookupComposer(value, options).DecomposeStatic(type),
            _ => new LookupComposer(value, options).DecomposeInstance()
        };
    }

    /// <summary>
    ///     Decompose an object without internal components
    /// </summary>
    /// <param name="value">The object to decompose</param>
    /// <param name="options">The decomposition options</param>
    /// <returns>The decomposed object</returns>
    [Pure]
    public static DecomposedObject DecomposeObject(object? value, DecomposeOptions? options = null)
    {
        if (value is null) return CreateNullableDecomposition();

        options ??= DecomposeOptions.Default;
        return value switch
        {
            Type type => new LookupComposer(value, options).DecomposeStaticObject(type),
            _ => new LookupComposer(value, options).DecomposeInstanceObject()
        };
    }

    /// <summary>
    ///     Decompose the object's internal components and evaluate their values
    /// </summary>
    /// <param name="value">The object to decompose</param>
    /// <param name="options">The decomposition options</param>
    /// <returns>The decomposed object structure</returns>
    [Pure]
    public static List<DecomposedMember> DecomposeMembers(object? value, DecomposeOptions? options = null)
    {
        if (value is null) return [];

        options ??= DecomposeOptions.Default;
        return value switch
        {
            Type type => new LookupComposer(value, options).DecomposeStaticMembers(type),
            _ => new LookupComposer(value, options).DecomposeInstanceMembers()
        };
    }
}