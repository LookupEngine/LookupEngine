using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    /// <summary>
    ///     Compile all solution configurations.
    /// </summary>
    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetBuild(settings => settings
                .SetProjectFile(Solution)
                .SetConfiguration("Release")
                .SetVersion(ReleaseVersionNumber)
                .SetVerbosity(DotNetVerbosity.minimal));
        });
}