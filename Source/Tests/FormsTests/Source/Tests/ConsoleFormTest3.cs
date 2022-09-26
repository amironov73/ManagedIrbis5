// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ConsoleFormTest3.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class ConsoleFormTest3
    : IFormsTest
{
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
        console.AllowInput = true;

        form.Show (ownerWindow);

        var text = console.ReadLine();

        form.Close();
        MessageBox.Show (text);
    }

    #endregion
}
