// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ConsoleFormTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class ConsoleFormTest
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

        form.Console.BackColor = Color.White;
        form.Console.ForeColor = Color.Blue;
        form.Console.Clear();

        form.Console.WriteLine
            (
                Color.Green,
                """
                #include <stdio.h>

                int main ( int argc, char** argv )
                {
                    printf (""Hello, world!"");
                    return 0;
                }
                """
            );

        form.Console.AllowInput = true;
        form.ShowDialog(ownerWindow);
    }

    #endregion
}
