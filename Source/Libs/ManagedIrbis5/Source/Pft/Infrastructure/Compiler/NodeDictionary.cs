﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NodeDictionary.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class NodeDictionary
    {
        #region Properties

        /// <summary>
        /// Forward dictionary.
        /// </summary>
        [NotNull]
        public Dictionary<int, NodeInfo> Forward { get; private set; }

        /// <summary>
        /// Backward dictionary.
        /// </summary>
        [NotNull]
        public Dictionary<PftNode, NodeInfo> Backward { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeDictionary()
        {
            Forward = new Dictionary<int, NodeInfo>();
            Backward = new Dictionary<PftNode, NodeInfo>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public void Add
            (
                [NotNull] NodeInfo info
            )
        {
            Code.NotNull(info, "info");

            Forward.Add(info.Id, info);
            Backward.Add(info.Node, info);
        }

        [CanBeNull]
        public NodeInfo Get
            (
                int id
            )
        {
            NodeInfo result;
            Forward.TryGetValue(id, out result);

            if (ReferenceEquals(result, null))
            {
                throw new PftCompilerException();
            }

            return result;
        }

        [NotNull]
        public NodeInfo Get
            (
                [NotNull] PftNode node
            )
        {
            NodeInfo result;
            Backward.TryGetValue(node, out result);

            if (ReferenceEquals(result, null))
            {
                throw new PftCompilerException();
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
