// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFoundCheckCell.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianFoundCheckCell
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
            if (!ReferenceEquals(found, null))
            {
                var state = found.Selected
                    ? CheckBoxState.CheckedNormal
                    : CheckBoxState.UncheckedNormal;

                var point = new Point
                    (
                        rectangle.X + 2,
                        rectangle.Y + 2
                    );

                CheckBoxRenderer.DrawCheckBox
                    (
                        graphics,
                        point,
                        state
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

            var found = (FoundLine?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(found, null))
            {
                text = $"{found.Mfn}: {found.Selected}";
            }

            return $"FoundCheckCell [{column}, {row}]: {text}";
        }

        #endregion

    } // class SiberianFoundCheckCell

} // namespace ManagedIrbis.WinForms.Grid
