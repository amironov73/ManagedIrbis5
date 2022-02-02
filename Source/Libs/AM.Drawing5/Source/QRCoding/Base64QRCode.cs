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

/* Base64QRCode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using static AM.Drawing.QRCoding.Base64QRCode;
using static AM.Drawing.QRCoding.QRCodeGenerator;

#endregion

// Поддерживается только в Windows
#pragma warning disable CA1416

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Базовая функциональность для QR-кода.
/// </summary>
[System.Runtime.Versioning.SupportedOSPlatform ("windows")]
public class Base64QRCode
    : AbstractQRCode,
    IDisposable
{
    #region Construction

    /// <summary>
    /// Constructor without params to be used in COM Objects connections
    /// </summary>
    public Base64QRCode()
    {
        _qrCode = new QRCode();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Base64QRCode
        (
            QRCodeData data
        )
        : base (data)
    {
        _qrCode = new QRCode (data);
    }

    #endregion

    #region Private members

    private readonly QRCode _qrCode;

    #endregion

    /// <summary>
    /// Установка данных.
    /// </summary>
    public override void SetQRCodeData
        (
            QRCodeData data
        )
    {
        _qrCode.SetQRCodeData (data);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pixelsPerModule"></param>
    /// <returns></returns>
    public string GetGraphic
        (
            int pixelsPerModule
        )
    {
        return GetGraphic (pixelsPerModule, Color.Black, Color.White);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColorHtmlHex"></param>
    /// <param name="lightColorHtmlHex"></param>
    /// <param name="drawQuietZones"></param>
    /// <param name="imgType"></param>
    /// <returns></returns>
    public string GetGraphic
        (
            int pixelsPerModule,
            string darkColorHtmlHex,
            string lightColorHtmlHex,
            bool drawQuietZones = true,
            ImageType imgType = ImageType.Png
        )
    {
        return GetGraphic (pixelsPerModule, ColorTranslator.FromHtml (darkColorHtmlHex),
            ColorTranslator.FromHtml (lightColorHtmlHex), drawQuietZones, imgType);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColor"></param>
    /// <param name="lightColor"></param>
    /// <param name="drawQuietZones"></param>
    /// <param name="imgType"></param>
    /// <returns></returns>
    public string GetGraphic
        (
            int pixelsPerModule,
            Color darkColor,
            Color lightColor,
            bool drawQuietZones = true,
            ImageType imgType = ImageType.Png
        )
    {
        using var bmp = _qrCode.GetGraphic (pixelsPerModule, darkColor, lightColor, drawQuietZones);
        var base64 = BitmapToBase64 (bmp, imgType);

        return base64;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColor"></param>
    /// <param name="lightColor"></param>
    /// <param name="icon"></param>
    /// <param name="iconSizePercent"></param>
    /// <param name="iconBorderWidth"></param>
    /// <param name="drawQuietZones"></param>
    /// <param name="imgType"></param>
    /// <returns></returns>
    public string GetGraphic
        (
            int pixelsPerModule,
            Color darkColor,
            Color lightColor,
            Bitmap icon,
            int iconSizePercent = 15,
            int iconBorderWidth = 6,
            bool drawQuietZones = true,
            ImageType imgType = ImageType.Png
        )
    {
        using var bmp = _qrCode.GetGraphic (pixelsPerModule, darkColor, lightColor, icon, iconSizePercent,
            iconBorderWidth, drawQuietZones);
        var base64 = BitmapToBase64 (bmp, imgType);

        return base64;
    }


    private string BitmapToBase64
        (
            Bitmap bmp,
            ImageType imgType
        )
    {
        var iFormat = imgType switch
        {
            ImageType.Png => ImageFormat.Png,
            ImageType.Jpeg => ImageFormat.Jpeg,
            ImageType.Gif => ImageFormat.Gif,
            _ => ImageFormat.Png
        };

        using var memoryStream = new MemoryStream();
        bmp.Save (memoryStream, iFormat);
        var base64 = Convert.ToBase64String (memoryStream.ToArray(), Base64FormattingOptions.None);

        return base64;
    }

    /// <summary>
    /// Тип растрового изображения.
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// GIF.
        /// </summary>
        Gif,

        /// <summary>
        /// JPEG.
        /// </summary>
        Jpeg,

        /// <summary>
        /// PNG.
        /// </summary>
        Png
    }
}

/// <summary>
///
/// </summary>
[System.Runtime.Versioning.SupportedOSPlatform ("windows")]
public static class Base64QRCodeHelper
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColorHtmlHex"></param>
    /// <param name="lightColorHtmlHex"></param>
    /// <param name="eccLevel"></param>
    /// <param name="forceUtf8"></param>
    /// <param name="utf8BOM"></param>
    /// <param name="eciMode"></param>
    /// <param name="requestedVersion"></param>
    /// <param name="drawQuietZones"></param>
    /// <param name="imgType"></param>
    /// <returns></returns>
    public static string GetQRCode
        (
            string plainText,
            int pixelsPerModule,
            string darkColorHtmlHex,
            string lightColorHtmlHex,
            ECCLevel eccLevel,
            bool forceUtf8 = false,
            bool utf8BOM = false,
            EciMode eciMode = EciMode.Default,
            int requestedVersion = -1,
            bool drawQuietZones = true,
            ImageType imgType = ImageType.Png
        )
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode (plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion);
        using var qrCode = new Base64QRCode (qrCodeData);

        return qrCode.GetGraphic (pixelsPerModule, darkColorHtmlHex, lightColorHtmlHex, drawQuietZones, imgType);
    }
}
