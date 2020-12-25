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

/* VARIANT.cs -- Win32 variant
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Win32 variant.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VARIANT
    {
        /// <summary>
        ///
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public short vt;

        /// <summary>
        ///
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public short reserved1;

        /// <summary>
        ///
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public short reserved2;

        /// <summary>
        ///
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public short reserved3;

        /// <summary>
        ///
        /// </summary>
        public IntPtr data1;

        /// <summary>
        ///
        /// </summary>
        public IntPtr data2;

    } // struct VARIANT

} // namespace AM.Win32
