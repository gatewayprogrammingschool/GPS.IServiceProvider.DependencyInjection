# GPS MS Extensions Dependency Injection Helpers

Allows initialization of objects obtained from IServiceProvider.

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