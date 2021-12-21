// ReSharper disable UnusedMember.Global
namespace GPS.Microsoft.Extensions.DependencyInjection;

using System.Reflection;

public class DependencyInitializer
{
#if NET5_0_OR_GREATER
    public List<Definition> Definitions { get; init; } = new();
#else
    public List<Definition> Definitions { get; set; } = new();
#endif

    public DependencyInitializer()
    {

    }

    public DependencyInitializer(params Definition[] definitions)
    {
        Definitions.AddRange(definitions);
    }

    public DependencyInitializer(IEnumerable<Definition> definitions)
    {
        Definitions.AddRange(definitions);
    }

    public abstract class Definition
    {
        protected const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public;

        public string Name { get; set; }
    }

    private static class Setter<TOwner>
    {
        public static void SetValue<TValue>(TOwner owner, ValueType<TOwner, TValue> definition)
            where TValue : struct
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (definition.MemberInfo.MemberType)
            {
                case MemberTypes.Property:
                    PropertyInfo pi = (PropertyInfo)definition.MemberInfo;

                    pi.SetValue(owner, definition.Value);

                    break;

                case MemberTypes.Field:
                    FieldInfo fi = (FieldInfo)definition.MemberInfo;

                    fi.SetValue(owner, definition.Value);

                    break;
            }
        }

        public static void SetValue<TValue>(TOwner owner, ReferenceType<TOwner, TValue> definition)
            where TValue : class
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (definition.MemberInfo.MemberType)
            {
                case MemberTypes.Property:
                    PropertyInfo pi = (PropertyInfo)definition.MemberInfo;

                    pi.SetValue(owner, definition.Value);

                    break;

                case MemberTypes.Field:
                    FieldInfo fi = (FieldInfo)definition.MemberInfo;

                    fi.SetValue(owner, definition.Value);

                    break;
            }
        }

    }

    public interface ISetter<in TOwner>
    {
        void Set(TOwner owner);
    }

    public class ValueType<TOwner, TValue> :
        Definition,
        ISetter<TOwner>
        where TValue : struct
    {
        public ValueType(string name, TValue value)
        {
            Type type = typeof(TOwner);

            MemberInfo[] mi = type.GetMember(name, Definition.FLAGS);

            if (mi is null
                or
                {
                    Length: 0,
                })
            {
                throw new InvalidInitializerDefinition(type, name);
            }

            MemberInfo = mi[0];
            Name = name;
            Value = value;
        }

        public MemberInfo MemberInfo { get; set; }

        public TValue Value { get; set; }

        public void Set(TOwner owner)
            => Setter<TOwner>.SetValue(owner, this);
    }

    public class ReferenceType<TOwner, TValue> :
        Definition,
        ISetter<TOwner>
        where TValue : class
    {
        public ReferenceType(string name, TValue value)
        {
            Type type = typeof(TOwner);

            MemberInfo[] mi = type.GetMember(name, Definition.FLAGS);

            if (mi is null
                or
                {
                    Length: 0,
                })
            {
                throw new InvalidInitializerDefinition(type, name);
            }

            MemberInfo = mi[0];
            Name = name;
            Value = value;
        }

        public MemberInfo MemberInfo { get; set; }

        public TValue Value { get; set; }

        public void Set(TOwner owner)
            => Setter<TOwner>.SetValue(owner, this);
    }

    public void Apply<TOwner>(TOwner owner)
    {
        foreach (var definition in Definitions)
        {
            if (definition is ISetter<TOwner> setter)
            {
                setter.Set(owner);
            }
            else
            {
                throw new ArgumentException(
                    $"{definition.GetType().Name} expects TOwner to be {definition.GetType().GenericTypeArguments.FirstOrDefault()?.FullName ?? "<< no type >>"} but received {typeof(TOwner).FullName}"
                );
            }
        }
    }
}
