// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftVerbatim.cs -- буквальный строковый литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Буквальный строковый литерал (в тройных угловых скобках).
    /// </summary>
    public sealed class PftVerbatim
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression => true;

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        /// <inheritdoc cref="PftNode.RequiresConnection" />
        public override bool RequiresConnection => false;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVerbatim()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVerbatim
            (
                string text
            )
        {
            Text = text;
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVerbatim
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.TripleLess);

            Text = PrepareText
                (
                    token.Text.ThrowIfNull("token.Text")
                );
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Prepare text.
        /// </summary>
        public static string? PrepareText(string text) => text.DosToUnix();

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
                        "Context.Write(\"{0}\")",
                        CompilerUtility.Escape(Text)
                    );

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

            context.Write(this, Text);

            OnAfterExecution(context);
        } // method Execute

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .WriteIndentIfNeeded()
                .Write("<<<")
                .Write(Text)
                .Write(">>>");
        } // method PrettyPrint

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString() => "<<<" + Text + ">>>";

        #endregion
    }
}
