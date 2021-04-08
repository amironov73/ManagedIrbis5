// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianToolTipEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianToolTipEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Grid.
        /// </summary>
        public SiberianGrid? Grid { get; internal set; }

        /// <summary>
        /// ToolTip text.
        /// </summary>
        public string? ToolTipText { get; set; }

        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X;

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y;

        #endregion

    }
}
