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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Barcode
{
    /// <summary>
    /// The base class for 2D-barcodes such as PDF417 and Datamatrix.
    /// </summary>
    public abstract class Barcode2DBase : BarcodeBase
    {
        private static Font FFont = new Font ("Arial", 8);

        private void DrawBarcode (IGraphics g, float width, float height)
        {
            var originalSize = CalcBounds();
            var kx = width / originalSize.Width;
            var ky = height / originalSize.Height;

            Draw2DBarcode (g, kx, ky);

            //If swiss qr, draw the swiss cross
            if (text.StartsWith ("SPC"))
            {
                var top = showText ? height - 21 : height;
                g.FillRectangle (Brushes.White, width / 2 - width / 100f * 7, top / 2 - top / 100 * 7,
                    width / 100f * 14, top / 100 * 14);
                g.FillRectangle (Brushes.Black, width / 2 - width / 100f * 6, top / 2 - top / 100 * 6,
                    width / 100f * 12, top / 100 * 12);
                g.FillRectangle (Brushes.White, width / 2 - width / 100f * 4, top / 2 - top / 100 * 1.5f,
                    width / 100f * 8, top / 100 * 3);
                g.FillRectangle (Brushes.White, width / 2 - width / 100f * 1.5f, top / 2 - top / 100 * 4,
                    width / 100f * 3, top / 100 * 8);
            }

            if (text.StartsWith ("ST"))
            {
                var skyBluePen = new Pen (Brushes.Black);
                skyBluePen.Width = (kx * 4 * zoom) / 2;

                g.DrawLine (skyBluePen, width - 2, height / 2, width - 2, height - 2);
                g.DrawLine (skyBluePen, width / 2, height - 2, width - 2, height - 2);
            }

            // draw the text.
            if (showText)
            {
                var data = StripControlCodes (text);
                if (data.Length > 0)
                {
                    // When we print, .Net automatically scales the font. However, we need to handle this process.
                    // Downscale the font to the screen resolution, then scale by required value (ky).
                    var fontZoom = 18f / (int)g.MeasureString (data, FFont).Height * ky;
                    using (var drawFont = new Font (FFont.FontFamily, FFont.Size * fontZoom, FFont.Style))
                    {
                        g.DrawString (data, drawFont, Brushes.Black,
                            new RectangleF (0, height - 18 * ky, width, 18 * ky));
                    }
                }
            }
        }

        internal virtual void Draw2DBarcode (IGraphics g, float kx, float ky)
        {
        }

        public override void DrawBarcode (IGraphics g, RectangleF displayRect)
        {
            var width = angle is 90 or 270 ? displayRect.Height : displayRect.Width;
            var height = angle is 90 or 270 ? displayRect.Width : displayRect.Height;
            var state = g.Save();
            try
            {
                // rotate
                g.TranslateTransform (displayRect.Left, displayRect.Top);
                g.RotateTransform (angle);

                switch (angle)
                {
                    case 90:
                        g.TranslateTransform (0, -displayRect.Width);
                        break;

                    case 180:
                        g.TranslateTransform (-displayRect.Width, -displayRect.Height);
                        break;

                    case 270:
                        g.TranslateTransform (-displayRect.Height, 0);
                        break;
                }

                DrawBarcode (g, width, height);
            }
            finally
            {
                g.Restore (state);
            }
        }
    }
}
