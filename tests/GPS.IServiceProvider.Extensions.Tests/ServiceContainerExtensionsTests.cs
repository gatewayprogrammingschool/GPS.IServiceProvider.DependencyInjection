#pragma warning disable CS0649

namespace GPS.IServiceProvider.Extensions.Tests;

using System;
using System.Reflection;

using Xunit;
using Xunit.Abstractions;

using FluentAssertions;

using global::Microsoft.Extensions.DependencyInjection;

public class ServiceContainerExtensionsTests
{
    public ITestOutputHelper Output
    {
        get;
    }

    public ServiceContainerExtensionsTests(ITestOutputHelper output)
    {
        Output = output;

        var collection = new ServiceCollection();

        collection.AddTransient<TestObject>();

        Provider = collection.BuildServiceProvider();

        Output.WriteLine($"{MethodBase.GetCurrentMethod()?.Name} has completed successfully");
    }

    private IServiceProvider Provider { get; }

    [Fact]
    public void GetService_DependencyInitializer_Test()
    {
        TestObject testObject = 
            Provider.GetService<TestObject>(TestObject._dependencyInitializer);

        testObject.Should()
            .NotBeNull();

        testObject._intField.Should()
            .Be(GetValueType<int>(0));

        testObject.IntProperty.Should()
            .Be(GetValueType<int>(1));

        testObject._parentField.Should()
            .Be(GetReferenceType<TestObject>(2));

        testObject.ParentProperty.Should()
            .Be(GetReferenceType<TestObject>(3));

        Output.WriteLine($"{MethodBase.GetCurrentMethod()?.Name} has completed successfully");
    }

    [Fact]
    public void GetService_ArrayInitializer_Test()
    {
        TestObject testObject = 
            Provider.GetService<TestObject>(TestObject.Definitions);

        testObject.Should()
            .NotBeNull();

        testObject._intField.Should()
            .Be(GetValueType<int>(0));

        testObject.IntProperty.Should()
            .Be(GetValueType<int>(1));

        testObject._parentField.Should()
            .Be(GetReferenceType<TestObject>(2));

        testObject.ParentProperty.Should()
            .Be(GetReferenceType<TestObject>(3));

        Output.WriteLine($"{MethodBase.GetCurrentMethod()?.Name} has completed successfully");
    }

    [Fact]
    public void GetService_IEnumerableDefinitions_Test()
    {
        TestObject testObject =
            Provider.GetService<TestObject>(TestObject.EnumerableDefinitions);

        testObject.Should()
            .NotBeNull();

        testObject._intField.Should()
            .Be(GetValueType<int>(0));

        testObject.IntProperty.Should()
            .Be(GetValueType<int>(1));

        testObject._parentField.Should()
            .Be(GetReferenceType<TestObject>(2));

        testObject.ParentProperty.Should()
            .Be(GetReferenceType<TestObject>(3));

        Output.WriteLine($"{MethodBase.GetCurrentMethod()?.Name} has completed successfully");
    }

    private TValue GetValueType<TValue>(int index)
        where TValue : struct
        => ((DependencyInitializer.ValueType<TestObject, TValue>)TestObject.Definitions[index])
            .Value;

    private TValue GetReferenceType<TValue>(int index)
        where TValue : class
        => ((DependencyInitializer.ReferenceType<TestObject, TValue>)TestObject.Definitions[index])
            .Value;
}
