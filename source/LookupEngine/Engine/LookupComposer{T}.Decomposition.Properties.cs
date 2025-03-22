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
using JetBrains.Annotations;
using LookupEngine.Abstractions.Configuration;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

[UsedImplicitly]
public partial class LookupComposer<TContext>
{
    /// <summary>
    ///     Try to resolve parametric in-context properties
    /// </summary>
    private protected override bool TryResolve(PropertyInfo member, ParameterInfo[] parameters, out object? value)
    {
        value = null;

        if (MemberDeclaringDescriptor is IDescriptorResolver resolver)
        {
            var handler = resolver.Resolve(member.Name, parameters);
            if (handler is not null)
            {
                value = EvaluateValue(handler);
                return true;
            }
        }

        if (MemberDeclaringDescriptor is IDescriptorResolver<TContext> contextResolver)
        {
            var handler = contextResolver.Resolve(member.Name, parameters);
            if (handler is not null)
            {
                value = EvaluateValue(handler);
                return true;
            }
        }

        return false;
    }
}