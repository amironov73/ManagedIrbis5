// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TextBoxWithButtonTest.cs --
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
    public sealed class TextBoxWithButtonTest
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

            var textBox = new TextBoxWithButton
            {
                Location = new Point(10, 10),
            };
            form.Controls.Add(textBox);

            textBox.ButtonClick += textBox_ButtonClick;

            form.ShowDialog(ownerWindow);
        }

        void textBox_ButtonClick
            (
                object sender,
                EventArgs e
            )
        {
            var textBox = (TextBoxWithButton) sender;
            var form = textBox.FindForm();

            MessageBox.Show
                (
                    form,
                    textBox.Text,
                    "TextBoxWithButton"
                );
        }

        #endregion
    }
}
