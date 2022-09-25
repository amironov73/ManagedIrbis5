// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ScrollControlTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class ScrollControlTest
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
            AutoScaleMode = AutoScaleMode.None, Size =
                new Size (800, 600)
        };

        var scroll = new ScrollControl
        {
            Location = new Point (10, 10),
            BackColor = Color.DarkGray
        };

        form.Controls.Add (scroll);

        var textBox = new TextBox
        {
            Location = new Point (40, 10),
            Size = new Size (400, 200),
            Multiline = true
        };
        form.Controls.Add (textBox);

        scroll.Scroll += (_, args) =>
        {
            var text = string.Create
                (
                    null,
                    $"Type: {args.Type}, Value: {args.NewValue}\r\n"
                );
            textBox.AppendText (text);
        };

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
