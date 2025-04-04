﻿// Copyright (c) Lookup Foundation and Contributors
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
public sealed class TypeEqualBenchmark
{
    private object Object { get; set; } = new RoundButton();

    [Params(null, typeof(ButtonBase))]
    public Type? Type { get; set; }

    [Benchmark]
    public string? FindValue1()
    {
        return FindValue1(Object);
    }

    [Benchmark]
    public string? FindValue2()
    {
        return FindValue2(Object, Type);
    }

    [Benchmark]
    public string? FindValue3()
    {
        return FindValue3(Object, Type);
    }

    private string? FindValue1(object? obj)
    {
        return obj switch
        {
            RoundButton value => value.ToString(),
            Button value => value.ToString(),
            ButtonBase value => value.ToString(),
            _ => Object.ToString()
        };
    }

    private string? FindValue2(object? obj, Type? type)
    {
        return obj switch
        {
            RoundButton value when type is null || type == typeof(RoundButton) => value.ToString(),
            Button value when type is null || type == typeof(Button) => value.ToString(),
            ButtonBase value when type is null || type == typeof(ButtonBase) => value.ToString(),
            _ => Object.ToString()
        };
    }

    private string? FindValue3(object? obj, Type? type)
    {
        return obj switch
        {
            RoundButton value when type is null || type.FullName == typeof(RoundButton).FullName => value.ToString(),
            Button value when type is null || type.FullName == typeof(Button).FullName => value.ToString(),
            ButtonBase value when type is null || type.FullName == typeof(ButtonBase).FullName => value.ToString(),
            _ => Object.ToString()
        };
    }
}

public class ButtonBase
{
}

public class Button : ButtonBase
{
}

public sealed class RoundButton : Button
{
}