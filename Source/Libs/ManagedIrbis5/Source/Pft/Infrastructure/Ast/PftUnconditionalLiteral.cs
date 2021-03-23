// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftUnconditionalLiteral.cs -- безусловный строковый литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Безусловный строковый литерал (в одинарных кавычках).
    /// </summary>
    public sealed class PftUnconditionalLiteral
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression => true;

        /// <inheritdoc cref="PftNode.Text" />
        public override string? Text
        {
            get => base.Text;
            set => base.Text = PftUtility.PrepareText(value);
        }

        /// <inheritdoc cref="PftNode.RequiresConnection" />
        public override bool RequiresConnection => false;

        /// <summary>
        /// Throw an exception when empty literal detected.
        /// </summary>
        public static bool ThrowOnEmpty { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PftUnconditionalLiteral()
        {
            ThrowOnEmpty = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnconditionalLiteral()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnconditionalLiteral
            (
                string text
            )
        {
            Text = text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnconditionalLiteral
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.UnconditionalLiteral);

            try
            {
                Text = token.Text.ThrowIfNull("token.Text");
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "PftUnconditionalLiteral::Constructor",
                        exception
                    );

                throw new PftSyntaxException(token, exception);
            }

            if (string.IsNullOrEmpty(Text) && ThrowOnEmpty)
            {
                Magna.Error
                    (
                        "PftUnconditionalLiteral::Constructor: "
                        + "empty unconditional literal"
                    );

                throw new PftSyntaxException(token);
            }
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
                .WriteLine
                (
                    "Context.Write(null,\"{0}\");",
                    Text
                );
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

            var text = Text;

            // Never throw on empty literal

            if (context.UpperMode
                && !ReferenceEquals(text, null))
            {
                text = IrbisText.ToUpper(text);
            }

            context.Write
                (
                    this,
                    text
                );

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            // Never optimize!

            return this;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write('\'')
                .Write(Text)
                .Write('\'');
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => '\'' + Text + '\'';

        #endregion

    } // class PftUnconditionalLiteral

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
