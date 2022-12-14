// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LabelGlyph.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Manina.Windows.Forms;

public partial class PagedControl
{
    /// <summary>
    /// Represent a toolbar label on the designer.
    /// </summary>
    protected internal class LabelGlyph
        : BaseGlyph
    {
        #region Member Variables

        private Size _textSize;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Gets the size of the label.
        /// </summary>
        public override Size Size
        {
            get
            {
                var hasText = !string.IsNullOrEmpty (Text);

                _textSize = (hasText ? TextRenderer.MeasureText (Text, Parent.Control.Font) : Size.Empty);

                return _textSize + Padding + Padding;
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Paints the glyph. The base class paints the background only.
        /// </summary>
        /// <param name="eventArgs">Paint event arguments.</param>
        public override void Paint
            (
                PaintEventArgs eventArgs
            )
        {
            base.Paint (eventArgs);

            using var textBrush = new SolidBrush (Parent.ButtonForeColor);
            if (!string.IsNullOrEmpty (Text))
            {
                var textBounds = GetCenteredRectangle (_textSize);
                eventArgs.Graphics.DrawString (Text, Parent.Control.Font, textBrush, textBounds);
            }
        }

        #endregion
    }
}
