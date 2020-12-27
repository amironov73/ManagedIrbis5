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

/* EVENTMSG.cs -- information about hardware message sent to the system message queue
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The EVENTMSG structure contains information about a hardware
    /// message sent to the system message queue. This structure is
    /// used to store message information for the JournalPlaybackProc
    /// callback function.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EVENTMSG
    {
        /// <summary>
        /// Specifies the message.
        /// </summary>
        public static WindowMessage message;

        /// <summary>
        /// Specifies additional information about the message.
        /// The exact meaning depends on the message value.
        /// </summary>
        public static int paramL;

        /// <summary>
        /// Specifies additional information about the message.
        /// The exact meaning depends on the message value.
        /// </summary>
        public static int paramH;

        /// <summary>
        /// Specifies the time at which the message was posted.
        /// </summary>
        public static int time;

        /// <summary>
        /// Handle to the window to which the message was posted.
        /// </summary>
        public static IntPtr hwnd;

    } // struct EVENTMSG

} // namespace AM.Win32
