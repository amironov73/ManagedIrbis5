// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ScrollControlTest.cs --
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
    public sealed class ScrollControlTest
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
                AutoScaleMode = AutoScaleMode.None, Size =
                    new Size(800, 600)
            };

            var scroll = new ScrollControl
            {
                Location = new Point(10, 10),
                BackColor = Color.DarkGray
            };

            form.Controls.Add(scroll);

            var textBox = new TextBox
            {
                Location = new Point(40, 10),
                Size = new Size(400, 200),
                Multiline = true
            };
            form.Controls.Add(textBox);

            scroll.Scroll += (sender, args) =>
            {
                var text = string.Format
                (
                    "Type: {0}, Value: {1}\r\n",
                    args.Type,
                    args.NewValue
                );
                textBox.AppendText(text);
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
