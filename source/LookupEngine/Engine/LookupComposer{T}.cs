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
using LookupEngine.Options;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public partial class LookupComposer
{
    /// <summary>
    ///     Decompose an in-context object into its internal components and evaluate their values
    /// </summary>
    /// <param name="value">The object to decompose</param>
    /// <param name="options">The decomposition options</param>
    /// <typeparam name="TContext">The type of execution context to resolve context-dependent members</typeparam>
    /// <returns>Decomposed object and its structure</returns>
    [Pure]
    public static DecomposedObject Decompose<TContext>(object? value, DecomposeOptions<TContext> options)
    {
        if (value is null) return CreateNullableDecomposition();

        return value switch
        {
            Type type => new LookupComposer<TContext>(value, options).DecomposeStatic(type),
            _ => new LookupComposer<TContext>(value, options).DecomposeInstance()
        };
    }

    /// <summary>
    ///     Decompose an in-context object without internal components
    /// </summary>
    /// <param name="value">The object to decompose</param>
    /// <param name="options">The decomposition options</param>
    /// <typeparam name="TContext">The type of execution context to resolve context-dependent members</typeparam>
    /// <returns>The decomposed object</returns>
    [Pure]
    public static DecomposedObject DecomposeObject<TContext>(object? value, DecomposeOptions<TContext> options)
    {
        if (value is null) return CreateNullableDecomposition();

        return value switch
        {
            Type type => new LookupComposer<TContext>(value, options).DecomposeStaticObject(type),
            _ => new LookupComposer<TContext>(value, options).DecomposeInstanceObject()
        };
    }

    /// <summary>
    ///     Decompose the in-context object's internal components and evaluate their values
    /// </summary>
    /// <param name="value">The object to decompose</param>
    /// <param name="options">The decomposition options</param>
    /// <typeparam name="TContext">The type of execution context to resolve context-dependent members</typeparam>
    /// <returns>The decomposed object structure</returns>
    [Pure]
    public static List<DecomposedMember> DecomposeMembers<TContext>(object? value, DecomposeOptions<TContext> options)
    {
        if (value is null) return [];

        return value switch
        {
            Type type => new LookupComposer<TContext>(value, options).DecomposeStaticMembers(type),
            _ => new LookupComposer<TContext>(value, options).DecomposeInstanceMembers()
        };
    }
}