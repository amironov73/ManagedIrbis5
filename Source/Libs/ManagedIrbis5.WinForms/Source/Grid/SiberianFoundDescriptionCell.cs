// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFoundDescriptionCell.cs -- ячейка, отображающая библиографическое описание найденного документа
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
    /// Ячейка, отображающая библиографического описание найденного документа.
    /// </summary>
    public class SiberianFoundDescriptionCell
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

            var found = (FoundLine?)Row?.Data;
            if (!ReferenceEquals(found, null))
            {
                var flags = TextFormatFlags.TextBoxControl
                      | TextFormatFlags.EndEllipsis
                      | TextFormatFlags.NoPrefix
                      | TextFormatFlags.VerticalCenter;

                TextRenderer.DrawText
                    (
                        graphics,
                        found.Description,
                        grid.Font,
                        rectangle,
                        foreColor,
                        flags
                    );
            }
        }

        /// <inheritdoc cref="object.ToString" />
        protected internal override void HandleToolTip
            (
                SiberianToolTipEventArgs eventArgs
            )
        {
            base.HandleToolTip(eventArgs);

            if (string.IsNullOrEmpty(eventArgs.ToolTipText))
            {
                var found = (FoundLine?)Row?.Data;
                if (!ReferenceEquals(found, null))
                {
                    eventArgs.ToolTipText = found.Description;
                }
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var row = Row?.Index ?? -1;
            var column = Column?.Index ?? -1;

            var found = (FoundLine?)Row?.Data;
            var text = string.Empty;
            if (!ReferenceEquals(found, null))
            {
                text = $"{found.Mfn}: {found.Description}";
            }

            return $"FoundDescriptionCell [{column}, {row}]: {text}";
        }

        #endregion

    } // class SiberianFoundDescriptionCell

} // namespace ManagedIrbis.WinForms.Grid
