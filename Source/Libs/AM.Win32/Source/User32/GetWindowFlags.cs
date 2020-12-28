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

/* GetWindowFlags.cs -- relationship between the specified window and the window whose handle is to be retrieved
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// Specifies the relationship between the specified window
    /// and the window whose handle is to be retrieved.
    /// </summary>
    public enum GetWindowFlags
    {
        /// <summary>
        /// The retrieved handle identifies the window of the same
        /// type that is highest in the Z order. If the specified
        /// window is a topmost window, the handle identifies the
        /// topmost window that is highest in the Z order. If the
        /// specified window is a top-level window, the handle
        /// identifies the top-level window that is highest in the
        /// Z order. If the specified window is a child window, the
        /// handle identifies the sibling window that is highest in
        /// the Z order.
        /// </summary>
        GW_HWNDFIRST = 0,

        /// <summary>
        /// The retrieved handle identifies the window of the same
        /// type that is lowest in the Z order. If the specified
        /// window is a topmost window, the handle identifies the
        /// topmost window that is lowest in the Z order. If the
        /// specified window is a top-level window, the handle
        /// identifies the top-level window that is lowest in the
        /// Z order. If the specified window is a child window, the
        /// handle identifies the sibling window that is lowest in
        /// the Z order.
        /// </summary>
        GW_HWNDLAST = 1,

        /// <summary>
        /// The retrieved handle identifies the window below the
        /// specified window in the Z order. If the specified window
        /// is a topmost window, the handle identifies the topmost
        /// window below the specified window. If the specified window
        /// is a top-level window, the handle identifies the top-level
        /// window below the specified window. If the specified window
        /// is a child window, the handle identifies the sibling window
        /// below the specified window.
        /// </summary>
        GW_HWNDNEXT = 2,

        /// <summary>
        /// The retrieved handle identifies the window above the
        /// specified window in the Z order. If the specified window
        /// is a topmost window, the handle identifies the topmost
        /// window above the specified window. If the specified window
        /// is a top-level window, the handle identifies the top-level
        /// window above the specified window. If the specified window
        /// is a child window, the handle identifies the sibling window
        /// above the specified window.
        /// </summary>
        GW_HWNDPREV = 3,

        /// <summary>
        /// The retrieved handle identifies the specified window's
        /// owner window, if any.
        /// </summary>
        GW_OWNER = 4,

        /// <summary>
        /// The retrieved handle identifies the child window at
        /// the top of the Z order, if the specified window is a
        /// parent window; otherwise, the retrieved handle is NULL.
        /// The function examines only child windows of the specified
        /// window. It does not examine descendant windows.
        /// </summary>
        GW_CHILD = 5,

        /// <summary>
        /// Windows 2000/XP: The retrieved handle identifies the
        /// enabled popup window owned by the specified window
        /// (the search uses the first such window found using
        /// GW_HWNDNEXT); otherwise, if there are no enabled popup
        /// windows, the retrieved handle is that of the specified
        /// window.
        /// </summary>
        GW_ENABLEDPOPUP = 6

    } // enum GetWindowFlags

} // namespace AM.Win32
