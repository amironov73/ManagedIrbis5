﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCodeBlock.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#if CLASSIC || NETCORE

using System.Reflection;

#endif

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftCodeBlock
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

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
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            Text = text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.TripleCurly);

            if (string.IsNullOrEmpty(token.Text))
            {
                Log.Error
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
        public override string ToString()
        {
            return "{{{" + Text + "}}}";
        }

        #endregion
    }
}

