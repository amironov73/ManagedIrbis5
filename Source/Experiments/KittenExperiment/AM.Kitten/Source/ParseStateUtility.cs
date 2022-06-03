// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ParserStateUtility.cs -- работа с состоянием разбора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.Kitten;

/// <summary>
/// Работа с состоянием разбора.
/// </summary>
public static class ParseStateUtility
{
    /// <summary>
    /// Вывод текущего символа (для отладки)
    /// </summary>
    public static void DumpChar
        (
            this ParseState<char> state,
            TextWriter? output = null
        )
    {
        output ??= Console.Out;

        if (state.HasCurrent)
        {
            var chr = state.Current;
            var code = (int) chr;
            output.WriteLine
                (
                    chr >= ' '
                        ? $"'{chr}' => {code}"
                        : $"=> {code}"
                );
        }
        else
        {
            output.WriteLine ("no current symbol");
        }
    }

    /// <summary>
    /// Проглатывание указанного символа (если есть).
    /// </summary>
    public static void EatChar
        (
            this ParseState<char> state,
            char chr
        )
    {
        if (state.HasCurrent)
        {
            if (state.Current == chr)
            {
                state.Advance ();
            }
        }
    }

    /// <summary>
    /// Пропуск пробелов.
    /// </summary>
    public static void EatWhitespace
        (
            this ParseState<char> state
        )
    {
        while (state.HasCurrent)
        {
            if (!char.IsWhiteSpace (state.Current))
            {
                break;
            }

            state.Advance();
        }
    }

    /// <summary>
    /// Считывание символа.
    /// </summary>
    /// <param name="state">Состояние.</param>
    /// <param name="advance">Надо ли продвигаться вперед?</param>
    /// <returns>Прочитанный символ либо <c>\0</c>.</returns>
    public static char ReadChar
        (
            this ParseState<char> state,
            bool advance = true
        )
    {
        var result = '\0';
        if (state.HasCurrent)
        {
            result = state.Current;
            if (advance)
            {
                state.Advance ();
            }
        }

        return result;
    }
}
