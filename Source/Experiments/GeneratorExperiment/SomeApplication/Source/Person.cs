// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using ManagedIrbis;

#endregion

namespace SomeApplication;

[GenerateAccessor]
public partial class Person
{
    [SubField('a')]
    public string? Name { get; set; }

    [SubField('b')]
    public int Age { get; set; }

    partial void FromFieldInternal (Field field);
    partial void ToFieldInternal (Field field);

    public void FromField (Field field) => FromFieldInternal(field);

    public void ToField (Field field) => ToFieldInternal(field);

    public override string ToString()
    {
        return $"{nameof(Name)}: '{Name}', {nameof(Age)}: {Age}";
    }
}
