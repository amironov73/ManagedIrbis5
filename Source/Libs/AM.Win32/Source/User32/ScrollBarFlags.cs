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

/* ScrollBarFlags.cs -- Flags for GetScrollPos and SetScrollPos functions
   Ars Magna project, http://arsmagna.ru */

#region Using directives

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Flags for GetScrollPos and SetScrollPos functions.
    /// </summary>
    public enum ScrollBarFlags
    {
        /// <summary>
        /// Retrieves the position of the scroll box in a window's
        /// standard horizontal scroll bar.
        /// </summary>
        SB_HORZ = 0,

        /// <summary>
        /// Retrieves the position of the scroll box in a window's
        /// standard vertical scroll bar.
        /// </summary>
        SB_VERT = 1,

        /// <summary>
        /// Retrieves the position of the scroll box in a scroll bar control.
        /// The hWnd parameter must be the handle to the scroll bar control.
        /// </summary>
        SB_CTL = 2,

        /// <summary>
        /// ???
        /// </summary>
        SB_BOTH = 3

    } // enum ScrollBarFlags

} // namespace AM.Win32
