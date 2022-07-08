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
using System.Windows.Forms;

using AM.AppServices;
using AM.Windows.Forms.AppServices;

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
        return new Program (args).Run<WinFormsApplication>();
    }

    #endregion
}
