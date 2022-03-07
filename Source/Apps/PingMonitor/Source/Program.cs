// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace PingMonitor;

static class Program
{
    static void _ThreadException
        (
            object sender,
            ThreadExceptionEventArgs eventArgs
        )
    {
        ExceptionBox.Show (eventArgs.Exception);
        Environment.FailFast
            (
                "Shutting down",
                eventArgs.Exception
            );
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        try
        {
            Magna.Initialize (Array.Empty<string>());

            Application.SetUnhandledExceptionMode (UnhandledExceptionMode.Automatic);
            Application.ThreadException += _ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault (false);

            Application.Run (new MainForm());
        }
        catch (Exception exception)
        {
            ExceptionBox.Show (exception);
        }
    }
}
