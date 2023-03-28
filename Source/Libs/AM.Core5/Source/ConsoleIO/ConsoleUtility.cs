// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ConsoleUtility.cs -- полезные методы для System.Console
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.ConsoleIO;

/// <summary>
/// Полезные методы для <see cref="Console"/>.
/// </summary>
public static class ConsoleUtility
{
    #region Public methods

    /// <summary>
    /// Вывод заданного количества пустых строк.
    /// </summary>
    /// <param name="howMany"></param>
    public static void WriteEmptyLines
        (
            int howMany
        )
    {
        while (howMany > 0)
        {
            Console.WriteLine();
            --howMany;
        }
    }

    /// <summary>
    /// Вывод параграфа текста.
    /// </summary>
    public static void WriteParagraph
        (
            string? text,
            int linesAfter = 1,
            int linesBefore = 0
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            WriteEmptyLines (linesBefore);
            Console.Write (text);
            WriteEmptyLines (linesAfter);
        }
    }

    /// <summary>
    /// Вывод (псевдо) подчеркнутой строки.
    /// </summary>
    public static void WriteUnderlined
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            Console.WriteLine (text);
            Console.WriteLine (new string ('-', text.Length));
        }
    }

    #endregion
}
