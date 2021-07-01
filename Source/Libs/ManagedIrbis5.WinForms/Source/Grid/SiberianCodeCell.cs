// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianCodeCell.cs --
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
    public class SiberianCodeCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        public SiberianField? Field { get; set; }

        #endregion

        #region Public methods

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

            var subField = (SiberianSubField?)Row?.Data;
            if (!ReferenceEquals(subField, null))
            {
                var text = $"{subField.Code}: {subField.Title}";

                var flags = TextFormatFlags.TextBoxControl
                      | TextFormatFlags.EndEllipsis
                      | TextFormatFlags.NoPrefix
                      | TextFormatFlags.VerticalCenter;

                TextRenderer.DrawText
                    (
                        graphics,
                        text,
                        grid.Font,
                        rectangle,
                        foreColor,
                        flags
                    );
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            int row = Row?.Index ?? -1,
                column = Column?.Index ?? -1;

            var subField = (SiberianSubField?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(subField, null))
            {
                text = $"{subField.Code}: {subField.Value} ({subField.OriginalValue})";
            }

            return $"CodeCell [{column}, {row}]: {text}";
        }

        #endregion

    } // class SiberianCodeCell

} // namespace ManagedIrbis.WinForms.Grid
