// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* COORD.cs -- координаты ячейки с символом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Структура COORD определяет координаты символьной ячейки
/// в экранном буфере консоли. Начало системы координат
/// (0,0) находится в верхней левой ячейке буфера.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Explicit, Size = 4)]
public struct COORD
{
    #region Properties

    /// <summary>
    /// Горизонтальная координата (номер столбца, нумерация с 0).
    /// </summary>
    [FieldOffset (0)]
    public short X;

    /// <summary>
    /// Вертикальная координата (номер строки, нумерация с 0).
    /// </summary>
    [FieldOffset (2)]
    public short Y;

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return $"{X}, {Y}";
    }

    #endregion
}
