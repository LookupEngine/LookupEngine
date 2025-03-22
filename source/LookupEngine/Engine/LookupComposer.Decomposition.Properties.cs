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
public partial class LookupComposer
{
    /// <summary>
    ///     Add properties to the decomposition
    /// </summary>
    private void DecomposeProperties(BindingFlags bindingFlags)
    {
        var members = MemberDeclaringType.GetProperties(bindingFlags);
        foreach (var member in members)
        {
            if (member.IsSpecialName) continue;

            object? value;
            var parameters = member.CanRead ? member.GetMethod!.GetParameters() : [];

            try
            {
                if (!TryResolve(member, parameters, out value))
                {
                    if (!TrySuppress(member, parameters, out value)) continue;

                    value ??= EvaluateValue(member);
                }
            }
            catch (TargetInvocationException exception)
            {
                value = exception.InnerException;
            }
            catch (Exception exception)
            {
                value = exception;
            }

            WriteDecompositionMember(value, member, parameters);
        }
    }

    /// <summary>
    ///     Try to resolve parametric properties
    /// </summary>
    private protected virtual bool TryResolve(PropertyInfo member, ParameterInfo[] parameters, out object? value)
    {
        value = null;
        if (MemberDeclaringDescriptor is not IDescriptorResolver resolver) return false;

        var handler = resolver.Resolve(member.Name, parameters);
        if (handler is null) return false;

        value = EvaluateValue(handler);

        return true;
    }

    /// <summary>
    ///     Try to suppress unsupported properties
    /// </summary>
    private bool TrySuppress(PropertyInfo member, ParameterInfo[] parameters, out object? value)
    {
        value = null;

        if (!member.CanRead)
        {
            value = new InvalidOperationException("Property does not have a get accessor, it cannot be read");
            return true;
        }

        if (parameters.Length > 0)
        {
            if (!_options.IncludeUnsupported) return false;

            value = new NotSupportedException("Unsupported property overload");
            return true;
        }

        return true;
    }
}