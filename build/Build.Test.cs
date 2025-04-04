﻿using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    /// <summary>
    ///     Run the solution tests.
    /// </summary>
    Target Test => _ => _
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(Solution)
                .SetConfiguration("Release")
                .SetVerbosity(DotNetVerbosity.minimal));
        });
}