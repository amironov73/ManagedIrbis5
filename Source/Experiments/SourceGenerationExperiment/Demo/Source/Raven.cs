// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable PartialTypeWithSinglePart

using ManagedIrbis.Mapping;

namespace Demo;

// атрибут CLSCompliant здесь только лишь для создания информационного шума
// проверить, что генератор не покупается на посторонние атрибуты
[CLSCompliant (false)]
internal partial class Raven
{
    // пользовательский класс
    [Field (97, '7')]
    public Canary? MinusTwo { get; set; }

    // массив
    [Field (98, '8')]
    public string[]? MinusOne { get; set; }

    // список
    [Field (99, '9')]
    public IList<string>? Zero { get; set; }

    // встроенный класс - строка
    [Field (100, 'a')]
    public string? First { get; set; }

    // поле с посторонним атрибутом
    [CLSCompliant (false)]
    public string? NotMapped { get; set; }

    [Field (200, 'b')]
    public string? Second { get; set; }

    [Field (300, 'c')]
    public string? Third { get; set; }

    // свойство вообще без атрибутов
    public string? Fourth { get; set; }

    public override string ToString() => $"'{First}' '{Second}' '{Third}' '{Fourth}'";
}
