// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* DataGridViewTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class DataGridViewTest
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

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill
            };
            form.Controls.Add(grid);

            var calendarColumn
                = new DataGridViewCalendarColumn
                {
                    HeaderText = "Calendar"
                };
            grid.Columns.Add(calendarColumn);

            var colorColumn
                = new DataGridViewColorColumn
                {
                    HeaderText = "Color"
                };
            grid.Columns.Add(colorColumn);

            var numericColumn
                = new DataGridViewNumericColumn
                {
                    HeaderText = "Numeric"
                };
            grid.Columns.Add(numericColumn);

            var progressColumn
                = new DataGridViewProgressColumn
                {
                    HeaderText = "Progress"
                };
            grid.Columns.Add(progressColumn);

            var ratingColumn
                = new DataGridViewRatingColumn
                {
                    HeaderText = "Rating"
                };
            grid.Columns.Add(ratingColumn);

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
