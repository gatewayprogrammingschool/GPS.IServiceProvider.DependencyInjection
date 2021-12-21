namespace GPS.IServiceProvider.Extensions.Tests;

using System.Collections.Generic;
using System.Linq;

public sealed class TestObject
{
    public int _intField;
    public int IntProperty { get; set; }

    public TestObject _parentField = null;
    public TestObject ParentProperty { get; set; } = null;

    static TestObject()
        => TestObject._dependencyInitializer = new DependencyInitializer
        {
            Definitions = new List<DependencyInitializer.Definition>(
                TestObject.Definitions)
        };

    public static readonly DependencyInitializer.Definition[] Definitions =
    {
        new DependencyInitializer.ValueType<TestObject, int>(
            nameof(TestObject._intField),
            int.MinValue
        ),
        new DependencyInitializer.ValueType<TestObject, int>(
            nameof(TestObject.IntProperty),
            int.MaxValue
        ),
        new DependencyInitializer.ReferenceType<TestObject, TestObject>(
            nameof(TestObject._parentField),
            new TestObject()
        ),
        new DependencyInitializer.ReferenceType<TestObject, TestObject>(
            nameof(TestObject.ParentProperty),
            new TestObject()
        ),
    };

    public static DependencyInitializer _dependencyInitializer;

    public static IEnumerable<DependencyInitializer.Definition> EnumerableDefinitions
        => TestObject.Definitions.AsEnumerable();
}
