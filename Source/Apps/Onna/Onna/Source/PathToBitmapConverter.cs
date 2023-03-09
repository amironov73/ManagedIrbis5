// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PathToBitmapConverter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

#endregion

namespace Onna;

internal sealed class PathToBitmapConverter
    : IValueConverter
{
    public object? Convert
        (
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
    {
        if (value is string path)
        {
            try
            {
                using var stream = File.OpenRead (path);
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
}
