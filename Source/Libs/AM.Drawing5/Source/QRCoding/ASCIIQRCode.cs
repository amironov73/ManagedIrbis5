// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsciiQRCode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using static AM.Drawing.QRCoding.QRCodeGenerator;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
///
/// </summary>
public class AsciiQRCode
    : AbstractQRCode,
    IDisposable
{
    #region Construction

    /// <summary>
    /// Constructor without params to be used in COM Objects connections
    /// </summary>
    public AsciiQRCode()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AsciiQRCode
        (
            QRCodeData data
        )
        : base (data)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Returns a strings that contains the resulting QR code as ASCII chars.
    /// </summary>
    /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
    /// <param name="darkColorString">String for use as dark color modules. In case of string make sure whiteSpaceString has the same length.</param>
    /// <param name="whiteSpaceString">String for use as white modules (whitespace). In case of string make sure darkColorString has the same length.</param>
    /// <param name="drawQuietZones"></param>
    /// <param name="endOfLine">End of line separator. (Default: \n)</param>
    public string GetGraphic
        (
            int repeatPerModule,
            string darkColorString = "██",
            string whiteSpaceString = "  ",
            bool drawQuietZones = true,
            string endOfLine = "\n"
        )
    {
        return string.Join
            (
                endOfLine,
                GetLineByLineGraphic (repeatPerModule, darkColorString, whiteSpaceString, drawQuietZones)
            );
    }


    /// <summary>
    /// Returns an array of strings that contains each line of the resulting QR code as ASCII chars.
    /// </summary>
    /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
    /// <param name="darkColorString">String for use as dark color modules. In case of string make sure whiteSpaceString has the same length.</param>
    /// <param name="whiteSpaceString">String for use as white modules (whitespace). In case of string make sure darkColorString has the same length.</param>
    /// <param name="drawQuietZones"></param>
    public string[] GetLineByLineGraphic
        (
            int repeatPerModule,
            string darkColorString = "██",
            string whiteSpaceString = "  ",
            bool drawQuietZones = true
        )
    {
        var qrCode = new List<string>();

        //We need to adjust the repeatPerModule based on number of characters in darkColorString
        //(we assume whiteSpaceString has the same number of characters)
        //to keep the QR code as square as possible.
        var quietZonesModifier = (drawQuietZones ? 0 : 8);
        var quietZonesOffset = (int)(quietZonesModifier * 0.5);
        var adjustmentValueForNumberOfCharacters = darkColorString.Length / 2 != 1 ? darkColorString.Length / 2 : 0;
        var verticalNumberOfRepeats = repeatPerModule + adjustmentValueForNumberOfCharacters;
        var qrCodeData = QrCodeData.ThrowIfNull ();
        var moduleMatrix = qrCodeData.ModuleMatrix.ThrowIfNull();
        var sideLength = (moduleMatrix.Count - quietZonesModifier) * verticalNumberOfRepeats;
        for (var y = 0; y < sideLength; y++)
        {
            var lineBuilder = new StringBuilder();
            for (var x = 0; x < moduleMatrix.Count - quietZonesModifier; x++)
            {
                var module = moduleMatrix
                    [x + quietZonesOffset]
                    [
                        ((y + verticalNumberOfRepeats) / verticalNumberOfRepeats - 1) + quietZonesOffset
                    ];
                for (var i = 0; i < repeatPerModule; i++)
                {
                    lineBuilder.Append (module ? darkColorString : whiteSpaceString);
                }
            }

            qrCode.Add (lineBuilder.ToString());
        }

        return qrCode.ToArray();
    }
}

/// <summary>
///
/// </summary>
public static class AsciiQRCodeHelper
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColorString"></param>
    /// <param name="whiteSpaceString"></param>
    /// <param name="eccLevel"></param>
    /// <param name="forceUtf8"></param>
    /// <param name="utf8BOM"></param>
    /// <param name="eciMode"></param>
    /// <param name="requestedVersion"></param>
    /// <param name="endOfLine"></param>
    /// <param name="drawQuietZones"></param>
    /// <returns></returns>
    public static string GetQRCode
        (
            string plainText,
            int pixelsPerModule,
            string darkColorString,
            string whiteSpaceString,
            ECCLevel eccLevel,
            bool forceUtf8 = false,
            bool utf8BOM = false,
            EciMode eciMode = EciMode.Default,
            int requestedVersion = -1,
            string endOfLine = "\n",
            bool drawQuietZones = true
        )
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode (plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion);
        using var qrCode = new AsciiQRCode (qrCodeData);

        return qrCode.GetGraphic (pixelsPerModule, darkColorString, whiteSpaceString, drawQuietZones, endOfLine);
    }
}
