// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* POINTL.cs -- координаты точки на растровом устройстве вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Координаты точки на растровом устройстве вывода.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct POINTL
{
    #region Properties

    /// <summary>
    /// Горизонтальная (x) координата точки.
    /// </summary>
    public int X;

    /// <summary>
    /// Вертикальная (y) координата точки.
    /// </summary>
    public int Y;

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return $"{X}, {Y}";
    }

    #endregion
}
