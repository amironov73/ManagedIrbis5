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
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using AM.Collections;

using ManagedIrbis.Scripting;

#endregion

#nullable enable

namespace SharpIrbis.WinForms;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static int Main (string[] args)
    {
        Application.SetHighDpiMode (HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault (false);

        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        var separated = ScriptCompiler.SeparateArguments (args);
        var compilerArguments = separated[0];
        var scriptArguments = separated[1];

        var compiler = new ScriptCompiler();
        var options = compiler.ParseArguments (compilerArguments);

        // добавляем специфичные для WinForms сборки
        compiler.AddReference (typeof (System.Drawing.Font));
        compiler.AddReference (typeof (System.ComponentModel.Component));
        compiler.AddReference (typeof (Form));
        compiler.AddReference (typeof (ManagedIrbis.WinForms.BusyForm));

        if (options.ExecuteOnly)
        {
            var fileName = Path.GetFullPath (options.OutputName);
            var assembly = Assembly.LoadFile (fileName);
            compiler.RunAssembly (assembly, scriptArguments);

            return 0;
        }

        if (options.InputFiles.IsNullOrEmpty())
        {
            MessageBox.Show
                (
                    "No input files specified",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

            return 1;
        }

        try
        {
            var compilation = compiler.Compile (options);
            if (options.CompileOnly)
            {
                compiler.EmitAssemblyToFile (compilation, options.OutputName);
            }
            else
            {
                var errors = new StringWriter();
                compiler.ErrorWriter = errors;
                var assembly = compiler.EmitAssemblyToMemory (compilation);
                if (assembly is null)
                {
                    MessageBox.Show
                        (
                            errors.ToString(),
                            "SharpIrbis",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                    return 1;
                }
                else
                {
                    compiler.RunAssembly (assembly, scriptArguments);
                }
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show
                (
                    exception.ToString(),
                    "ScriptIrbis",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

            return 1;
        }

        return 0;
    }
}
