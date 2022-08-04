// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IsisText.cs -- специфичное для ISIS текстовое представление записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.ImportExport;

/// <summary>
/// Специфичное для ISIS текстовое представление записей.
/// </summary>
public static class IsisText
{
    /// <summary>
    /// Формирует текстовое представление записи,
    /// характерное для ISIS.
    /// </summary>
    public static string ToIsisText
        (
            this Record record
        )
    {
        Sure.NotNull (record);

        var builder = StringBuilderPool.Shared.Get();
        foreach (var field in record.Fields)
        {
            builder.AppendFormat
                (
                    "<{0}>{1}</{0}>",
                    field.Tag,
                    field.ToText()
                );
            builder.AppendLine();
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }
}
