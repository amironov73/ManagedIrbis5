// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftDebugEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

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
        public PftContext? Context { get; set; }

        /// <summary>
        /// AST node.
        /// </summary>
        public PftNode? Node { get; set; }

        /// <summary>
        /// Variable.
        /// </summary>
        public PftVariable? Variable { get; set; }

        #endregion

    }
}
