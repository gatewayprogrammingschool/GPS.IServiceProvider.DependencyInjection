// ReSharper disable UnusedMember.Global
namespace GPS.Microsoft.Extensions.DependencyInjection;

public static class ServiceContainerExtensions
{
    public static TType GetService<TType>(
        this IServiceProvider provider,
        DependencyInitializer initializer
    )
    {
        TType result = (TType)provider.GetService(typeof(TType));

        if (result is not null)
        {
            result.ApplyInitializer(initializer);
        }

        return result;
    }

    public static TType GetService<TType>(
        this IServiceProvider provider,
        params DependencyInitializer.Definition[] definitions
    )
    {
        TType result = (TType)provider.GetService(typeof(TType));

        if (result is not null)
        {
            result.ApplyInitializer(definitions);
        }

        return result;
    }

    public static TType GetService<TType>(
        this IServiceProvider provider,
        IEnumerable<DependencyInitializer.Definition> definitions
    )
    {
        TType result = (TType)provider.GetService(typeof(TType));

        if (result is not null)
        {
            result.ApplyInitializer(definitions);
        }

        return result;
    }

    internal static void ApplyInitializer<TType>(
        this TType injected,
        DependencyInitializer initializer)
    {
        initializer.Apply(injected);
    }

    internal static void ApplyInitializer<TType>(
        this TType injected,
        params DependencyInitializer.Definition[] definitions)
    {
        DependencyInitializer initializer = new (definitions);
        initializer.Apply(injected);
    }

    internal static void ApplyInitializer<TType>(
        this TType injected,
        IEnumerable<DependencyInitializer.Definition> definitions
    )
    {
        DependencyInitializer initializer = new(definitions);
        initializer.Apply(injected);
    }
}