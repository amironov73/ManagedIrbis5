// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ALTTABINFO.cs -- информация о статусе переключения приложений с помощью ALT+TAB
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Информация о статусе переключения приложений с помощью ALT+TAB.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct ALTTABINFO
{
    /// <summary>
    /// Размер структуры в байтах. Это значение должно быть передано
    /// в <see cref="Size" />.
    /// </summary>
    public const int SIZE = 40;

    /// <summary>
    /// Размер структуры в байтах. Вызывающая сторона должна заполнить
    /// это поле перед вызовом API.
    /// </summary>
    public int Size;

    /// <summary>
    /// Количество элементов в окне выбора приложения
    /// для переключения по ALT+TAB.
    /// </summary>
    public int Items;

    /// <summary>
    /// Количество столбцов в окне выбора приложения
    /// для переключения по ALT+TAB.
    /// </summary>
    public int Columns;

    /// <summary>
    /// Количество строк в окне выбора приложения
    /// для переключения по ALT+TAB.
    /// </summary>
    public int Rows;

    /// <summary>
    /// Номер активного столбца в окне выбора приложения
    /// (нумерация с 0).
    /// </summary>
    public int ColumnFocus;

    /// <summary>
    /// Номер активной строки в окне выбора приложения
    /// (нумерация с 0).
    /// </summary>
    public int RowFocus;

    /// <summary>
    /// Ширина иконки в окне выбора приложения.
    /// </summary>
    public int XItem;

    /// <summary>
    /// Высота иконки в окне выбора приложения.
    /// </summary>
    public int YItem;

    /// <summary>
    /// Координаы левого верхнего угла первой иконки
    /// в окне выбора приложения.
    /// </summary>
    public Point Start;
}
