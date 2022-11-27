// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* MapMode.cs -- указывает режим сопоставления GDI
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Указывает режим сопоставления GDI.
/// </summary>
public enum MapMode
{
    /// <summary>
    /// Для FXCop.
    /// </summary>
    None = 0,

    /// <summary>
    /// Каждая логическая единица сопоставляется с одним пикселем устройства.
    /// Положительный x справа; положительный y вниз.
    /// </summary>
    MM_TEXT = 1,

    /// <summary>
    /// Каждая логическая единица отображается на 0,1 миллиметра.
    /// Положительный x справа; положительный y вверх.
    /// </summary>
    MM_LOMETRIC = 2,

    /// <summary>
    /// Каждая логическая единица сопоставляется с 0,01 миллиметра.
    /// Положительный x справа; положительный y вверх.
    /// </summary>
    MM_HIMETRIC = 3,

    /// <summary>
    /// Каждая логическая единица соответствует 0,01 дюйма.
    /// Положительный x справа; положительный y вверх.
    /// </summary>
    MM_LOENGLISH = 4,

    /// <summary>
    /// Каждая логическая единица соответствует 0,001 дюйма.
    /// Положительный x справа; положительный y вверх.
    /// </summary>
    MM_HIENGLISH = 5,

    /// <summary>
    /// Каждая логическая единица сопоставляется с одной двадцатой
    /// точки принтера (11440 дюймов, также называемой твипом).
    /// Положительный x справа; положительный y вверх.
    /// </summary>
    MM_TWIPS = 6,

    /// <summary>
    /// Логические единицы сопоставляются с произвольными единицами
    /// с одинаково масштабируемыми осями; то есть одна единица
    /// по оси x равна одной единице по оси y. Используйте функции
    /// SetWindowExtEx и SetViewportExtEx, чтобы указать единицы
    /// измерения и ориентацию осей. Интерфейс графического
    /// устройства (GDI) вносит коррективы, необходимые для обеспечения
    /// того, чтобы единицы измерения x и y оставались одного размера
    /// (когда установлен размер окна, область просмотра будет настроена
    /// так, чтобы единицы измерения оставались изотропными).
    /// </summary>
    MM_ISOTROPIC = 7,

    /// <summary>
    /// Логические единицы сопоставляются с произвольными единицами
    /// с произвольно масштабируемыми осями. Используйте функции
    /// SetWindowExtEx и SetViewportExtEx, чтобы задать единицы
    /// измерения, ориентацию и масштабирование.
    /// </summary>
    MM_ANISOTROPIC = 8
}
