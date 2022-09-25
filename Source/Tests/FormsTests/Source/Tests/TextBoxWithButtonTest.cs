// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* TextBoxWithButtonTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class TextBoxWithButtonTest
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

        var textBox = new TextBoxWithButton
        {
            Location = new Point (10, 10),
        };
        form.Controls.Add (textBox);

        textBox.ButtonClick += TextBox_ButtonClick;

        form.ShowDialog (ownerWindow);
    }

    private static void TextBox_ButtonClick
        (
            object? sender,
            EventArgs e
        )
    {
        var textBox = (TextBoxWithButton) sender.ThrowIfNull (nameof (sender));
        var form = textBox.FindForm();

        MessageBox.Show
            (
                form,
                textBox.Text,
                "TextBoxWithButton"
            );
    }

    #endregion
}
