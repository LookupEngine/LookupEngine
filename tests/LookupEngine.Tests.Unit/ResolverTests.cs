using LookupEngine.Descriptors;
using LookupEngine.Options;
using LookupEngine.Tests.Unit.Contexts;
using LookupEngine.Tests.Unit.Descriptors;
using LookupEngine.Tests.Unit.Objects;

namespace LookupEngine.Tests.Unit;

public sealed class ResolverTests
{
    [Test]
    public async Task Decompose_IncludingUnresolvedData_ResolvedData()
    {
        //Arrange
        var data = new ResolvableObject();
        var options = new DecomposeOptions
        {
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    ResolvableObject => new ResolverDescriptor(),
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
            await Assert.That(comparableResult.Members[1].AllocatedBytes).IsPositive();
            await Assert.That(comparableResult.Members[1].ComputationTime).IsPositive();
            await Assert.That(comparableResult.Members[1].Value.Description).IsNotNullOrEmpty();
        }
    }

    [Test]
    public async Task Decompose_IncludingUnresolvedContextData_ResolvedData()
    {
        //Arrange
        var data = new ResolvableObject();
        var context = new EngineTestContext();
        var options = new DecomposeOptions
        {
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    ResolvableObject => new ResolverDescriptor(),
                    _ => new ObjectDescriptor(obj)
                };
            }
        };

        var contextOptions = new DecomposeOptions<EngineTestContext>
        {
            Context = context,
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    ResolvableObject => new ResolverDescriptor(),
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
            await Assert.That(comparableContextResult.Members.Count).IsGreaterThan(comparableResult.Members.Count);
        }
    }
}