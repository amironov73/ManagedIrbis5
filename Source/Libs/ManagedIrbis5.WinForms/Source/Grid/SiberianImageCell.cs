// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianImageCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianImageCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// Picture.
        /// </summary>
        public Image? Picture
        {
            get => _picture;
            set => _SetPicture(value);
        }

        /// <summary>
        /// Text.
        /// </summary>
        public string? Text
        {
            get => _text;
            set => _SetText(value);
        }

        #endregion

        #region Private members

        private Image? _picture;

        private string? _text;

        private void _SetPicture
            (
                Image? picture
            )
        {
            _picture = picture;
            Column?.PutData(Row?.Data, this);
            Grid?.Invalidate();
        }

        private void _SetText
            (
                string? text
            )
        {
            _text = text;
            Grid?.Invalidate();
        }

        #endregion

        #region SiberianCell members

        /// <inheritdoc />
        public override void CloseEditor
            (
                bool accept
            )
        {
            if (!ReferenceEquals(Grid?.Editor, null))
            {
                if (accept)
                {
                    // Image = ....
                }
            }

            base.CloseEditor(accept);
        }

        /// <inheritdoc/>
        public override void Paint
            (
                PaintEventArgs args
            )
        {
            var grid = Grid;
            if (grid is null)
            {
                // TODO: some paint

                return;
            }

            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;
            var textRectangle = new Rectangle
                (
                    rectangle.Left + 20,
                    rectangle.Y,
                    rectangle.Width - 20,
                    rectangle.Height
                );

            var foreColor = Color.Black;
            if (ReferenceEquals(Row, grid.CurrentRow))
            {
                foreColor = Color.White;
            }

            if (ReferenceEquals(this, grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using var brush = new SolidBrush(backColor);
                graphics.FillRectangle(brush, rectangle);
            }

            var flags = TextFormatFlags.TextBoxControl
                | TextFormatFlags.EndEllipsis
                | TextFormatFlags.NoPrefix
                | TextFormatFlags.VerticalCenter;

            TextRenderer.DrawText
                (
                    graphics,
                    Text,
                    grid.Font,
                    textRectangle,
                    foreColor,
                    flags
                );
        }

        #endregion

        #region Object members

        #endregion

    }
}
