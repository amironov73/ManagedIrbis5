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
        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        static int Main
            (
                string[] args
            )
        {
            Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

            if (args.Length == 0)
            {
                return 1;
            }

            var interpreter = new Interpreter();

            try
            {
                var dump = false;

                foreach (var fileName in args)
                {
                    if (fileName == "-d")
                    {
                        dump = true;
                        continue;
                    }

                    var sourceCode = File.ReadAllText (fileName);
                    interpreter.Execute (sourceCode);
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
