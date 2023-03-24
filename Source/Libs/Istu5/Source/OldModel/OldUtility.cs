// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* OldUtility.cs -- полезные методы для старой модели
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;

using JetBrains.Annotations;

using SkiaSharp;
using SkiaSharp.QrCode.Image;

#endregion

#nullable enable

namespace Istu.OldModel;

/// <summary>
/// Полезные методы для старой модели.
/// </summary>
[PublicAPI]
public static class OldUtility
{
    #region Public methods

    /// <summary>
    /// Сохранение QR-кода в указанный файл.
    /// </summary>
    public static void SaveQrCodeToFile
        (
            string ticket,
            string fileName
        )
    {
        Sure.NotNullNorEmpty (ticket);
        Sure.NotNullNorEmpty (fileName);

        using var output = new FileStream
            (
                fileName,
                FileMode.OpenOrCreate
            );

        var qrCode = new QrCode
            (
                ticket,
                new Vector2Slim (256, 256),
                SKEncodedImageFormat.Png
            );

        qrCode.GenerateImage (output);
    }

    /// <summary>
    /// Сохранение QR-кода в оперативную
    /// память в формате PNG.
    /// </summary>
    public static byte[] SaveQrCodeToMemory
        (
            string ticket
        )
    {
        Sure.NotNullNorEmpty (ticket);

        using var output = new MemoryStream();

        var qrCode = new QrCode
            (
                ticket,
                new Vector2Slim (256, 256),
                SKEncodedImageFormat.Png
            );

        qrCode.GenerateImage (output);

        return output.ToArray();
    }

    /// <summary>
    /// Сохранение QR-кода в указанный файл.
    /// </summary>
    public static void SaveQrCodeToFile
        (
            Reader reader,
            string fileName
        )
    {
        Sure.NotNull (reader);
        Sure.NotNullNorEmpty (fileName);

        var ticket = reader.Ticket.ThrowIfNullOrEmpty();
        SaveQrCodeToFile (ticket, fileName);
    }

    /// <summary>
    /// Сохранение QR-кода в оперативную
    /// память в формате PNG.
    /// </summary>
    public static byte[] SaveQrCodeToMemory
        (
            Reader reader
        )
    {
        Sure.NotNull (reader);

        var ticket = reader.Ticket.ThrowIfNullOrEmpty();

        return SaveQrCodeToMemory (ticket);
    }

    #endregion
}
