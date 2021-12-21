namespace GPS.IServiceProvider.Extensions;

public class InvalidInitializerDefinition : Exception
{
    public InvalidInitializerDefinition(Type ownerType, string name)
        : base($"{ownerType} does not contain a public, instance field " +
               $"or property named {name}.")
    {
    }
}
