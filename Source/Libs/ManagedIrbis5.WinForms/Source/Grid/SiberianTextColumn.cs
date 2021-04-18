// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianTextColumn.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Windows.Forms;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianTextColumn
        : SiberianColumn
    {
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
            SiberianCell result = new SiberianTextCell();
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

            var textCell = (SiberianTextCell)cell;
            var rectangle = grid.GetCellRectangle(cell);
            rectangle.Inflate(-1,-1);

            var result = new TextBox
            {
                AutoSize = false,
                Location = rectangle.Location,
                Size = rectangle.Size,
                Font = grid.Font,
                BorderStyle = BorderStyle.FixedSingle
            };
            result.KeyDown += Editor_KeyDown;

            if (edit)
            {
                result.Text = textCell.Text;
            }
            else
            {
                if (!ReferenceEquals(state, null))
                {
                    result.Text = state.ToString();
                    result.SelectionStart = result.TextLength;
                }
            }

            result.Parent = Grid;
            result.Show();
            result.Focus();

            return result;
        }

        /// <inheritdoc />
        public override void GetData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            var textCell = (SiberianTextCell) cell;

            if (!string.IsNullOrEmpty(Member)
                && !ReferenceEquals(theObject, null))
            {
                var type = theObject.GetType();
                var memberInfo = type.GetMember(Member)
                    .First();
                var property = new PropertyOrField
                    (
                        memberInfo
                    );

                var value = property.GetValue(theObject);
                textCell.Text = value?.ToString();
            }
        }

        /// <inheritdoc />
        public override void PutData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            var textCell = (SiberianTextCell)cell;

            if (!string.IsNullOrEmpty(Member)
                && !ReferenceEquals(theObject, null))
            {
                var type = theObject.GetType();
                var memberInfo = type.GetMember(Member)
                    .First();
                var property = new PropertyOrField
                    (
                        memberInfo
                    );

                property.SetValue(theObject, textCell.Text);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
