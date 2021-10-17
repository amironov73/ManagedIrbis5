// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftEmpty.cs -- проверка на пустую строку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Проверка, не пустая ли строка.
    /// </summary>
    /// <example>
    /// <code>
    /// if empty('Hello') then 'Empty' else 'Not empty' fi/
    /// if empty(v500) then 'Empty' else 'Not empty' fi/
    /// </code>
    /// </example>
    public sealed class PftEmpty
        : PftCondition
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PftEmpty()
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PftEmpty
            (
                PftToken token
            )
            : base (token)
        {
            token.MustBe (PftTokenKind.Empty);

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PftEmpty
            (
                params PftNode[] children
            )
            : base (children)
        {
        } // constructor

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.CompileNodes (Children);

            var actionName = compiler.CompileAction (Children);

            compiler.StartMethod (this);

            if (string.IsNullOrEmpty (actionName))
            {
                compiler
                    .WriteIndent()
                    .WriteLine ("bool result = true;");
            }
            else
            {
                compiler
                    .WriteIndent()
                    .WriteLine ("string text = Evaluate({0});", actionName);

                compiler
                    .WriteIndent()
                    .WriteLine ("bool result = string.IsNullOrEmpty (text);");
            }

            compiler
                .WriteIndent()
                .WriteLine ("return result;");

            compiler.EndMethod (this);
            compiler.MarkReady (this);

        } // method COmpile

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution (context);

            var text = context.Evaluate (Children);

            Value = string.IsNullOrEmpty (text);

            OnAfterExecution (context);

        } // method Execute

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write ("empty(");
            base.PrettyPrint (printer);
            printer.Write (')');

        } // method PrettyPrint

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.Append ("empty(");
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

        } // method ToString

        #endregion

    } // class PftEmpty

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
