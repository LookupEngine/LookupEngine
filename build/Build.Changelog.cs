using System.Text;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitHub;

sealed partial class Build
{
    /// <summary>
    ///     Creates a Builder for creating a changelog for the current release.
    /// </summary>
    StringBuilder CreateChangelogBuilder()
    {
        Assert.True(File.Exists(ChangelogPath), $"Unable to locate the changelog file: {ChangelogPath}");
        Log.Information("Changelog: {Path}", ChangelogPath);

        var changelog = BuildChangelog();
        Assert.True(changelog.Length > 0, $"No version entry exists in the changelog: {ReleaseVersion}");

        return changelog;
    }

    /// <summary>
    ///     Builds the changelog content for the current release.
    /// </summary>
    StringBuilder BuildChangelog()
    {
        const string separator = "# ";

        var hasEntry = false;
        var changelog = new StringBuilder();
        foreach (var line in File.ReadLines(ChangelogPath))
        {
            if (hasEntry)
            {
                if (line.StartsWith(separator)) break;

                changelog.AppendLine(line);
                continue;
            }

            if (line.StartsWith(separator) && line.Contains(ReleaseVersion))
            {
                hasEntry = true;
            }
        }

        TrimEmptyLines(changelog);
        return changelog;
    }

    /// <summary>
    ///     Trims empty lines from the changelog builder.
    /// </summary>
    static void TrimEmptyLines(StringBuilder builder)
    {
        if (builder.Length == 0) return;

        while (builder[^1] == '\r' || builder[^1] == '\n')
        {
            builder.Remove(builder.Length - 1, 1);
        }

        while (builder[0] == '\r' || builder[0] == '\n')
        {
            builder.Remove(0, 1);
        }
    }
}