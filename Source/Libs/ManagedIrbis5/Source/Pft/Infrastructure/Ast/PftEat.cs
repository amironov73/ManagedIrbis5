// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftEat.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Eat the output.
    /// </summary>
    /// <example>
    /// <code>
    /// 'Hello,', [[['again,']]] 'World'
    /// </code>
    /// </example>
    public sealed class PftEat
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
        public PftEat()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftEat
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.EatOpen);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftEat
            (
                params PftNode[] children
            )
            : base(children)
        {
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

            // Eat the output
            context.Evaluate(Children);

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write("[[[");
            base.PrettyPrint(printer);
            printer.Write("]]] ");
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("[[[");
            PftUtility.NodesToText(result, Children);
            result.Append("]]]");

            return result.ToString();
        }

        #endregion
    }
}
