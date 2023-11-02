// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ManagedIrbis.Mapping;

[AttributeUsage (AttributeTargets.Property)]
public sealed class FieldAttribute
    : Attribute
{
    public int Tag { get; }

    public char Code { get; }

    public FieldAttribute (int tag)
    {
        Tag = tag;
    }

    public FieldAttribute (int tag, char code)
    {
        Tag = tag;
        Code = code;
    }
}
