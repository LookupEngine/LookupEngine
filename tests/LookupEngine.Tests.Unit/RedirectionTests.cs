using LookupEngine.Descriptors;
using LookupEngine.Options;
using LookupEngine.Tests.Unit.Contexts;
using LookupEngine.Tests.Unit.Descriptors;
using LookupEngine.Tests.Unit.Objects;

namespace LookupEngine.Tests.Unit;

public sealed class RedirectionTests
{
    [Test]
    public async Task Decompose_ExcludingRedirection_RedirectedToAnotherValue()
    {
        //Arrange
        var data = new RedirectContainerObject();
        var options = new DecomposeOptions
        {
            EnableRedirection = false,
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    RedirectableObject => new RedirectionDescriptor(),
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
            await Assert.That(defaultResult.Members).IsNotEmpty();
            await Assert.That(comparableResult.Members).IsNotEmpty();
            await Assert.That(comparableResult.Members[0].AllocatedBytes).IsPositive();
            await Assert.That(comparableResult.Members[0].ComputationTime).IsPositive();
            await Assert.That(comparableResult.Members[0].Value.TypeName).IsEqualTo(defaultResult.Members[0].Value.TypeName);
        }
    }

    [Test]
    public async Task Decompose_IncludingRedirection_RedirectedToAnotherValue()
    {
        //Arrange
        var data = new RedirectContainerObject();
        var options = new DecomposeOptions
        {
            EnableRedirection = true,
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    RedirectableObject => new RedirectionDescriptor(),
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
            await Assert.That(defaultResult.Members).IsNotEmpty();
            await Assert.That(comparableResult.Members).IsNotEmpty();
            await Assert.That(comparableResult.Members[0].AllocatedBytes).IsPositive();
            await Assert.That(comparableResult.Members[0].ComputationTime).IsPositive();
            await Assert.That(comparableResult.Members[0].Value.TypeName).IsNotEqualTo(defaultResult.Members[0].Value.TypeName);
        }
    }

    [Test]
    public async Task Decompose_IncludingContextRedirection_RedirectedToAnotherValue()
    {
        //Arrange
        var context = new EngineTestContext();
        var data = new RedirectContainerObject();
        var options = new DecomposeOptions
        {
            EnableRedirection = true,
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    RedirectableObject => new RedirectionDescriptor(),
                    _ => new ObjectDescriptor(obj)
                };
            }
        };

        var contextOptions = new DecomposeOptions<EngineTestContext>
        {
            Context = context,
            EnableRedirection = true,
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    RedirectableObject => new RedirectionDescriptor(),
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
            await Assert.That(defaultResult.Members).IsNotEmpty();
            await Assert.That(comparableResult.Members).IsNotEmpty();
            await Assert.That(comparableContextResult.Members).IsNotEmpty();
            await Assert.That(comparableContextResult.Members[0].AllocatedBytes).IsPositive();
            await Assert.That(comparableContextResult.Members[0].ComputationTime).IsPositive();
            await Assert.That(comparableContextResult.Members[0].Value.TypeName).IsNotEqualTo(defaultResult.Members[0].Value.TypeName);
            await Assert.That(comparableContextResult.Members[0].Value.TypeName).IsNotEqualTo(comparableResult.Members[0].Value.TypeName);
        }
    }
}