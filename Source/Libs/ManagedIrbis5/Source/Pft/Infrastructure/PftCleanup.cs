﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCleanup.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// PFT cleanup flags.
    /// </summary>
    [Flags]
    public enum PftCleanup
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Double text removal.
        /// </summary>
        DoubleText = 1,

        /// <summary>
        /// RTF markup removal.
        /// </summary>
        Rtf = 2,

        /// <summary>
        /// HTML markup removal.
        /// </summary>
        Html = 4,

        /// <summary>
        /// ibatrak команды контекстного выделения
        /// </summary>
        ContextMarkup = 8
    }
}
