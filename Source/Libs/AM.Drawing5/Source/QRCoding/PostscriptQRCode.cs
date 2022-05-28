// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PostscriptQRCode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

using static AM.Drawing.QRCoding.QRCodeGenerator;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// QR-код в виде PostScript.
/// </summary>
[System.Runtime.Versioning.SupportedOSPlatform ("windows")]
public class PostscriptQRCode
    : AbstractQRCode,
        IDisposable
{
    #region Construction

    /// <summary>
    /// Constructor without params to be used in COM Objects connections
    /// </summary>
    public PostscriptQRCode()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PostscriptQRCode (QRCodeData data)
        : base (data)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public string GetGraphic
        (
            int pointsPerModule,
            bool epsFormat = false
        )
    {
        var viewBox = new Size (pointsPerModule * QrCodeData!.ModuleMatrix!.Count,
            pointsPerModule * this.QrCodeData.ModuleMatrix.Count);

        return GetGraphic (viewBox, Color.Black, Color.White, true, epsFormat);
    }

    /// <summary>
    ///
    /// </summary>
    public string GetGraphic
        (
            int pointsPerModule,
            Color darkColor,
            Color lightColor,
            bool drawQuietZones = true,
            bool epsFormat = false
        )
    {
        var viewBox = new Size (pointsPerModule * QrCodeData!.ModuleMatrix!.Count,
            pointsPerModule * this.QrCodeData.ModuleMatrix.Count);

        return this.GetGraphic (viewBox, darkColor, lightColor, drawQuietZones, epsFormat);
    }

    /// <summary>
    ///
    /// </summary>
    public string GetGraphic
        (
            int pointsPerModule,
            string darkColorHex,
            string lightColorHex,
            bool drawQuietZones = true,
            bool epsFormat = false
        )
    {
        var viewBox = new Size (pointsPerModule * QrCodeData!.ModuleMatrix!.Count,
            pointsPerModule * QrCodeData.ModuleMatrix.Count);

        return GetGraphic (viewBox, darkColorHex, lightColorHex, drawQuietZones, epsFormat);
    }

    /// <summary>
    ///
    /// </summary>
    public string GetGraphic
        (
            Size viewBox,
            bool drawQuietZones = true,
            bool epsFormat = false
        )
    {
        return GetGraphic (viewBox, Color.Black, Color.White, drawQuietZones, epsFormat);
    }

    /// <summary>
    ///
    /// </summary>
    public string GetGraphic
        (
            Size viewBox,
            string darkColorHex,
            string lightColorHex,
            bool drawQuietZones = true,
            bool epsFormat = false
        )
    {
        return GetGraphic (viewBox, ColorTranslator.FromHtml (darkColorHex), ColorTranslator.FromHtml (lightColorHex),
            drawQuietZones, epsFormat);
    }

    /// <summary>
    ///
    /// </summary>
    public string GetGraphic
        (
            Size viewBox,
            Color darkColor,
            Color lightColor,
            bool drawQuietZones = true,
            bool epsFormat = false
        )
    {
        var offset = drawQuietZones ? 0 : 4;
        var drawableModulesCount = QrCodeData!.ModuleMatrix!.Count - (drawQuietZones ? 0 : offset * 2);
        var pointsPerModule = Math.Min (viewBox.Width, viewBox.Height) / (double) drawableModulesCount;

        var psFile = string.Format (psHeader, new object[]
        {
            DateTime.Now.ToString ("s"), CleanSvgVal (viewBox.Width), CleanSvgVal (pointsPerModule),
            epsFormat ? "EPSF-3.0" : string.Empty
        });
        psFile += string.Format (psFunctions, new object[]
        {
            CleanSvgVal (darkColor.R / 255.0), CleanSvgVal (darkColor.G / 255.0), CleanSvgVal (darkColor.B / 255.0),
            CleanSvgVal (lightColor.R / 255.0), CleanSvgVal (lightColor.G / 255.0), CleanSvgVal (lightColor.B / 255.0),
            drawableModulesCount
        });

        for (var xi = offset; xi < offset + drawableModulesCount; xi++)
        {
            if (xi > offset)
                psFile += "nl\n";
            for (var yi = offset; yi < offset + drawableModulesCount; yi++)
            {
                psFile += (this.QrCodeData.ModuleMatrix[xi][yi] ? "f " : "b ");
            }

            psFile += "\n";
        }

        return psFile + psFooter;
    }

    private string CleanSvgVal (double input)
    {
        //Clean double values for international use/formats
        return input.ToString (System.Globalization.CultureInfo.InvariantCulture);
    }

    private const string psHeader = @"%!PS-Adobe-3.0 {3}
%%Creator: QRCoder.NET
%%Title: QRCode
%%CreationDate: {0}
%%DocumentData: Clean7Bit
%%Origin: 0
%%DocumentMedia: Default {1} {1} 0 () ()
%%BoundingBox: 0 0 {1} {1}
%%LanguageLevel: 2
%%Pages: 1
%%Page: 1 1
%%EndComments
%%BeginConstants
/sz {1} def
/sc {2} def
%%EndConstants
%%BeginFeature: *PageSize Default
<< /PageSize [ sz sz ] /ImagingBBox null >> setpagedevice
%%EndFeature
";

    private const string psFunctions = @"%%BeginFunctions
/csquare {{
    newpath
    0 0 moveto
    0 1 rlineto
    1 0 rlineto
    0 -1 rlineto
    closepath
    setrgbcolor
    fill
}} def
/f {{
    {0} {1} {2} csquare
    1 0 translate
}} def
/b {{
    1 0 translate
}} def
/background {{
    {3} {4} {5} csquare
}} def
/nl {{
    -{6} -1 translate
}} def
%%EndFunctions
%%BeginBody
0 0 moveto
gsave
sz sz scale
background
grestore
gsave
sc sc scale
0 {6} 1 sub translate
";

    private const string psFooter = @"%%EndBody
grestore
showpage
%%EOF
";
}

[System.Runtime.Versioning.SupportedOSPlatform ("windows")]
internal static class PostscriptQRCodeHelper
{
    public static string GetQRCode
        (
            string plainText,
            int pointsPerModule,
            string darkColorHex,
            string lightColorHex,
            ECCLevel eccLevel,
            bool forceUtf8 = false,
            bool utf8BOM = false,
            EciMode eciMode = EciMode.Default,
            int requestedVersion = -1,
            bool drawQuietZones = true,
            bool epsFormat = false
        )
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode (plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion);
        using var qrCode = new PostscriptQRCode (qrCodeData);

        return qrCode.GetGraphic (pointsPerModule, darkColorHex, lightColorHex, drawQuietZones, epsFormat);
    }
}
