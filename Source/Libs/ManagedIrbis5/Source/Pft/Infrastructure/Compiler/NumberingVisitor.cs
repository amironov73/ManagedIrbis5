// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NumberingVisitor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Pft.Infrastructure.Walking;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class NumberingVisitor
        : PftVisitor
    {
        #region Properties

        public NodeDictionary Dictionary { get; private set; }

        public int LastId { get; private set; }

        #endregion

        #region Construction

        public NumberingVisitor
            (
                NodeDictionary dictionary,
                int start
            )
        {
            Dictionary = dictionary;
            LastId = start;
        }

        #endregion

        #region PftVisitor members

        /// <inheritdoc cref="PftVisitor.VisitNode" />
        public override bool VisitNode
            (
                PftNode node
            )
        {
            int id = ++LastId;
            NodeInfo info = new NodeInfo(id, node);
            Dictionary.Add(info);

            return true;
        }

        #endregion
    }
}
