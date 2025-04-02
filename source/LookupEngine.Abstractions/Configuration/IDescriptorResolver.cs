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

using System.Reflection;
using LookupEngine.Abstractions.Decomposition;

namespace LookupEngine.Abstractions.Configuration;

/// <summary>
///     Indicates that the descriptor can resolve not-evaluated members
/// </summary>
public interface IDescriptorResolver : IDescriptorCollector
{
    /// <summary>
    ///     Resolves the target member if the member requires parameters
    /// </summary>
    /// <param name="target">The target object member name</param>
    /// <param name="parameters">The member runtime parameters</param>
    /// <returns>The lazy function to resolve the target member value</returns>
    Func<IVariant>? Resolve(string target, ParameterInfo[] parameters);
}