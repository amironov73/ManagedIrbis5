// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable PartialTypeWithSinglePart

/* Canary.cs -- канареечный класс для маппинга данных на поле записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Mapping;

#endregion

namespace SourceGenerationTests;

// атрибут CLSCompliant здесь только лишь для создания информационного шума
// проверить, что генератор не покупается на посторонние атрибуты
[CLSCompliant (false)]
internal partial class Canary
{
    [SubField ('a')]
    public string? First { get; set; }

    // поле с посторонним атрибутом
    [CLSCompliant (false)]
    public string? NotMapped { get; set; }

    [SubField ('b')]
    public int Second { get; set; }

    [SubField ('c')]
    public bool Third { get; set; }

    // свойство вообще без атрибутов
    public string? Fourth { get; set; }

    public override string ToString() => $"'{First}' '{Second}' '{Third}' '{Fourth}'";
}
