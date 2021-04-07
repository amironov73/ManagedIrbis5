// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* LabeledComboBoxTest.cs --
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
    public sealed class LabeledComboBoxTest
        : IFormsTest
    {
        #region IFormsTest members

        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var form = new Form();
            form.Size = new Size(800, 600);

            var comboBox = new LabeledComboBox
            {
                Location = new Point(10, 10),
                Size = new Size(100, 100)
            };
            comboBox.Label.Text = "Labeled ComboBox";
            comboBox.ComboBox.Items.AddRange(new object[]
            {
                "First",
                "Second",
                "Third",
                "Fourth",
                "Fifth"
            });
            form.Controls.Add(comboBox);

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
