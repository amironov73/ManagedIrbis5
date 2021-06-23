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
using System.Reflection;
using System.Text;

using AM.Collections;

using ManagedIrbis.Scripting;

#endregion

#nullable enable

namespace SharpIrbis
{
    class Program
    {
        static int Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var separated = ScriptCompiler.SeparateArguments(args);
            var compilerArguments = separated[0];
            var scriptArguments = separated[1];

            var compiler = new ScriptCompiler();

            // Добавляем специфичные для ИРНИТУ сборки
            compiler.AddReference("System.Data.SqlClient");
            compiler.AddReference(typeof(LinqToDB.Sql));
            compiler.AddReference(typeof(Istu.OldModel.Attendance));

            var options = compiler.ParseArguments(compilerArguments);

            if (options.ExecuteOnly)
            {
                var fileName = Path.GetFullPath(options.OutputName);
                var assembly = Assembly.LoadFile(fileName);
                compiler.RunAssembly(assembly, scriptArguments);

                return 0;
            }

            if (options.InputFiles.IsNullOrEmpty())
            {
                Console.Error.WriteLine("No input files specified");

                return 1;
            }

            try
            {
                var compilation = compiler.Compile(options);
                if (options.CompileOnly)
                {
                    compiler.EmitAssemblyToFile(compilation, options.OutputName);
                }
                else
                {
                    var assembly = compiler.EmitAssemblyToMemory(compilation);
                    compiler.RunAssembly(assembly, scriptArguments);
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);

                return 1;
            }

            return 0;
        }
    }
}
