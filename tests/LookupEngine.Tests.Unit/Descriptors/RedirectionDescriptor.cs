using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Tests.Unit.Contexts;

namespace LookupEngine.Tests.Unit.Descriptors;

public sealed class RedirectionDescriptor : Descriptor, IDescriptorRedirector, IDescriptorRedirector<EngineTestContext>
{
    public RedirectionDescriptor()
    {
        Name = "Redirection";
    }

    public bool TryRedirect(string target, out object result)
    {
        result = 69;
        return true;
    }

    public bool TryRedirect(string target, EngineTestContext context, out object result)
    {
        result = context.Version switch
        {
            1 => $"Target: {target}, context: {context.Metadata}",
            _ => $"Target: {target}, context: {context.Version}"
        };

        return true;
    }
}