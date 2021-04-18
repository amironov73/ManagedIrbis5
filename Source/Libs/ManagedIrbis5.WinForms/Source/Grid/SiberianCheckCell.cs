// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianCheckCell.cs --
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
    public class SiberianCheckCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// State.
        /// </summary>
        public bool State
        {
            get => _state;
            set => _SetState(value);
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

        private bool _state;

        private string? _text;

        private void _SetState
            (
                bool state
            )
        {
            _state = state;
            Column?.PutData(Row?.Data, this);
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
            var grid = Grid;
            if (grid is null)
            {
                // TODO: some paint
                return;
            }

            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;
            var textRectangle = new Rectangle
                (
                    rectangle.Left + 20,
                    rectangle.Y,
                    rectangle.Width - 20,
                    rectangle.Height
                );

            if (ReferenceEquals(this, grid.CurrentCell))
            {
                var backColor = Color.Blue;
                using var brush = new SolidBrush(backColor);
                graphics.FillRectangle(brush, rectangle);
            }

            var flags = TextFormatFlags.TextBoxControl
                | TextFormatFlags.EndEllipsis
                | TextFormatFlags.NoPrefix
                | TextFormatFlags.VerticalCenter;

            var state = State
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
                    textRectangle,
                    Text,
                    grid.Font,
                    flags,
                    false,
                    state
                );
        }

        #endregion

    }
}
