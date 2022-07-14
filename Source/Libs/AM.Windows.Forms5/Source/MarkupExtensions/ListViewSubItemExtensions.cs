// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ListViewSubItemExtensions.cs -- методы расширения для ListViewItem.ListViewSubItem
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ListViewItem.ListViewSubItem"/>.
/// </summary>
public static class ListViewSubItemExtensions
{
    #region Public methods

    /// <summary>
    /// Цвет фона.
    /// </summary>
    public static TSubItem BackColor<TSubItem>
        (
            this TSubItem subItem,
            Color color
        )
        where TSubItem: ListViewItem.ListViewSubItem
    {
        Sure.NotNull (subItem);

        subItem.BackColor = color;

        return subItem;
    }

    /// <summary>
    /// Шрифт.
    /// </summary>
    public static TSubItem Font<TSubItem>
        (
            this TSubItem subItem,
            Font font
        )
        where TSubItem: ListViewItem.ListViewSubItem
    {
        Sure.NotNull (subItem);
        Sure.NotNull (font);

        subItem.Font = font;

        return subItem;
    }

    /// <summary>
    /// Цвет текста.
    /// </summary>
    public static TSubItem ForeColor<TSubItem>
        (
            this TSubItem subItem,
            Color color
        )
        where TSubItem: ListViewItem.ListViewSubItem
    {
        Sure.NotNull (subItem);

        subItem.ForeColor = color;

        return subItem;
    }

    /// <summary>
    /// Имя.
    /// </summary>
    public static TSubItem Name<TSubItem>
        (
            this TSubItem subItem,
            string name
        )
        where TSubItem: ListViewItem.ListViewSubItem
    {
        Sure.NotNull (subItem);
        Sure.NotNullNorEmpty (name);

        subItem.Name = name;

        return subItem;
    }

    /// <summary>
    /// Произвольная пользовательская метка.
    /// </summary>
    public static TSubItem Tag<TSubItem>
        (
            this TSubItem subItem,
            object? tag
        )
        where TSubItem: ListViewItem.ListViewSubItem
    {
        Sure.NotNull (subItem);

        subItem.Tag = tag;

        return subItem;
    }

    /// <summary>
    /// Текст.
    /// </summary>
    public static TSubItem Text<TSubItem>
        (
            this TSubItem subItem,
            string text
        )
        where TSubItem: ListViewItem.ListViewSubItem
    {
        Sure.NotNull (subItem);
        Sure.NotNullNorEmpty (text);

        subItem.Text = text;

        return subItem;
    }

    #endregion
}
