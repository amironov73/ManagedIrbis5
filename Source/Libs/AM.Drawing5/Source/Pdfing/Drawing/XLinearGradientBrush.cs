// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XLinearGradientBrush.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Defines a Brush with a linear gradient.
    /// </summary>
    public sealed class XLinearGradientBrush : XBaseGradientBrush
    {
        //internal XLinearGradientBrush();

        /// <summary>
        /// Initializes a new instance of the <see cref="XLinearGradientBrush"/> class.
        /// </summary>
        public XLinearGradientBrush(XPoint point1, XPoint point2, XColor color1, XColor color2) : base(color1, color2)
        {
            _point1 = point1;
            _point2 = point2;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="XLinearGradientBrush"/> class.
        /// </summary>
        public XLinearGradientBrush(XRect rect, XColor color1, XColor color2, XLinearGradientMode linearGradientMode) : base(color1, color2)
        {
            if (!Enum.IsDefined(typeof(XLinearGradientMode), linearGradientMode))
                throw new InvalidEnumArgumentException("linearGradientMode", (int)linearGradientMode, typeof(XLinearGradientMode));

            if (rect.Width == 0 || rect.Height == 0)
                throw new ArgumentException("Invalid rectangle.", "rect");

            _useRect = true;
            _rect = rect;
            _linearGradientMode = linearGradientMode;
        }

        // TODO: 
        //public XLinearGradientBrush(Rectangle rect, XColor color1, XColor color2, double angle);
        //public XLinearGradientBrush(RectangleF rect, XColor color1, XColor color2, double angle);
        //public XLinearGradientBrush(Rectangle rect, XColor color1, XColor color2, double angle, bool isAngleScaleable);
        //public XLinearGradientBrush(RectangleF rect, XColor color1, XColor color2, double angle, bool isAngleScaleable);
        //public XLinearGradientBrush(RectangleF rect, XColor color1, XColor color2, double angle, bool isAngleScaleable);

        internal bool _useRect;
        internal XPoint _point1, _point2;
        internal XRect _rect;
        internal XLinearGradientMode _linearGradientMode;
    }
}