// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftSemicolon.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftSemicolon
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
        public PftSemicolon()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSemicolon
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Semicolon);
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            // Nothing to do actually

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode? Optimize()
        {
            // Take the node away from the AST

            return null;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            // Добавляем пробел для читабельности
            printer.Write("; ");
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString"/>
        public override string ToString() => ";";

        #endregion
    }
}
