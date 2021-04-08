// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianRepeatCell.cs --
 * Ars Magna project, http://arsmagna.ru
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
    public class SiberianRepeatCell
        : SiberianCell
    {
        #region SiberianCell members

        /// <inheritdoc/>
        protected internal override void HandleClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            base.HandleClick(eventArgs);

            var row = eventArgs.Row;
            if (!ReferenceEquals(row, null))
            {
                var field = (SiberianField) row.Data;
                if (!ReferenceEquals(field, null)
                    && field.Repeatable)
                {
                    MessageBox.Show("Make repeat");
                }
            }
        }

        /// <inheritdoc/>
        public override void Paint
            (
                PaintEventArgs args
            )
        {
            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;

            var foreColor = Color.Black;
            if (ReferenceEquals(Row, Grid.CurrentRow))
            {
                foreColor = Color.White;
            }

            if (ReferenceEquals(this, Grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using (Brush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, rectangle);
                }
            }

            var field = (SiberianField)Row.Data;

            if (!ReferenceEquals(field, null)
                && field.Repeatable)
            {
                var text = string.Format
                    (
                        "{0}",
                        field.Repeat
                    );

                var flags
                    = TextFormatFlags.TextBoxControl
                      | TextFormatFlags.EndEllipsis
                      | TextFormatFlags.NoPrefix
                      | TextFormatFlags.VerticalCenter;

                ButtonRenderer.DrawButton
                    (
                        graphics,
                        rectangle,
                        text,
                        Grid.Font,
                        flags,
                        false,
                        PushButtonState.Normal
                    );

                //TextRenderer.DrawText
                //    (
                //        graphics,
                //        text,
                //        Grid.Font,
                //        rectangle,
                //        foreColor,
                //        flags
                //    );
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

            var field = (SiberianField)Row.Data;
            var text = string.Empty;
            if (!ReferenceEquals(field, null))
            {
                text = string.Format
                    (
                        "{0}/{1}",
                        field.Tag,
                        field.Repeat
                    );
            }

            return string.Format
                (
                    "RepeatCell [{0}, {1}]: {2}",
                    column,
                    row,
                    text
                );
        }

        #endregion
    }
}
