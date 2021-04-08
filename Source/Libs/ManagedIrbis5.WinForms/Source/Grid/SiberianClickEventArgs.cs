// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianClickEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public sealed class SiberianClickEventArgs
        : CancelableEventArgs
    {
        #region Properties

        /// <summary>
        /// Cell.
        /// </summary>
        public SiberianCell? Cell { get; internal set; }

        /// <summary>
        /// Column.
        /// </summary>
        public SiberianColumn? Column { get; internal set; }

        /// <summary>
        /// Grid.
        /// </summary>
        public SiberianGrid? Grid { get; internal set; }

        /// <summary>
        /// Row.
        /// </summary>
        public SiberianRow? Row { get; internal set; }

        #endregion
    }
}
