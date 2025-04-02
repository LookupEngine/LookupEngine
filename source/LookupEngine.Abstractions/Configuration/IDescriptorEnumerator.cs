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

using System.Collections;

namespace LookupEngine.Abstractions.Configuration;

/// <summary>
///     Indicates that the descriptor is handled as a collection of descriptors
/// </summary>
public interface IDescriptorEnumerator : IDescriptorCollector
{
    /// <summary>
    ///     Indicates that the current described collection is empty
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    ///     The enumerator of the current described collection
    /// </summary>
    public IEnumerator Enumerator { get; }
}