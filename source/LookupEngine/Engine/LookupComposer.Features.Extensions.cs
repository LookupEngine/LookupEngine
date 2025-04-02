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

using System.Diagnostics.CodeAnalysis;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

//ReSharper disable once CheckNamespace
namespace LookupEngine;

[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
public partial class LookupComposer : IExtensionManager
{
    /// <summary>
    ///     Add extension members to the decomposition
    /// </summary>
    private protected virtual void ExecuteExtensions()
    {
        if (!_options.EnableExtensions) return;

        if (MemberDeclaringDescriptor is IDescriptorExtension extension)
        {
            extension.RegisterExtensions(this);
        }
    }

    /// <summary>
    ///     Callback of the extension registration
    /// </summary>
    public void Register(string methodName, Func<IVariant> handler)
    {
        try
        {
            var result = EvaluateValue(handler);
            WriteExtensionMember(result, methodName);
        }
        catch (Exception exception)
        {
            WriteExtensionMember(exception, methodName);
        }
    }
}