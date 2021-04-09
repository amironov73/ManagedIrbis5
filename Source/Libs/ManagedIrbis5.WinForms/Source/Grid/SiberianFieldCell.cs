// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFieldCell.cs --
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
    public class SiberianFieldCell
        : SiberianCell
    {
        #region SiberianCell members

        /// <inheritdoc />
        public override void CloseEditor
            (
                bool accept
            )
        {
            if (!ReferenceEquals(Grid.Editor, null))
            {
                if (accept)
                {
                    var field = (SiberianField)Row.Data;
                    if (!ReferenceEquals(field, null))
                    {
                        field.Value = Grid.Editor.Text;
                    }
                }
            }

            base.CloseEditor(accept);
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
            var codeColor = Color.FromArgb(220, 0, 0);
            if (ReferenceEquals(Row, Grid.CurrentRow))
            {
                foreColor = Color.White;
                codeColor = Color.FromArgb(0, 255, 255);
            }

            if (ReferenceEquals(this, Grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using (Brush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, rectangle);
                }
            }

            var field = (SiberianField) Row.Data;

            if (!ReferenceEquals(field, null))
            {
                var text = field.Value;

                if (!string.IsNullOrEmpty(text))
                {

                    var flags = TextFormatFlags.TextBoxControl
                          | TextFormatFlags.EndEllipsis
                          | TextFormatFlags.NoPrefix
                          | TextFormatFlags.VerticalCenter;


                    using var painter = new FieldPainter
                    {
                        CodeColor = codeColor,
                        TextColor = foreColor
                    };

                    painter.DrawLine
                        (
                            graphics,
                            Grid.Font,
                            rectangle,
                            text
                        );

                    /* TextRenderer.DrawText
                        (
                            graphics,
                            text,
                            Grid.Font,
                            rectangle,
                            foreColor,
                            flags
                        );
                        */
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

            var field = (SiberianField)Row.Data;
            var text = string.Empty;
            if (!ReferenceEquals(field, null))
            {
                text = string.Format
                    (
                        "{0}/{1}: {2} ({3})",
                        field.Tag,
                        field.Repeat,
                        field.Value,
                        field.OriginalValue
                    );
            }

            return string.Format
                (
                    "FieldCell [{0}, {1}]: {2}",
                    column,
                    row,
                    text
                );
        }

        #endregion
    }
}
