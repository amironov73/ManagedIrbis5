// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ColorComboBoxTest.cs --
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
    public sealed class ColorComboBoxTest
        : IFormsTest
    {
        #region IUITest members

        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var form = new Form
            {
                Size = new Size(800, 600)
            };

            var colorBox = new ColorComboBox
            {
                Location = new Point(10, 10),
                Width = 200
            };
            form.Controls.Add(colorBox);

            var textBox = new TextBox
            {
                Location = new Point(310, 10),
                Width = 300
            };
            form.Controls.Add(textBox);

            colorBox.SelectedIndexChanged += (sender, args) =>
            {
                textBox.Text = colorBox.SelectedColor.ToString();
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion

    } // class ColorComboBoxTest

} // namespace FormsTests
