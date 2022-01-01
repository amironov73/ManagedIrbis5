// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* NewLine.cs -- работа с переводом строки в разных системах
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Text;

//
// Carriage Return = ASCII 13 (0x0D), '\r'
// Line Feed       = ASCII 10 (0x0A), '\n'
//

/// <summary>
/// Работа с переводом строки в разных системах.
/// </summary>
public static class NewLine
{
    #region Constants

    /// <summary>
    /// Apple.
    /// </summary>
    public const string Apple = "\r";

    /// <summary>
    /// Carriage Return.
    /// </summary>
    public const string CarriageReturn = "\r";

    /// <summary>
    /// Carriage Return.
    /// </summary>
    public const string CR = "\r";

    /// <summary>
    /// Carriage Return + Line Feed.
    /// </summary>
    public const string CRLF = "\r\n";

    /// <summary>
    /// Line Feed.
    /// </summary>
    public const string LF = "\n";

    /// <summary>
    /// Line Feed.
    /// </summary>
    public const string LineFeed = "\n";

    /// <summary>
    /// Linux.
    /// </summary>
    public const string Linux = "\n";

    /// <summary>
    /// Mac OS.
    /// </summary>
    public const string MacOS = "\r";

    /// <summary>
    /// MS-DOS.
    /// </summary>
    public const string MsDos = "\r\n";

    /// <summary>
    /// UNIX.
    /// </summary>
    public const string Unix = "\n";

    /// <summary>
    /// Windows.
    /// </summary>
    public const string Windows = "\r\n";

    #endregion

    #region Public methods

    /// <summary>
    /// Change MS-DOS to UNIX line endings.
    /// </summary>
    public static string? DosToUnix
        (
            this string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        if (!text.Contains (MsDos))
        {
            return text;
        }

        var result = text.Replace
            (
                MsDos,
                Unix
            );

        return result;
    }

    /// <summary>
    /// Remove NewLine symbol.
    /// </summary>
    public static string? RemoveLineBreaks
        (
            this string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        if (!text.Contains (CarriageReturn)
            && !text.Contains (LineFeed))
        {
            return text;
        }

        var result = new StringBuilder (text);

        result.Replace (CarriageReturn, string.Empty);
        result.Replace (LineFeed, string.Empty);

        return result.ToString();
    }

    /// <summary>
    /// Change UNIX to MS-DOS line endings.
    /// </summary>
    public static string? UnixToDos
        (
            this string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        if (text.Contains (MsDos) || !text.Contains (Unix))
        {
            return text;
        }

        var result = text.Replace
            (
                Unix,
                MsDos
            );

        return result;
    }

    #endregion
}
