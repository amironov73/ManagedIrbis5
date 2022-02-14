// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

using AM.Scripting;
using AM.Scripting.Barsik;
using AM.Scripting.WinForms;

#endregion

#nullable enable

namespace Barsik.WinForms;

internal static class Program
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
        Application.SetHighDpiMode (HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault (false);

        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        if (args.Length == 0)
        {
            // показываем подсказку и завершаемся нафиг
            MessageBox.Show ("Usage: Barsik <file...>");
            return 0;
        }

        var result = BarsikUtility.CreateAndRunInterpreter
            (
                args,
                interpreter =>
                {
                    var debugWriter = new DebugTextWriter();
                    interpreter.WithStdLib().WithWinForms();
                    interpreter.Context.Input = TextReader.Null;
                    interpreter.Context.Output = debugWriter;
                    interpreter.Context.Error = debugWriter;
                },
                (_, exception) =>
                {
                    MessageBox.Show (exception.ToString());
                },
                interpreter =>
                {
                    var output = new StringWriter();
                    var context = new Context
                        (
                            TextReader.Null,
                            output,
                            output,
                            interpreter.Context
                        );
                    context.DumpVariables();
                    MessageBox.Show (output.ToString());
                }
            );

        return result;
    }
}
