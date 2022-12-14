// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SeparatorGlyph.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Manina.Windows.Forms
{
    public partial class PagedControl
    {
        /// <summary>
        /// Represent a toolbar separator on the designer.
        /// </summary>
        protected internal class SeparatorGlyph : BaseGlyph
        {
            #region Member Variables

            private Size _lineSize;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the size of the label.
            /// </summary>
            public override Size Size
            {
                get
                {
                    _lineSize = new Size (1, Parent.DefaultIconSize.Height);

                    return _lineSize + Padding + Padding;
                }
            }

            #endregion

            #region Overriden Methods

            /// <summary>
            /// Paints the glyph. The base class paints the background only.
            /// </summary>
            /// <param name="pe">Paint event arguments.</param>
            public override void Paint (PaintEventArgs pe)
            {
                base.Paint (pe);

                using var linePen = new Pen (Parent.SeparatorColor);
                var lineBounds = GetCenteredRectangle (_lineSize);
                pe.Graphics.DrawLine
                    (
                        linePen,
                        lineBounds.Left,
                        lineBounds.Top,
                        lineBounds.Left,
                        lineBounds.Bottom
                    );
            }

            #endregion
        }
    }
}
