// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianFieldColumn.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;
using AM;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianFieldColumn
        : SiberianColumn
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianFieldColumn()
        {
            Palette.BackColor = Color.White;
        }

        #endregion

        #region Private members

        private void Editor_KeyDown
            (
                object? sender,
                KeyEventArgs args
            )
        {
            if (ReferenceEquals(Grid, null))
            {
                return;
            }

            if (args.Modifiers == 0)
            {
                switch (args.KeyCode)
                {
                    case Keys.Escape:
                        Grid.CloseEditor(false);
                        break;

                    case Keys.Up:
                        Grid.MoveOneLineUp();
                        break;

                    case Keys.Down:
                    case Keys.Enter:
                        Grid.MoveOneLineDown();
                        break;

                    case Keys.PageUp:
                        Grid.MoveOnePageUp();
                        break;

                    case Keys.PageDown:
                        Grid.MoveOnePageDown();
                        break;
                }
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianFieldCell();
            result.Column = this;

            return result;
        }

        /// <inheritdoc />
        public override Control? CreateEditor
            (
                SiberianCell cell,
                bool edit,
                object? state
            )
        {
            var grid = Grid;
            if (grid is null)
            {
                return default;
            }

            var fieldCell = (SiberianFieldCell)cell;

            var field = (SiberianField?) fieldCell.Row?.Data;
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            var text = field.Value;

            var rectangle = grid.GetCellRectangle(cell);
            rectangle.Inflate(-1, -1);

            var result = new TextBoxWithButton
            {
                AutoSize = false,
                Location = rectangle.Location,
                Size = rectangle.Size,
                Font = grid.Font,
                BorderStyle = BorderStyle.FixedSingle
            };

            result.TextBox.ThrowIfNull("result.TextBox").KeyDown += Editor_KeyDown;

            if (edit)
            {
                result.Text = text ?? string.Empty;
            }
            else
            {
                if (!ReferenceEquals(state, null))
                {
                    result.Text = state.ToString() ?? string.Empty;
                    result.SelectionStart = result.TextLength;
                }
            }

            result.Parent = Grid;
            result.Show();
            result.Focus();

            return result;
        }

        #endregion

    } // class SiberianFieldColumn

} // namespace ManagedIrbis.WinForms.Grid
