// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;
using System.Windows.Forms;

using AM.Kotik.Barsik;
using AM.Kotik.WinForms;

#endregion

#nullable enable

namespace Meow.WinForms;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static int Main
        (
            string[] args
        )
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        var result = Interpreter.CreateAndRunInterpreter
            (
                args,
                interpreter =>
                {
                    interpreter
                        .WithStdLib()
                        .WithWinForms();
                    interpreter.Context.Commmon.Settings.ReplMode = false;
                },
                (_, exception) =>
                {
                    MessageBox.Show
                        (
                            exception.ToString(),
                            "Exception",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    Console.Error.WriteLine (exception);
                }
            );

        return result;
    }
}
