// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ListViewItemExtensions.cs -- методы расширения для ListViewItem
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ListViewItem"/>.
/// </summary>
public static class ListViewItemExtensions
{
    #region Public methods

    /// <summary>
    /// Цвет фона.
    /// </summary>
    public static TItem BackColor<TItem>
        (
            this TItem item,
            Color color
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.BackColor = color;

        return item;
    }

    /// <summary>
    /// Элемент отмечен?
    /// </summary>
    public static TItem Checked<TItem>
        (
            this TItem item,
            bool isChecked = true
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.Checked = isChecked;

        return item;
    }

    /// <summary>
    /// Элемент находится в фокусе?
    /// </summary>
    public static TItem Focused<TItem>
        (
            this TItem item,
            bool focused = true
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.Focused = focused;

        return item;
    }

    /// <summary>
    /// Шрифт для элемента.
    /// </summary>
    public static TItem Focused<TItem>
        (
            this TItem item,
            Font font
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);
        Sure.NotNull (font);

        item.Font = font;

        return item;
    }

    /// <summary>
    /// Цвет текста.
    /// </summary>
    public static TItem ForeColor<TItem>
        (
            this TItem item,
            Color color
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.ForeColor = color;

        return item;
    }

    /// <summary>
    /// Номер картинки.
    /// </summary>
    public static TItem ImageIndex<TItem>
        (
            this TItem item,
            int index
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);
        Sure.NonNegative (index);

        item.ImageIndex = index;

        return item;
    }

    /// <summary>
    /// Имя картинки.
    /// </summary>
    public static TItem ImageKey<TItem>
        (
            this TItem item,
            string key
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);
        Sure.NotNullNorEmpty (key);

        item.ImageKey = key;

        return item;
    }

    /// <summary>
    /// Величина отступа.
    /// </summary>
    public static TItem IndentCount<TItem>
        (
            this TItem item,
            int indentCount
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);
        Sure.NonNegative (indentCount);

        item.IndentCount = indentCount;

        return item;
    }

    /// <summary>
    /// Имя элемента.
    /// </summary>
    public static TItem Name<TItem>
        (
            this TItem item,
            string name
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);
        Sure.NotNullNorEmpty (name);

        item.Name = name;

        return item;
    }

    /// <summary>
    /// Позиция элемента.
    /// </summary>
    public static TItem Position<TItem>
        (
            this TItem item,
            Point position
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.Position = position;

        return item;
    }

    /// <summary>
    /// Позиция элемента.
    /// </summary>
    public static TItem Position<TItem>
        (
            this TItem item,
            int left,
            int top
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.Position = new Point (left, top);

        return item;
    }

    /// <summary>
    /// Элемент выбран?
    /// </summary>
    public static TItem Selected<TItem>
        (
            this TItem item,
            bool selected = true
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.Selected = selected;

        return item;
    }

    /// <summary>
    /// Индекс картинки для состояния элемента.
    /// </summary>
    public static TItem StateImageIndex<TItem>
        (
            this TItem item,
            int index
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);
        Sure.NonNegative (index);

        item.StateImageIndex = index;

        return item;
    }

    /// <summary>
    /// Субэлементы.
    /// </summary>
    public static TItem SubItems<TItem>
        (
            this TItem item,
            params ListViewItem.ListViewSubItem[] subItems
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);
        Sure.NotNull (subItems);

        item.SubItems.AddRange (subItems);

        return item;
    }

    /// <summary>
    /// Произвольная пользовательская метка.
    /// </summary>
    public static TItem Tag<TItem>
        (
            this TItem item,
            object? tag
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.Tag = tag;

        return item;
    }

    /// <summary>
    /// Текст элемента.
    /// </summary>
    public static TItem Text<TItem>
        (
            this TItem item,
            string? text
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.Text = text;

        return item;
    }

    /// <summary>
    /// Текст тултипа для элемента.
    /// </summary>
    public static TItem ToolTipText<TItem>
        (
            this TItem item,
            string? text
        )
        where TItem: ListViewItem
    {
        Sure.NotNull (item);

        item.ToolTipText = text;

        return item;
    }

    #endregion
}
