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

/* QRCode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using static AM.Drawing.QRCoding.QRCodeGenerator;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
///
/// </summary>
[System.Runtime.Versioning.SupportedOSPlatform ("windows")]
public class QRCode
    : AbstractQRCode, IDisposable
{
    /// <summary>
    /// Constructor without params to be used in COM Objects connections
    /// </summary>
    public QRCode()
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    public QRCode (QRCodeData data)
        : base (data)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pixelsPerModule"></param>
    /// <returns></returns>
    public Bitmap GetGraphic
        (
            int pixelsPerModule
        )
    {
        return GetGraphic
            (
                pixelsPerModule,
                darkColor: Color.Black,
                lightColor: Color.White,
                drawQuietZones: true
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColorHtmlHex"></param>
    /// <param name="lightColorHtmlHex"></param>
    /// <param name="drawQuietZones"></param>
    /// <returns></returns>
    public Bitmap GetGraphic
        (
            int pixelsPerModule,
            string darkColorHtmlHex,
            string lightColorHtmlHex,
            bool drawQuietZones = true
        )
    {
        return GetGraphic
            (
                pixelsPerModule,
                darkColor: ColorTranslator.FromHtml (darkColorHtmlHex),
                lightColor: ColorTranslator.FromHtml (lightColorHtmlHex),
                drawQuietZones
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColor"></param>
    /// <param name="lightColor"></param>
    /// <param name="drawQuietZones"></param>
    /// <returns></returns>
    public Bitmap GetGraphic
        (
            int pixelsPerModule,
            Color darkColor,
            Color lightColor,
            bool drawQuietZones = true
        )
    {
        var size = (QrCodeData!.ModuleMatrix!.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
        var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

        var bmp = new Bitmap (size, size);
        using (var gfx = Graphics.FromImage (bmp))
        using (var lightBrush = new SolidBrush (lightColor))
        using (var darkBrush = new SolidBrush (darkColor))
        {
            for (var x = 0; x < size + offset; x = x + pixelsPerModule)
            {
                for (var y = 0; y < size + offset; y = y + pixelsPerModule)
                {
                    var module =
                        QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][
                            (x + pixelsPerModule) / pixelsPerModule - 1];

                    if (module)
                    {
                        gfx.FillRectangle (darkBrush,
                            new Rectangle (x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                    }
                    else
                    {
                        gfx.FillRectangle (lightBrush,
                            new Rectangle (x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                    }
                }
            }

            gfx.Save();
        }

        return bmp;
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
    /// <param name="iconBackgroundColor"></param>
    /// <returns></returns>
    public Bitmap GetGraphic
        (
            int pixelsPerModule,
            Color darkColor,
            Color lightColor,
            Bitmap? icon = null,
            int iconSizePercent = 15,
            int iconBorderWidth = 0,
            bool drawQuietZones = true,
            Color? iconBackgroundColor = null
        )
    {
        var size = (QrCodeData!.ModuleMatrix!.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
        var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

        var bmp = new Bitmap (size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        using var gfx = Graphics.FromImage (bmp);
        using var lightBrush = new SolidBrush (lightColor);
        using var darkBrush = new SolidBrush (darkColor);
        gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
        gfx.CompositingQuality = CompositingQuality.HighQuality;
        gfx.Clear (lightColor);
        var drawIconFlag = icon != null && iconSizePercent is > 0 and <= 100;

        for (var x = 0; x < size + offset; x = x + pixelsPerModule)
        {
            for (var y = 0; y < size + offset; y = y + pixelsPerModule)
            {
                var moduleBrush =
                    QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][
                        (x + pixelsPerModule) / pixelsPerModule - 1]
                        ? darkBrush
                        : lightBrush;
                gfx.FillRectangle (moduleBrush,
                    new Rectangle (x - offset, y - offset, pixelsPerModule, pixelsPerModule));
            }
        }

        if (drawIconFlag)
        {
            var iconDestWidth = iconSizePercent * bmp.Width / 100f;
            var iconDestHeight = drawIconFlag ? iconDestWidth * icon!.Height / icon.Width : 0;
            var iconX = (bmp.Width - iconDestWidth) / 2;
            var iconY = (bmp.Height - iconDestHeight) / 2;
            var centerDest = new RectangleF (iconX - iconBorderWidth, iconY - iconBorderWidth,
                iconDestWidth + iconBorderWidth * 2, iconDestHeight + iconBorderWidth * 2);
            var iconDestRect = new RectangleF (iconX, iconY, iconDestWidth, iconDestHeight);
            var iconBgBrush = iconBackgroundColor != null ? new SolidBrush ((Color)iconBackgroundColor) : lightBrush;

            //Only render icon/logo background, if iconBorderWith is set > 0
            if (iconBorderWidth > 0)
            {
                using var iconPath = CreateRoundedRectanglePath (centerDest, iconBorderWidth * 2);

                gfx.FillPath (iconBgBrush, iconPath);
            }

            gfx.DrawImage (icon!, iconDestRect, new RectangleF (0, 0, icon!.Width, icon.Height), GraphicsUnit.Pixel);
        }

        gfx.Save();

        return bmp;
    }

    internal GraphicsPath CreateRoundedRectanglePath
        (
            RectangleF rect,
            int cornerRadius
        )
    {
        var roundedRect = new GraphicsPath();
        roundedRect.AddArc (rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
        roundedRect.AddLine (rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
        roundedRect.AddArc (rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270,
            90);
        roundedRect.AddLine (rect.Right, rect.Y + cornerRadius * 2, rect.Right,
            rect.Y + rect.Height - cornerRadius * 2);
        roundedRect.AddArc (rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2,
            cornerRadius * 2, cornerRadius * 2, 0, 90);
        roundedRect.AddLine (rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
        roundedRect.AddArc (rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
        roundedRect.AddLine (rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
        roundedRect.CloseFigure();

        return roundedRect;
    }
}

/// <summary>
/// Вспомогательные методы для <see cref="QRCode"/>.
/// </summary>
[System.Runtime.Versioning.SupportedOSPlatform ("windows")]
internal static class QRCodeHelper
{
    public static Bitmap GetQRCode
        (
            string plainText,
            int pixelsPerModule,
            Color darkColor,
            Color lightColor,
            ECCLevel eccLevel,
            bool forceUtf8 = false,
            bool utf8BOM = false,
            EciMode eciMode = EciMode.Default,
            int requestedVersion = -1,
            Bitmap? icon = null,
            int iconSizePercent = 15,
            int iconBorderWidth = 0,
            bool drawQuietZones = true
        )
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData =
            qrGenerator.CreateQrCode (plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion);
        using var qrCode = new QRCode (qrCodeData);

        return qrCode.GetGraphic
            (
                pixelsPerModule,
                darkColor,
                lightColor,
                icon,
                iconSizePercent,
                iconBorderWidth,
                drawQuietZones
            );
    }
}
