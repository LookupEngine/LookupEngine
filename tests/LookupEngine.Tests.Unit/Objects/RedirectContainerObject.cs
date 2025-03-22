using JetBrains.Annotations;

namespace LookupEngine.Tests.Unit.Objects;

[PublicAPI]
public sealed class RedirectContainerObject
{
    public RedirectableObject PropertyToRedirect => new();
}

[PublicAPI]
public sealed class RedirectableObject
{
    public Random Random { get; set; } = new(69);
}