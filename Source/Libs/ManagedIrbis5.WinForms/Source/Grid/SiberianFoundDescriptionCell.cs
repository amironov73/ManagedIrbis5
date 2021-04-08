// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFoundDescriptionCell.cs --
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
    public class SiberianFoundDescriptionCell
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

            var found = (FoundLine)Row.Data;

            if (!ReferenceEquals(found, null))
            {
                var flags
                    = TextFormatFlags.TextBoxControl
                      | TextFormatFlags.EndEllipsis
                      | TextFormatFlags.NoPrefix
                      | TextFormatFlags.VerticalCenter;

                TextRenderer.DrawText
                    (
                        graphics,
                        found.Description,
                        Grid.Font,
                        rectangle,
                        foreColor,
                        flags
                    );
            }
        }

        /// <inheritdoc/>
        protected internal override void HandleToolTip
            (
                SiberianToolTipEventArgs eventArgs
            )
        {
            base.HandleToolTip(eventArgs);

            if (string.IsNullOrEmpty(eventArgs.ToolTipText))
            {
                var found = (FoundLine)Row.Data;
                if (!ReferenceEquals(found, null))
                {
                    eventArgs.ToolTipText = found.Description;
                }
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
                        found.Description
                    );
            }

            return string.Format
                (
                    "FoundDescriptionCell [{0}, {1}]: {2}",
                    column,
                    row,
                    text
                );
        }

        #endregion
    }
}
