﻿// Copyright 2003-2024 by Autodesk, Inc.
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

using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Exceptions;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public partial class LookupComposer<TContext>
{
    /// <summary>
    ///     Redirect the in-context member value to another object
    /// </summary>
    private protected override object RedirectValue(object value)
    {
        if (!_options.EnableRedirection) return value;

        var valueDescriptor = _options.TypeResolver.Invoke(value, null);
        while (true)
        {
            var redirected = false;

            // Generic interface is prioritised
            if (valueDescriptor is IDescriptorRedirector<TContext> genericRedirector)
            {
                if (genericRedirector.TryRedirect(string.Empty, _options.Context, out value))
                {
                    redirected = true;
                }
            }
            else if (valueDescriptor is IDescriptorRedirector redirector)
            {
                if (redirector.TryRedirect(string.Empty, out value))
                {
                    redirected = true;
                }
            }

            if (!redirected) break;

            valueDescriptor = _options.TypeResolver.Invoke(value, null);
        }

        return value;
    }

    /// <summary>
    ///     Redirect the decomposed in-context value to another object
    /// </summary>
    private protected override object RedirectValue(object value, string target, out Descriptor valueDescriptor)
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
            while (true)
            {
                var redirected = false;

                // Generic interface is prioritised
                if (valueDescriptor is IDescriptorRedirector<TContext> genericRedirector)
                {
                    if (genericRedirector.TryRedirect(target, _options.Context, out value))
                    {
                        redirected = true;
                    }
                }
                else if (valueDescriptor is IDescriptorRedirector redirector)
                {
                    if (redirector.TryRedirect(target, out value))
                    {
                        redirected = true;
                    }
                }

                if (!redirected) break;

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