// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ManagedIrbis.Mapping;

[AttributeUsage (AttributeTargets.Property)]
public sealed class SubFieldAttribute
    : Attribute
{
    public char Code { get; }

    public SubFieldAttribute (char code)
    {
        Code = code;
    }
}
