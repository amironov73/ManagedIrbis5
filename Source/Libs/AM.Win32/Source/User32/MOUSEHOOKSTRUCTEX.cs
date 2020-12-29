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

/* MOUSEHOOKSTRUCTEX.cs -- mouse event passed to a WH_MOUSE hook procedure
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Contains information about a mouse event passed
    /// to a WH_MOUSE hook procedure, MouseProc.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEHOOKSTRUCTEX
    {
        /// <summary>
        /// Specifies a POINT structure that contains
        /// the x- and y-coordinates of the cursor,
        /// in screen coordinates.
        /// </summary>
        public Point pt;

        /// <summary>
        /// Handle to the window that will receive the
        /// mouse message corresponding to the mouse event.
        /// </summary>
        public IntPtr hwnd;

        /// <summary>
        /// Specifies the hit-test value. For a list of hit-test
        /// values, see the description of the WM_NCHITTEST message.
        /// </summary>
        public HitTestCode wHitTestCode;

        /// <summary>
        /// Specifies extra information associated with the message.
        /// </summary>
        public IntPtr dwExtraInfo;

        /// <summary>
        /// Data.
        /// </summary>
        public int mouseData;

    } // struct MOUSEHOOKSTRUCTEX

} // namespace AM.Win32
