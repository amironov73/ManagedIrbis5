// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* BITMAPINFO.cs -- определяет размеры и информацию о цвете для DIB
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Структура BITMAINFO определяет размеры и информацию о цвете для DIB.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential, Pack = 1)]
public struct BITMAPINFO
{
    /// <summary>
    /// Задает структуру <see cref="BITMAPINFOHEADER" />,
    /// которая содержит информацию о размерах цветового формата.
    /// </summary>
    public BITMAPINFOHEADER bmiHeader;

    // Fixed-length Array "bmiColors[1]". Members can be
    // accessed with (&my_BITMAPINFO.bmiColors_1)[index]
    /// <summary>
    /// <para>Член bmiColors содержит одно из следующего:</para>
    ///
    /// <para> * Массив RGBQUAD. Элементы массива, составляющие
    /// таблицу цветов.</para>
    ///
    /// <para> * Массив 16-битных целых чисел без знака,
    /// указывающий индексы в текущей реализованной логической палитре.
    /// Такое использование bmiColors разрешено для функций,
    /// использующих DIB. Когда элементы bmiColors содержат
    /// индексы реализованной логической палитры, они также должны
    /// вызывать следующие функции растрового изображения:
    /// CreateDIBitmap, CreateDIBPatternBrush, CreateDIBSection</para>
    ///
    /// <para>Параметр iUsage CreateDIBSection должен иметь значение
    /// DIB_PAL_COLORS.</para>
    ///
    /// <para>Количество записей в массиве зависит от значений элементов
    /// biBitCount и biClrUsed структуры BITMAPINFOHEADER.</para>
    /// </summary>
    public RGBQUAD bmiColors_1;
}
