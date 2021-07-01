// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFoundMfnCell.cs -- ячейка, отображающая MFN найденного документа
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
    /// Ячейка, отображающая MFN найденного документа.
    /// </summary>
    public class SiberianFoundMfnCell
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
            var column = (SiberianFoundMfnColumn?) Column;
            if (!ReferenceEquals(found, null))
            {
                // TODO: вернуть использование порядковых номеров
                // var useSerial = column?.UseSerialNumber ?? false;
                var number = found.Mfn;
                var text = number.ToInvariantString();

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
        } // method Paint

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

            return $"FoundMfnCell [{column}, {row}]: {text}";

        } // method ToString

        #endregion

    } // class SiberianFoundMfnCell

} // namespace ManagedIrbis.WinForms.Grid
