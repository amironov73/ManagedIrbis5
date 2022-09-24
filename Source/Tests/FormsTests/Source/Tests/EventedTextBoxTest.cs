// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
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

namespace FormsTests;

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
            Size = new Size (800, 600)
        };

        var textBox = new EventedTextBox
        {
            Left = 10,
            Top = 10,
            Width = 400
        };
        form.Controls.Add (textBox);

        var resultBox = new TextBox
        {
            Left = 10,
            Top = 100,
            Width = 400
        };
        form.Controls.Add (resultBox);

        textBox.EnterPressed += (_, _) =>
        {
            resultBox.Text = "ENTER: " + textBox.Text;
        };

        textBox.DelayedTextChanged += (_, _) =>
        {
            resultBox.Text = "DELAY: " + textBox.Text;
        };

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
