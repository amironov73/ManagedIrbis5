﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftSlash.cs -- переход на новую строку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Команда / приводит к размещению последующих данных
    /// с начала следующей строки.
    /// Однако подряд расположенные команды /,
    /// хотя и являются синтаксически правильными,
    /// но имеют тот же смысл, что и одна команда /,
    /// т.е.команда / никогда не создает пустых строк.
    /// </summary>
    public sealed class PftSlash
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression => true;

        /// <inheritdoc cref="PftNode.RequiresConnection" />
        public override bool RequiresConnection => false;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSlash()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSlash
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Slash);
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.StartMethod(this);
            compiler
                .WriteIndent()
                .WriteLine("if (!Context.Output.HaveEmptyLine())")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                    .WriteIndent()
                    .WriteLine("Context.WriteLine(null);")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}");
            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!context.Output.HaveEmptyLine())
            {
                context.WriteLine(this);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer
                .SingleSpace()
                .Write('/')
                .SingleSpace()
                .WriteLineIfNeeded();
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => "/";

        #endregion
    }
}
