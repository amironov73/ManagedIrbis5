// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ConsoleControlOutputTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Text.Output;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class ConsoleControlOutputTest
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

            var textBox = new TextBox
            {
                Location = new Point(10, 10),
                Width = 200
            };
            form.Controls.Add(textBox);

            var button = new Button
            {
                Location = new Point(250, 10),
                Text = "Enter"
            };
            form.Controls.Add(button);

            var console = new ConsoleControl
            {
                Location = new Point(10, 50),
                ForeColor = Color.Yellow
            };
            form.Controls.Add(console);

            AbstractOutput output
                = new ConsoleControlOutput(console);

            button.Click += (sender, args) =>
            {
                var text = textBox.Text;
                output.WriteLine(text);
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
