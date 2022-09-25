// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* RadioGroupTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class RadioGroupTest
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

        var group = new RadioGroup
        {
            Location = new Point (10, 10),
            Size = new Size (200, 200),
            Text = "Group of RadioButtons",
            Lines = new[]
            {
                "One", "Two", "Three",
                "Four", "Five", "Six"
            }
        };
        form.Controls.Add (group);

        var textBox = new TextBox
        {
            Location = new Point (220, 10),
            Width = 100
        };
        form.Controls.Add (textBox);

        group.CurrentChanged += (_, _) =>
        {
            textBox.Text = group.Current.ToString();
        };

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
