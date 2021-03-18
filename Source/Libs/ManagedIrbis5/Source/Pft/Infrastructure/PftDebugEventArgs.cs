﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftDebugEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;



#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    ///
    /// </summary>

    public sealed class PftDebugEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Cancel execution?
        /// </summary>
        public bool CancelExecution { get; set; }

        /// <summary>
        /// Context.
        /// </summary>
        [CanBeNull]
        public PftContext Context { get; set; }

        /// <summary>
        /// AST node.
        /// </summary>
        [CanBeNull]
        public PftNode Node { get; set; }

        /// <summary>
        /// Variable.
        /// </summary>
        [CanBeNull]
        public PftVariable Variable { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftDebugEventArgs()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftDebugEventArgs
            (
                [CanBeNull] PftContext context,
                [CanBeNull] PftNode node
            )
        {
            Context = context;
            Node = node;
        }

        #endregion
    }
}
