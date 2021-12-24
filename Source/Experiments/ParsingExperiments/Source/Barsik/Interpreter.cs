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

#endregion

#nullable enable

namespace ParsingExperiments.Barsik;

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
    }

    #endregion

    #region Public methods

    // /// <summary>
    // /// Вычисление значения переменной.
    // /// </summary>
    // public AtomNode? Evaluate
    //     (
    //         string sourceCode
    //     )
    // {
    //     // Sure.NotNull (sourceCode);
    //
    //     var node = Grammar.ParseExpression (sourceCode);
    //
    //     return node ?? null;
    // }

    /// <summary>
    /// Запуск скрипта на исполнение.
    /// </summary>
    public void Execute
        (
            string sourceCode
        )
    {
        // Sure.NotNull (sourceCode);

        var program = Grammar.ParseProgram (sourceCode);

        // foreach (var statement in program.Statements)
        // {
        //     if (statement is DefinitionNode node)
        //     {
        //         var name = node.theName;
        //         var definition = new FunctionDefinition
        //             (
        //                 name,
        //                 node.theArguments,
        //                 node.theBody
        //             );
        //         var descriptor = new FunctionDescriptor
        //             (
        //                 name,
        //                 definition.CreateCallPoint()
        //             );
        //         Context.Functions[name] = descriptor;
        //     }
        // }

        program.Execute (Context);
    }

    // /// <summary>
    // /// Разбор текста программы.
    // /// </summary>
    // public static ProgramNode Parse
    //     (
    //         string sourceCode
    //     )
    // {
    //     return Grammar.ParseProgram (sourceCode);
    // }
    //

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // no code here
    }

    #endregion
}
