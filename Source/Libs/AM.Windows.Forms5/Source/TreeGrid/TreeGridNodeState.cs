// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridNodeState.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum TreeGridNodeState
    {
        /// <summary>
        ///
        /// </summary>
        Normal = 0,

        /// <summary>
        ///
        /// </summary>
        Selected = 1,

        /// <summary>
        ///
        /// </summary>
        Disabled = 2,

        /// <summary>
        ///
        /// </summary>
        Checked = 4,

        /// <summary>
        ///
        /// </summary>
        ReadOnly = 8
    }
}
