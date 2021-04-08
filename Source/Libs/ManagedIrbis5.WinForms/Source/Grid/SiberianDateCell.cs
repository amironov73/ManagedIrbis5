// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianDateCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianDateCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// Date.
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set { _SetDate(value); }
        }

        /// <summary>
        /// Text.
        /// </summary>
        public string? Text
        {
            get => _text;
            set => _SetText(value);
        }

        #endregion

        #region Private members

        private DateTime _date;

        private string? _text;

        private void _SetDate
            (
                DateTime date
            )
        {
            _date = date;
            Column?.PutData(Row.Data, this);
            Grid?.Invalidate();
        }

        private void _SetText
            (
                string? text
            )
        {
            _text = text;
            Grid?.Invalidate();
        }

        #endregion

        #region SiberianCell members

        /// <inheritdoc />
        public override void CloseEditor
            (
                bool accept
            )
        {
            if (!ReferenceEquals(Grid?.Editor, null))
            {
                if (accept)
                {
                    // State = ....
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
            var textRectangle = new Rectangle
                (
                    rectangle.Left + 20,
                    rectangle.Y,
                    rectangle.Width - 20,
                    rectangle.Height
                );

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

            var flags
                = TextFormatFlags.TextBoxControl
                | TextFormatFlags.EndEllipsis
                | TextFormatFlags.NoPrefix
                | TextFormatFlags.VerticalCenter;

            TextRenderer.DrawText
                (
                    graphics,
                    Text,
                    Grid.Font,
                    textRectangle,
                    foreColor,
                    flags
                );

        }

        #endregion

        #region Object members

        #endregion

    }
}
