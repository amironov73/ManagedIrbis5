// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* IMAGEINFO.cs -- информация об изображении
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Информация об изображения (из списка изображений).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct IMAGEINFO
{
    /// <summary>
    /// Дескриптор растрового изображения, содержащего изображения.
    /// </summary>
    public IntPtr hbmImage;

    /// <summary>
    /// Дескриптор монохромного растрового изображения, содержащего
    /// маски для изображений. Если список изображений не содержит
    /// маски, этот член имеет значение NULL.
    /// </summary>
    public IntPtr hbmMask;

    /// <summary>
    /// Не используется. Этот член всегда должен быть равен нулю.
    /// </summary>
    public int Unused1;

    /// <summary>
    /// Не используется. Этот член всегда должен быть равен нулю.
    /// </summary>
    public int Unused2;

    /// <summary>
    /// Ограничивающий прямоугольник указанного изображения в растровом
    /// изображении, заданном параметром hbmImage.
    /// </summary>
    public int rcImage_left;

    /// <summary>
    /// Ограничивающий прямоугольник указанного изображения в растровом
    /// изображении, заданном параметром hbmImage.
    /// </summary>
    public int rcImage_top;

    /// <summary>
    /// Ограничивающий прямоугольник указанного изображения в растровом
    /// изображении, заданном параметром hbmImage.
    /// </summary>
    public int rcImage_right;

    /// <summary>
    /// Ограничивающий прямоугольник указанного изображения в растровом
    /// изображении, заданном параметром hbmImage.
    /// </summary>
    public int rcImage_bottom;
}
