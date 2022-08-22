// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* SMALL_RECT.cs -- координаты верхнего левого и нижнего правого углов прямоугольника
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Координаты верхнего левого и нижнего правого углов прямоугольника.
/// </summary>
/// <remarks>
/// Эта структура используется консольными функциями для указания
/// прямоугольных областей экранных буферов консоли, где координаты
/// определяют строки и столбцы символьных ячеек экранного буфера.
/// </remarks>
[Serializable]
[StructLayout (LayoutKind.Sequential, Pack = 2, Size = 8)]
public struct SMALL_RECT
{
    /// <summary>
    /// Координата X верхнего левого угла прямоугольника.
    /// </summary>
    public short Left;

    /// <summary>
    /// Координата Y верхнего левого угла прямоугольника.
    /// </summary>
    public short Top;

    /// <summary>
    /// Координата X нижнего правого угла прямоугольника.
    /// </summary>
    public short Right;

    /// <summary>
    /// Координата Y нижнего правого угла прямоугольника.
    /// </summary>
    public short Bottom;
}
