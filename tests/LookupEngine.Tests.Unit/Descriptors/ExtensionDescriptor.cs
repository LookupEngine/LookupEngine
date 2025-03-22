using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Tests.Unit.Contexts;

namespace LookupEngine.Tests.Unit.Descriptors;

public sealed class ExtensionDescriptor : Descriptor, IDescriptorExtension, IDescriptorExtension<EngineTestContext>
{
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("Extension", Extension);
        return;

        IVariant Extension()
        {
            return Variants.Value("Extended");
        }
    }

    public void RegisterExtensions(IExtensionManager<EngineTestContext> manager)
    {
        manager.Register("VersionExtension", VersionExtension);
        manager.Register("MetadataExtension", MetadataExtension);
        return;

        IVariant VersionExtension(EngineTestContext context)
        {
            return Variants.Value(context.Version);
        }

        IVariant MetadataExtension(EngineTestContext context)
        {
            return Variants.Value(context.Metadata);
        }
    }
}