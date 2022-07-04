// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BarcodeRenderInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing.BarCodes
{
    /// <summary>
    /// Holds all temporary information needed during rendering.
    /// </summary>
    class BarCodeRenderInfo
    {
        public BarCodeRenderInfo(XGraphics gfx, XBrush brush, XFont font, XPoint position)
        {
            Gfx = gfx;
            Brush = brush;
            Font = font;
            Position = position;
        }

        public XGraphics Gfx;
        public XBrush Brush;
        public XFont Font;
        public XPoint Position;
        public double BarHeight;
        public XPoint CurrPos;
        public int CurrPosInString;
        public double ThinBarWidth;
    }
}
