sealed partial class Build
{
    /// <summary>
    ///     Publish a new GitHub release.
    /// </summary>
    Target PublishGitHub => _ => _
        .DependsOn(Test)
        .Requires(() => ReleaseVersion)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            var changelogBuilder = CreateChangelogBuilder();
            Assert.True(changelogBuilder.Length > 0, "Changelog is required for publishing a new Tag");
        });
}