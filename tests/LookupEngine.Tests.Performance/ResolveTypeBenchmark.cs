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

using BenchmarkDotNet.Attributes;

namespace LookupEngine.Tests.Performance;

[MediumRunJob]
[MemoryDiagnoser]
public sealed class ResolveTypeBenchmark
{
    private static object Obj => "Text";

    [Benchmark]
    public bool TypeIsEquals()
    {
        return Obj is IDisposable;
    }

    [Benchmark]
    public bool NamespaceEquals()
    {
        return Obj.GetType().Namespace == "System";
    }

    [Benchmark]
    public bool NamespaceStartsWith()
    {
        return Obj.GetType().Namespace!.StartsWith("System");
    }

    [Benchmark]
    public bool AssemblyStartsWith()
    {
        return Obj.GetType().Assembly.FullName!.StartsWith("System");
    }
}