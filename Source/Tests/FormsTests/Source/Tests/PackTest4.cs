// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PackTest4.cs --
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

public sealed class PackTest4
    : IFormsTest
{
    #region IFormsTest members

    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var form = new Form()
            .Size (550, 300)
            .MinimumSize()
            .Padding (5)
            .MinimizeBox (false)
            .ControlBox (false)
            .Text ("Это четвертая тестовая форма");

        form.VerticalArea<GroupBox> (420)
            .Text ("Группа контролов по предварительному сговору")
            .Padding (5)
            .Pack
                (
                    new Row
                    {
                        new Label().Text ("Первая метка")
                            .AutoSize()
                            .ForeColor (Color.Blue)
                            .DockFill(),

                        new Label().Text ("Вторая метка")
                            .AutoSize()
                            .ForeColor (Color.Green)
                            .DockFill(),

                        new Label().Text ("Третья метка")
                            .AutoSize()
                            .ForeColor (Color.Red)
                            .DockFill(),
                    },

                    new LabeledTextBox
                    {
                        Name = "_textBox",
                        Label = { Text = "Текстбокс с надписью" },
                        Left = 5,
                        Dock = DockStyle.Top,
                        TextBox = { Text = "Тут какой-то текст" }
                    },

                    new CheckBox
                    {
                        Name = "_checkBox",
                        Text = "Отметь меня",
                        Left = 5,
                        Dock = DockStyle.Top
                    },

                    new LabeledComboBox
                    {
                        Name = "_comboBox",
                        Label = { Text = "Комбобокс с надписью" },
                        Left = 5,
                        Dock = DockStyle.Top,
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
            .Packed()
            .DialogResultOK()
            .OnClick ((_, _) => MessageBox.Show ("OK pressed"));

        var cancelButton = new Button()
            .Text ("&Cancel")
            .Packed()
            .DialogResultCancel()
            .OnClick ((_, _) => MessageBox.Show ("Cancel pressed"));

        form.VerticalArea<Panel> (0)
            .Padding (5)
            .BorderStyleNone()
            .Pack (okButton, cancelButton);

        form.AcceptButton (okButton)
            .CancelButton (cancelButton)
            .ShowDialog (ownerWindow);
    }

    #endregion
}
