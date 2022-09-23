// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ConsoleControlTest3.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class ConsoleControlTest3
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

        var button = new Button
        {
            Location = new Point (10, 10),
            Text = "Press me"
        };
        form.Controls.Add (button);

        var console = new ConsoleControl
        {
            Location = new Point (10, 50),
            ForeColor = Color.LawnGreen
        };
        form.Controls.Add (console);

        button.Click += (_, _) =>
        {
            for (var i = 0; i < 100; i += 5)
            {
                var text = $"\rProcessing files \x1\xE{i}%";
                console.Write (text);
                Application.DoEvents();
                Thread.Sleep (100);
            }

            console.WriteLine ("\rProcessing files \x1\x000Fdone");
        };

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
