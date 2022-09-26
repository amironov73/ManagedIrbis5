// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ConsoleFormTest2.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class ConsoleFormTest2
    : IFormsTest
{
    static readonly string[] _knownCommands =
    {
        "Clear-Screen",
        "Show-Help",
        "Current-Date",
        "Set-Time",
        "Print-Directory",
        "Exit"
    };

    static void HandleTabKey
        (
            object? sender,
            ConsoleInputEventArgs eventArgs
        )
    {
        var console = (ConsoleControl)sender.ThrowIfNull (nameof (sender));
        var text = eventArgs.Text;

        if (string.IsNullOrEmpty (text))
        {
            console.DropInput();
            console.WriteLine
                (
                    Color.Gray,
                    "Available commands are: "
                    + string.Join (", ", _knownCommands)
                );
            return;
        }

        foreach (var command in _knownCommands)
        {
            if (command.ToLower().StartsWith (text.ToLower()))
            {
                console.SetInput (command);
                return;
            }
        }

        console.DropInput();
        console.WriteLine
            (
                Color.Red,
                "No suggestions found"
            );
        console.SetInput (text);
    }

    private static void HandleInput
        (
            object? sender,
            ConsoleInputEventArgs eventArgs
        )
    {
        var console = (ConsoleControl)sender.ThrowIfNull (nameof (sender));
        var text = eventArgs.Text;

        if (string.IsNullOrEmpty (text))
        {
            console.WriteLine
                (
                    Color.Red,
                    "Idle command"
                );
            return;
        }

        var color = Color.Green;

        console.WriteLine
            (
                color,
                "Command received: '" + text + "'"
            );
        console.Write
            (
                color,
                "Processing... "
            );
        Application.DoEvents();
        Thread.Sleep (2000);
        console.WriteLine
            (
                color,
                "done"
            );

        console.WriteLine
            (
                color,
                new string ('=', 70)
            );
        console.WriteLine();

        if (text.SameString ("Clear-Screen"))
        {
            console.Clear();
        }

        if (text.SameString ("Exit"))
        {
            var form = console.FindForm().ThrowIfNull();
            form.Close();
        }
    }

    #region IFormsTest members

    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var form = new ConsoleForm
        {
            Text = "ConsoleControl in action"
        };

        var console = form.Console;

        console.BackColor = Color.White;
        console.ForeColor = Color.Blue;
        console.Clear();
        console.WriteLine
            (
                Color.Black,
                "NekoShell 10.1.2.522 ready"
            );
        console.WriteLine();

        console.AllowInput = true;
        console.TabPressed += HandleTabKey;
        console.Input += HandleInput;

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
