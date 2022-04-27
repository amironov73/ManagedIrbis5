// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
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

    /// <inheritdoc cref="WinFormsApplication.CreateMainForm"/>
    public override MainForm CreateMainForm()
    {
        return new MyMainForm();
    }

    #endregion

    #region Main

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static int Main (string[] args)
    {
        return new Program (args).Run();
    }

    #endregion
}
