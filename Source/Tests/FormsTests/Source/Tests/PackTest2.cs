// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PackTest2.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;
using AM.Windows.Forms.MarkupExtensions;

#endregion

#nullable enable

namespace FormsTests;

public sealed class PackTest2
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
            .Text ("Это другая тестовая форма")
            .Pack
                (
                    new LabeledTextBox
                    {
                        Label = { Text = "Привет!" },
                        TextBox = { Text = "Мир!" }
                    },

                    new Row()
                        .Add
                            (
                                100f,
                                new Label()
                                .Text ("Первый пошел")
                                .BackColor (Color.Khaki)
                                .AutoSize()
                            )
                        .Add
                            (
                                100f,
                                new Label()
                                .Text ("Второй пошел")
                                .BackColor (Color.Pink)
                                .AutoSize (false)
                                .DockFill()
                            )
                        .Add
                            (
                                250f,
                                new Label()
                                .Text ("Третий пошел")
                                .BackColor (Color.Lime)
                                .AutoSize()
                            ),

                    new LabeledComboBox
                    {
                        Label = { Text = "Комбо бокс" },
                        ComboBox =
                        {
                            DropDownStyle = ComboBoxStyle.DropDownList,
                            Items =
                            {
                                "Первая строка",
                                "Вторая строка",
                                "Третья строка",
                            },
                            SelectedIndex = 1
                        }
                    },

                    new TextBox()
                        .Text ("Некий текстбокс")
                        .BackColor (Color.CornflowerBlue)
                        .Width (300),

                    new Button()
                        .Text ("Некая кнопка")
                        .BackColor (Color.Chartreuse)
                        .AutoSize()
                        .OnClick ((_, _) => MessageBox.Show ("АГА!"))
                );

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
