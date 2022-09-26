// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ConsoleControlTest2.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class ConsoleControlTest2
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
            Width = 100
        };
        form.Controls.Add (textBox);

        var console = new ConsoleControl
        {
            Location = new Point (10, 50),
            ForeColor = Color.Yellow,
            AllowInput = true
        };
        form.Controls.Add (console);

        console.WriteLine
            (
                Color.LawnGreen,
                """
                #include <stdio.h>

                int main ( int argc, char** argv )
                {
                    printf (""Hello, world!"");
                    return 0;
                }
                """
            );

        console.Input += (_, args) =>
        {
            console.WriteLine
                (
                    Color.DeepSkyBlue,
                    "You entered: " + args.Text
                );
        };

        console.TabPressed += (_, args) =>
        {
            var text = DateTime.Now.ToShortTimeString()
                       + ": " + args.Text;
            console.SetInput (text);
        };

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
