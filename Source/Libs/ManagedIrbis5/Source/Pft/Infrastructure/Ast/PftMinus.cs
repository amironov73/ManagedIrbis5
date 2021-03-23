// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PftMinus.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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
    public sealed class PftMinus
        : PftNumeric
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMinus()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMinus
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Minus);
        } // constructor

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
                 .WriteLine("double result = -value;");

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

            var child = Children.FirstOrDefault() as PftNumeric
                        ?? throw new PftSyntaxException();

            child.Execute(context);
            Value = - child.Value;

            OnAfterExecution(context);
        } // method Execute

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("-(");
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
            StringBuilder result = new StringBuilder();
            result.Append("-(");
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return result.ToString();
        } // method ToString

        #endregion

    } // class PftMinus

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
