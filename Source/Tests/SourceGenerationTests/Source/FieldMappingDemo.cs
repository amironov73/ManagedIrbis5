// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable PartialTypeWithSinglePart

/* FieldMappingDemo.cs -- демонстрация работы маппера поля записи в данные
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis;
using ManagedIrbis.Mapping;

#endregion

namespace SourceGenerationTests;

internal partial class FieldMappingDemo
{
    #region Private members

    [FieldMapper]
    private partial Canary ConvertForward (Field from, Canary to);

    [FieldMapper]
    private partial Field ConvertBackward (Canary from, Field to);

    #endregion

    #region Public methods

    public void DoMapping()
    {
        var field = new Field
        {
            Subfields =
            {
                new SubField { Code = 'a', Value = "SubA" },
                new SubField { Code = 'b', Value = "321" },
                new SubField { Code = 'c', Value = "True" },
                new SubField { Code = 'd', Value = "SubD" },
                new SubField { Code = 'z', Value = "SubZ" },
            }
        };
        var canary = new Canary
        {
            Fourth = "Fourth"
        };
        var result1 = ConvertForward (field, canary);
        Console.WriteLine (result1);

        canary.First = null!;
        canary.Second = 123;
        canary.Third = false;
        canary.Fourth = "DSub";
        var result2 = ConvertBackward (canary,
            new Field { Tag = 100, Subfields =
            {
                new SubField { Code = 'z', Value = "ZSub" }
            }});
        Console.WriteLine (result2);
    }

    #endregion
}
