// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StorageItemUriConverter.cs -- преобразует IStorageItem в Uri
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Data.Converters;

using System;
using System.Globalization;

using Avalonia.Platform.Storage;

#endregion

#nullable enable

namespace AM.Avalonia.Converters;

/// <summary>
/// Преобразует <see cref="IStorageItem"/> в <see cref="Uri"/>.
/// </summary>
public sealed class StorageItemToUriConverter
    : IValueConverter
{
    #region Properties

    /// <summary>
    /// Общий экземпляр конвертера.
    /// </summary>
    public static readonly IValueConverter Instance = new StorageItemToUriConverter();

    #endregion

    #region IValueConverter members

    /// <inheritdoc cref="IValueConverter.Convert"/>
    public object? Convert
        (
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
    {
        // if (value is IStorageItem item && item.TryGetUri (out var uri))
        // {
        //     return uri.ToString();
        // }

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
        throw new NotImplementedException();
    }

    #endregion
}
