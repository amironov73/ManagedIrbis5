// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianTextCell.cs --
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
    public class SiberianTextCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        public string? Text
        {
            get => _text;
            set => _SetText(value);
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private string? _text;

        private void _SetText
            (
                string? text
            )
        {
            _text = text;
            Column?.PutData(Row?.Data, this);
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
                    Text = Grid.Editor.Text;
                }
            }

            base.CloseEditor(accept);
        }

        /// <inheritdoc/>
        public override void OnPaint
            (
                PaintEventArgs args
            )
        {
            var grid = Grid;
            if (grid is null)
            {
                // TODO: some paint?
                return;
            }

            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;

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
                    rectangle,
                    foreColor,
                    flags
                );
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            int rowIndex = Row?.Index ?? -1,
                columnIndex = Column?.Index ?? -1;

            return $"TextCell [{columnIndex}, {rowIndex}]: {Text}";
        }

        #endregion
    }
}
