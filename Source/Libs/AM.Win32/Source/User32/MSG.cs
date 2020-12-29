// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

/* MSG.cs -- message information from a thread's message queue
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Contains message information from a thread's message queue.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        /// <summary>
        /// Handle to the window whose window procedure receives the message.
        /// </summary>
        public IntPtr hwnd;

        /// <summary>
        /// Specifies the message identifier. Applications can only use the
        /// low word; the high word is reserved by the system.
        /// </summary>
        public int message;

        /// <summary>
        /// Specifies additional information about the message. The exact
        /// meaning depends on the value of the message member.
        /// </summary>
        public int wParam;

        /// <summary>
        /// Specifies additional information about the message. The exact
        /// meaning depends on the value of the message member.
        /// </summary>
        public int lParam;

        /// <summary>
        /// Specifies the time at which the message was posted.
        /// </summary>
        public int time;

        /// <summary>
        /// Specifies the cursor position, in screen coordinates, when the
        /// message was posted.
        /// </summary>
        public Point pt;

    } // struct MSG

} // namespace AM.Win32
