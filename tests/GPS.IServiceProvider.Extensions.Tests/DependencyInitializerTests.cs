#pragma warning disable CS0649

namespace GPS.IServiceProvider.Extensions.Tests;

using System;
using System.Reflection;

using Xunit;
using Xunit.Abstractions;

using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

public partial class DependencyInitializerTests
{
    public ITestOutputHelper Output
    {
        get;
    }

    public DependencyInitializerTests(ITestOutputHelper output)
    {
        Output = output;

        TestObject._dependencyInitializer = new DependencyInitializer
        {
            Definitions = new List<DependencyInitializer.Definition>(TestObject.Definitions),
        };

        IEnumerable<string> definitionNames
            = TestObject.Definitions.Select(
                static (definition, i) => $"{i}: {definition.Name}");

        Output.WriteLine($"Definition Names: [{string.Join(",", definitionNames)}]");

        Output.WriteLine($"{MethodBase.GetCurrentMethod().Name} has completed successfully");
    }

    [Fact]
    public void DependencyInitializer_DefaultConstructor_Test()
    {
        DependencyInitializer initializer = new();

        initializer
            .Should()
            .NotBeNull();

        initializer
            .Definitions
            .Should()
            .NotBeNull();

        Output.WriteLine($"{MethodBase.GetCurrentMethod().Name} has completed successfully");
    }


    [Fact]
    public void DependencyInitializer_ArrayConstructor_Test()
    {
        DependencyInitializer initializer = new(TestObject.Definitions);

        initializer
            .Should()
            .NotBeNull();

        initializer
            .Definitions
            .Should()
            .NotBeNullOrEmpty();

        initializer
            .Definitions
            .Should()
            .BeEquivalentTo(TestObject.Definitions);

        Output.WriteLine($"{MethodBase.GetCurrentMethod().Name} has completed successfully");
    }

    [Fact]
    public void DependencyInitializer_IEnumerableConstructor_Test()
    {
        DependencyInitializer initializer = new(TestObject.EnumerableDefinitions);

        initializer
            .Should()
            .NotBeNull();

        initializer
            .Definitions
            .Should()
            .NotBeNullOrEmpty();

        initializer
            .Definitions
            .Should()
            .BeEquivalentTo(TestObject.EnumerableDefinitions);

        Output.WriteLine($"{MethodBase.GetCurrentMethod().Name} has completed successfully");
    }

    [Fact]
    public void ApplyTest()
    {
        TestObject testObject = new();

        TestObject._dependencyInitializer.Apply(testObject);

        testObject._intField.Should()
            .Be(GetValueType<int>(0));

        testObject.IntProperty.Should()
            .Be(GetValueType<int>(1));

        testObject._parentField.Should()
            .Be(GetReferenceType<TestObject>(2));

        testObject.ParentProperty.Should()
            .Be(GetReferenceType<TestObject>(3));

        Output.WriteLine($"{MethodBase.GetCurrentMethod().Name} has completed successfully");
    }

    private TValue GetValueType<TValue>(int index)
        where TValue : struct
        => ((DependencyInitializer.ValueType<TestObject, TValue>)TestObject.Definitions[index]).Value;

    private TValue GetReferenceType<TValue>(int index)
        where TValue : class
        => ((DependencyInitializer.ReferenceType<TestObject, TValue>)TestObject.Definitions[index]).Value;

    [Fact]
    public void Apply_InvalidObject_Test()
    {
        Action invalidApply = static () =>
        {
            TestObject._dependencyInitializer.Apply(
                new
                {
                }
            );
        };

        invalidApply.Should()
            .Throw<ArgumentException>();
    }
}

public partial class DependencyInitializerTests
{
    [ Fact]
    public void ValueType_InvalidMember_Test()
    {
        Action createInvalidly = static () =>
        {
            _ = new DependencyInitializer.ValueType<TestObject, int>("invalid", 0);
        };

        createInvalidly.Should()
            .Throw<InvalidInitializerDefinition>();
    }

    [ Fact]
    public void ReferenceType_InvalidMember_Test()
    {
        Action createInvalidly = static () =>
        {
            _ = new DependencyInitializer.ReferenceType<TestObject, TestObject>("invalid", new TestObject());
        };

        createInvalidly.Should()
            .Throw<InvalidInitializerDefinition>();
    }
}