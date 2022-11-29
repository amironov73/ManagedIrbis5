// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ArrayToListConverter.cs -- преобразует массив в строку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Data.Converters;

using System;
using System.Collections.Generic;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Avalonia.Converters;

/// <summary>
/// Преобразует массив в строку.
/// </summary>
public sealed class ArrayToListConverter
    : IValueConverter
{
    /// <summary>
    /// Общий экземпляр конвертера.
    /// </summary>
    public static readonly IValueConverter Instance = new ArrayToListConverter();

    /// <inheritdoc cref="IValueConverter.Convert"/>
    public object? Convert
        (
            object? value,
            Type targetType,
            object? parameter, CultureInfo culture
        )
    {
        if (value is IEnumerable<string> enumerable)
        {
            return string.Join (", ", enumerable);
        }

        return null;
    }

    /// <inheritdoc cref="IValueConverter.ConvertBack"/>
    public object? ConvertBack
        (
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
    {
        return null;
    }
}
