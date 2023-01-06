// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Sparcer.cs -- удаляет из текста ненужные пробелы, добавляет нужные
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.Text;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Удаляет из текста ненужные пробелы, добавляет нужные.
/// </summary>
public sealed class Sparcer
{
    #region Properties

    /// <summary>
    /// Добавлять пробелы после и удалять пробелы перед.
    /// </summary>
    public string SpaceAfter = ",.])!?:;";

    /// <summary>
    /// Добавлять пробелы перед и удалять пробелы после.
    /// </summary>
    public string SpaceBefore = "[(";

    #endregion

    #region Public methods

    /// <summary>
    /// Обработка текста.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [return: NotNullIfNotNull (nameof (input))]
    public string? SparceText
        (
            string? input
        )
    {
        if (input is null)
        {
            return input;
        }

        input = input.Trim();
        if (input.Length == 0)
        {
            return input;
        }

        var result = new StringBuilder (input.Length);
        var quot = false;
        var apos = false;

        for (var i = 0; i < input.Length; i++)
        {
            var chr = input[i];
            var next = i < input.Length - 1 ? input[i + 1] : '\0';
            var prev = i > 0 ? input[i - 1] : '\0';
            if (chr == ' ')
            {
                if (next == ' ')
                {
                    continue;
                }

                if (SpaceAfter.Contains (next))
                {
                    continue;
                }
            }
            else if (chr == '"')
            {
                if (quot)
                {
                    result.Append (chr);
                    if (next != ' '
                        && !SpaceAfter.Contains (next))
                    {
                        result.Append (' ');
                    }
                }
                else
                {
                    if (prev != ' ')
                    {
                        result.Append (' ');
                    }

                    result.Append (chr);
                }

                quot = !quot;
                continue;
            }
            else if (chr == '\'')
            {
                if (apos)
                {
                    result.Append (chr);
                    if (next != ' '
                        && !SpaceAfter.Contains (next))
                    {
                        result.Append (' ');
                    }
                }
                else
                {
                    if (prev != ' ')
                    {
                        result.Append (' ');
                    }

                    result.Append (chr);
                }

                apos = !apos;
                continue;
            }
            else if (SpaceBefore.Contains (chr))
            {
                if (prev != ' ' && i > 0
                                && !SpaceBefore.Contains (next)
                                && !SpaceAfter.Contains (next))
                {
                    result.Append (' ');
                }
            }
            else if (SpaceAfter.Contains (chr))
            {
                result.Append (chr);
                if (next == '-')
                {
                    continue;
                }

                if (char.IsDigit (prev) && char.IsDigit (next))
                {
                    continue;
                }

                if (next != ' '
                    && !SpaceBefore.Contains (next)
                    && !SpaceAfter.Contains (next))
                    result.Append (' ');
                {
                    continue;
                }
            }

            result.Append (chr);
        }

        return result.ToString().TrimEnd();
    }

    #endregion
}
