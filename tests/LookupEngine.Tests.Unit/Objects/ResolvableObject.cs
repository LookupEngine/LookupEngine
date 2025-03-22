using JetBrains.Annotations;

namespace LookupEngine.Tests.Unit.Objects;

[PublicAPI]
public sealed class ResolvableObject
{
    public string UnsupportedMethod(int parameter)
    {
        return parameter.ToString();
    }

    public string UnsupportedDescribedMethod(int parameter)
    {
        return parameter.ToString();
    }

    public string UnsupportedMultiMethod(int parameter)
    {
        return parameter.ToString();
    }
}