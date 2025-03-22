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

using System.Diagnostics;

namespace LookupEngine.Diagnostic;

/// <summary>
///     The engine diagnoser to measure the evaluation member time
/// </summary>
internal sealed class TimeDiagnoser : IEngineDiagnoser
{
    private long _startTimeStamp;
    private long _endTimeStamp;

    public void StartMonitoring()
    {
        _startTimeStamp = Stopwatch.GetTimestamp();
    }

    public void StopMonitoring()
    {
        _endTimeStamp = Stopwatch.GetTimestamp();
    }

    public TimeSpan GetElapsed()
    {
#if NETCOREAPP
        var elapsed = Stopwatch.GetElapsedTime(_startTimeStamp, _endTimeStamp);
#else
        var tickFrequency = (double) TimeSpan.TicksPerSecond / Stopwatch.Frequency;
        var elapsed = new TimeSpan((long)((_endTimeStamp - _startTimeStamp) * tickFrequency));
#endif
        _startTimeStamp = 0;
        _endTimeStamp = 0;

        return elapsed;
    }
}