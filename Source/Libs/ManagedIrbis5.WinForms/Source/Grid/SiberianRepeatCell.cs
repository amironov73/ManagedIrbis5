// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianRepeatCell.cs -- ячейка, отображающая номер повторения поля
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
    /// Ячейка, отображающая номер повторения поля.
    /// </summary>
    public class SiberianRepeatCell
        : SiberianCell
    {
        #region SiberianCell members

        /// <inheritdoc cref="SiberianCell.HandleClick"/>
        protected internal override void HandleClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            base.HandleClick(eventArgs);

            var row = eventArgs.Row;
            if (!ReferenceEquals(row, null))
            {
                var field = (SiberianField?) row.Data;
                if (field is {Repeatable: true})
                {
                    // TODO: implement
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
            var grid = Grid;
            if (grid is null)
            {
                // TODO: some paint
                return;
            }

            // var foreColor = Color.Black;
            // if (ReferenceEquals(Row, Grid.CurrentRow))
            // {
            //     foreColor = Color.White;
            // }

            if (ReferenceEquals(this, grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using Brush brush = new SolidBrush(backColor);
                graphics.FillRectangle(brush, rectangle);
            }

            var field = (SiberianField?)Row?.Data;
            if (field is {Repeatable: true})
            {
                var text = $"{field.Repeat}";

                var flags = TextFormatFlags.TextBoxControl
                            | TextFormatFlags.EndEllipsis
                            | TextFormatFlags.NoPrefix
                            | TextFormatFlags.VerticalCenter;

                ButtonRenderer.DrawButton
                    (
                        graphics,
                        rectangle,
                        text,
                        grid.Font,
                        flags,
                        false,
                        PushButtonState.Normal
                    );
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            int rowIndex = Row?.Index ?? -1,
                columnIndex = Column?.Index ?? -1;

            var field = (SiberianField?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(field, null))
            {
                text = $"{field.Tag}/{field.Repeat}";
            }

            return $"RepeatCell [{columnIndex}, {rowIndex}]: {text}";
        }

        #endregion
    }
}
