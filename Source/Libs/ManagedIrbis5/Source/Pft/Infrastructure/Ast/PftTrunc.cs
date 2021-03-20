// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftTrunc.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftTrunc
        : PftNumeric
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTrunc()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTrunc
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Trunc);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTrunc
            (
                PftNumeric value
            )
        {
            Children.Add(value);
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            var child = Children.FirstOrDefault() as PftNumeric
                ?? throw new PftCompilerException();

            child.Compile(compiler);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("double value = ")
                .CallNodeMethod(child);

            compiler
                .WriteIndent()
                .WriteLine("double result = Math.Truncate(value);");

            compiler
                .WriteIndent()
                .WriteLine("return result;");

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

            var child = Children.FirstOrDefault() as PftNumeric;
            if (!ReferenceEquals(child, null))
            {
                child.Execute(context);
                Value = Math.Truncate(child.Value);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("trunc(");
            base.PrettyPrint(printer);
            printer.Write(')');
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }


        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("trunc(");
            var child = Children.FirstOrDefault();
            if (!ReferenceEquals(child, null))
            {
                result.Append(child);
            }
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
