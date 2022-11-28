// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable CA1069 // некоторые элементы перечисления имеют повторяющиеся значения

/* TextAlign.cs -- выравнивание текста по маске значений
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Выравнивание текста с использованием маски значений.
/// Можно выбрать только один флаг из тех, которые влияют
/// на горизонтальное и вертикальное выравнивание. Кроме того,
/// можно выбрать только один из двух флагов, изменяющих текущую позицию.
/// </summary>
[Flags]
public enum TextAlign
{
    /// <summary>
    /// Текущая позиция не обновляется после каждого вызова текстового
    /// вывода. Контрольная точка передается в функцию вывода текста.
    /// </summary>
    TA_NOUPDATECP = 0,

    /// <summary>
    /// Текущая позиция обновляется после каждого вызова текстового вывода.
    /// Текущее положение используется в качестве точки отсчета.
    /// </summary>
    TA_UPDATECP = 1,

    /// <summary>
    /// Контрольная точка будет на левом краю ограничивающего прямоугольника.
    /// </summary>
    TA_LEFT = 0,

    /// <summary>
    /// Контрольная точка будет находиться на правом краю
    /// ограничивающего прямоугольника.
    /// </summary>
    TA_RIGHT = 2,

    /// <summary>
    /// Контрольная точка будет выровнена по горизонтали с центром
    /// ограничивающего прямоугольника.
    /// </summary>
    TA_CENTER = 6,

    /// <summary>
    /// Контрольная точка будет находиться на верхнем краю
    /// ограничивающего прямоугольника.
    /// </summary>
    TA_TOP = 0,

    /// <summary>
    /// Контрольная точка будет находиться на нижнем краю
    /// ограничивающего прямоугольника.
    /// </summary>
    TA_BOTTOM = 8,

    /// <summary>
    /// Контрольная точка будет находиться на базовой линии текста.
    /// </summary>
    TA_BASELINE = 24,

    /// <summary>
    /// Редакция Windows на ближневосточном языке: текст располагается
    /// справа налево в порядке чтения, в отличие от порядка чтения слева
    /// направо по умолчанию. Это применимо только в том случае,
    /// если шрифт, выбранный в контексте устройства, является либо ивритом,
    /// либо арабским.
    /// </summary>
    TA_RTLREADING = 256,

    /// <summary>
    /// ???
    /// </summary>
    TA_MASK = TA_BASELINE + TA_CENTER + TA_UPDATECP + TA_RTLREADING,

    /// <summary>
    /// Контрольная точка будет находиться на базовой линии текста.
    /// </summary>
    VTA_BASELINE = TA_BASELINE,

    /// <summary>
    /// ???
    /// </summary>
    VTA_LEFT = TA_BOTTOM,

    /// <summary>
    /// ???
    /// </summary>
    VTA_RIGHT = TA_TOP,

    /// <summary>
    /// ???
    /// </summary>
    VTA_CENTER = TA_CENTER,

    /// <summary>
    /// ???
    /// </summary>
    VTA_BOTTOM = TA_RIGHT,

    /// <summary>
    /// ???
    /// </summary>
    VTA_TOP = TA_LEFT
}
