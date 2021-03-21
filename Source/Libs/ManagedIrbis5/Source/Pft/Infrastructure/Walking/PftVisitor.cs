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
    /// Abstract AST visitor.
    /// </summary>
    public abstract class PftVisitor
    {
        #region Public methods

        /// <summary>
        /// Visit the node.
        /// </summary>
        /// <returns>
        /// <c>true</c> means "continue".
        /// </returns>
        public abstract bool VisitNode
            (
                PftNode node
            );

        #endregion
    }
}
