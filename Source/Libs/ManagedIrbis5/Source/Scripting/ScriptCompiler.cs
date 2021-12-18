// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* ScriptCompiler.cs -- компилятор скриптов
 * Ars Magna project, http://arsmagna.ru
 */

// IL3000: Avoid accessing Assembly file path when publishing as a single file
#pragma warning disable IL3000

#region Using directives

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using AM.Collections;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting
{
    /// <summary>
    /// Компилятор скриптов.
    /// </summary>
    public sealed class ScriptCompiler
    {
        #region Properties

        /// <summary>
        /// Ссылки на сборки.
        /// </summary>
        public List<MetadataReference> References { get; }

        /// <summary>
        /// Поток для вывода ошибок.
        /// </summary>
        public TextWriter ErrorWriter { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ScriptCompiler()
        {
            References = new List<MetadataReference>();
            ErrorWriter = Console.Error;
        }

        #endregion

        #region Private members

        private static readonly string _applicationSourceCode = @"using Microsoft.Extensions.Logging;
using AM.AppServices;
using ManagedIrbis.AppServices;

new Program(args).Run();

internal class Program : IrbisApplication
{
    public Program(string[] args) : base(args) {}

    protected override int ActualRun()
    {
    }

    static int Main(string[] args) => new Program(args).Run();

}";

        private static string _AddLines
            (
                string text,
                string prefix,
                List<string> lines
            )
        {
            if (lines.IsNullOrEmpty())
            {
                return text;
            }

            var builder = new StringBuilder();
            foreach (var line in lines)
            {
                builder.AppendLine ($"{prefix}{line}");
            }

            // пустая строка для красоты
            builder.AppendLine();

            return builder.ToString();
        }

        private static string _MergeCode
            (
                string outerCode,
                string innerCode
            )
        {
            // синтаксическое дерево
            var outerTree = CSharpSyntaxTree.ParseText (outerCode);
            var innerTree = CSharpSyntaxTree.ParseText (innerCode);

            // корневой узел
            var outerRoot = outerTree.GetRoot();
            var innerRoot = innerTree.GetRoot();

            // находим метод ActualRun
            var actualRun =
                (
                    from method in outerRoot.DescendantNodes()
                        .OfType<MethodDeclarationSyntax>()
                    where method.Identifier.ValueText == "ActualRun"
                    select method
                )
                .First();

            var statements = innerRoot.ChildNodes();
            var newActualRun = actualRun;
            foreach (var node in statements)
            {
                newActualRun = newActualRun.AddBodyStatements (((GlobalStatementSyntax)node).Statement);
            }

            // return 0;
            newActualRun = newActualRun.AddBodyStatements
                (
                    SyntaxFactory.ReturnStatement
                        (
                            SyntaxFactory.LiteralExpression
                                (
                                    SyntaxKind.NumericLiteralExpression,
                                    SyntaxFactory.Literal (0)
                                )
                        )
                );

            var resultRoot = outerRoot.ReplaceNode (actualRun, newActualRun)
                .NormalizeWhitespace();

            return resultRoot.ToFullString();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление ссылок на сборки по умолчанию.
        /// </summary>
        public void AddDefaultReferences()
        {
            AddReference ("System.Runtime");
            AddReference (typeof (object));
            AddReference (typeof (Console));
            AddReference (typeof (System.Collections.IEnumerable));
            AddReference (typeof (List<>));
            AddReference (typeof (Encoding));
            AddReference (typeof (File));
            AddReference (typeof (Enumerable));
            AddReference ("System.ComponentModel");
            AddReference ("System.Data.Common");
            AddReference ("System.Linq.Expressions");

            AddReference (typeof (AM.Utility));
            AddReference (typeof (ISyncProvider));

            AddReference (typeof (Microsoft.Extensions.Logging.Abstractions.NullLogger));
            AddReference (typeof (Microsoft.Extensions.Logging.Logger<>));
        }

        /// <summary>
        /// Добавление ссылки на указанную сборку.
        /// </summary>
        public void AddReference (string assemblyRef)
        {
            AddReference (Assembly.Load (assemblyRef));
        }

        /// <summary>
        /// Добавление ссылки на указанную сборку.
        /// </summary>
        public void AddReference (Assembly assembly)
        {
            // TODO: в single-exe-application .Location возвращает string.Empty
            // consider using the AppContext.BaseDirectory
            References.Add (MetadataReference.CreateFromFile (assembly.Location));
        }

        /// <summary>
        /// Добавление ссылки на сборку, содержащую указанный тип.
        /// </summary>
        public void AddReference (Type type)
        {
            AddReference (type.Assembly);
        }

        /// <summary>
        /// Компиляция текста скрипта в соответствии с опциями.
        /// </summary>
        public CSharpCompilation Compile
            (
                ScriptOptions options
            )
        {
            if (!options.NoDefaultReferences)
            {
                AddDefaultReferences();
            }

            foreach (var reference in options.References)
            {
                AddReference (reference);
            }

            var forest = new List<SyntaxTree>();
            foreach (var inputFileName in options.InputFiles)
            {
                var sourceCode = File.ReadAllText (inputFileName);
                if (options.ApplicationMode)
                {
                    sourceCode = _MergeCode (_applicationSourceCode, sourceCode);
                }

                sourceCode = _AddLines (sourceCode, "using ", options.Usings);
                sourceCode = _AddLines (sourceCode, "#define ", options.Defines);

                if (options.ShowApplicationCode)
                {
                    Console.WriteLine (sourceCode);
                }

                var syntaxTree = CSharpSyntaxTree.ParseText (sourceCode);
                forest.Add (syntaxTree);
            }

            var compilationOptions = options.CompilationOptions
                ?? new CSharpCompilationOptions (OutputKind.ConsoleApplication);

            var result = CSharpCompilation.Create
                (
                    options.OutputName,
                    forest,
                    References,
                    compilationOptions
                );

            return result;
        }

        /// <summary>
        /// Простая компиляция текста скрипта.
        /// </summary>
        public CSharpCompilation CompieScriptText
            (
                string fileName,
                string scriptText
            )
        {
            var syntaxTree = CSharpSyntaxTree.ParseText (scriptText);
            var result = CSharpCompilation.Create
                (
                    fileName,
                    new[] { syntaxTree },
                    References
                );

            return result;
        }

        /// <summary>
        /// Получение сборки в указанный поток.
        /// </summary>
        public bool EmitAssemblyToStream
            (
                Compilation compilation,
                Stream exeStream,
                Stream? pdbStream = null
            )
        {
            EmitResult emitResult;
            if (pdbStream is null)
            {
                emitResult = compilation.Emit (exeStream);
            }
            else
            {
                var emitOptions = new EmitOptions (debugInformationFormat: DebugInformationFormat.Pdb);
                emitResult = compilation.Emit (exeStream, pdbStream, options: emitOptions);
            }

            if (!emitResult.Success)
            {
                var failures = emitResult.Diagnostics.Where
                    (
                        diagnostic => diagnostic.IsWarningAsError
                                      || diagnostic.Severity == DiagnosticSeverity.Error
                    );

                foreach (var failure in failures)
                {
                    ErrorWriter.WriteLine ($"{failure.Id}: {failure.GetMessage()}");
                }
            }

            return emitResult.Success;
        }

        /// <summary>
        /// Получение сборки на диске.
        /// </summary>
        public bool EmitAssemblyToFile
            (
                Compilation compilation,
                string exeName,
                string? pdbName = null
            )
        {
            using Stream? pdbStream = pdbName is null ? null : File.Create (pdbName);
            using var exeStream = File.Create (exeName);

            return EmitAssemblyToStream (compilation, exeStream);
        }

        /// <summary>
        /// Получение сборки в памяти по результатам компиляции.
        /// </summary>
        public Assembly? EmitAssemblyToMemory
            (
                Compilation compilation
            )
        {
            using var stream = new MemoryStream();
            if (!EmitAssemblyToStream (compilation, stream))
            {
                return null;
            }

            stream.Seek (0, SeekOrigin.Begin);
            var memory = stream.ToArray();
            var result = Assembly.Load (memory);

            return result;
        }

        /// <summary>
        /// Разбор аргументов, предназначенных для компилятора.
        /// </summary>
        public ScriptOptions ParseArguments
            (
                string[] args
            )
        {
            var refOption = new Option<string[]> ("r")
            {
                Description = "reference to assembly",
                Arity = ArgumentArity.ZeroOrMore
            };
            var compileOption = new Option<bool> ("c")
            {
                Description = "compile only"
            };
            var outputOption = new Option<string> ("o")
            {
                Description = "output file name"
            };
            var executeOption = new Option<bool> ("e")
            {
                Description = "execute only"
            };
            var applicationOption = new Option<bool> ("a")
            {
                Description = "application mode"
            };
            var defineOption = new Option<string[]> ("d")
            {
                Description = "#define",
                Arity = ArgumentArity.OneOrMore
            };
            var usingOption = new Option<string[]> ("u")
            {
                Description = "using directive",
                Arity = ArgumentArity.ZeroOrMore
            };
            var showOption = new Option<bool> ("s")
            {
                Description = "show resulting application code"
            };
            var inputArg = new Argument<string[]> ("input")
            {
                Arity = ArgumentArity.ZeroOrMore,
                Description = "input files"
            };
            var rootCommand = new RootCommand ("SharpIrbis")
            {
                refOption,
                compileOption,
                outputOption,
                executeOption,
                applicationOption,
                defineOption,
                usingOption,
                showOption,
                inputArg
            };

            var parseResult = new CommandLineBuilder (rootCommand)
                .UseDefaults()
                .Build()
                .Parse (args);

            var result = new ScriptOptions();

            var references = parseResult.ValueForOption (refOption);
            if (references is not null)
            {
                result.References.AddRange (references);
            }

            var defines = parseResult.ValueForOption (defineOption);
            if (defines is not null)
            {
                result.Defines.AddRange (defines);
            }

            var usings = parseResult.ValueForOption (usingOption);
            if (usings is not null)
            {
                result.Usings.AddRange (usings);
            }

            var inputs = parseResult.ValueForArgument (inputArg);
            if (inputs is not null)
            {
                result.InputFiles.AddRange (inputs);
            }

            var outputName = parseResult.ValueForOption (outputOption);
            if (!string.IsNullOrEmpty (outputName))
            {
                result.OutputName = outputName;
            }

            result.ApplicationMode = parseResult.ValueForOption (applicationOption);
            result.CompileOnly = parseResult.ValueForOption (compileOption);
            result.ExecuteOnly = parseResult.ValueForOption (executeOption);
            result.ShowApplicationCode = parseResult.ValueForOption (showOption);

            return result;
        }

        /// <summary>
        /// Запуск скомпилированной сборки.
        /// </summary>
        public void RunAssembly
            (
                Assembly? assembly,
                string[] args
            )
        {
            if (assembly is not null)
            {
                var entryPoint = assembly.EntryPoint;
                if (entryPoint is not null)
                {
                    entryPoint.Invoke (null, new object?[] { args });
                }
            }
        }

        /// <summary>
        /// Разделение аргументов на компиляторные и скриптовые.
        /// Разделителем служит "--".
        /// </summary>
        public static string[][] SeparateArguments
            (
                string[] args
            )
        {
            var compilerArgs = new List<string>();
            var scriptArgs = new List<string>();

            int index;

            // сначала отбираем аргументы компилятора
            for (index = 0; index < args.Length; index++)
            {
                if (args[index] == "--")
                {
                    ++index;
                    break;
                }

                compilerArgs.Add (args[index]);
            }

            // все, что осталось -- аргументы скрипта
            for (; index < args.Length; index++)
            {
                scriptArgs.Add (args[index]);
            }

            return new[] { compilerArgs.ToArray(), scriptArgs.ToArray() };
        }

        #endregion
    }
}
