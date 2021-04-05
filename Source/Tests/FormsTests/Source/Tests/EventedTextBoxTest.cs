// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* EventedTextBoxTest.cs --
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
    public sealed class EventedTextBoxTest
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

            var textBox = new EventedTextBox
            {
                Left = 10,
                Top = 10,
                Width = 400
            };
            form.Controls.Add(textBox);

            var resultBox = new TextBox
            {
                Left = 10,
                Top = 100,
                Width = 400
            };
            form.Controls.Add(resultBox);

            textBox.EnterPressed += (sender, args) =>
            {
                resultBox.Text = "ENTER: " + textBox.Text;
            };

            textBox.DelayedTextChanged += (sender, args) =>
            {
                resultBox.Text = "DELAY: " + textBox.Text;
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
