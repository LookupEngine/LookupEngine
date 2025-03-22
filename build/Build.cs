using JetBrains.Annotations;

[PublicAPI]
sealed partial class Build : NukeBuild
{
    /// <summary>
    ///     Pipeline entry point.
    /// </summary>
    public static int Main() => Execute<Build>(build => build.Compile);
}