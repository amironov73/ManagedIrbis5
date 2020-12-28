﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FlashWindowFlags.cs -- options for FlashWindowEx method
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Options for FlashWindowEx method.
    /// </summary>
    [Flags]
    public enum FlashWindowFlags
    {
        /// <summary>
        /// Stop flashing. The system restores the window to its
        /// original state.
        /// </summary>
        FLASHW_STOP = 0,

        /// <summary>
        /// Flash the window caption.
        /// </summary>
        FLASHW_CAPTION = 0x00000001,

        /// <summary>
        /// Flash the taskbar button.
        /// </summary>
        FLASHW_TRAY = 0x00000002,

        /// <summary>
        /// Flash both the window caption and taskbar button.
        /// This is equivalent to setting the
        /// FLASHW_CAPTION | FLASHW_TRAY flags.
        /// </summary>
        FLASHW_ALL = FLASHW_CAPTION | FLASHW_TRAY,

        /// <summary>
        /// Flash continuously, until the FLASHW_STOP flag is set.
        /// </summary>
        FLASHW_TIMER = 0x00000004,

        /// <summary>
        /// Flash continuously until the window comes to the foreground.
        /// </summary>
        FLASHW_TIMERNOFG = 0x0000000C

    } // enum FlashWindowFlags

} // namespace AM.Win32
