using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    /// <summary>
    ///     Clean projects with dependencies.
    /// </summary>
    Target Clean => _ => _
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() =>
        {
            DotNetClean(settings => settings
                .SetProject(Solution)
                .SetVerbosity(DotNetVerbosity.minimal)
                .EnableNoLogo());
        });
}