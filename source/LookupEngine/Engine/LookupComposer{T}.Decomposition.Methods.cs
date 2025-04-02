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
using LookupEngine.Abstractions.Configuration;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public partial class LookupComposer<TContext>
{
    /// <summary>
    ///     Try to resolve parametric in-context methods
    /// </summary>
    private protected override bool TryResolve(MethodInfo member, ParameterInfo[] parameters, out object? value)
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