// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* 
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

#nullable enable

namespace AM.Drawing.QRCoding
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class Base64QRCode : AbstractQRCode, IDisposable
    {
        private QRCode qr;

        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public Base64QRCode() {
            qr = new QRCode();
        }

        public Base64QRCode(QRCodeData data) : base(data) {
            qr = new QRCode(data);
        }

        public override void SetQRCodeData(QRCodeData data) {
            this.qr.SetQRCodeData(data);
        }

        public string GetGraphic(int pixelsPerModule)
        {
            return this.GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
        }


        public string GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            return this.GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones, imgType);
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            var base64 = string.Empty;
            using (Bitmap bmp = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones))
            {
                base64 = BitmapToBase64(bmp, imgType);
            }
            return base64;
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            var base64 = string.Empty;
            using (Bitmap bmp = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones))
            {
                base64 = BitmapToBase64(bmp, imgType);
            }
            return base64;
        }


        private string BitmapToBase64(Bitmap bmp, ImageType imgType)
        {
            var base64 = string.Empty;
            ImageFormat iFormat;
            switch (imgType) {
                case ImageType.Png:
                    iFormat = ImageFormat.Png;
                    break;
                case ImageType.Jpeg:
                    iFormat = ImageFormat.Jpeg;
                    break;
                case ImageType.Gif:
                    iFormat = ImageFormat.Gif;
                    break;
                default:
                    iFormat = ImageFormat.Png;
                    break;
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bmp.Save(memoryStream, iFormat);
                base64 = Convert.ToBase64String(memoryStream.ToArray(), Base64FormattingOptions.None);
            }
            return base64;
        }

        public enum ImageType
        {
            Gif,
            Jpeg,
            Png
        }

    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public static class Base64QRCodeHelper
    {
        public static string GetQRCode(string plainText, int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new Base64QRCode(qrCodeData))
                return qrCode.GetGraphic(pixelsPerModule, darkColorHtmlHex, lightColorHtmlHex, drawQuietZones, imgType);
        }
    }
}
