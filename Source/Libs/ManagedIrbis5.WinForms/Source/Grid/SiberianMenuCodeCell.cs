// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianMenuCodeCell.cs -- колонка, отображающая значение из элемента меню
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Колонка, отображающая значение из элемента меню.
    /// </summary>
    public class SiberianMenuCodeCell
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

            var entry = (MenuEntry?)Row?.Data;
            if (!ReferenceEquals(entry, null))
            {
                var flags = TextFormatFlags.TextBoxControl
                      | TextFormatFlags.EndEllipsis
                      | TextFormatFlags.NoPrefix
                      | TextFormatFlags.VerticalCenter;

                TextRenderer.DrawText
                    (
                        graphics,
                        entry.Code,
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

            var entry = (MenuEntry?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(entry, null))
            {
                text = entry.Code;
            }

            return $"MenuCodeCell [{column}, {row}]: {text}";
        }

        #endregion

    } // class SiberianMenuCodeCell

} // namespace ManagedIrbis.WinForms.Grid
