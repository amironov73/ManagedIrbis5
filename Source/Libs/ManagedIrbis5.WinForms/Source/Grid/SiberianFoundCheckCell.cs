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

        /// <inheritdoc/>
        public override void Paint
            (
                PaintEventArgs args
            )
        {
            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;

            if (ReferenceEquals(this, Grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using (Brush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, rectangle);
                }
            }

            var found = (FoundLine)Row.Data;

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

        /// <inheritdoc/>
        public override string ToString()
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            int row = ReferenceEquals(Row, null) ? -1 : Row.Index,
                column = ReferenceEquals(Column, null) ? -1 : Column.Index;
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var found = (FoundLine)Row.Data;
            var text = string.Empty;
            if (!ReferenceEquals(found, null))
            {
                text = string.Format
                    (
                        "{0}: {1}",
                        found.Mfn,
                        found.Selected
                    );
            }

            return string.Format
                (
                    "FoundCheckCell [{0}, {1}]: {2}",
                    column,
                    row,
                    text
                );
        }

        #endregion
    }
}
