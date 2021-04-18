// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianTermCountCell.cs -- ячейка, отображащающая количество ссылок на термин
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Ячейка, отображающая количество ссылок на термин поисового словаря.
    /// </summary>
    public class SiberianTermCountCell
        : SiberianCell
    {
        #region SiberianCell members

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

            var term = (Term?)Row?.Data;
            if (!ReferenceEquals(term, null))
            {
                var text = term.Count.ToInvariantString();
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
            var row = Row?.Index ?? -1;
            var column = Column?.Index ?? -1;

            var term = (Term?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(term, null))
            {
                text = $"{term.Text} ({term.Count})";
            }

            return $"TermCountCell [{column}, {row}]: {text}";
        }

        #endregion

    } // class SiberianTermCountCell

} // namespace ManagedIrbis.WinForms.Grid
