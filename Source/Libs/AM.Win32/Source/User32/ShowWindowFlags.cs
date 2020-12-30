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

/* ShowWindowFlags.cs -- specifies how the window is to be shown
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// Specifies how the window is to be shown.
    /// </summary>
    public enum ShowWindowFlags
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        SW_HIDE = 0,

        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        HIDE_WINDOW = 0,

        /// <summary>
        /// Activates and displays a window. If the window is
        /// minimized or maximized, the system restores it to its
        /// original size and position. An application should specify
        /// this flag when displaying the window for the first time.
        /// </summary>
        SW_SHOWNORMAL = 1,

        /// <summary>
        /// Activates and displays a window. If the window is
        /// minimized or maximized, the system restores it to its
        /// original size and position. An application should specify
        /// this flag when displaying the window for the first time.
        /// </summary>
        SW_NORMAL = 1,

        /// <summary>
        /// Activates and displays a window. If the window is
        /// minimized or maximized, the system restores it to its
        /// original size and position. An application should specify
        /// this flag when displaying the window for the first time.
        /// </summary>
        SHOW_OPENWINDOW = 1,

        /// <summary>
        /// ???
        /// </summary>
        SW_SHOWMINIMIZED = 2,

        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        SW_SHOWMAXIMIZED = 3,

        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        SW_MAXIMIZE = 3,

        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        SHOW_FULLSCREEN = 3,

        /// <summary>
        /// Displays a window in its most recent size and position.
        /// This value is similar to SW_SHOWNORMAL, except the window
        /// is not actived.
        /// </summary>
        SW_SHOWNOACTIVATE = 4,

        /// <summary>
        /// Displays a window in its most recent size and position.
        /// This value is similar to SW_SHOWNORMAL, except the window
        /// is not actived.
        /// </summary>
        SHOW_OPENNOACTIVATE = 4,

        /// <summary>
        /// Activates the window and displays it in its current
        /// size and position.
        /// </summary>
        SW_SHOW = 5,

        /// <summary>
        /// Minimizes the specified window and activates the next
        /// top-level window in the Z order.
        /// </summary>
        SW_MINIMIZE = 6,

        /// <summary>
        /// Displays the window as a minimized window. This value
        /// is similar to SW_SHOWMINIMIZED, except the window is
        /// not activated.
        /// </summary>
        SW_SHOWMINNOACTIVE = 7,

        /// <summary>
        /// Displays the window in its current size and position.
        /// This value is similar to SW_SHOW, except the window is
        /// not activated.
        /// </summary>
        SW_SHOWNA = 8,

        /// <summary>
        /// Activates and displays the window. If the window is
        /// minimized or maximized, the system restores it to its
        /// original size and position. An application should specify
        /// this flag when restoring a minimized window.
        /// </summary>
        SW_RESTORE = 9,

        /// <summary>
        /// Sets the show state based on the SW_ value specified
        /// in the STARTUPINFO structure passed to the CreateProcess
        /// function by the program that started the application.
        /// </summary>
        SW_SHOWDEFAULT = 10,

        /// <summary>
        /// Windows 2000/XP: Minimizes a window, even if the
        /// thread that owns the window is hung. This flag should
        /// only be used when minimizing windows from a different
        /// thread.
        /// </summary>
        SW_FORCEMINIMIZE = 11,

        /// <summary>
        /// ???
        /// </summary>
        SW_MAX = 11

    } // enum ShowWindowFlags

} // namespace AM.Win32
