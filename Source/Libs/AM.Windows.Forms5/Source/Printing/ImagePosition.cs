// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ImagePosition.cs -- позицич для распечатывания картинки
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms.Printing;

/// <summary>
/// Позиция для распечатывания картинки..
/// </summary>
public enum ImagePosition
{
    /// <summary>
    /// В центре страницы..
    /// </summary>
    PageCenter,

    /// <summary>
    /// Левый верхний угол.
    /// </summary>
    TopLeftCorner
}
