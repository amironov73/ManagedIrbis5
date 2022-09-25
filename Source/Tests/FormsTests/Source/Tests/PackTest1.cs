// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PackTest1.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms.MarkupExtensions;

#endregion

#nullable enable

namespace FormsTests;

public sealed class PackTest1
    : IFormsTest
{
    #region IFormsTest members

    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var form = new Form()
            .Width (500)
            .Padding (5)
            .Text ("Это тестовая форма")
            .Pack
                (
                    new Label().Text ("Некий лейбл")
                        .BackColor (Color.Aqua)
                        .AutoSize (false)
                        .DockFill(),

                    new TextBox()
                        .Text ("Некий текстбокс")
                        .BackColor (Color.CornflowerBlue)
                        .Width (300),

                    new Button()
                        .Text ("Некая кнопка")
                        .BackColor (Color.Chartreuse)
                        .AutoSize()
                );

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
