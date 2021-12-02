// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Interpreter.cs -- интерпретатор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Интерпретатор.
    /// </summary>
    public sealed class Interpreter
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
                Dictionary<string, dynamic?>? variables = null,
                TextWriter? output = null
            )
        {
            variables ??= new ();
            output ??= Console.Out;

            Context = new (variables, output);
        }

        #endregion

        #region Public methods

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

            foreach (var statement in program.Statements)
            {
                if (statement is DefinitionNode node)
                {
                    var name = node.theName;
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

            program.Execute (Context);
        }

        #endregion
    }
}
