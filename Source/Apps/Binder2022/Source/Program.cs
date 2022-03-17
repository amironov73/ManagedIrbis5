// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using AM.AppServices;
using AM.Windows.Forms;

using ManagedIrbis.AppServices;

using Microsoft.Extensions.Logging;

#endregion

namespace Binder2022;

/// <summary>
/// ТОчка входа в программу.
/// </summary>
internal sealed class Program
    : IrbisApplication
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    private Program (string[] args)
        : base (args)
    {
    }

    /// <inheritdoc cref="MagnaApplication.ActualRun"/>
    protected override int ActualRun()
    {
        try
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new MainForm (this));

            Connection?.Disconnect();
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error");
            ExceptionBox.Show (exception);
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main
        (
            string[] args
        )
    {
        new Program (args).Run();
    }
}
