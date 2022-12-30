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

#endregion

#nullable enable

namespace AM.Reporting.Matrix
{
    internal class MatrixStyleSheet : StyleSheet
    {
        public Bitmap GetStyleBitmap (int index)
        {
            var styleCollection = this[index];
            var style = styleCollection[styleCollection.IndexOf ("Header")];

            var headerColor = Color.White;
            if (style.Fill is SolidFill fill)
            {
                headerColor = fill.Color;
            }
            else if (style.Fill is LinearGradientFill gradientFill)
            {
                headerColor = gradientFill.StartColor;
            }

            style = styleCollection[styleCollection.IndexOf ("Body")];
            var bodyColor = Color.White;
            if (style.Fill is SolidFill solidFill)
            {
                bodyColor = solidFill.Color;
            }
            else if (style.Fill is LinearGradientFill gradientFill)
            {
                bodyColor = gradientFill.StartColor;
            }

            // draw style picture
            var result = new Bitmap (16, 16);
            using (var g = Graphics.FromImage (result))
            {
                g.FillRectangle (Brushes.White, 0, 0, 16, 16);

                using (Brush b = new SolidBrush (headerColor))
                {
                    g.FillRectangle (b, 0, 0, 15, 8);
                }

                using (Brush b = new SolidBrush (bodyColor))
                {
                    g.FillRectangle (b, 0, 8, 15, 8);
                }

                g.DrawRectangle (Pens.Silver, 0, 0, 14, 14);
            }

            return result;
        }
    }
}
