// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PagingDataGridViewEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// EventArgs for <see cref="PagingDataGridView"/>.
    /// </summary>
    public sealed class PagingDataGridViewEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Current row.
        /// </summary>
        public DataGridViewRow? CurrentRow { get; set; }

        /// <summary>
        /// Initial call?
        /// </summary>
        public bool InitialCall { get; set; }

        /// <summary>
        /// Scroll direction.
        /// </summary>
        public bool ScrollDown { get; set; }

        /// <summary>
        /// Success.
        /// </summary>
        public bool Success { get; set; }

        #endregion
    }
}
