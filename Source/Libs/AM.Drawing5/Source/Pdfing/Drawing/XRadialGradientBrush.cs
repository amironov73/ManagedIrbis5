// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XRadialGradientBrush.cs --
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
    public sealed class XRadialGradientBrush : XBaseGradientBrush
    {
        //internal XRadialGradientBrush();

        /// <summary>
        /// Initializes a new instance of the <see cref="XRadialGradientBrush"/> class.
        /// </summary>
        public XRadialGradientBrush(XPoint center1, XPoint center2, double r1, double r2, XColor color1, XColor color2) : base(color1, color2)
        {
            _center1 = center1;
            _center2 = center2;
            _r1 = r1;
            _r2 = r2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XRadialGradientBrush"/> class.
        /// </summary>
        public XRadialGradientBrush(XPoint center, double r1, double r2, XColor color1, XColor color2) : base(color1, color2)
        {
            _center1 = center;
            _center2 = center;
            _r1 = r1;
            _r2 = r2;
        }

        internal XPoint _center1, _center2;
        internal double _r1, _r2;
    }
}