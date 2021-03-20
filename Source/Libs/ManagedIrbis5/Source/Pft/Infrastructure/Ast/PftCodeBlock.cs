// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftCodeBlock.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftCodeBlock
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock
            (
                string text
            )
        {
            Text = text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.TripleCurly);

            if (string.IsNullOrEmpty(token.Text))
            {
                Magna.Error
                    (
                        "PftCodeBlock::Constructor: "
                        + "token text not set"
                    );

                throw new PftSyntaxException(token);
            }

            Text = token.Text;
        }

        #endregion

        #region Private members

#if CLASSIC || NETCORE

        private bool _compiled;

        private MethodInfo _method;

#endif

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

#if CLASSIC || NETCORE

            if (!_compiled)
            {
                Log.Trace("PftCodeBlock::Execute: compile method");

                _compiled = true;

                string text = Text;
                if (!string.IsNullOrEmpty(text))
                {
                    _method = SharpRunner.CompileSnippet
                        (
                            text,
                            "PftNode node, PftContext context",
                            err => context.WriteLine(this, err)
                        );
                }
            }

            if (!ReferenceEquals(_method, null))
            {
                Log.Trace ("PftCodeBlock::Execute: invoke method");

                _method.Invoke(null, new object[] {this, context});
            }

#endif

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer.EatNewLine();
            printer
                .WriteLine()
                .WriteIndentIfNeeded()
                .Write("{{{")
                .Write(Text)
                .WriteLine("}}}");
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString()" />
        public override string ToString() => "{{{" + Text + "}}}";

        #endregion
    }
}

