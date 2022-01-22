// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* InsertEffect.cs -- метод вставки элемента
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms;

/// <summary>
/// Метод вставки элемента.
/// </summary>
public enum InsertEffect
{
    /// <summary>
    /// Нет.
    /// </summary>
    None,

    /// <summary>
    /// Вставка перед элементом.
    /// </summary>
    InsertBefore,

    /// <summary>
    /// Вставка после элемента.
    /// </summary>
    InsertAfter,

    /// <summary>
    /// Замена элемента.
    /// </summary>
    Replace,

    /// <summary>
    /// Вставка как потомка.
    /// </summary>
    AddAsChild
}
