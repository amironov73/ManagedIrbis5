// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* IMAGEINFO.cs -- информация об изображении
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
    /// Информация об изображения (из списка изображений).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGEINFO
    {
        /// <summary>
        /// A handle to the bitmap that contains the images.
        /// </summary>
        public IntPtr hbmImage;

        /// <summary>
        /// A handle to a monochrome bitmap that contains the masks
        /// for the images. If the image list does not contain
        /// a mask, this member is NULL.
        /// </summary>
        public IntPtr hbmMask;

        /// <summary>
        /// Not used. This member should always be zero.
        /// </summary>
        public int Unused1;

        /// <summary>
        /// Not used. This member should always be zero.
        /// </summary>
        public int Unused2;

        /// <summary>
        /// The bounding rectangle of the specified image within the bitmap specified by hbmImage.
        /// </summary>
        public int rcImage_left;

        /// <summary>
        /// The bounding rectangle of the specified image within the bitmap specified by hbmImage.
        /// </summary>
        public int rcImage_top;

        /// <summary>
        /// The bounding rectangle of the specified image within the bitmap specified by hbmImage.
        /// </summary>
        public int rcImage_right;

        /// <summary>
        /// The bounding rectangle of the specified image within the bitmap specified by hbmImage.
        /// </summary>
        public int rcImage_bottom;

    } // struct IMAGEINFO

} // namespace AM.Win32
