// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ListBoxExTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class ListBoxExTest
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

            var listBox = new ListBoxEx
            {
                Location = new Point(10, 10),
                Width = 200
            };
            listBox.Items.AddRange(new object[]
            {
                "Item1",
                "Item2",
                "Item3",
                "Item4",
                "Item5",
                "Item6",
                "Item7",
                "Item8",
                "Item9",
                "Item10"
            });
            form.Controls.Add(listBox);

            var textBox = new TextBox
            {
                Location = new Point(310, 10),
                Width = 300
            };
            form.Controls.Add(textBox);

            listBox.SelectedIndexChanged += (sender, args) =>
            {
                textBox.Text = listBox.SelectedItem.ToVisibleString();
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
