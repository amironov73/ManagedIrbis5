// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ListViewGroupExtensions.cs -- методы расширения для ListViewGroup
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ListViewGroup"/>.
/// </summary>
public static class ListViewGroupExtensions
{
    #region Public methods

    /// <summary>
    /// Группа схлопнута?
    /// </summary>
    public static ListViewGroup CollapsedState
        (
            this ListViewGroup listViewGroup,
            ListViewGroupCollapsedState collapsedState
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.Defined (collapsedState);

        listViewGroup.CollapsedState = collapsedState;

        return listViewGroup;
    }

    /// <summary>
    /// Футер группы.
    /// </summary>
    public static ListViewGroup Footer
        (
            this ListViewGroup listViewGroup,
            string footer,
            HorizontalAlignment alignment = HorizontalAlignment.Left
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NotNull (footer);
        Sure.Defined (alignment);

        listViewGroup.Footer = footer;
        listViewGroup.FooterAlignment = alignment;

        return listViewGroup;
    }

    /// <summary>
    /// Заголовок группы.
    /// </summary>
    public static ListViewGroup Header
        (
            this ListViewGroup listViewGroup,
            string header,
            HorizontalAlignment alignment = HorizontalAlignment.Left
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NotNull (header);
        Sure.Defined (alignment);

        listViewGroup.Header = header;
        listViewGroup.HeaderAlignment = alignment;

        return listViewGroup;
    }

    /// <summary>
    /// Добавление элементов в группу.
    /// </summary>
    public static ListViewGroup Items
        (
            this ListViewGroup listViewGroup,
            params ListViewItem[] items
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NotNull (items);

        listViewGroup.Items.AddRange (items);

        return listViewGroup;
    }

    /// <summary>
    /// Добавление элементов в группу.
    /// </summary>
    public static ListViewGroup Items
        (
            this ListViewGroup listViewGroup,
            params string[] items
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NotNull (items);

        foreach (var item in items)
        {
            listViewGroup.Items.Add (new ListViewItem (item));
        }

        return listViewGroup;
    }

    /// <summary>
    /// Задание имени группы.
    /// </summary>
    public static ListViewGroup Name
        (
            this ListViewGroup listViewGroup,
            string name
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NotNullNorEmpty (name);

        listViewGroup.Name = name;

        return listViewGroup;
    }

    /// <summary>
    /// Задание подзаголовка для группы.
    /// </summary>
    public static ListViewGroup Subtitle
        (
            this ListViewGroup listViewGroup,
            string subtitle
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NotNullNorEmpty (subtitle);

        listViewGroup.Subtitle = subtitle;

        return listViewGroup;
    }

    /// <summary>
    /// Задание произвольной пользовательской метки для группы.
    /// </summary>
    public static ListViewGroup Tag
        (
            this ListViewGroup listViewGroup,
            object? tag
        )
    {
        Sure.NotNull (listViewGroup);

        listViewGroup.Tag = tag;

        return listViewGroup;
    }

    /// <summary>
    /// Имя ссылки, отображаемой в заголовке группы.
    /// </summary>
    public static ListViewGroup TaskLink
        (
            this ListViewGroup listViewGroup,
            string taskLink
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NotNullNorEmpty (taskLink);

        listViewGroup.TaskLink = taskLink;

        return listViewGroup;
    }

    /// <summary>
    /// Индекс картинки, отображаемой в заголовке группы.
    /// </summary>
    public static ListViewGroup TitleImage
        (
            this ListViewGroup listViewGroup,
            int index
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NonNegative (index);

        listViewGroup.TitleImageIndex = index;

        return listViewGroup;
    }

    /// <summary>
    /// Имя картинки, отображаемой в заголовке группы.
    /// </summary>
    public static ListViewGroup TitleImage
        (
            this ListViewGroup listViewGroup,
            string key
        )
    {
        Sure.NotNull (listViewGroup);
        Sure.NotNullNorEmpty (key);

        listViewGroup.TitleImageKey = key;

        return listViewGroup;
    }

    #endregion
}
