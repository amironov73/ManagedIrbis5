// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* IrbisText.cs -- работа с текстом, специфичная для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Работа с текстом, специфичная для ИРБИС.
/// </summary>
public static class IrbisText
{
    #region Constants

    /// <summary>
    /// Стандартный разделитель строк ИРБИС.
    /// </summary>
    public const string IrbisDelimiter = "\x001F\x001E";

    /// <summary>
    /// Стандартный разделитель строк ИРБИС.
    /// </summary>
    public static readonly byte[] IrbisDelimiterBytes = { 0x1F, 0x1E };

    /// <summary>
    /// Стандартный разделитель строк в DOS/Windows.
    /// </summary>
    public const string WindowsDelimiter = "\r\n";

    /// <summary>
    /// Стандартный разделитель строк в DOS/Windows.
    /// </summary>
    public static readonly byte[] WindowsDelimiterBytes = { 13, 10 };

    #endregion

    #region Private members

    // короткий разделитель строк в ИРБИС
    private static readonly char[] _delimiters = { '\x1F' };

    private static string _CleanupEvaluator
        (
            Match match
        )
    {
        var length = match.Value.Length;

        if ((length & 1) == 0)
        {
            return new string('.', length / 2);
        }

        return new string('.', length / 2 + 2);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка текста от команд форматирования [[]].
    /// </summary>
    public static string? CleanupMarkup
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty(text)
            || !text.Contains("[["))
        {
            return text;
        }

        while (true)
        {
            // Remove repeating area delimiters.
            var result = Regex.Replace
                (
                    text,
                    @"\[\[(?<tag>.*?)\]\](?<meat>.*?)\[\[/\k<tag>\]\]",
                    "${meat}"
                );
            if (result == text)
            {
                text = result;
                break;
            }

            text = result;
        }

        return text;
    }

    /// <summary>
    /// Очистка текста от различных ИРБИС-артефактов.
    /// </summary>
    public static string? CleanupText
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        // Удаление задвоившихся разделителей областей биб. описания
        var result = Regex.Replace
            (
                text,
                @"(\.\s-\s){2,}",
                ". - "
            );

        // Удаление задвоившихся точек
        result = Regex.Replace
            (
                result,
                @"\.{2,}",
                _CleanupEvaluator
            );

        // Удаление "повисших" разделителей областей биб. описания
        // (в конце параграфа)
        result = Regex.Replace
            (
                result,
                @"(\.\s-\s)+(<br>|<br\s*/>|\\par|\x0A|\x0D\x0A)",
                "$2"
            );

        return result;
    }

    /// <summary>
    /// Преобразование переводов строк ИРБИС в Windows.
    /// </summary>
    public static string? IrbisToWindows
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        if (!text.Contains(IrbisDelimiter))
        {
            return text;
        }

        var result = text.Replace
            (
                IrbisDelimiter,
                WindowsDelimiter
            );

        return result;
    }

    /// <summary>
    /// Заменяет переводы строк ИРБИС на Windows.
    /// Замена происходит на месте.
    /// </summary>
    public static void IrbisToWindows
        (
            byte[]? text
        )
    {
        if (text.IsNullOrEmpty())
        {
            return;
        }

        var index = 0;
        while (true)
        {
            index = Utility.IndexOf(text, IrbisDelimiterBytes, index);
            if (index < 0)
            {
                break;
            }

            Array.Copy(WindowsDelimiterBytes, 0, text, index, WindowsDelimiterBytes.Length);
        }
    }

    /// <summary>
    /// Разбивает текст на строки в соответствии с ИРБИС-разделителями.
    /// </summary>
    public static string[] SplitIrbisToLines
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty(text))
        {
            return Array.Empty<string>();
        }

        var provenText = IrbisToWindows(text)!;
        var result = string.IsNullOrEmpty(provenText)
            ? new[] { string.Empty }
            : provenText.Split
                (
                    _delimiters,
                    StringSplitOptions.None
                );

        return result;
    }

    /// <summary>
    /// Преобразует текст в нижний регистр.
    /// </summary>
    public static string? ToLower
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var result = text.ToLowerInvariant();

        return result;
    }

    /// <summary>
    /// Преобразует текст в верхний регистр.
    /// </summary>
    public static string? ToUpper
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        // TODO use isisucw.txt ?

        var result = text.ToUpperInvariant();

        return result;
    }

    /// <summary>
    /// Заменяет переводы строк DOS/Windows на ИРБИС.
    /// </summary>
    public static string? WindowsToIrbis
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        if (!text.Contains(WindowsDelimiter))
        {
            return text;
        }

        var result = text.Replace
            (
                WindowsDelimiter,
                IrbisDelimiter
            );

        return result;
    }

    /// <summary>
    /// Заменяет переводы строк Windows на ИРБИС.
    /// Замена происходит на месте.
    /// </summary>
    public static void WindowsToIrbis
        (
            byte[]? text
        )
    {
        if (text.IsNullOrEmpty())
        {
            return;
        }

        var index = 0;
        while (true)
        {
            index = Utility.IndexOf(text, WindowsDelimiterBytes, index);
            if (index < 0)
            {
                break;
            }

            Array.Copy(IrbisDelimiterBytes, 0, text, index, IrbisDelimiterBytes.Length);
        }
    }

    #endregion
}
