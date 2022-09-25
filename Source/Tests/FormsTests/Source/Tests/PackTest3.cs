// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PackTest3.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using AM.Windows.Forms;
using AM.Windows.Forms.MarkupExtensions;

#endregion

#nullable enable

namespace FormsTests;

public sealed class PackTest3
    : IFormsTest
{
    #region IFormsTest members

    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        const AnchorStyles allButOne =
            AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

        const int buttonWidth = 90;
        using var form = new Form()
            .Size (550, 300)
            .MinimumSize()
            .Padding (5)
            .Text ("Это третья тестовая форма");

        var groupBox = new GroupBox()
            .Text ("Группа контролов по предварительному сговору")
            .Location (10, 10)
            .Size
                (
                    form.WidthMinusPadding (10)
                        - buttonWidth - form.Padding.Horizontal,
                    220
                )
            .Padding (10);

        var buttonBox = new Panel()
            .Location (groupBox.Right + 10, groupBox.Top)
            .Size
                (
                    buttonWidth + form.Padding.Horizontal,
                    form.ClientSize.Height - form.Padding.Vertical - 10
                )
            .AnchorAll()
            .Padding (10)
            .BorderStyleNone();

        groupBox.Pack
                (
                    new LabeledTextBox
                    {
                        Name = "_textBox",
                        Label = { Text = "Текстбокс с надписью" },
                        Left = 5,
                        Width = groupBox.WidthMinusPadding (5),
                        Anchor = allButOne,
                        TextBox =
                        {
                            Text = "Тут какой-то текст"
                        }
                    },

                    new CheckBox
                    {
                        Name = "_checkBox",
                        Text = "Отметь меня",
                        Left = 5,
                        Width = groupBox.WidthMinusPadding (5),
                        Anchor = allButOne
                    },

                    new LabeledComboBox
                    {
                        Name = "_comboBox",
                        Label = { Text = "Комбобокс с надписью" },
                        Left = 5,
                        Width = groupBox.WidthMinusPadding (5),
                        Anchor = allButOne,
                        ComboBox =
                        {
                            DropDownStyle = ComboBoxStyle.DropDownList,
                            Items =
                            {
                                "Первая строка",
                                "Вторая строка",
                                "Третья строка",
                                "Четвертая строка"
                            },
                            SelectedIndex = 1
                        }
                    }

                );

        var okButton = new Button()
            .Text ("&OK")
            .AutoSize (false)
            .DialogResultOK();

        var cancelButton = new Button()
            .Text ("&Cancel")
            .AutoSize (false)
            .DialogResultCancel();

        buttonBox.Pack (okButton, cancelButton);

        form.Controls
                (
                    groupBox,
                    buttonBox
                )
            .AcceptButton (okButton)
            .CancelButton (cancelButton);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
