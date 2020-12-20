// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* POINTL.cs -- координаты точки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32
{
    /// <summary>
    /// Координаты точки.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        /// <summary>
        /// The horizontal (x) coordinate of the point.
        /// </summary>
        public int x;

        /// <summary>
        /// The vertical (y) coordinate of the point.
        /// </summary>
        public int y;

    } // struct POINTL

} // namespace AM.Win32
