// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniversalVisitor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Walking
{
    /// <summary>
    /// Context for AST visitor.
    /// </summary>
    public sealed class UniversalVisitor
        : PftVisitor
    {
        #region Events

        /// <summary>
        /// Visit the node.
        /// </summary>
        public event Action<VisitorContext>? Visitor;

        #endregion

        #region Properties

        /// <summary>
        /// Some arbitrary user data (optional).
        /// </summary>
        public object? UserData { get; set; }

        #endregion

        #region PftVisitor members

        /// <inheritdoc cref="PftVisitor.VisitNode" />
        public override bool VisitNode
            (
                PftNode node
            )
        {
            var visitor = Visitor;
            if (!ReferenceEquals(visitor, null))
            {
                var context = new VisitorContext(this, node);
                visitor(context);

                return context.Result;
            }

            return false;
        }

        #endregion
    }
}
