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

using LookupEngine.Abstractions.Decomposition;

namespace LookupEngine.Abstractions.Configuration;

/// <summary>
///     The manager to register extensions for object descriptors
/// </summary>
/// <typeparam name="TContext">The type of execution context</typeparam>
public interface IExtensionManager<out TContext>
{
    /// <summary>
    ///     Registers the extension for the object
    /// </summary>
    /// <param name="name">The extension name</param>
    /// <param name="extension">The function for lazy extension creation</param>
    void Register(string name, Func<TContext, IVariant> extension);
}