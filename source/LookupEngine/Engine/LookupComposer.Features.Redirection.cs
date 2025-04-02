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

using System.Diagnostics.CodeAnalysis;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Exceptions;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
public partial class LookupComposer
{
    /// <summary>
    ///     Redirect the member value to another object
    /// </summary>
    private protected virtual object RedirectValue(object value)
    {
        if (!_options.EnableRedirection) return value;

        var valueDescriptor = _options.TypeResolver.Invoke(value, null);
        while (valueDescriptor is IDescriptorRedirector redirector)
        {
            if (!redirector.TryRedirect(string.Empty, out value)) break;
            valueDescriptor = _options.TypeResolver.Invoke(value, null);
        }

        return value;
    }

    /// <summary>
    ///     Redirect the input value to another object
    /// </summary>
    private object RedirectValue(object value, out Descriptor valueDescriptor)
    {
        return RedirectValue(value, string.Empty, out valueDescriptor);
    }

    /// <summary>
    ///     Redirect the decomposed value to another object
    /// </summary>
    private protected virtual object RedirectValue(object value, string target, out Descriptor valueDescriptor)
    {
        var variant = value as IVariant;
        if (variant is not null)
        {
            value = variant.Value ?? throw new EngineException("Nullable variant must be handled before decomposition");
        }

        valueDescriptor = _options.TypeResolver.Invoke(value, null);

        var description = valueDescriptor.Description;
        if (variant is not null && description is null)
        {
            description = variant.Description;
        }

        if (_options.EnableRedirection)
        {
            while (valueDescriptor is IDescriptorRedirector redirector)
            {
                if (!redirector.TryRedirect(target, out value)) break;
                valueDescriptor = _options.TypeResolver.Invoke(value, null);

                if (valueDescriptor.Description is not null)
                {
                    description = valueDescriptor.Description;
                }
            }
        }

        valueDescriptor.Description = description;
        return value;
    }
}