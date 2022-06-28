// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CheckedListBoxExtensions.cs -- методы расширения для CheckedListBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="CheckedListBox"/>.
/// </summary>
public static class CheckedListBoxExtensions
{
    #region Public methods

    /// <summary>
    /// Задание индексов отмеченных элементов.
    /// </summary>
    public static TCheckedListBox CheckedIndices<TCheckedListBox>
        (
            this TCheckedListBox listBox,
            params int[] indices
        )
        where TCheckedListBox: CheckedListBox
    {
        Sure.NotNull (listBox);
        Sure.NotNull (indices);

        foreach (var index in indices)
        {
            Sure.NonNegative (index);
            listBox.SelectedIndices.Add (index);
        }

        return listBox;
    }

    /// <summary>
    /// Отмечать элемент при клике.
    /// </summary>
    public static TCheckedListBox CheckOnClick<TCheckedListBox>
        (
            this TCheckedListBox listBox,
            bool enabled = true
        )
        where TCheckedListBox: CheckedListBox
    {
        Sure.NotNull (listBox);

        listBox.CheckOnClick = enabled;

        return listBox;
    }

    /// <summary>
    /// Добавление элементов в список.
    /// </summary>
    public static TCheckedListBox Items<TCheckedListBox>
        (
            this TCheckedListBox listBox,
            params object[] items
        )
        where TCheckedListBox: CheckedListBox
    {
        Sure.NotNull (listBox);
        Sure.NotNull (items);

        listBox.Items.AddRange (items);

        return listBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="CheckedListBox.Click"/>.
    /// </summary>
    public static TCheckedListBox OnClick<TCheckedListBox>
        (
            this TCheckedListBox listBox,
            EventHandler handler
        )
        where TCheckedListBox: CheckedListBox
    {
        Sure.NotNull (listBox);
        Sure.NotNull (handler);

        listBox.Click += handler;

        return listBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="CheckedListBox.ItemCheck"/>.
    /// </summary>
    public static TCheckedListBox OnItemCheck<TCheckedListBox>
        (
            this TCheckedListBox listBox,
            ItemCheckEventHandler handler
        )
        where TCheckedListBox: CheckedListBox
    {
        Sure.NotNull (listBox);
        Sure.NotNull (handler);

        listBox.ItemCheck += handler;

        return listBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="CheckedListBox.MouseClick"/>.
    /// </summary>
    public static TCheckedListBox OnMouseClick<TCheckedListBox>
        (
            this TCheckedListBox listBox,
            MouseEventHandler handler
        )
        where TCheckedListBox: CheckedListBox
    {
        Sure.NotNull (listBox);
        Sure.NotNull (handler);

        listBox.MouseClick += handler;

        return listBox;
    }

    #endregion
}
