// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PftFloor.cs -- наибольшее целое число, которое не больше, чем заданное
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Функция floor() возвращает наибольшее целое число (представленное как double),
    /// которое не больше, чем заданное.
    /// </summary>
    /// <example>
    /// <code>
    /// f(floor(1.2),0,0)
    /// </code>
    /// </example>
    public sealed class PftFloor
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
        public PftFloor()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFloor
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Floor);
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFloor
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
            var child = Children.FirstOrDefault() as PftNumeric;
            if (ReferenceEquals(child, null))
            {
                throw new PftCompilerException();
            }

            child.Compile(compiler);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("double value = ")
                .CallNodeMethod(child);

            compiler
                .WriteIndent()
                .WriteLine("double result = Math.Floor(value);");

            compiler
                .WriteIndent()
                .WriteLine("return result;");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        } // method Compile

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (Children.FirstOrDefault() is PftNumeric child)
            {
                child.Execute(context);
                Value = Math.Floor(child.Value);
            }

            OnAfterExecution(context);
        } // method Execute

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer
                .SingleSpace()
                .Write("floor(");
            base.PrettyPrint(printer);
            printer.Write(')');
        } // method PrettyPrint

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("floor(");
            var child = Children.FirstOrDefault();
            if (!ReferenceEquals(child, null))
            {
                result.Append(child);
            }
            result.Append(')');

            return result.ToString();
        } // method ToString

        #endregion

    } // class PftFloor

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
