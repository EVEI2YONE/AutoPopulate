# AutoPopulate Library Documentation

## Summary
AutoPopulate is a C#/.NET library designed to automate the initialization of object properties. It minimizes boilerplate by automatically assigning default or randomized values to class properties, making it ideal for rapid prototyping, unit testing, or simply reducing repetitive code in your applications.

## Purpose
The primary purpose of AutoPopulate is to:
- **Accelerate Development:** Automate the tedious task of manually initializing object properties.
- **Enhance Testing:** Quickly generate valid and diverse test data for robust unit tests.
- **Improve Maintainability:** Centralize configuration and behavior for property initialization, so updates need to be made in one location.
- **Increase Extensibility:** Provide hooks and interfaces that allow custom behavior without modifying core library code.

## Key Features
- **Plug-and-Play Integration:** Easily integrate with any C#/.NET project.
- **Fluent Configuration API:** Set up and customize default behaviors with an intuitive API.
- **Comprehensive Type Support:** Automatically handles primitives, strings, complex objects, and collections.
- **Randomized Data Generation:** Optionally generate randomized values for testing scenarios.
- **Custom Data Generators:** Enable custom generators for specific types or scenarios.
- **Optimized Performance:** Designed with efficiency in mind, ensuring minimal impact on application performance.
- **Extensible Architecture:** Interfaces allow developers to override or extend default functionality.

## Configuration Options

---

## Overview

AutoPopulate uses the IEntityGenerationConfig interface to control key aspects of entity generation, including:
- **Nullability Chances:** Probabilities for setting object or primitive properties to null.
- **Custom Primitive Generators:** Functions to generate specific primitive values.
- **List Generation:** Rules for generating lists, such as minimum/maximum sizes and whether the list size should be randomized.
- **Recursive Object Handling:** Limits for recursion depth and handling of references to avoid circular dependency issues.

## IEntityGenerationConfig
### Below is the definition of the configuration interface:

```csharp
public interface IEntityGenerationConfig
{
    public int MinListSize { get; set; }
    public int MaxListSize { get; set; }
    public bool RandomizeListSize { get; set; }
    public int MaxRecursionDepth { get; set; }
    public Dictionary<Type, Func<object>> TypeInterceptorValueProviders { get; set; }
    public Dictionary<Attribute, IAttributeHandler> AttributeHandlers { get; set; }
    public Dictionary<GenerationOption, double> OptionChances { get; set; }
}

//open to extensibility to define probability order instead
public enum GenerationOption
{
    NullablePrimitiveChance,
    NullableObjectChance,
    RecursionExistingReferenceChance //not implemented yet
}
```


## Example
### Configuration
```csharp
Config = new EntityGenerationConfig
{
    MinListSize = 2,
    MaxListSize = 5,
    RandomizeListSize = true,
    MaxRecursionDepth = 3,
    TypeInterceptorValueProviders = _defaultValues,
    AttributeHandlers = new Dictionary<Attribute, IAttributeHandler>(),
    OptionChances = new Dictionary<GenerationOption, double>()
    {
        { GenerationOption.NullableObjectChance, 0.1 },
        { GenerationOption.NullablePrimitiveChance, 0.1 },
        { GenerationOption.RecursionExistingReferenceChance, 0.1 },
    }
};

EntityGenerator = new EntityGenerator(config: Config);
```

### AutoPopulateAttribute
```csharp
private class SampleAttributeObject //Use TypeInterceptor, or use the AutoPopulate attribute
{
    [AutoPopulate("TestValue1_", "TestValue_2", 3, true "...TestValue_N")] //pre-defined, user-defined set of options
    public string Name { get; set; }
}
```

## Default types supported
### Can be overriden via TypeInterceptorValueProviders
```csharp
private readonly Dictionary<Type, Func<object>> DefaultTypeInterceptorValueProviders = new()
{
    { typeof(string), () => "_" },
    { typeof(bool), () => true },
    { typeof(short), () => (short)1 },
    { typeof(int), () => 1 },
    { typeof(uint), () => 1u },
    { typeof(long), () => 1L },
    { typeof(ulong), () => 1ul },
    { typeof(decimal), () => 1m },
    { typeof(double), () => 1.0d },
    { typeof(float), () => 1.0f },
    { typeof(char), () => '_' },
    { typeof(byte), () => (byte)('_') },
    { typeof(sbyte), () => (sbyte)1 },
    { typeof(DateTime), () => DateTime.Now },
    { typeof(object), () => "object" },
};
```