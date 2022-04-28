// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в приложение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;
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
        : base(args, mainForm)
    {
        // пустое тело конструктора
    }

    #endregion

    #region WinFormsApplication members

    // /// <inheritdoc cref="WinFormsApplication.CreateMainForm"/>
    // public override MainForm CreateMainForm()
    // {
    //     return new MyMainForm();
    // }

    /// <inheritdoc cref="WinFormsApplication.VisualInitialization"/>
    public override void VisualInitialization()
    {
        MainForm.AddLogBox();
    }

    #endregion

    #region Main

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static int Main (string[] args)
    {
        return new Program (args).Run(() =>
        {
            var app = (Program) Magna.Application;
            var main = app.MainForm;

            main.WriteLog ("Hello from WinFormsApp");
            main.WriteLog ("Hello again");
            main.AddToolButton ("Hello").Click += (sender, eventArgs) =>
            {
                MessageBox.Show ("Hello");
            };
            main.AddToolButton ("World").Click += (sender, eventArgs) =>
            {
                MessageBox.Show ("World");
            };
            main.AddStatusLabel ("Status1");
            main.AddStatusLabel ("Status2");
            main.AddStatusLabel ("Status3");
            main.AddMenuItem ("File")
                .DropDownItems.Add ("Exit", null, (sender, eventArgs) =>
                {
                    MessageBox.Show ("Exit");
                });
            main.AddMenuItem ("Edit");
            main.AddMenuItem ("View");

            return 0;
        });
    }

    #endregion
}
