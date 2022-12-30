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

#endregion

#nullable enable

namespace AM.Reporting.Barcode.QRCode
{
    /*/// <summary> This object renders a QR Code as a ByteMatrix 2D array of greyscale values.
    ///
    /// </summary>
    /// <author>  dswitkin@google.com (Daniel Switkin)
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
    /// </author>*/
    internal static class QRCodeWriter
    {
        private const int QUIET_ZONE_SIZE = 4;

        public static ByteMatrix encode (string contents, int width, int height,
            ErrorCorrectionLevel errorCorrectionLevel, string encoding, bool quietZone)
        {
            if (contents == null || contents.Length == 0)
            {
                throw new ArgumentException ("Found empty contents");
            }

            if (width < 0 || height < 0)
            {
                throw new ArgumentException ("Requested dimensions are too small: " + width + 'x' + height);
            }

            var code = new QRCode();
            Encoder.encode (contents, errorCorrectionLevel, encoding, code);
            return renderResult (code, width, height, quietZone);
        }

        // Note that the input matrix uses 0 == white, 1 == black, while the output matrix uses
        // 0 == black, 255 == white (i.e. an 8 bit greyscale bitmap).
        private static ByteMatrix renderResult (QRCode code, int width, int height, bool quietZone)
        {
            unchecked
            {
                var input = code.Matrix;
                var inputWidth = input.Width;
                var inputHeight = input.Height;
                var qrWidth = inputWidth + (quietZone ? QUIET_ZONE_SIZE << 1 : 0);
                var qrHeight = inputHeight + (quietZone ? QUIET_ZONE_SIZE << 1 : 0);
                var outputWidth = Math.Max (width, qrWidth);
                var outputHeight = Math.Max (height, qrHeight);

                var multiple = Math.Min (outputWidth / qrWidth, outputHeight / qrHeight);

                // Padding includes both the quiet zone and the extra white pixels to accommodate the requested
                // dimensions. For example, if input is 25x25 the QR will be 33x33 including the quiet zone.
                // If the requested size is 200x160, the multiple will be 4, for a QR of 132x132. These will
                // handle all the padding from 100x100 (the actual QR) up to 200x160.
                var leftPadding = (outputWidth - (inputWidth * multiple)) / 2;
                var topPadding = (outputHeight - (inputHeight * multiple)) / 2;

                var output = new ByteMatrix (outputWidth, outputHeight);
                sbyte[][] outputArray = output.Array;

                // We could be tricky and use the first row in each set of multiple as the temporary storage,
                // instead of allocating this separate array.
                var row = new sbyte[outputWidth];

                // 1. Write the white lines at the top
                for (var y = 0; y < topPadding; y++)
                {
                    setRowColor (outputArray[y], (sbyte)SupportClass.Identity (255));
                }

                // 2. Expand the QR image to the multiple
                sbyte[][] inputArray = input.Array;
                for (var y = 0; y < inputHeight; y++)
                {
                    // a. Write the white pixels at the left of each row
                    for (var x = 0; x < leftPadding; x++)
                    {
                        row[x] = (sbyte)SupportClass.Identity (255);
                    }

                    // b. Write the contents of this row of the barcode
                    var offset = leftPadding;
                    for (var x = 0; x < inputWidth; x++)
                    {
                        // Redivivus.in Java to c# Porting update - Type cased sbyte
                        // 30/01/2010
                        // sbyte value_Renamed = (inputArray[y][x] == 1)?0:(sbyte) SupportClass.Identity(255);
                        var value_Renamed = (sbyte)((inputArray[y][x] == 1) ? 0 : SupportClass.Identity (255));
                        for (var z = 0; z < multiple; z++)
                        {
                            row[offset + z] = value_Renamed;
                        }

                        offset += multiple;
                    }

                    // c. Write the white pixels at the right of each row
                    offset = leftPadding + (inputWidth * multiple);
                    for (var x = offset; x < outputWidth; x++)
                    {
                        row[x] = (sbyte)SupportClass.Identity (255);
                    }

                    // d. Write the completed row multiple times
                    offset = topPadding + (y * multiple);
                    for (var z = 0; z < multiple; z++)
                    {
                        Array.Copy (row, 0, outputArray[offset + z], 0, outputWidth);
                    }
                }

                // 3. Write the white lines at the bottom
                var offset2 = topPadding + (inputHeight * multiple);
                for (var y = offset2; y < outputHeight; y++)
                {
                    setRowColor (outputArray[y], (sbyte)SupportClass.Identity (255));
                }

                return output;
            }
        }

        private static void setRowColor (sbyte[] row, sbyte value_Renamed)
        {
            for (var x = 0; x < row.Length; x++)
            {
                row[x] = value_Renamed;
            }
        }
    }
}
