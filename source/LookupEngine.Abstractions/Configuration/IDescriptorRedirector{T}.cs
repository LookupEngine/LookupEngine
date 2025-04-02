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

namespace LookupEngine.Abstractions.Configuration;

/// <summary>
///     Indicates that the object can be redirected to another
/// </summary>
/// <typeparam name="TContext">The type of execution context</typeparam>
public interface IDescriptorRedirector<in TContext>
{
    /// <summary>
    ///     Tries to redirect the object to another
    /// </summary>
    /// <param name="target">The target object member name</param>
    /// <param name="context">The type of execution context</param>
    /// <param name="result">The result of redirection</param>
    /// <returns>True if the redirection was successful, otherwise false</returns>
    bool TryRedirect(string target, TContext context, out object result);
}