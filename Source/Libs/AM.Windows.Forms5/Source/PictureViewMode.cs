// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* PictureViewMode.cs -- режим просмотра картинки
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms;

/// <summary>
/// Режим просмотра картинки для формы <see cref="PictureViewForm"/>.
/// </summary>
public enum PictureViewMode
{
    /// <summary>
    /// Режим выбирается автоматически.
    /// </summary>
    Auto,

    /// <summary>
    /// Подгонка масштаба, чтобы картинка поместилась на экране полностью.
    /// </summary>
    Fit,

    /// <summary>
    /// Режим прокрутки.
    /// </summary>
    Scroll
}
