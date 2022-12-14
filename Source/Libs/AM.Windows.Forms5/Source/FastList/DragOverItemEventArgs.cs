// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DragOverItemEventArgs.cs -- аргумент для события "мышь над элементом"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргумент для события "мышь над элементом".
/// </summary>
public sealed class DragOverItemEventArgs
    : DragEventArgs
{
    #region Properties

    /// <summary>
    /// Индекс элемента.
    /// </summary>
    public int ItemIndex { get; set; }

    /// <summary>
    /// Метод вставки элемента.
    /// </summary>
    public InsertEffect InsertEffect { get; set; }

    /// <summary>
    /// Прямоугольник элемента.
    /// </summary>
    public Rectangle ItemRect { get; private set; }

    /// <summary>
    /// Прямоугольник текста.
    /// </summary>
    public Rectangle TextRect { get; private set; }

    /// <summary>
    /// Произвольные поользовательские данные.
    /// </summary>
    public object? Tag { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DragOverItemEventArgs
        (
            IDataObject? data,
            int keyState,
            int x,
            int y,
            DragDropEffects allowedEffects,
            DragDropEffects effect,
            Rectangle itemRect,
            Rectangle textRect
        )
        : base (data, keyState, x, y, allowedEffects, effect)
    {
        ItemRect = itemRect;
        TextRect = textRect;
    }

    #endregion
}
