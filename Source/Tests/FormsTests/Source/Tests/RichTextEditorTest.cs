// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* RichTextEditorTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class RichTextEditorTest
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

        const string richText = """
        {\rtf1 Hello, world!
        \par Mary has a {\b little lamb}!}
        """;

        var editor = new RichTextEditor
        {
            Dock = DockStyle.Fill,
            Rtf = richText
        };
        form.Controls.Add (editor);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
