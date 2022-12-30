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
    /*/// <author>  satorux@google.com (Satoru Takabayashi) - creator
    /// </author>
    /// <author>  dswitkin@google.com (Daniel Switkin) - ported from C++
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
    /// </author>*/
    internal sealed class QRCode
    {
        public Mode Mode
        {
            // Mode of the QR Code.

            get;
            set;
        }

        public ErrorCorrectionLevel ECLevel
        {
            // Error correction level of the QR Code.

            get;
            set;
        }

        public int Version
        {
            // Version of the QR Code.  The bigger size, the bigger version.

            get;
            set;
        }

        public int MatrixWidth
        {
            // ByteMatrix width of the QR Code.

            get;
            set;
        }

        public int MaskPattern
        {
            // Mask pattern of the QR Code.

            get;
            set;
        }

        public int NumTotalBytes
        {
            // Number of total bytes in the QR Code.

            get;
            set;
        }

        public int NumDataBytes
        {
            // Number of data bytes in the QR Code.

            get;
            set;
        }

        public int NumECBytes
        {
            // Number of error correction bytes in the QR Code.

            get;
            set;
        }

        public int NumRSBlocks
        {
            // Number of Reedsolomon blocks in the QR Code.

            get;
            set;
        }

        public ByteMatrix Matrix
        {
            // ByteMatrix data of the QR Code.

            get;

            // This takes ownership of the 2D array.

            set;
        }

        public bool Valid =>

            // Checks all the member variables are set properly. Returns true on success. Otherwise, returns
            // false.
            Mode != null && ECLevel != null && Version != -1 && MatrixWidth != -1 && MaskPattern != -1 &&
            NumTotalBytes != -1 && NumDataBytes != -1 && NumECBytes != -1 && NumRSBlocks != -1 &&
            isValidMaskPattern (MaskPattern) && NumTotalBytes == NumDataBytes + NumECBytes &&
            Matrix != null && MatrixWidth == Matrix.Width &&
            Matrix.Width == Matrix.Height; // Must be square.

        public const int NUM_MASK_PATTERNS = 8;

        public QRCode()
        {
            Mode = null;
            ECLevel = null;
            Version = -1;
            MatrixWidth = -1;
            MaskPattern = -1;
            NumTotalBytes = -1;
            NumDataBytes = -1;
            NumECBytes = -1;
            NumRSBlocks = -1;
            Matrix = null;
        }

        // Check if "mask_pattern" is valid.
        public static bool isValidMaskPattern (int maskPattern)
        {
            return maskPattern >= 0 && maskPattern < NUM_MASK_PATTERNS;
        }
    }
}
