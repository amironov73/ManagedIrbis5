// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* StringToImageConverter.cs -- преобразует строку в картинку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Data.Converters;

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;

using Avalonia.Media.Imaging;

#endregion

#nullable enable

namespace TheNude;

/// <summary>
/// Преобразует строку в картинку.
/// </summary>
internal sealed class StringToImageConverter
    : IValueConverter
{
    public static readonly IValueConverter Instance = new StringToImageConverter();

    public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string url)
        {
            try
            {
                var client = new HttpClient();
                var bytes = client.GetByteArrayAsync (url).Result;
                var stream = new MemoryStream (bytes);
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

    public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
