// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InvariantFormat.cs -- форматирование текста, не зависящее от культуры
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Форматирование текста, не зависящее от культуы.
/// </summary>
public static class InvariantFormat
{
    #region Private members

    private static readonly IFormatProvider _formatProvider
        = CultureInfo.InvariantCulture;

    #endregion

    #region Public methods

    /// <summary>
    /// Форматирование строки.
    /// </summary>
    public static string Format
        (
            string format,
            object? arg0
        )
    {
        Sure.NotNull (format);

        return string.Format (_formatProvider, format, arg0);
    }

    /// <summary>
    /// Форматирование строки.
    /// </summary>
    public static string Format
        (
            string format,
            object? arg0,
            object? arg1
        )
    {
        Sure.NotNull (format);

        return string.Format (_formatProvider, format, arg0, arg1);
    }

    /// <summary>
    /// Форматирование строки.
    /// </summary>
    public static string Format
        (
            string format,
            object? arg0,
            object? arg1,
            object? arg2
        )
    {
        Sure.NotNull (format);

        return string.Format (_formatProvider, format, arg0, arg1, arg2);
    }

    /// <summary>
    /// Форматирование строки.
    /// </summary>
    public static string Format
        (
            string format,
            params object[] args
        )
    {
        Sure.NotNull (format);
        Sure.NotNull (args);

        return string.Format (_formatProvider, format, args);
    }

    /// <summary>
    /// Форматирование целого числа.
    /// </summary>
    public static string Format
        (
            string format,
            int arg0
        )
    {
        Sure.NotNull (format);

        return string.Format (_formatProvider, format, arg0);
    }

    /// <summary>
    /// Форматирование числа с плавающей точкой.
    /// </summary>
    public static string Format
        (
            string format,
            double arg0
        )
    {
        Sure.NotNull (format);

        return string.Format (_formatProvider, format, arg0);
    }

    /// <summary>
    /// Форматирование числа с фиксированной точкой.
    /// </summary>
    public static string Format
        (
            string format,
            decimal arg0
        )
    {
        Sure.NotNull (format);

        return string.Format (_formatProvider, format, arg0);
    }

    /// <summary>
    /// Форматирование целого числа.
    /// </summary>
    public static string Format
        (
            int arg0
        )
    {
        return arg0.ToString (_formatProvider);
    }

    /// <summary>
    /// Форматирование числа с плавающей точкой.
    /// </summary>
    public static string Format
        (
            double arg0
        )
    {
        return arg0.ToString (_formatProvider);
    }

    /// <summary>
    /// Форматирование числа с фиксированной точкой.
    /// </summary>
    public static string Format
        (
            decimal arg0
        )
    {
        return arg0.ToString (_formatProvider);
    }

    #endregion
}
