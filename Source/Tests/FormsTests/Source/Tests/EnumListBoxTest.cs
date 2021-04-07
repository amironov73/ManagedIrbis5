// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* EnumListBoxTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

using ManagedIrbis.Marc;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class EnumListBoxTest
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

            var listBox = new EnumListBox
            {
                EnumType = typeof(MarcBibliographicalIndex),
                Location = new Point(10, 10),
                Size = new Size(200, 200)
            };
            form.Controls.Add(listBox);

            var textBox = new TextBox
            {
                Location = new Point(220, 10),
                Width = 200
            };
            form.Controls.Add(textBox);

            listBox.SelectedValueChanged += (sender, args) =>
            {
                int? value = listBox.Value;
                var text
                    = ((MarcBibliographicalIndex)value).ToString();

                textBox.Text = text;
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
