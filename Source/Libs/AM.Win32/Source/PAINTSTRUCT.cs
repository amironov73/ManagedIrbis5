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

/* PAINTSTRUCT.cs -- used to paint client area of a window
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32
{
    /// <summary>
    /// The PAINTSTRUCT structure contains information for an application.
    /// This information can be used to paint the client area of a window
    /// owned by that application.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct PAINTSTRUCT
    {
        /// <summary>
        /// Handle to the display DC to be used for painting.
        /// </summary>
        [FieldOffset(0)]
        public IntPtr hdc;

        /// <summary>
        /// Specifies whether the background must be erased. This value is
        /// nonzero if the application should erase the background. The
        /// application is responsible for erasing the background if a window
        /// class is created without a background brush.
        /// </summary>
        [FieldOffset(4)]
        public int fErase;

        /// <summary>
        /// Specifies a RECT structure that specifies the upper left and lower
        /// right corners of the rectangle in which the painting is requested,
        /// in device units relative to the upper-left corner of the client area.
        /// </summary>
        [FieldOffset(8)]
        public Rectangle rcPaint;

        /// <summary>
        /// Reserved; used internally by the system.
        /// </summary>
        [FieldOffset(24)]
        public int fRestore;

        /// <summary>
        /// Reserved; used internally by the system.
        /// </summary>
        [FieldOffset(28)]
        public int fIncUpdate;

        /// <summary>
        /// Reserved; used internally by the system.
        /// </summary>
        [FieldOffset(32)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;

    } // struct PAINTSTRUCT

} // namespace AM.Win32
