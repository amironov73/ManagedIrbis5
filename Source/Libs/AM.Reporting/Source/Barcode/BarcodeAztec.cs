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
using System.Drawing;
using System.ComponentModel;

using AM.Reporting.Barcode.Aztec;
using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Barcode
{
    /// <summary>
    /// Generates the 2D Aztec barcode.
    /// </summary>
    public class BarcodeAztec : Barcode2DBase
    {
        BitMatrix matrix;
        int errorCorrectionPercent;
        const int PIXEL_SIZE = 4;

        /// <summary>
        /// Gets or sets the error correction percent.
        /// </summary>
        [DefaultValue (33)]
        public int ErrorCorrectionPercent
        {
            get => errorCorrectionPercent;
            set => errorCorrectionPercent = (value < 5) ? 5 : ((value > 95) ? 95 : value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodeAztec"/> class with default settings.
        /// </summary>
        public BarcodeAztec()
        {
            ErrorCorrectionPercent = 33;
        }

        internal override void Initialize (string text, bool showText, int angle, float zoom)
        {
            base.Initialize (text, showText, angle, zoom);

            matrix = Encoder.encode (System.Text.Encoding.ASCII.GetBytes (text), ErrorCorrectionPercent, 0).Matrix;
        }

        internal override SizeF CalcBounds()
        {
            var textAdd = showText ? 18 : 0;
            return new SizeF (matrix.Width * PIXEL_SIZE, matrix.Height * PIXEL_SIZE + textAdd);
        }

        internal override void Draw2DBarcode (IGraphics g, float kx, float ky)
        {
            var light = Brushes.White;
            Brush dark = new SolidBrush (Color);

            for (var y = 0; y < matrix.Height; y++)
            {
                for (var x = 0; x < matrix.Width; x++)
                {
                    var b = matrix.getRow (y, null)[x];

                    var brush = /*b == true ?*/ dark /*: light*/;
                    if (b == true)
                    {
                        g.FillRectangle (brush, x * PIXEL_SIZE * kx, y * PIXEL_SIZE * ky,
                            PIXEL_SIZE * kx, PIXEL_SIZE * ky);
                    }
                }
            }

            dark.Dispose();
        }

        /// <inheritdoc/>
        public override void Assign (BarcodeBase source)
        {
            base.Assign (source);
            var src = source as BarcodeAztec;

            ErrorCorrectionPercent = src.ErrorCorrectionPercent;
        }

        internal override void Serialize (FRWriter writer, string prefix, BarcodeBase diff)
        {
            base.Serialize (writer, prefix, diff);

            if (diff is not BarcodeAztec c || ErrorCorrectionPercent != c.ErrorCorrectionPercent)
            {
                writer.WriteInt (prefix + "ErrorCorrection", ErrorCorrectionPercent);
            }
        }
    }
}
