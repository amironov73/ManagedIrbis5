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

/* CheckedGroupBoxTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class CheckedGroupBoxTest
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

        var group = new CheckedGroupBox
        {
            Text = "Checked group box",
            Location = new Point (10, 10),
            Size = new Size (250, 100)
        };
        form.Controls.Add (group);

        var label = new Label
        {
            Text = "Label",
            Location = new Point (10, 20)
        };
        group.Controls.Add (label);

        var textBox = new TextBox
        {
            Text = "Text box",
            Location = new Point (10, 50)
        };
        group.Controls.Add (textBox);

        var button = new Button
        {
            Text = "Button",
            Location = new Point (120, 20)
        };
        group.Controls.Add (button);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
