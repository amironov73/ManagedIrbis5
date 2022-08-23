// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* RECTL.cs -- координаты левого верхнего и нижнего правого углов прямоугольника
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Координаты левого верхнего и правого нижнего углов прямоугольника.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct RECTL
{
    #region Properties

    /// <summary>
    /// X - координата верхнего левого угла прямоугольника.
    /// </summary>
    public int Left;

    /// <summary>
    /// Y - координата верхнего левого угла прямоугольника.
    /// </summary>
    public int Top;

    /// <summary>
    /// X - координата нижнего правого угла прямоугольника.
    /// </summary>
    public int Right;

    /// <summary>
    /// Y - координата нижнего правого угла прямоугольника.
    /// </summary>
    public int Bottom;

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return $"({Left}, {Top}) - ({Right}, {Bottom})";
    }

    #endregion
}
