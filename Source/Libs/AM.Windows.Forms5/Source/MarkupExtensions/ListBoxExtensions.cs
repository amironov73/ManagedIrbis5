// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ListBoxExtensions.cs -- методы расширения для ListBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ListBox"/>.
/// </summary>
public static class ListBoxExtensions
{
    #region Public methods

    /// <summary>
    /// Установка ширины колонки (для мультиколоночного режима).
    /// </summary>
    public static TListBox ColumnWidth<TListBox>
        (
            this TListBox listBox,
            int width
        )
        where TListBox: ListBox
    {
        Sure.NotNull (listBox);
        Sure.Positive (width);

        listBox.ColumnWidth = width;

        return listBox;
    }

    /// <summary>
    /// Показ только целых элементов списка.
    /// </summary>
    public static TListBox IntegralHeight<TListBox>
        (
            this TListBox listBox,
            bool integralHeight = true
        )
        where TListBox: ListBox
    {
        Sure.NotNull (listBox);

        listBox.IntegralHeight = integralHeight;

        return listBox;
    }

    /// <summary>
    /// Установка высоты элементов списка.
    /// </summary>
    public static TListBox ItemHeight<TListBox>
        (
            this TListBox listBox,
            int height
        )
        where TListBox: ListBox
    {
        Sure.NotNull (listBox);
        Sure.Positive (height);

        listBox.ItemHeight = height;

        return listBox;
    }

    /// <summary>
    /// Добавление элементов.
    /// </summary>
    public static TListBox Items<TListBox>
        (
            this TListBox listBox,
            params string[] items
        )
        where TListBox: ListBox
    {
        Sure.NotNull (listBox);
        Sure.NotNull (items);

        listBox.Items.AddRange (items);

        return listBox;
    }

    /// <summary>
    /// Включение многоколоночного режима.
    /// </summary>
    public static TListBox MultiColumn<TListBox>
        (
            this TListBox listBox,
            bool multiColumn = true
        )
        where TListBox: ListBox
    {
        Sure.NotNull (listBox);

        listBox.MultiColumn = multiColumn;

        return listBox;
    }

    /// <summary>
    /// Полоса прокрутки всегда видна?
    /// </summary>
    public static TListBox ScrollAlwaysVisible<TListBox>
        (
            this TListBox listBox,
            bool alwaysVisible = true
        )
        where TListBox: ListBox
    {
        Sure.NotNull (listBox);

        listBox.ScrollAlwaysVisible = alwaysVisible;

        return listBox;
    }

    /// <summary>
    /// Установка режима пометки элементов.
    /// </summary>
    public static TListBox SelectionMode<TListBox>
        (
            this TListBox listBox,
            SelectionMode mode = System.Windows.Forms.SelectionMode.MultiExtended
        )
        where TListBox: ListBox
    {
        Sure.NotNull (listBox);
        Sure.Defined (mode);

        listBox.SelectionMode = mode;

        return listBox;
    }

    /// <summary>
    /// Включение сортировки списка.
    /// </summary>
    public static TListBox Sorted<TListBox>
        (
            this TListBox listBox,
            bool sorted = true
        )
        where TListBox: ListBox
    {
        Sure.NotNull (listBox);

        listBox.Sorted = true;

        return listBox;
    }

    #endregion
}
