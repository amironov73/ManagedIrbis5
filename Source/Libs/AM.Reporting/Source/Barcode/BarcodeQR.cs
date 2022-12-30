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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

using AM.Reporting.Barcode.QRCode;
using AM.Reporting.Utils;

using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace AM.Reporting.Barcode
{
    /// <summary>
    /// Specifies the QR code error correction level.
    /// </summary>
    public enum QRCodeErrorCorrection
    {
        /// <summary>
        /// L = ~7% correction.
        /// </summary>
        L,

        /// <summary>
        /// M = ~15% correction.
        /// </summary>
        M,

        /// <summary>
        /// Q = ~25% correction.
        /// </summary>
        Q,

        /// <summary>
        /// H = ~30% correction.
        /// </summary>
        H
    }

    /// <summary>
    /// Specifies the QR Code encoding.
    /// </summary>
    public enum QRCodeEncoding
    {
        /// <summary>
        /// UTF-8 encoding.
        /// </summary>
        UTF8,

        /// <summary>
        /// ISO 8859-1 encoding.
        /// </summary>
        ISO8859_1,

        /// <summary>
        /// Shift_JIS encoding.
        /// </summary>
        Shift_JIS,

        /// <summary>
        /// Windows-1251 encoding.
        /// </summary>
        Windows_1251,

        /// <summary>
        /// cp866 encoding.
        /// </summary>
        cp866
    }

    /// <summary>
    /// Generates the 2D QR code barcode.
    /// </summary>
    public class BarcodeQR : Barcode2DBase
    {
        #region Fields

        private ByteMatrix matrix;
        private const int PixelSize = 4;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the error correction.
        /// </summary>
        [DefaultValue (QRCodeErrorCorrection.L)]
        public QRCodeErrorCorrection ErrorCorrection { get; set; }

        /// <summary>
        /// Gets or sets the encoding used for text conversion.
        /// </summary>
        [DefaultValue (QRCodeEncoding.UTF8)]
        public QRCodeEncoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the value indicating that quiet zone must be shown.
        /// </summary>
        [DefaultValue (true)]
        public bool QuietZone { get; set; }

        #endregion

        #region Private Methods

        private ErrorCorrectionLevel GetErrorCorrectionLevel()
        {
            switch (ErrorCorrection)
            {
                case QRCodeErrorCorrection.L:
                    return ErrorCorrectionLevel.L;

                case QRCodeErrorCorrection.M:
                    return ErrorCorrectionLevel.M;

                case QRCodeErrorCorrection.Q:
                    return ErrorCorrectionLevel.Q;

                case QRCodeErrorCorrection.H:
                    return ErrorCorrectionLevel.H;
            }

            return ErrorCorrectionLevel.L;
        }

        private string GetEncoding()
        {
            switch (Encoding)
            {
                case QRCodeEncoding.UTF8:
                    return "UTF-8";

                case QRCodeEncoding.ISO8859_1:
                    return "ISO-8859-1";

                case QRCodeEncoding.Shift_JIS:
                    return "Shift_JIS";

                case QRCodeEncoding.Windows_1251:
                    return "Windows-1251";

                case QRCodeEncoding.cp866:
                    return "cp866";
            }

            return "";
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (BarcodeBase source)
        {
            base.Assign (source);
            var src = source as BarcodeQR;

            ErrorCorrection = src.ErrorCorrection;
            Encoding = src.Encoding;
            QuietZone = src.QuietZone;
        }

        internal override void Serialize (FRWriter writer, string prefix, BarcodeBase diff)
        {
            base.Serialize (writer, prefix, diff);
            var c = diff as BarcodeQR;

            if (c == null || ErrorCorrection != c.ErrorCorrection)
            {
                writer.WriteValue (prefix + "ErrorCorrection", ErrorCorrection);
            }

            if (c == null || Encoding != c.Encoding)
            {
                writer.WriteValue (prefix + "Encoding", Encoding);
            }

            if (c == null || QuietZone != c.QuietZone)
            {
                writer.WriteBool (prefix + "QuietZone", QuietZone);
            }
        }

        internal override void Initialize (string text, bool showText, int angle, float zoom)
        {
            base.Initialize (text, showText, angle, zoom);
            matrix = QRCodeWriter.encode (this.text, 0, 0, GetErrorCorrectionLevel(), GetEncoding(), QuietZone);
        }

        internal override SizeF CalcBounds()
        {
            var textAdd = showText ? 18 : 0;
            return new SizeF (matrix.Width * PixelSize, matrix.Height * PixelSize + textAdd);
        }

        internal override void Draw2DBarcode (IGraphics g, float kx, float ky)
        {
            Brush dark = new SolidBrush (Color);

            for (var y = 0; y < matrix.Height; y++)
            {
                for (var x = 0; x < matrix.Width; x++)
                {
                    if (matrix.get_Renamed (x, y) == 0)
                    {
                        g.FillRectangle (dark, new RectangleF (
                                x * PixelSize * kx,
                                y * PixelSize * ky,
                                PixelSize * kx,
                                PixelSize * ky
                            ));
                    }
                }
            }

            if (text.StartsWith ("SPC"))
            {
                ErrorCorrection = QRCodeErrorCorrection.M;
            }

            dark.Dispose();
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodeQR"/> class with default settings.
        /// </summary>
        public BarcodeQR()
        {
            Encoding = QRCodeEncoding.UTF8;
            QuietZone = true;
        }
    }
}
