// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LabelExTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class LabelExTest
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

        var label = new LabelEx
        {
            Text = "This is label",
            Location = new Point (10, 10),
        };
        form.Controls.Add (label);

        var textBox1 = new TextBox
        {
            Text = "This is text box",
            Location = new Point (10, 40)
        };
        form.Controls.Add (textBox1);
        var textBox2 = new TextBox
        {
            Text = "This is another text box",
            Location = new Point (10, 70)
        };
        form.Controls.Add (textBox2);
        textBox2.Focus();

        label.BuddyControl = textBox1;

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
