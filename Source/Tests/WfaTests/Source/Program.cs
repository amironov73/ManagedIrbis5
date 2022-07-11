// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в приложение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using AM.AppServices;
using AM.Windows.Forms;
using AM.Windows.Forms.AppServices;
using AM.Windows.Forms.MarkupExtensions;

#endregion

namespace WfaTests;

/// <summary>
/// Точка входа в приложение.
/// </summary>
internal sealed class Program
    : WinFormsApplication
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Program
        (
            string[] args,
            MainForm? mainForm = null
        )
        : base (args, mainForm)
    {
        // пустое тело конструктора
    }

    #endregion

    #region WinFormsApplication members

    /// <inheritdoc cref="MagnaApplication.DoTheWork"/>
    protected override int DoTheWork()
    {
        MainForm.WriteLog ("Hello from WinFormsApp");
        MainForm.WriteLog ("Hello again");
        MainForm.AddToolButton ("Hello").Click += (sender, eventArgs) =>
        {
            MessageBox.Show ("Hello");
        };

        MainForm.AddToolButton ("World").Click += (sender, eventArgs) =>
        {
            MessageBox.Show ("World");
        };

        MainForm.AddStatusLabel ("Status1");
        MainForm.AddStatusLabel ("Status2");
        MainForm.AddStatusLabel ("Status3");
        MainForm.AddMenuItem ("File")
            .DropDownItems.Add ("Exit", null, (sender, eventArgs) =>
            {
                MessageBox.Show ("Exit");
            });
        MainForm.AddMenuItem ("Edit");
        MainForm.AddMenuItem ("View");

        MainForm.MinimumSize();

        var panel = MainForm.ContentPanel;
        panel.VerticalArea<GroupBox> (620)
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

        panel.VerticalArea<Panel> (0)
            .Padding (5)
            .BorderStyleNone()
            .Pack (okButton, cancelButton);

        return 0;
    }

    /// <inheritdoc cref="WinFormsApplication.VisualInitialization"/>
    public override void VisualInitialization()
    {
        MainForm.Width = 780;
        MainForm.Height = 500;
        MainForm.AddLogBox();
    }

    #endregion

    #region Main

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static int Main (string[] args)
    {
        return new Program (args)
            .SetTitle ("Some WinForms Application")
            .PostConfigure (app =>
            {
                app.MainForm.AddStatusClock();
                app.MainForm.AddStatusMemory();
                app.MainForm.AddStatusLanguage();

                var timer = new Timer()
                {
                    Interval = 1000,
                    Enabled = true
                };
                timer.Tick += (_, _) =>
                {
                    app.MainForm.WriteLog ($"Now: {DateTime.Now:hh:mm:ss}");
                };
            })
            .Run<WinFormsApplication>();
    }

    #endregion
}
