// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftEat.cs -- съедает результат выполнения операторов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Съедает результат выполнения операторов.
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
        /// Конструктор.
        /// </summary>
        public PftEat()
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PftEat
            (
                PftToken token
            )
            : base (token)
        {
            token.MustBe (PftTokenKind.EatOpen);

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PftEat
            (
                params PftNode[] children
            )
            : base (children)
        {
        } // constructor

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution (context);

            // Eat the output
            context.Evaluate (Children);

            OnAfterExecution (context);

        } // method Execute

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write ("[[[");
            base.PrettyPrint (printer);
            printer.Write ("]]] ");

        } // method PrettyPrint

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.Append ("[[[");
            PftUtility.NodesToText (builder, Children);
            builder.Append ("]]]");

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        } // method ToString

        #endregion

    } // class PftEat

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
