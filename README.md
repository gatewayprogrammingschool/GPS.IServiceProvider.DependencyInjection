# IServiceProvider Extensions

Allows initialization of objects obtained from `IServiceProvider`.  The implementation of `IServiceProvider` does not matter.

## Here's Our Badges

| Technical | Social |
|---|---|
| [![.NET](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/dotnet.yml) | [![Repo Stars](https://img.shields.io/github/stars/gatewayprogrammingschool/GPS.IServiceProvider.Extensions?label=Repository%20Stars&style=plastic)](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions) |
| [![Codacy Security Scan](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/codacy-analysis.yml/badge.svg)](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/codacy-analysis.yml) | [![NuGet Version](https://img.shields.io/nuget/vpre/IServiceProviderExtensions)](https://www.nuget.org/packages/IServiceProviderExtensions/) |
| [![CodeQL](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/codeql-analysis.yml) | [![Discussions](https://img.shields.io/github/discussions/gatewayprogrammingschool/GPS.IServiceProvider.Extensions)](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/discussions) |

## Example Usage - Prebuild Initializer

[Source](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/blob/main/tests/GPS.IServiceProvider.Extensions.Tests/TestObject.cs) for `TestObject`

```csharp
DependencyInitializer initializer = new DependencyInitializer<TestObject>
{
    new DependencyInitializer.ValueType<TestObject, int>(
        nameof(TestObject._intField),
        intValue
    ),
    new DependencyInitializer.ValueType<TestObject, int>(
        nameof(TestObject.IntProperty),
        intValue
    ),
    new DependencyInitializer.ReferenceType<TestObject, TestObject>(
        nameof(TestObject._parentField),
        parent
    ),
    new DependencyInitializer.ReferenceType<TestObject, TestObject>(
        nameof(TestObject.ParentProperty),
        parent
    ),
};

var testObject = Provider.GetService<TestObject>(initializer);

```

Any `public`, instance field or property may be initialized.  If a Property has no setter, the expect exception from reflection will be thrown.

## Example Usage - On-the-fly Initializer

```csharp
var testObject = Provider.GetService<TestObject>(new DependencyInitializer<TestObject>
{
    new DependencyInitializer.ValueType<TestObject, int>(
        nameof(TestObject._intField),
        intValue
    ),
    new DependencyInitializer.ValueType<TestObject, int>(
        nameof(TestObject.IntProperty),
        intValue
    ),
    new DependencyInitializer.ReferenceType<TestObject, TestObject>(
        nameof(TestObject._parentField),
        parent
    ),
    new DependencyInitializer.ReferenceType<TestObject, TestObject>(
        nameof(TestObject.ParentProperty),
        parent
    ),
});

```
