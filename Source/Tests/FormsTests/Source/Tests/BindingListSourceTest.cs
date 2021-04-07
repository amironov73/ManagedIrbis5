// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* BindingListSourceTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class BindingListSourceTest
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

            var source = new BindingListSource<string>();

            var button = new Button
            {
                Location = new Point(10, 10),
                Width = 200,
                Text = "Add an item"
            };
            form.Controls.Add(button);

            var listBox = new ListBox
            {
                Location = new Point(220, 10),
                Size = new Size(200, 200),
                DataSource = source
            };
            form.Controls.Add(listBox);

            button.Click += (sender, args) =>
            {
                var item = DateTime.Now.Ticks.ToString();
                source.Add(item);
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
