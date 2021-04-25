// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianSubFieldColumn.cs -- колонка, отображающая подполя MARC-записи
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
    /// Колонка, отображающая подполя MARC-записи.
    /// </summary>
    public class SiberianSubFieldColumn
        : SiberianColumn
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianSubFieldColumn()
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

        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianSubFieldCell();
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
            if (Grid is null)
            {
                return default;
            }

            var subFieldCell = (SiberianSubFieldCell)cell;

            var subField = (SiberianSubField?)subFieldCell.Row?.Data;
            if (ReferenceEquals(subField, null))
            {
                return default;
            }

            var text = subField.Value;
            var rectangle = Grid.GetCellRectangle(cell);
            rectangle.Inflate(-1, -1);

            var result = new TextBoxWithButton
            {
                AutoSize = false,
                Location = rectangle.Location,
                Size = rectangle.Size,
                Font = Grid.Font,
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

    } // class SiberianSubFieldColumn

} // namespace ManagedIrbis.WinForms.Grid
