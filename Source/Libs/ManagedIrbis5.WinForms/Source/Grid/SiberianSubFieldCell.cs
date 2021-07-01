// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianSubFieldCell.cs -- ячейка, отображающая подполе MARC-записи
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
    /// Ячейка, отображающая подполе MARC-записи.
    /// </summary>
    public class SiberianSubFieldCell
        : SiberianCell
    {
        #region SiberianCell members

        /// <inheritdoc cref="SiberianCell.CloseEditor" />
        public override void CloseEditor
            (
                bool accept
            )
        {
            if (!ReferenceEquals(Grid?.Editor, null))
            {
                if (accept)
                {
                    var subField = (SiberianSubField?)Row?.Data;
                    if (!ReferenceEquals(subField, null))
                    {
                        subField.Value = Grid.Editor.Text;
                    }
                }
            }

            base.CloseEditor(accept);
        }

        /// <inheritdoc cref="Control.Paint" />
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
                var text = subField.Value;
                if (!string.IsNullOrEmpty(text))
                {
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
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var row = Row?.Index ?? -1;
            var column = Column?.Index ?? -1;

            var subField = (SiberianSubField?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(subField, null))
            {
                text = $"{subField.Code}: {subField.Value} ({subField.OriginalValue})";
            }

            return $"SubFieldCell [{column}, {row}]: {text}";
        }

        #endregion

    } // class SiberianSubFieldCell

} // namespace ManagedIrbis.WinForms.Grid
