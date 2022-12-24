// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StorageItemToImageConverter.cs -- преобразует IStorageItem в Bitmap
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Data.Converters;

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

#endregion

#nullable enable

namespace AM.Avalonia.Converters;

/// <summary>
/// Преобразует <see cref="IStorageItem"/> в <see cref="Bitmap"/>.
/// </summary>
public sealed class StorageItemToImageConverter
    : IValueConverter
{
    #region Properties

    /// <summary>
    /// Общий экземпляр конвертера.
    /// </summary>
    public static readonly IValueConverter Instance = new StorageItemToImageConverter();

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
        if (value is IStorageItem item && item.TryGetUri (out var uri))
        {
            try
            {
                using var stream = File.OpenRead (uri.LocalPath);
                var image = new Bitmap (stream);

                return image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine (ex);
            }
        }

        return null;
    }

    /// <inheritdoc cref="ConvertBack"/>
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
