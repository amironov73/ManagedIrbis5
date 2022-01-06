// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Interpreter.cs -- интерпретатор Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.IO;
using System.Linq;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Интерпретатор Барсика.
/// </summary>
public sealed class Interpreter
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Контекст исполнения программы.
    /// </summary>
    public Context Context { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Interpreter
        (
            TextReader? input = null,
            TextWriter? output = null,
            TextWriter? error = null
        )
    {
        input ??= Console.In;
        output ??= Console.Out;
        error ??= Console.Error;

        Context = new (input, output, error);
        Context.Interpreter = this;

        // устанавливаем значения стандартных переменных
        Context.SetVariable ("__DIR__", string.Empty);
        Context.SetVariable ("__FILE__", string.Empty);
        Context.SetVariable ("__DOTNET__", Environment.Version);
        Context.SetVariable ("__ROOT__", AppContext.BaseDirectory);
        Context.SetVariable ("__VER__", Assembly.GetExecutingAssembly().GetName().Version);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Вычисление значения переменной.
    /// </summary>
    public AtomNode Evaluate
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        var node = Grammar.ParseExpression (sourceCode);

        return node;
    }

    /// <summary>
    /// Запуск скрипта на исполнение.
    /// </summary>
    public void Execute
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        var program = Grammar.ParseProgram (sourceCode);

        var haveDefinitions = false;
        foreach (var statement in program.Statements)
        {
            if (statement is DefinitionNode node)
            {
                haveDefinitions = true;
                var name = node.Name;
                var definition = new FunctionDefinition
                    (
                        name,
                        node.theArguments,
                        node.theBody
                    );
                var descriptor = new FunctionDescriptor
                    (
                        name,
                        definition.CreateCallPoint()
                    );
                Context.Functions[name] = descriptor;
            }
        }

        if (haveDefinitions)
        {
            program.Statements = program.Statements
                .Where (stmt => stmt is not PseudoNode)
                .ToList();
        }

        program.Execute (Context);
    }

    /// <summary>
    /// Загрузка исходного кода из указанного файла
    /// с последующим исполнением.
    /// </summary>
    /// <param name="fileName">Имя файла скрипта.</param>
    public void ExecuteFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        try
        {
            var fullPath = Path.GetFullPath (fileName);
            Context.Variables["__FILE__"] = fullPath;
            Context.Variables["__DIR__"] = Path.GetDirectoryName (fullPath);

            var sourceCode = File.ReadAllText (fileName);
            Execute (sourceCode);
        }
        finally
        {
            Context.Variables.Remove ("__FILE__");
            Context.Variables.Remove ("__DIR__");
        }
    }

    /// <summary>
    /// Разбор текста программы.
    /// </summary>
    public static ProgramNode ParseProgram
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        return Grammar.ParseProgram (sourceCode);
    }

    /// <summary>
    /// Подключение стандартной библиотеки.
    /// </summary>
    /// <returns><c>this</c>.</returns>
    public Interpreter WithStdLib()
    {
        Context.AttachModule (new StdLib());

        return this;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // no code here
    }

    #endregion
}
