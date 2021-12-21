namespace GPS.IServiceProvider.Extensions.Tests;

using System.Collections.Generic;
using System.Collections.Immutable;

using System.Linq;

public sealed class TestObject
{
    public int _intField;
    public int IntProperty { get; set; }

    public TestObject _parentField;
    public TestObject ParentProperty { get; set; }

    static TestObject()
    {
        _definitions =
            new DependencyInitializer.Definition[] {
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
                )
            }.ToImmutableArray();

        TestObject.DependencyInitializer = new DependencyInitializer
        {
            Definitions = new List<DependencyInitializer.Definition>(
          TestObject.Definitions)
        };
    }

    public static DependencyInitializer.Definition[] Definitions => _definitions.ToArray();

    public static readonly ImmutableArray<DependencyInitializer.Definition> _definitions;

    public static IEnumerable<DependencyInitializer.Definition> EnumerableDefinitions
        => _definitions.AsEnumerable();

    public static DependencyInitializer DependencyInitializer { get; set; }
}
