// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* PagingDataGridViewTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Data;
using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class PagingDataGridViewTest
        : IFormsTest
    {
        #region IFormsTest members

        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var form = new Form
            {
                Size = new Size(800, 600)
            };

            var grid = new PagingDataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(600, 300)
            };
            DataGridViewColumn column1 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Column1",
                DataPropertyName = "Column1",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            grid.Columns.Add(column1);
            DataGridViewColumn column2 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Column2",
                DataPropertyName = "Column2",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            grid.Columns.Add(column2);
            form.Controls.Add(grid);

            grid.Paging += Grid_Paging;

            grid.PerformPaging(true, true);

            form.ShowDialog(ownerWindow);
        }

        private void Grid_Paging
            (
                object sender,
                PagingDataGridViewEventArgs e
            )
        {
            var grid = (DataGridView) sender;

            var table = new DataTable();
            var column3 = new DataColumn("Column1", typeof(int));
            table.Columns.Add(column3);
            var column4 = new DataColumn("Column2", typeof(int));
            table.Columns.Add(column4);

            var counter = 0;

            if (e.InitialCall)
            {
                for (var i = 0; i < 20; i++)
                {
                    var row = table.NewRow();
                    row[0] = ++counter;
                    row[1] = ++counter;
                    table.Rows.Add(row);
                }
                grid.DataSource = table;

                return;
            }

            var firstRow = grid.Rows[0];
            counter = (int) firstRow.Cells[0].Value
                      + (e.ScrollDown ? 39: -40);
            for (var i = 0; i < 20; i++)
            {
                var row = table.NewRow();
                row[0] = ++counter;
                row[1] = ++counter;
                table.Rows.Add(row);
            }
            grid.DataSource = table;
        }

        #endregion
    }
}
