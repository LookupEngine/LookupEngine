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
using LookupEngine.Diagnostic;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public partial class LookupComposer
{
    private protected readonly TimeDiagnoser TimeDiagnoser = new();
    private protected readonly MemoryDiagnoser MemoryDiagnoser = new();

    /// <summary>
    ///     Evaluate value with diagnostics
    /// </summary>
    private object? EvaluateValue(FieldInfo member)
    {
        TimeDiagnoser.StartMonitoring();
        MemoryDiagnoser.StartMonitoring();

        var value = member.GetValue(_input);

        MemoryDiagnoser.StopMonitoring();
        TimeDiagnoser.StopMonitoring();

        return value;
    }

    /// <summary>
    ///     Evaluate value with diagnostics
    /// </summary>
    private object? EvaluateValue(PropertyInfo member)
    {
        try
        {
            TimeDiagnoser.StartMonitoring();
            MemoryDiagnoser.StartMonitoring();

            return member.GetValue(_input);
        }
        finally
        {
            MemoryDiagnoser.StopMonitoring();
            TimeDiagnoser.StopMonitoring();
        }
    }

    /// <summary>
    ///     Evaluate value with diagnostics
    /// </summary>
    private object? EvaluateValue(MethodInfo member)
    {
        try
        {
            TimeDiagnoser.StartMonitoring();
            MemoryDiagnoser.StartMonitoring();

            return member.Invoke(_input, null);
        }
        finally
        {
            MemoryDiagnoser.StopMonitoring();
            TimeDiagnoser.StopMonitoring();
        }
    }

    /// <summary>
    ///     Evaluate value with diagnostics
    /// </summary>
    private protected IVariant EvaluateValue(Func<IVariant> handler)
    {
        try
        {
            TimeDiagnoser.StartMonitoring();
            MemoryDiagnoser.StartMonitoring();

            return handler.Invoke();
        }
        finally
        {
            MemoryDiagnoser.StopMonitoring();
            TimeDiagnoser.StopMonitoring();
        }
    }
}