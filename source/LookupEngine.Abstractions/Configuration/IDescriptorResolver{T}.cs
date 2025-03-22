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

using System.Reflection;
using LookupEngine.Abstractions.Decomposition;

namespace LookupEngine.Abstractions.Configuration;

/// <summary>
///     Indicates that the descriptor can resolve not-evaluated members
/// </summary>
/// <typeparam name="TContext">The type of execution context</typeparam>
public interface IDescriptorResolver<in TContext> : IDescriptorCollector
{
    /// <summary>
    ///     Resolves the target member if the member requires parameters
    /// </summary>
    /// <param name="target">The target object member name</param>
    /// <param name="parameters">The member runtime parameters</param>
    /// <returns>The lazy function to resolve the target member value</returns>
    Func<TContext, IVariant>? Resolve(string target, ParameterInfo[] parameters);
}