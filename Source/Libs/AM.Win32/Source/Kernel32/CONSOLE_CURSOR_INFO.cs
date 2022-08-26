// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* CONSOLE_CURSOR_INFO.cs -- информация о консольном курсоре
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Структура содержит информацию о консольном курсоре.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Explicit, Size = 8)]
public struct CONSOLE_CURSOR_INFO
{
    /// <summary>
    /// Процент символьной ячейки, заполненной курсором. Это значение
    /// должно находиться в диапазоне от 1 до 100. Внешний вид курсора
    /// может быть разным: от полного заполнения ячейки до отображения
    /// в виде горизонтальной линии в нижней части ячейки.
    /// </summary>
    [FieldOffset (0)]
    public int dwSize;

    /// <summary>
    /// Видимость курсора. Если курсор виден, этот член имеет
    /// значение TRUE.
    /// </summary>
    [FieldOffset (4)]
    public bool bVisible;
}
