// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianOptions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Options for <see cref="SiberianColumn"/>.
    /// </summary>
    [Flags]
    public enum SiberianOptions
    {
        /// <summary>
        /// Editable.
        /// </summary>
        Editable = 0x0001,

        /// <summary>
        /// Selectable.
        /// </summary>
        Selectable = 0x0002,

        /// <summary>
        /// Resizeable.
        /// </summary>
        Resizeable = 0x0004,

        /// <summary>
        /// Can grow horizontally.
        /// </summary>
        CanGrowHorizontally = 0x0008,

        /// <summary>
        /// Can shrink horizontally.
        /// </summary>
        CanShrinkHorizontally = 0x0010,

        /// <summary>
        /// Can grow vertically.
        /// </summary>
        CanGrowVertically = 0x0020,

        /// <summary>
        /// Can shrink vertically.
        /// </summary>
        CanShrinkVertically = 0x0040
    }
}
