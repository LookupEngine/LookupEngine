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