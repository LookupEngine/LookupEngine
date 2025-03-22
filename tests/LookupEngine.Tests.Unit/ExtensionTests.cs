using LookupEngine.Descriptors;
using LookupEngine.Options;
using LookupEngine.Tests.Unit.Contexts;
using LookupEngine.Tests.Unit.Descriptors;
using LookupEngine.Tests.Unit.Objects;

namespace LookupEngine.Tests.Unit;

public sealed class ExtensionTests
{
    [Test]
    public async Task Decompose_IncludingExtensions_ExtensionHandled()
    {
        //Arrange
        var data = new ExtensibleObject();
        var options = new DecomposeOptions
        {
            EnableExtensions = true,
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    ExtensibleObject => new ExtensionDescriptor(),
                    _ => new ObjectDescriptor(obj)
                };
            }
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        using (Assert.Multiple())
        {
            await Assert.That(defaultResult.Members).IsEmpty();
            await Assert.That(comparableResult.Members).IsNotEmpty();
            await Assert.That(comparableResult.Members[0].AllocatedBytes).IsPositive();
            await Assert.That(comparableResult.Members[0].ComputationTime).IsPositive();
        }
    }

    [Test]
    public async Task Decompose_IncludingContextExtensions_ExtensionHandled()
    {
        //Arrange
        var data = new ExtensibleObject();
        var context = new EngineTestContext();
        var options = new DecomposeOptions
        {
            EnableExtensions = true,
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    ExtensibleObject => new ExtensionDescriptor(),
                    _ => new ObjectDescriptor(obj)
                };
            }
        };

        var contextOptions = new DecomposeOptions<EngineTestContext>
        {
            Context = context,
            EnableExtensions = true,
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    ExtensibleObject => new ExtensionDescriptor(),
                    _ => new ObjectDescriptor(obj)
                };
            }
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);
        var comparableContextResult = LookupComposer.Decompose(data, contextOptions);

        //Assert
        using (Assert.Multiple())
        {
            await Assert.That(defaultResult.Members).IsEmpty();
            await Assert.That(comparableResult.Members).IsNotEmpty();
            await Assert.That(comparableContextResult.Members).IsNotEmpty();
            await Assert.That(comparableContextResult.Members[0].AllocatedBytes).IsPositive();
            await Assert.That(comparableContextResult.Members[0].ComputationTime).IsPositive();
            await Assert.That(comparableContextResult.Members[0].ComputationTime).IsPositive();
            await Assert.That(comparableContextResult.Members.Count).IsGreaterThan(comparableResult.Members.Count);
        }
    }
}