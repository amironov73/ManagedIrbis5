﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BatteryFlags.cs -- battery charge status.
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Battery charge status.
    /// </summary>
    [Flags]
    public enum BatteryFlags
        : byte
    {
        /// <summary>
        /// High.
        /// </summary>
        High = 1,

        /// <summary>
        /// Low.
        /// </summary>
        Low = 2,

        /// <summary>
        /// Critical.
        /// </summary>
        Critical = 4,

        /// <summary>
        /// Charging.
        /// </summary>
        Charging = 8,

        /// <summary>
        /// No system battery.
        /// </summary>
        NoSystemBattery = 128,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 255

    } // enum BatteryFlags

} // namespace AM.Win32
