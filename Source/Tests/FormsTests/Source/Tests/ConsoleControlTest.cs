// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ConsoleControlTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class ConsoleControlTest
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
            Size = new Size (800, 600)
        };

        var textBox = new TextBox
        {
            Location = new Point (10, 10),
            Width = 200,
            Text = "Hello, world! "
        };
        form.Controls.Add (textBox);

        var button1 = new Button
        {
            Location = new Point (230, 10),
            Width = 100,
            Text = "Write"
        };
        form.Controls.Add (button1);

        var button2 = new Button
        {
            Location = new Point (340, 10),
            Width = 100,
            Text = "Many"
        };
        form.Controls.Add (button2);

        var button3 = new Button
        {
            Location = new Point (450, 10),
            Width = 100,
            Text = "Clear"
        };
        form.Controls.Add (button3);

        var console = new ConsoleControl
        {
            Location = new Point (10, 50),
            AllowInput = false,

            //Size = new Size(580, 300),
            //Font = new Font("Consolas", 10f)
        };
        form.Controls.Add (console);

        for (var row = 0; row < 4; row++)
        {
            for (var column = 0; column < 80; column++)
            {
                if (column % 10 == 0)
                {
                    console.Write
                        (
                            row,
                            column,
                            '0',
                            Color.Red,
                            Color.White,
                            false
                        );
                }
                else
                {
                    var c = (char)('0' + column % 10);
                    console.Write
                        (
                            row,
                            column,
                            c,
                            Color.Blue,
                            Color.GreenYellow,
                            false
                        );
                }
            }
        }

        console.CursorTop = 4;

        button1.Click += (_, _) => { console.Write (textBox.Text); };

        button2.Click += (_, _) =>
        {
            console.ForeColor = Color.LimeGreen;
            for (var i = 0; i < 100; i++)
            {
                console.Write
                    (
                        i % 2 == 0,
                        "Mary has a little lamb. "
                    );
                Application.DoEvents();
            }
        };

        button3.Click += (_, _) => { console.Clear(); };

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
