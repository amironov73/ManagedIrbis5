// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftUtility.cs -- полезные методы для форматтера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Полезные методы для форматтера.
/// </summary>
internal static class PftUtility
{
    #region Public methods

    /*

     Когда система форматирует поле, содержащее подполе, в режимах
     заголовка или данных, она автоматически заменяет имеющиеся
     разделители подполей знаками пунктуации (при этом разделитель
     первого подполя, если он имеется, всегда игнорируется).
     Специальная комбинация символов "><" заменяется на "; "
     (а отдельные символы “<” и “>” подавляются), обеспечивая простой
     способ форматирования полей, содержащих перечень ключевых фраз,
     заключенных в угловые скобки.

     Таблица стандартного замещения разделителей подполей выглядит так:

     ^a                замещается на "; "
     от ^b до ^i       замещается на ", "
     все другие        замещаются на ". "

     */

    public static void HeaderMode
        (
            PftContext context,
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return;
        }

        Span<char> buffer = stackalloc char[1024];
        using var builder = new ValueStringBuilder (buffer);
        builder.Append (text);
        builder.Replace ("><", "; ");
        builder.Replace ("<", string.Empty);
        builder.Replace (">", string.Empty);
        context.Write (builder.AsSpan());
    }

    /// <summary>
    /// Режим заголовка обычно используется для печати заголовков
    /// при выводе указателей и таблиц. Все управляющие символы,
    /// введенные вместе с данными, такие как разделители терминов
    /// (&lt; и &gt;) игнорируются (за исключением указанных ниже
    /// случаев), а разделители подполей заменяются знаками пунктуации.
    /// </summary>
    public static void HeaderMode
        (
            PftContext context,
            IList<SubField> subfields
        )
    {
        if (subfields.Count == 0)
        {
            return;
        }

        HeaderMode (context, subfields[0].Value);
        var code = char.ToLower (subfields[0].Code);
        for (var index = 1; index < subfields.Count; index++)
        {
            var delimiter = ". ";
            switch (code)
            {
                case 'a':
                    delimiter = "; ";
                    break;

                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                    delimiter = ", ";
                    break;
            }

            context.Write (delimiter);
            var value = subfields[index].Value;
            HeaderMode (context, value);
            code = char.ToLower(subfields[index].Code);
        }
    }

    #endregion
}
