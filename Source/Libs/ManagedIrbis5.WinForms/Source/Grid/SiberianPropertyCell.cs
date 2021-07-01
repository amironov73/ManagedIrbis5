// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianPropertyCell.cs -- ячейка, привязанная к свойству объекта
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
    /// Ячейка, привязанная к свойству объекта.
    /// </summary>
    public class SiberianPropertyCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// Значение объекта
        /// </summary>
        public object? Value
        {
            get => default;
            set => throw new NotImplementedException();

        } // property Value

        #endregion

        #region Private members

        #endregion

        #region SiberianCell members

        /// <inheritdoc cref="SiberianCell.CloseEditor" />
        public override void CloseEditor
            (
                bool accept
            )
        {
            if (this.EnsureGrid().Editor is { } editor)
            {
                if (accept)
                {
                    // State = ....
                }
            }

            base.CloseEditor(accept);

        } // method CloseEditor

        /// <inheritdoc cref="SiberianCell.OnPaint" />
        public override void OnPaint
            (
                PaintEventArgs args
            )
        {
            var grid = this.EnsureGrid();
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

            var theObject = Row?.Data;
            if (!ReferenceEquals(theObject, null))
            {
                var column = (SiberianPropertyColumn) this.EnsureColumn();
                var value = column.GetValue(theObject);
                if (value is not null)
                {
                    var flags = TextFormatFlags.TextBoxControl
                                | TextFormatFlags.EndEllipsis
                                | TextFormatFlags.NoPrefix
                                | TextFormatFlags.VerticalCenter;

                    TextRenderer.DrawText
                        (
                            graphics,
                            value.ToString(),
                            grid.Font,
                            rectangle,
                            foreColor,
                            flags
                        );
                }
            }

        } // method OnPaint

        #endregion

    } // class SiberianPropertyCell

} // namespace ManagedIrbis.WinForms.Grid
