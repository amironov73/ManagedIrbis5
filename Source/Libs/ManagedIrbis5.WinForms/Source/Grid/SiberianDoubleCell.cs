// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianDoubleCell.cs -- ячейка, отображающее число с плавающей точкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Ячейка, отображающая число с плавающей точкой двойной точности.
    /// </summary>
    public class SiberianDoubleCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// Value.
        /// </summary>
        public double Value
        {
            get => _value;
            set => _SetValue(value);
        }

        #endregion

        #region Private members

        private double _value;

        private void _SetValue
            (
                double value
            )
        {
            _value = value;
            Column?.PutData(Row?.Data, this);
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
                    // State = ....
                }
            }

            base.CloseEditor(accept);
        }

        /// <inheritdoc cref="Control.Paint" />
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

            var text = Value.ToString(CultureInfo.InvariantCulture);
            var flags = TextFormatFlags.TextBoxControl
                        | TextFormatFlags.EndEllipsis
                        | TextFormatFlags.NoPrefix
                        | TextFormatFlags.VerticalCenter;

            TextRenderer.DrawText
                (
                    graphics,
                    text,
                    grid.Font,
                    textRectangle,
                    foreColor,
                    flags
                );

        }

        #endregion

    } // class SiberianDoubleCell

} // namespace ManagedIrbis.WinForms.Grid
