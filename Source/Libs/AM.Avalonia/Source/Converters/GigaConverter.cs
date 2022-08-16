// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* GigaConverter.cs -- конвертер длинных целых в кило/мега/гигабайты
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

using Avalonia.Data.Converters;

#endregion

#nullable enable

namespace AM.Avalonia.Converters;

/// <summary>
/// Конвертер длинных целых в кило/мега/гигабайты.
/// </summary>
public sealed class GigaConverter
    : IValueConverter
{
    #region Properties

    /// <summary>
    /// Общий экземпляр.
    /// </summary>
    public static readonly GigaConverter Instance = new();

    #endregion

    #region Private members

    private static readonly string[] _suffixes = { "", "K", "M", "G", "T", "P" };

    private static string _ConcatSuffix
        (
            string prefix,
            string? suffix
        )
    {
        return string.IsNullOrWhiteSpace (suffix)
            ? prefix
            : prefix + " " + suffix;
    }

    private static string _ConvertToSize
        (
            long value,
            IFormatProvider culture
        )
    {
        var size = (double) value;

        foreach (var suffix in _suffixes)
        {
            if (size < 1024.0)
            {
                return _ConcatSuffix (size.ToString ("G3", culture), suffix);
            }

            size /= 1024.0;
        }

        return _ConcatSuffix (size.ToString ("G3", culture), _suffixes[^1]);
    }

    #endregion

    #region IValueConverter members

    /// <inheritdoc cref="IValueConverter.Convert"/>
    public object Convert
        (
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
    {
        if (value is long size)
        {
            return _ConvertToSize (size, culture);
        }

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IValueConverter.ConvertBack"/>
    public object ConvertBack
        (
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
    {
        throw new NotImplementedException();
    }

    #endregion
}
