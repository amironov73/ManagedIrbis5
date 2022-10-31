// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ImageExtractor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace AeroSuite;

/// <summary>
/// Provides a method for extracting images from the system via LoadImage
/// </summary>
public static class IconExtractor
{
    /// <summary>
    /// Loads the icon.
    /// </summary>
    /// <param name="type">The type of icon.</param>
    /// <param name="size">The size.</param>
    /// <returns>The icon.</returns>
    /// <exception cref="System.PlatformNotSupportedException"></exception>
    public static Icon? LoadIcon (IconType type, Size size)
    {
        var hIcon = NativeMethods.LoadImage
            (
                IntPtr.Zero,
                "#" + (int)type,
                1,
                size.Width,
                size.Height,
                0
            );
        return hIcon == IntPtr.Zero ? null : Icon.FromHandle (hIcon);
    }

    /// <summary>
    ///
    /// </summary>
    public enum IconType
    {
        /// <summary>
        ///
        /// </summary>
        Warning = 101,

        /// <summary>
        ///
        /// </summary>
        Help = 102,

        /// <summary>
        ///
        /// </summary>
        Error = 103,

        /// <summary>
        ///
        /// </summary>
        Info = 104,

        /// <summary>
        ///
        /// </summary>
        Shield = 106
    }
}
