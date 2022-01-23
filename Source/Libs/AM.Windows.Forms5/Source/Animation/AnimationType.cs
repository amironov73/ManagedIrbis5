// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AnimationType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Windows.Forms.Animation;

/// <summary>
///
/// </summary>
public enum AnimationType
{
    /// <summary>
    /// Пользовательская анимация.
    /// </summary>
    Custom = 0,

    /// <summary>
    /// Вращение.
    /// </summary>
    Rotate,

    /// <summary>
    /// Горизонтальный сдвиг.
    /// </summary>
    HorizontalSlide,

    /// <summary>
    /// Вертикальный сдвиг.
    /// </summary>
    VerticalSlide,

    /// <summary>
    /// Масштабирование.
    /// </summary>
    Scale,

    /// <summary>
    /// Масштабирование и вращение.
    /// </summary>
    ScaleAndRotate,

    /// <summary>
    /// Горизонтальный сдвиг и вращение.
    /// </summary>
    HorizontalSlideAndRotate,

    /// <summary>
    /// Масштабирование и горизонтальный сдвиг.
    /// </summary>
    ScaleAndHorizontalSlide,

    /// <summary>
    /// Прозрачность.
    /// </summary>
    Transparent,

    /// <summary>
    /// Створки.
    /// </summary>
    Leaf,

    /// <summary>
    /// Мозаика.
    /// </summary>
    Mosaic,

    /// <summary>
    /// Частицы.
    /// </summary>
    Particles,

    /// <summary>
    /// Вертикальные жалюзи.
    /// </summary>
    VerticalBlind,

    /// <summary>
    /// Горизонтальные жалюзи.
    /// </summary>
    HorizontalBlind
}
