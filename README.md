# GPS MS Extensions Dependency Injection Helpers

Allows initialization of objects obtained from IServiceProvider.

## Here's Our Badges

| Technical | Social |
|---|---|
| [![.NET](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/dotnet.yml) | ![Repo Stars](https://img.shields.io/github/stars/gatewayprogrammingschool/GPS.IServiceProvider.Extensions?label=Repository%20Stars&style=plastic) |
| [![Codacy Security Scan](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/codacy-analysis.yml/badge.svg)](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/codacy-analysis.yml) | |
| [![CodeQL](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gatewayprogrammingschool/GPS.IServiceProvider.Extensions/actions/workflows/codeql-analysis.yml) | |



## Example Usage

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