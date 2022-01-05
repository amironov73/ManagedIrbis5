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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AM.Scripting.Barsik;
using AM.Scripting.WinForms;

#endregion

namespace Barsik.WinForms;

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
        Application.SetHighDpiMode (HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault (false);

        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        try
        {
            var dump = false;
            var index = 0;
            var interpreter = new Interpreter()
                .WithStdLib()
                .WithWinForms();

            foreach (var fileName in args)
            {
                if (fileName == "-d")
                {
                    dump = true;
                    continue;
                }

                string sourceCode;
                if (fileName == "-e")
                {
                    sourceCode = string.Join (' ', args.Skip (index + 1));
                    interpreter.Execute (sourceCode);
                    break;
                }

                sourceCode = File.ReadAllText (fileName);
                interpreter.Execute (sourceCode);

                index++;
            }

            if (dump)
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
        }
        catch (Exception exception)
        {
            MessageBox.Show (exception.ToString());
            return 1;
        }

        return 0;
    }
}
