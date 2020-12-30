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

/* WindowLongFlags.cs -- zero-based offset to the value retrieved by GetWindowLong function
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// Specifies the zero-based offset to the value to be retrieved by
    /// GetWindowLong function or set by SetWindowLong function.
    /// </summary>
    public enum WindowLongFlags
    {
        /// <summary>
        /// Retrieves the address of the window procedure, or a handle
        /// representing the address of the window procedure.
        /// </summary>
        GWL_WNDPROC = -4,

        /// <summary>
        /// Retrieves a handle to the application instance.
        /// </summary>
        GWL_HINSTANCE = -6,

        /// <summary>
        /// Retrieves a handle to the parent window, if any.
        /// </summary>
        GWL_HWNDPARENT = -8,

        /// <summary>
        /// Retrieves the window styles.
        /// </summary>
        GWL_STYLE = -16,

        /// <summary>
        /// Retrieves the extended window styles.
        /// </summary>
        GWL_EXSTYLE = -20,

        /// <summary>
        /// Retrieves the user data associated with the window.
        /// This data is intended for use by the application that
        /// created the window. Its value is initially zero.
        /// </summary>
        GWL_USERDATA = -21,

        /// <summary>
        /// Retrieves the identifier of the window.
        /// </summary>
        GWL_ID = -12,

        /// <summary>
        /// Retrieves the return value of a message processed in the
        /// dialog box procedure.
        /// </summary>
        DWL_MSGRESULT = 0,

        /// <summary>
        /// Retrieves the address of the dialog box procedure, or a
        /// handle representing the address of the dialog box procedure.
        /// You must use the CallWindowProc function to call the dialog box
        /// procedure.
        /// </summary>
        DWL_DLGPROC = 4,

        /// <summary>
        /// Retrieves extra information private to the application, such
        /// as handles or pointers.
        /// </summary>
        DWL_USER = 8

    } // enum WindowLongFlags

} // namespace AM.Win32
