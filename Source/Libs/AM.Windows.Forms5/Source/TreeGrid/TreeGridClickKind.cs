// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridClickKind.cs -- перечисление: вид клика мышью по элементу грида
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms;

/// <summary>
/// Перечисление: вид клика мышью по элементу грида.
/// </summary>
public enum TreeGridClickKind
{
    /// <summary>
    /// Неизвестно.
    /// </summary>
    Unknown,

    /// <summary>
    /// Клик по тексту.
    /// </summary>
    Text,

    /// <summary>
    /// Разворачивание региона.
    /// </summary>
    Expand,

    /// <summary>
    /// Чекбокс или другой элемент для проставления отметки.
    /// </summary>
    Check,

    /// <summary>
    /// Иконка.
    /// </summary>
    Icon
}
