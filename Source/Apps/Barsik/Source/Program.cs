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

using System;
using System.IO;
using System.Linq;
using System.Text;

using AM.Scripting.Barsik;

#endregion

#nullable enable

namespace Barsik
{
    /// <summary>
    /// Вся логика программы в одном классе.
    /// </summary>
    class Program
    {
        static void DoRepl
            (
                Interpreter interpreter
            )
        {
            var version = typeof (Interpreter).Assembly.GetName().Version;
            interpreter.Context.Output.WriteLine ($"Barsik interpreter {version}");
            interpreter.Context.Output.WriteLine ("Press ENTER twice to exit");
            new Repl (interpreter).Loop();
        }

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        static int Main
            (
                string[] args
            )
        {
            Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

            var interpreter = new Interpreter().WithStdLib();

            try
            {
                var dump = false;
                var index = 0;
                string sourceCode;

                if (args.Length == 0)
                {
                    DoRepl (interpreter);
                }

                foreach (var fileName in args)
                {
                    if (fileName == "-d")
                    {
                        dump = true;
                        continue;
                    }

                    if (fileName == "-r")
                    {
                        DoRepl (interpreter);
                        continue;
                    }

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
                    interpreter.Context.DumpVariables();
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine (exception);
                return 1;
            }

            return 0;
        }
    }
}
