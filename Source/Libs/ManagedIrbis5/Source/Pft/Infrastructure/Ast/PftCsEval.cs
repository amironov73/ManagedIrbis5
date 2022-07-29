// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftCsEval.cs -- исполнение динамического формата на языке C#
 * Ars Magna project, http://arsmagna.ru
 */

// IL3000: Avoid accessing Assembly file path when publishing as a single file

#pragma warning disable IL3000

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using AM;
using AM.IO;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Исполнение динамического формата на языке C#.
/// </summary>
public sealed class PftCsEval
    : PftNode
{
    #region Nested classes

    class SnippetCompiler
    {
        /// <summary>
        /// Ссылки на сборки.
        /// </summary>
        private List<MetadataReference> References { get; } = new ();

        private const string Prologue = @"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

namespace ManagedIrbis.UserSpace
{
    public static class <<<CLASSNAME>>>
    {
        public static void UserCode ()
        {
";

        private const string Epilogue = @"
        }
    }
}
";

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

            AddReference (typeof (Utility));
            AddReference (typeof (ISyncProvider));

            AddReference (typeof (Microsoft.Extensions.Logging.Abstractions.NullLogger));
            AddReference (typeof (Microsoft.Extensions.Logging.Logger<>));
        }

        /// <summary>
        /// Добавление ссылки на указанную сборку.
        /// </summary>
        private void AddReference (string assemblyRef)
        {
            AddReference (Assembly.Load (assemblyRef));
        }

        /// <summary>
        /// Добавление ссылки на указанную сборку.
        /// </summary>
        private void AddReference (Assembly assembly)
        {
            // TODO: в single-exe-application .Location возвращает string.Empty
            // consider using the AppContext.BaseDirectory
            References.Add (MetadataReference.CreateFromFile (assembly.Location));
        }

        /// <summary>
        /// Добавление ссылки на сборку, содержащую указанный тип.
        /// </summary>
        private void AddReference (Type type)
        {
            AddReference (type.Assembly);
        }

        /// <summary>
        /// Компиляция сниппета.
        /// </summary>
        public MethodInfo? CompileSnippet
            (
                string sourceCode
            )
        {
            var className = "Class" + Guid.NewGuid().ToString ("N");
            sourceCode = Prologue.Replace ("<<<CLASSNAME>>>", className)
                         + sourceCode
                         + Epilogue;

            var syntaxTree = CSharpSyntaxTree.ParseText (sourceCode);
            var compilationOptions = new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create
                (
                    className + ".dll",
                    new[] { syntaxTree },
                    References,
                    compilationOptions
                );

            using var stream = MemoryCenter.GetMemoryStream();
            var emitResult = compilation.Emit (stream);
            if (!emitResult.Success)
            {
                var failures = emitResult.Diagnostics.Where
                    (
                        diagnostic => diagnostic.IsWarningAsError
                                      || diagnostic.Severity == DiagnosticSeverity.Error
                    );

                // foreach (var failure in failures)
                // {
                //     ErrorWriter.WriteLine ($"{failure.Id}: {failure.GetMessage()}");
                // }

                foreach (var _ in failures)
                {
                    return null;
                }
            }

            var bytes = stream.ToArray();
            var assembly = Assembly.Load (bytes);
            var type = assembly.GetType ("ManagedIrbis.UserSpace." + className)
                .ThrowIfNull (className);
            var method = type.GetMethod ("UserCode");

            return method;
        }
    }

    #endregion

    #region Properties

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftCsEval()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftCsEval
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.CsEval);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftCsEval
        (
            params PftNode[] children
        )
        : base (children)
    {
    }

    #endregion

    #region Private members

    private string? _text;
    private MethodInfo? _method;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        OnBeforeExecution (context);

        var expression = context.Evaluate (Children);
        if (!string.IsNullOrEmpty (expression))
        {
            if (expression != _text)
            {
                _text = expression;
                var compiler = new SnippetCompiler();
                compiler.AddDefaultReferences();
                _method = compiler.CompileSnippet (expression);
            }

            if (!ReferenceEquals (_method, null))
            {
                _method.Invoke (null, null);
            }
        }

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        printer
            .WriteIndentIfNeeded()
            .Write ("cseval(")
            .WriteNodes (Children)
            .Write (")");
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString()" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("cseval(");
        var first = true;
        foreach (var child in Children)
        {
            if (!first)
            {
                builder.Append (' ');
            }

            builder.Append (child);
            first = false;
        }

        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
