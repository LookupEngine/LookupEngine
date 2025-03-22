using LookupEngine.Abstractions.Decomposition;

namespace LookupEngine.Descriptors;

public sealed class StringDescriptor : Descriptor
{
    public StringDescriptor(string text)
    {
        Name = text;
    }
}