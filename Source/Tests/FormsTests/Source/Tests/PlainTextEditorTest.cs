// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* PlainTextEditorTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class PlainTextEditorTest
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

        var editor = new PlainTextEditor
        {
            Dock = DockStyle.Fill,
            Text = "Mary has a little lamb"
        };
        form.Controls.Add (editor);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
