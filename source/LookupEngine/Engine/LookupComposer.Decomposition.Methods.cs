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
using LookupEngine.Abstractions.Configuration;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public partial class LookupComposer
{
    /// <summary>
    ///     Add methods to the decomposition
    /// </summary>
    private void DecomposeMethods(BindingFlags bindingFlags)
    {
        var members = MemberDeclaringType.GetMethods(bindingFlags);
        foreach (var member in members)
        {
            if (member.IsSpecialName) continue;
            if (member is {IsFamily: true, IsSecurityCritical: true}) continue; //Object-critical methods cause CLR exception

            object? value;
            var parameters = member.GetParameters();

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
    ///     Try to resolve parametric methods
    /// </summary>
    private protected virtual bool TryResolve(MethodInfo member, ParameterInfo[] parameters, out object? value)
    {
        value = null;
        if (MemberDeclaringDescriptor is not IDescriptorResolver resolver) return false;

        var handler = resolver.Resolve(member.Name, parameters);
        if (handler is null) return false;

        value = EvaluateValue(handler);

        return true;
    }

    /// <summary>
    ///     Try to suppress unsupported methods
    /// </summary>
    private bool TrySuppress(MethodInfo member, ParameterInfo[] parameters, out object? value)
    {
        value = null;
        if (member.ReturnType.Name == "Void")
        {
            if (!_options.IncludeUnsupported) return false;

            value = new InvalidOperationException("Method doesn't return a value");
            return true;
        }

        if (parameters.Length > 0)
        {
            if (!_options.IncludeUnsupported) return false;

            value = new NotSupportedException("Unsupported method overload");
            return true;
        }

        return true;
    }
}