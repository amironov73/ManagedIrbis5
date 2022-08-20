// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* BITMAP.cs -- определяет тип, ширину, высоту, цветовой формат и битовые значения растрового изображения
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
///Структура BITMAP определяет тип, ширину, высоту, цветовой формат
/// и битовые значения растрового изображения.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential, Pack = 1)]
public struct BITMAP
{
    /// <summary>
    /// Указывает тип растрового изображения.
    /// Этот элемент должен быть равен нулю.
    /// </summary>
    public int bmType;

    /// <summary>
    /// Задает ширину растрового изображения в пикселях.
    /// Ширина должна быть больше нуля.
    /// </summary>
    public int bmWidth;

    /// <summary>
    /// Задает высоту растрового изображения в пикселях.
    /// Высота должна быть больше нуля.
    /// </summary>
    public int bmHeight;

    /// <summary>
    /// Определяет количество байтов в каждой строке сканирования.
    /// Это значение должно делиться на 2, поскольку система
    /// предполагает, что битовые значения растрового изображения
    /// образуют массив, выровненный по словам.
    /// </summary>
    public int bmWidthBytes;

    /// <summary>
    /// Указывает количество цветовых плоскостей.
    /// </summary>
    public ushort bmPlanes;

    /// <summary>
    /// Указывает количество битов, необходимых для указания
    /// цвета пикселя.
    /// </summary>
    public ushort bmBitsPixel;

    /// <summary>
    /// Указатель на расположение битовых значений растрового изображения.
    /// Элемент bmBits должен быть длинным указателем на массив
    /// символов (однобайтовых значений).
    /// </summary>
    public IntPtr bmBits;
}
