// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFoundIconCell.cs -- ячейка, отображающая иконку найденного документа
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
    /// Ячейка, отображающая иконку найденного документа (если таковая имеется).
    /// </summary>
    public class SiberianFoundIconCell
        : SiberianCell
    {
        #region SiberianCell members

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

            if (ReferenceEquals(this, grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using var brush = new SolidBrush(backColor);
                graphics.FillRectangle(brush, rectangle);
            }

            var found = (FoundLine?)Row?.Data;
            if (found is {Icon: Image icon})
            {
                graphics.DrawImage(icon, rectangle);
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var rowIndex = Row?.Index ?? -1;
            var columnIndex = Column?.Index ?? -1;

            var found = (FoundLine?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(found, null))
            {
                text = $"{found.Mfn}: {found.Icon}";
            }

            return $"FoundIconCell [{columnIndex}, {rowIndex}]: {text}";
        }

        #endregion

    } // class SiberianFoundIconCell

} // namespace ManagedIrbis.WinForms.Grid
