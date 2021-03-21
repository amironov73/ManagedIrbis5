// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftVisitor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Walking
{
    /// <summary>
    /// Context for AST visitor.
    /// </summary>
    public sealed class VisitorContext
    {
        #region Properties

        /// <summary>
        /// Visitor.
        /// </summary>
        public PftVisitor Visitor { get; private set; }

        /// <summary>
        /// Node.
        /// </summary>
        public PftNode Node { get; private set; }

        /// <summary>
        /// Result. <c>true</c> means "continue" (default value).
        /// </summary>
        public bool Result { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public VisitorContext
            (
                PftVisitor visitor,
                PftNode node
            )
        {
            Visitor = visitor;
            Node = node;
            Result = true;
        }

        #endregion
    }
}
