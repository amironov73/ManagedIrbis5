// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianTagCell.cs -- ячейка, отображающая метку поля.
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
    /// Ячейка, отображающая метку поля.
    /// </summary>
    public class SiberianTagCell
        : SiberianCell
    {
        #region SiberianCell members

        /// <inheritdoc cref="SiberianCell.Paint"/>
        public override void Paint
            (
                PaintEventArgs args
            )
        {
            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;
            var grid = Grid.ThrowIfNull("Grid");

            var foreColor = Color.Black;
            if (ReferenceEquals(Row, grid.CurrentRow))
            {
                foreColor = Color.White;
            }

            if (ReferenceEquals(this, grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using Brush brush = new SolidBrush(backColor);
                graphics.FillRectangle(brush, rectangle);
            }

            var row = Row.ThrowIfNull("Row");
            var field = (SiberianField?)row.Data;
            if (!ReferenceEquals(field, null))
            {
                var text = $"{field.Tag}: {field.Title}";

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
            int rowIndex = Row?.Index ?? -1,
                columnIndex = Column?.Index ?? -1;

            var field = (SiberianField?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(field, null))
            {
                text = $"{field.Tag}/{field.Repeat}: {field.Value} ({field.OriginalValue})";
            }

            return $"TagCell [{columnIndex}, {rowIndex}]: {text}";
        }

        #endregion
    }
}
