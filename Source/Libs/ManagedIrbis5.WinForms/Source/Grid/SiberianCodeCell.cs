// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianCodeCell.cs --
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
    public class SiberianCodeCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        public SiberianField? Field { get; set; }

        #endregion

        #region Public methods

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

            var subField = (SiberianSubField?)Row.Data;
            if (!ReferenceEquals(subField, null))
            {
                var text = string.Format
                    (
                        "{0}: {1}",
                        subField.Code,
                        subField.Title
                    );

                var flags
                    = TextFormatFlags.TextBoxControl
                      | TextFormatFlags.EndEllipsis
                      | TextFormatFlags.NoPrefix
                      | TextFormatFlags.VerticalCenter;

                TextRenderer.DrawText
                    (
                        graphics,
                        text,
                        Grid.Font,
                        rectangle,
                        foreColor,
                        flags
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

            var subField = (SiberianSubField?)Row.Data;
            var text = string.Empty;
            if (!ReferenceEquals(subField, null))
            {
                text = string.Format
                    (
                        "{0}: {1} ({2})",
                        subField.Code,
                        subField.Value,
                        subField.OriginalValue
                    );
            }

            return string.Format
                (
                    "CodeCell [{0}, {1}]: {2}",
                    column,
                    row,
                    text
                );
        }

        #endregion
    }
}
