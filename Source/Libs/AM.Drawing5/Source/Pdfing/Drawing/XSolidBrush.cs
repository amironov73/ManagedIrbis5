// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XSolidBrush.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Defines a single color object used to fill shapes and draw text.
    /// </summary>
    public sealed class XSolidBrush : XBrush
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XSolidBrush"/> class.
        /// </summary>
        public XSolidBrush()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XSolidBrush"/> class.
        /// </summary>
        public XSolidBrush(XColor color)
            : this(color, false)
        { }

        internal XSolidBrush(XColor color, bool immutable)
        {
            _color = color;
            _immutable = immutable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XSolidBrush"/> class.
        /// </summary>
        public XSolidBrush(XSolidBrush brush)
        {
            _color = brush.Color;
        }

        /// <summary>
        /// Gets or sets the color of this brush.
        /// </summary>
        public XColor Color
        {
            get { return _color; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XSolidBrush"));
                _color = value;
            }
        }
        internal XColor _color;

        /// <summary>
        /// Gets or sets a value indicating whether the brush enables overprint when used in a PDF document.
        /// Experimental, takes effect only on CMYK color mode.
        /// </summary>
        public bool Overprint
        {
            get { return _overprint; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XSolidBrush"));
                _overprint = value;
            }
        }
        internal bool _overprint;
        readonly bool _immutable;
    }
}
