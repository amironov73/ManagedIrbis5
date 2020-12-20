// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* SIZEL.cs -- ширина и высота прямоугольника
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
    /// Высота и ширина прямоугольника.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZEL
    {
        /// <summary>
        /// Specifies the rectangle's width. The units depend on which function uses this.
        /// </summary>
        public int cx;

        /// <summary>
        /// Specifies the rectangle's height. The units depend on which function uses this.
        /// </summary>
        public int cy;

    } // struct SIZEL

} // namespace AM.Win32
