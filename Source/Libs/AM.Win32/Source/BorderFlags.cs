// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BorderFlags.cs -- указание, как рисовать рамку окна
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Указание, как рисовать рамку окна.
/// </summary>
[Flags]
public enum BorderFlags
{
    /// <summary>
    /// Левая сторона граничного прямоугольника..
    /// </summary>
    BF_LEFT = 0x0001,

    /// <summary>
    /// Верхняя сторона граничного прямоугольника.
    /// </summary>
    BF_TOP = 0x0002,

    /// <summary>
    /// Правая сторона граничного прямоугольника.
    /// </summary>
    BF_RIGHT = 0x0004,

    /// <summary>
    /// Нижняя сторона граничного прямоугольника.
    /// </summary>
    BF_BOTTOM = 0x0008,

    /// <summary>
    /// Верхняя и левая стороны граничного прямоугольника.
    /// </summary>
    BF_TOPLEFT = BF_TOP | BF_LEFT,

    /// <summary>
    /// Верхняя и правая стороны граничного прямоугольника.
    /// </summary>
    BF_TOPRIGHT = BF_TOP | BF_RIGHT,

    /// <summary>
    /// Нижняя и левая стороны граничного прямоугольника.
    /// </summary>
    BF_BOTTOMLEFT = BF_BOTTOM | BF_LEFT,

    /// <summary>
    /// Нижняя и правая стороны граничного прямоугольника.
    /// </summary>
    BF_BOTTOMRIGHT = BF_BOTTOM | BF_RIGHT,

    /// <summary>
    /// Все стороны граничного прямоугольника.
    /// </summary>
    BF_RECT = BF_LEFT | BF_TOP | BF_RIGHT | BF_BOTTOM,

    /// <summary>
    /// Диагональная граница.
    /// </summary>
    BF_DIAGONAL = 0x0010,

    /// <summary>
    /// Диагональная граница. Конечная точка — правый верхний угол
    /// прямоугольника; исходной точкой является нижний левый угол.
    /// </summary>
    BF_DIAGONAL_ENDTOPRIGHT = BF_DIAGONAL | BF_TOP | BF_RIGHT,

    /// <summary>
    /// Диагональная граница. Конечная точка — левый верхний угол
    /// прямоугольника; исходной точкой является нижний правый угол.
    /// </summary>
    BF_DIAGONAL_ENDTOPLEFT = BF_DIAGONAL | BF_TOP | BF_LEFT,

    /// <summary>
    /// Диагональная граница. Конечная точка — левый нижний угол
    /// прямоугольника; исходной точкой является верхний правый угол.
    /// </summary>
    BF_DIAGONAL_ENDBOTTOMLEFT = BF_DIAGONAL | BF_BOTTOM | BF_LEFT,

    /// <summary>
    /// Диагональная граница. Конечная точка — правый нижний угол
    /// прямоугольника; исходной точкой является верхний левый угол.
    /// </summary>
    BF_DIAGONAL_ENDBOTTOMRIGHT = BF_DIAGONAL | BF_BOTTOM | BF_RIGHT,

    /// <summary>
    /// Внутреннее пространство прямоугольника должно быть заполнено.
    /// </summary>
    BF_MIDDLE = 0x0800,

    /// <summary>
    /// Софтовые кнопки вместо черепицы.
    /// </summary>
    BF_SOFT = 0x1000,

    /// <summary>
    /// Прямоугольник, на который указывает параметр pDestRect,
    /// сжимается, чтобы исключить нарисованные края; в противном
    /// случае прямоугольник не меняется.
    /// </summary>
    BF_ADJUST = 0x2000,

    /// <summary>
    /// Плоская граница.
    /// </summary>
    BF_FLAT = 0x4000,

    /// <summary>
    /// Одномерная граница.
    /// </summary>
    BF_MONO = 0x8000
}
