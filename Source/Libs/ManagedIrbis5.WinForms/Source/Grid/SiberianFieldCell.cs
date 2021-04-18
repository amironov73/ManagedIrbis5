// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFieldCell.cs --
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
    public class SiberianFieldCell
        : SiberianCell
    {
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
                    var field = (SiberianField?)Row?.Data;
                    if (!ReferenceEquals(field, null))
                    {
                        field.Value = Grid.Editor.Text;
                    }
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
                // TODO: some paint?
                return;
            }

            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;

            var foreColor = Color.Black;
            var codeColor = Color.FromArgb(220, 0, 0);
            if (ReferenceEquals(Row, grid.CurrentRow))
            {
                foreColor = Color.White;
                codeColor = Color.FromArgb(0, 255, 255);
            }

            if (ReferenceEquals(this, grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using var brush = new SolidBrush(backColor);
                graphics.FillRectangle(brush, rectangle);
            }

            var field = (SiberianField?) Row?.Data;
            if (!ReferenceEquals(field, null))
            {
                var text = field.Value;
                if (!string.IsNullOrEmpty(text))
                {
                    using var painter = new FieldPainter
                    {
                        CodeColor = codeColor,
                        TextColor = foreColor
                    };

                    painter.DrawLine
                        (
                            graphics,
                            grid.Font,
                            rectangle,
                            text
                        );
                }
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var rowIndex = Row?.Index ?? -1;
            var columnIndex = Column?.Index ?? -1;

            var field = (SiberianField?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(field, null))
            {
                text = $"{field.Tag}/{field.Repeat}: {field.Value} ({field.OriginalValue})";
            }

            return $"FieldCell [{columnIndex}, {rowIndex}]: {text}";
        }

        #endregion

    } // class SiberianFieldCell

} // namespace ManagedIrbis.WinForms.Grid
