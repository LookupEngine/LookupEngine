# LookupEngine

A high-performance .NET library for runtime object analysis that provides deep inspection of object members through reflection, with built-in performance metrics and configurable evaluation strategies.

## Features

- Runtime inspection of public, private and static fields, properties and methods of any object
- Low-allocation execution with optimized member access for performance-critical operations
- Built-in computation time and memory allocation tracking for each evaluated member
- Extensible type descriptor system with custom resolvers and value converters
- Context-aware member resolution for enhanced metadata and value evaluation
- Support for multiple value variants based on method overloads and parameters
- Safe execution model with configurable error handling and member access control

## How to use

Basic Example:

```C#
var data = Colors.Red;
var decomposition = LookupComposer.Decompose(data);

Console.WriteLine(JsonSerializer.Serialize(decomposition));
```

Static object:

```C#
var data = typeof(Colors);
var decomposition = LookupComposer.Decompose(data);

Console.WriteLine(JsonSerializer.Serialize(decomposition));
```

Using context to provide additional metadata to the engine:

```C#
// Any object can be used as a context. 
// It is used by descriptors that require context to resolve members, or add in-context extensions.
var options = new DecomposeOptions<ExecutionContext>
{
    Context = new ExecutionContext
    {
        Version = "1.0",
        Runtime = "CoreCLR",
        Description = "LookupEngine Context"
    }
};

var decomposition = LookupComposer.Decompose(data, options);
```

Custom options:

```C#
var data = Colors.Red;
var options = new DecomposeOptions
{
    IncludeRoot = false,
    IncludeFields = false,
    IncludeEvents = true,
    IncludeUnsupported = false,
    IncludePrivateMembers = false,
    IncludeStaticMembers = true,
    EnableExtensions = true,
    EnableRedirection = true,
    TypeResolver = (obj, type) =>
    {
        return obj switch
        {
            bool value when type is null || type == typeof(bool) => new BooleanDescriptor(value),
            string value when type is null || type == typeof(string) => new StringDescriptor(value),
            IEnumerable value => new EnumerableDescriptor(value),
            Exception value when type is null || type == typeof(Exception) => new ExceptionDescriptor(value),
            _ => new ObjectDescriptor(obj)
        };
    }
};

var decomposition = LookupComposer.Decompose(data, options);

Console.WriteLine(JsonSerializer.Serialize(decomposition));
```

Output:

```json
{
    "Name": "#FFFF0000",
    "TypeName": "Color",
    "TypeFullName": "System.Windows.Media.Color",
    "Members": [
        {
            "Name": "R",
            "DeclaringTypeName": "Color",
            "DeclaringTypeFullName": "System.Windows.Media.Color",
            "ComputationTime": 0.0008,
            "AllocatedBytes": 192,
            "MemberAttributes": 8,
            "Value": {
                "Name": "255",
                "TypeName": "Byte",
                "TypeFullName": "System.Byte"
            }
        },
        {
            "Name": "G",
            "DeclaringTypeName": "Color",
            "DeclaringTypeFullName": "System.Windows.Media.Color",
            "ComputationTime": 0.0004,
            "AllocatedBytes": 192,
            "MemberAttributes": 8,
            "Value": {
                "Name": "0",
                "TypeName": "Byte",
                "TypeFullName": "System.Byte"
            }
        },
        {
            "Name": "B",
            "DeclaringTypeName": "Color",
            "DeclaringTypeFullName": "System.Windows.Media.Color",
            "ComputationTime": 0.0005,
            "AllocatedBytes": 192,
            "MemberAttributes": 8,
            "Value": {
                "Name": "0",
                "TypeName": "Byte",
                "TypeFullName": "System.Byte"
            }
        }
    ]
}
```

## Descriptors

Descriptors describe exactly how the engine should handle types, parametric methods, and provide additional metadata for the object.

To register a descriptor, it is required to set the `TypeResolver` property of `DecomposeOptions`, that is responsible for mapping a descriptor to a type.

```C#
var options = new DecomposeOptions
{
    TypeResolver = (obj, type) =>
    {
        return obj switch
        {
            bool value => new BooleanDescriptor(value),
            string value => new StringDescriptor(value),
            _ => new ObjectDescriptor(obj)
        };
    }
};
```

Describing an object is implemented with interfaces. There are quite a lot of them, let's consider each of them.

### IDescriptorResolver

Describes exactly how the return value should be resolved.
There is support for a single value or multiple values.
To return the result of a member, use the `Variants` class, that contains possible scenarios for evaluating the value.

Resolution with only one variant:

```c#
public class ElementDescriptor(Element element) : Descriptor, IDescriptorResolver
{
    public virtual Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Element.IsHidden) => ResolveIsHidden,
            nameof(Element.CanBeHidden) => ResolveCanBeHidden,
            _ => null
        };

        IVariant ResolveCanBeHidden()
        {
            return Variants.Value(_element.CanBeHidden(Context.ActiveView));
        }

        IVariant ResolveIsHidden()
        {
            return Variants.Value(_element.IsHidden(Context.ActiveView), "Active view");
        }
    }
}
```

Resolution with multiple values:

```c#
public class ElementDescriptor(Element element) : Descriptor, IDescriptorResolver
{
    public virtual Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Element.GetMaterialIds) => ResolveGetMaterialIds,
            nameof(Element.GetBoundingBox) => ResolveBoundingBox,
            _ => null
        };

        IVariant ResolveGetMaterialIds()
        {
            return Variants.Values<ICollection<ElementId>>(2)
                .Add(_element.GetMaterialIds(true))
                .Add(_element.GetMaterialIds(false))
                .Consume();
        }

        IVariant ResolveBoundingBox()
        {
            return Variants.Values<BoundingBoxXYZ>(2)
                .Add(_element.get_BoundingBox(null), "Model")
                .Add(_element.get_BoundingBox(Context.ActiveView), "Active view")
                .Consume();
        }
    }
}
```

If you need an evaluation context for member resolving, use the generic interface version.
Context is passed to the engine as an option and is single for all descriptors.
Generic and none-generic version can be contained in the single class:

```C#
public sealed class ReferenceDescriptor(Reference reference) : Descriptor, IDescriptorResolver<Document>
{
    public Func<Document, IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Reference.ConvertToStableRepresentation) => ResolveConvertToStableRepresentation,
            _ => null
        };

        IVariant ResolveConvertToStableRepresentation(Document context)
        {
            return Variants.Value(reference.ConvertToStableRepresentation(context));
        }
    }
}
```

Disable the member evaluation:

```c#
public class DocumentDescriptor(Document document) : Descriptor, IDescriptorResolver
{
    public virtual Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Document.Close) => Variants.Disabled
            _ => null
        };
    }
}
```

If you only want to resolve a specific overload, parameters are your friends:

```C#
public sealed class EntityDescriptor(Entity entity) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Entity.Get) when parameters.Length == 1 &&
                                    parameters[0].ParameterType == typeof(string) => ResolveGetByField,
            nameof(Entity.Get) when parameters.Length == 2 &&
                                    parameters[0].ParameterType == typeof(string) &&
                                    parameters[1].ParameterType == typeof(ForgeTypeId) => ResolveGetByFieldForge,
            _ => null
        };
    }
}
```

### IDescriptorExtension

Adds registration of additional metadata for the object. For example, new methods or properties that the original object doesn't have:

```c#
public sealed class ColorDescriptor(Color color) : Descriptor, IDescriptorExtension
{
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("HEX", () => Variants.Value(ColorRepresentationUtils.ColorToHex(color)));
        manager.Register("RGB", () => Variants.Value(ColorRepresentationUtils.ColorToRgb(color)));
        manager.Register("CMYK", () => Variants.Value(ColorRepresentationUtils.ColorToCmyk(color)));
    }
}
```

If you need an evaluation context for extension registration, use the generic interface version.
Context is passed to the engine as an option and is single for all descriptors.
Generic and none-generic version can be contained in the single class:

```C#
public sealed class SchemaDescriptor(Schema schema) : Descriptor, IDescriptorExtension<Document>
{
    public void RegisterExtensions(IExtensionManager<Document> manager)
    {
        manager.Register("GetElements", context => Variants.Value(context
            .GetElements()
            .WherePasses(new ExtensibleStorageFilter(schema.GUID))
            .ToElements()));
    }
}
```

### IDescriptorRedirector

Redirects the evaluation of the current object to another object.
As a result, you will get a new evaluated value instead of the original one.
For example, you can get the object itself instead of its ID in the output:

```c#
public sealed class ElementIdDescriptor(long elementId) : Descriptor, IDescriptorRedirector
{
    public bool TryRedirect(string target, out object result)
    {
        if (elementId < 0) return false;

        result = Database.GetElementById(elementId);
        return true;
    }
}
```

If you need an evaluation context for redirection, use the generic interface version.
Context is passed to the engine as an option and is single for all descriptors:

```c#
public sealed class ElementIdDescriptor(ElementId elementId) : Descriptor, IDescriptorRedirector<Document>
{
    public bool TryRedirect(string target, Document context, out object result)
    {
        if (elementId == ElementId.InvalidElementId) return false;

        result = elementId.ToElement(context);
        return true;
    }
}
```

### IDescriptorCollector

Serves as a marker that the object is maintainable, and available for internal component analysis. Advantage of being used as a marker in UI applications. Does not have any effect for CLI applications.

```c#
public sealed class ApplicationDescriptor : Descriptor, IDescriptorCollector
{
    public ApplicationDescriptor(Application application)
    {
        Name = application.VersionName;
    }
}
```