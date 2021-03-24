// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NodeDictionary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class NodeDictionary
    {
        #region Properties

        /// <summary>
        /// Forward dictionary.
        /// </summary>
        public Dictionary<int, NodeInfo> Forward { get; } = new();

        /// <summary>
        /// Backward dictionary.
        /// </summary>
        public Dictionary<PftNode, NodeInfo> Backward { get; } = new();

        #endregion

        #region Public methods

        public void Add
            (
                NodeInfo info
            )
        {
            Forward.Add(info.Id, info);
            Backward.Add(info.Node, info);
        } // method Add

        public NodeInfo Get
            (
                int id
            )
        {
            Forward.TryGetValue(id, out var result);

            if (ReferenceEquals(result, null))
            {
                throw new PftCompilerException();
            }

            return result;
        } // method Get

        public NodeInfo Get
            (
                PftNode node
            )
        {
            Backward.TryGetValue(node, out var result);

            if (ReferenceEquals(result, null))
            {
                throw new PftCompilerException();
            }

            return result;
        } // method Get

        #endregion

    } // class NodeDictionary

} // namespace ManagedIrbis.Pft.Infrastructure.Compiler
