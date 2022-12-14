// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BaseGlyph.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Manina.Windows.Forms;

public partial class PagedControl
{
    /// <summary>
    /// Represent the base class of toolbar glyphs on the designer.
    /// </summary>
    protected internal abstract class BaseGlyph
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent toolbar.
        /// </summary>
        internal GlyphToolBar? Parent { get; set; }

        /// <summary>
        /// Gets or sets the bounds of the glyph.
        /// </summary>
        internal Rectangle Bounds { get; set; }

        /// <summary>
        /// Overriden by derived classes to supply the size of the glyph.
        /// </summary>
        public abstract Size Size { get; }

        /// <summary>
        /// Gets or sets the padding around the gylph contents and its border.
        /// </summary>
        public virtual Size Padding { get; set; } = new Size (2, 2);

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Paints the glyph. The base class paints the background only.
        /// </summary>
        /// <param name="eventArgs">Paint event arguments.</param>
        public virtual void Paint
            (
                PaintEventArgs eventArgs
            )
        {
            eventArgs.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            using Brush backBrush = new SolidBrush (Parent!.ButtonBackColor);
            var bounds = Bounds;

            eventArgs.Graphics.FillRectangle (backBrush, bounds);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a rectangle of the given size centered about the glyph bounds.
        /// </summary>
        /// <param name="size">The size of the new rectangle.</param>
        /// <returns>A new rectangle centered about the glyph.</returns>
        protected Rectangle GetCenteredRectangle (Size size)
        {
            var bounds = Bounds;
            return new Rectangle (bounds.Left + (bounds.Width - size.Width) / 2,
                bounds.Top + (bounds.Height - size.Height) / 2,
                size.Width,
                size.Height);
        }

        #endregion
    }
}
