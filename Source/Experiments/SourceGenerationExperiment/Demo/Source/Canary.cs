// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable PartialTypeWithSinglePart

using ManagedIrbis.Mapping;

namespace Demo;

// атрибут CLSCompliant здесь только лишь для создания информационного шума
// проверить, что генератор не покупается на посторонние атрибуты
[CLSCompliant (false)]
internal partial class Canary
{
    [SubField ('a')]
    public string? First { get; set; }

    [CLSCompliant (false)]
    public string? NotMapped { get; set; }

    [SubField ('b')]
    public string? Second { get; set; }

    [SubField ('c')]
    public string? Third { get; set; }

    public string? Fourth { get; set; }

    public override string ToString() => $"'{First}' '{Second}' '{Third}' '{Fourth}'";
}
