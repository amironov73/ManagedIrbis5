// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ListViewExtensions.cs -- методы расширения для ListView
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ListView"/>.
/// </summary>
public static class ListViewExtensions
{
    #region Public methods

    /// <summary>
    /// Задание метода активации для элементов в списке.
    /// </summary>
    public static TListView Activation<TListView>
        (
            this TListView listView,
            ItemActivation activation
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.Defined (activation);

        listView.Activation = activation;

        return listView;
    }

    /// <summary>
    /// Выравнивание элементов в списке.
    /// </summary>
    public static TListView Alignment<TListView>
        (
            this TListView listView,
            ListViewAlignment alignment
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.Defined (alignment);

        listView.Alignment = alignment;

        return listView;
    }

    /// <summary>
    /// Разрешение/запрет переупорядочивания колонок.
    /// </summary>
    public static TListView AllowColumnReorder<TListView>
        (
            this TListView listView,
            bool allow = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.AllowColumnReorder = allow;

        return listView;
    }

    /// <summary>
    /// Автоматическое (пере-) размещение иконок.
    /// </summary>
    public static TListView AutoArrange<TListView>
        (
            this TListView listView,
            bool allow = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.AutoArrange = allow;

        return listView;
    }

    /// <summary>
    /// Задание стиля границы.
    /// </summary>
    public static TListView BorderStyle<TListView>
        (
            this TListView listView,
            BorderStyle borderStyle
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.Defined (borderStyle);

        listView.BorderStyle = borderStyle;

        return listView;
    }

    /// <summary>
    /// Разрешение/запрет отображения чекбоксов для элементов списка.
    /// </summary>
    public static TListView CheckBoxes<TListView>
        (
            this TListView listView,
            bool allow = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.CheckBoxes = allow;

        return listView;
    }

    /// <summary>
    /// Задание индексов отмеченных элементов.
    /// </summary>
    public static TListView CheckedIndices<TListView>
        (
            this TListView listView,
            params int[] indices
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (indices);

        foreach (ListViewItem item in listView.Items)
        {
            item.Checked = false;
        }

        foreach (var index in indices)
        {
            listView.Items[index].Checked = true;
        }

        return listView;
    }

    /// <summary>
    /// Задание колонок.
    /// </summary>
    public static TListView Columns<TListView>
        (
            this TListView listView,
            params ColumnHeader[] headers
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (headers);

        listView.Columns.AddRange (headers);

        return listView;
    }

    /// <summary>
    /// Задание элемента, на котором установлен фокус.
    /// </summary>
    public static TListView FocusedItem<TListView>
        (
            this TListView listView,
            ListViewItem? focusedItem
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.FocusedItem = focusedItem;

        return listView;
    }

    /// <summary>
    /// Выделение целой строки.
    /// </summary>
    public static TListView FullRowSelect<TListView>
        (
            this TListView listView,
            bool fullRowSelect = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.FullRowSelect = fullRowSelect;

        return listView;
    }

    /// <summary>
    /// Отображение линий сетки.
    /// </summary>
    public static TListView GridLines<TListView>
        (
            this TListView listView,
            bool gridLines = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.GridLines = gridLines;

        return listView;
    }

    /// <summary>
    /// Иконки для групп.
    /// </summary>
    public static TListView GroupImageList<TListView>
        (
            this TListView listView,
            ImageList imageList
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (imageList);

        listView.GroupImageList = imageList;

        return listView;
    }

    /// <summary>
    /// Задание групп.
    /// </summary>
    public static TListView Groups<TListView>
        (
            this TListView listView,
            params ListViewGroup[] groups
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (groups);

        listView.Groups.AddRange (groups);

        return listView;
    }

    /// <summary>
    /// Задание стиля заголовков.
    /// </summary>
    public static TListView HeaderStyle<TListView>
        (
            this TListView listView,
            ColumnHeaderStyle headerStyle
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.Defined (headerStyle);

        listView.HeaderStyle = headerStyle;

        return listView;
    }

    /// <summary>
    /// Прятать выделение при потере фокуса?
    /// </summary>
    public static TListView HideSelection<TListView>
        (
            this TListView listView,
            bool hideSelection = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.HideSelection = hideSelection;

        return listView;
    }

    /// <summary>
    /// Включение/выключение отслеживания мыши.
    /// </summary>
    public static TListView HotTracking<TListView>
        (
            this TListView listView,
            bool hotTracking = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.HotTracking = hotTracking;

        return listView;
    }

    /// <summary>
    /// Выделение элементов зависанием мыши.
    /// </summary>
    public static TListView HoverSelection<TListView>
        (
            this TListView listView,
            bool hoverSelection = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.HoverSelection = hoverSelection;

        return listView;
    }

    /// <summary>
    /// Добавление элементов в список.
    /// </summary>
    public static TListView Items<TListView>
        (
            this TListView listView,
            params ListViewItem[] items
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.Items.AddRange (items);

        return listView;
    }

    /// <summary>
    /// Добавление элементов в список.
    /// </summary>
    public static TListView Items<TListView>
        (
            this TListView listView,
            params string[] items
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        foreach (var item in items)
        {
            var listViewItem = new ListViewItem (item);
            listView.Items.Add (listViewItem);
        }

        return listView;
    }

    /// <summary>
    /// Сравнение элементов списка для сортировки.
    /// </summary>
    public static TListView ItemSorter<TListView>
        (
            this TListView listView,
            IComparer comparer
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (comparer);

        listView.ListViewItemSorter = comparer;

        return listView;
    }

    /// <summary>
    /// Разрешение/запрет редактирования меток.
    /// </summary>
    public static TListView LabelEdit<TListView>
        (
            this TListView listView,
            bool labelEdit = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.LabelEdit = labelEdit;

        return listView;
    }

    /// <summary>
    /// Разрешение/запрет автоматического перевода строки в метках.
    /// </summary>
    public static TListView LabelWrap<TListView>
        (
            this TListView listView,
            bool labelWrap = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.LabelWrap = labelWrap;

        return listView;
    }

    /// <summary>
    /// Большие картинки.
    /// </summary>
    public static TListView LargeImageList<TListView>
        (
            this TListView listView,
            ImageList imageList
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (imageList);

        listView.LargeImageList = imageList;

        return listView;
    }

    /// <summary>
    /// Включение/выключение множественного выбора.
    /// </summary>
    public static TListView MultiSelect<TListView>
        (
            this TListView listView,
            bool multiSelect = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.MultiSelect = multiSelect;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.ColumnClick"/>.
    /// </summary>
    public static TListView OnColumnClick<TListView>
        (
            this TListView listView,
            ColumnClickEventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.ColumnClick += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.DrawItem"/>.
    /// </summary>
    public static TListView OnDrawItem<TListView>
        (
            this TListView listView,
            DrawListViewItemEventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.DrawItem += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.DrawSubItem"/>.
    /// </summary>
    public static TListView OnDrawSubItem<TListView>
        (
            this TListView listView,
            DrawListViewSubItemEventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.DrawSubItem += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.ItemActivate"/>.
    /// </summary>
    public static TListView OnItemActivate<TListView>
        (
            this TListView listView,
            EventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.ItemActivate += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.ItemCheck"/>.
    /// </summary>
    public static TListView OnItemCheck<TListView>
        (
            this TListView listView,
            ItemCheckEventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.ItemCheck += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.ItemChecked"/>.
    /// </summary>
    public static TListView OnItemChecked<TListView>
        (
            this TListView listView,
            ItemCheckedEventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.ItemChecked += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.ItemMouseHover"/>.
    /// </summary>
    public static TListView OnItemMouseHover<TListView>
        (
            this TListView listView,
            ListViewItemMouseHoverEventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.ItemMouseHover += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.ItemSelectionChanged"/>.
    /// </summary>
    public static TListView OnItemSelectionChanged<TListView>
        (
            this TListView listView,
            ListViewItemSelectionChangedEventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.ItemSelectionChanged += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.RetrieveVirtualItem"/>.
    /// </summary>
    public static TListView OnRetrieveVirtualItem<TListView>
        (
            this TListView listView,
            RetrieveVirtualItemEventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.RetrieveVirtualItem += handler;

        return listView;
    }

    /// <summary>
    /// Подписка на событие <see cref="ListView.SelectedIndexChanged"/>.
    /// </summary>
    public static TListView OnRetrieveVirtualItem<TListView>
        (
            this TListView listView,
            EventHandler handler
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (handler);

        listView.SelectedIndexChanged += handler;

        return listView;
    }

    /// <summary>
    /// Включение/выключение пользовательской отрисовки элементов списка.
    /// </summary>
    public static TListView OwnerDraw<TListView>
        (
            this TListView listView,
            bool ownerDraw = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.OwnerDraw = ownerDraw;

        return listView;
    }

    /// <summary>
    /// Включение/выключение прокрутки при нехватке места на экране.
    /// </summary>
    public static TListView Scrollable<TListView>
        (
            this TListView listView,
            bool scrollable = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.Scrollable = scrollable;

        return listView;
    }

    /// <summary>
    /// Задание выделенных элементов по индексам.
    /// </summary>
    public static TListView SelectedIndices<TListView>
        (
            this TListView listView,
            params int[] indices
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (indices);

        foreach (ListViewItem item in listView.Items)
        {
            item.Selected = false;
        }

        foreach (var index in indices)
        {
            listView.Items[index].Selected = true;
        }

        return listView;
    }

    /// <summary>
    /// Задание выделенных элементов.
    /// </summary>
    public static TListView SelectedItems<TListView>
        (
            this TListView listView,
            params ListViewItem[] items
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (items);

        foreach (ListViewItem item in listView.Items)
        {
            item.Selected = false;
        }

        foreach (var item in items)
        {
            item.Selected = true;
        }

        return listView;
    }

    /// <summary>
    /// Отображать группы элементов?
    /// </summary>
    public static TListView ShowGroups<TListView>
        (
            this TListView listView,
            bool showGroups = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.ShowGroups = showGroups;

        return listView;
    }

    /// <summary>
    /// Отображать подсказки для элементов?
    /// </summary>
    public static TListView ShowItemToolTips<TListView>
        (
            this TListView listView,
            bool showItemToolTips = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.ShowItemToolTips = showItemToolTips;

        return listView;
    }

    /// <summary>
    /// Маленькие картинки.
    /// </summary>
    public static TListView SmallImageList<TListView>
        (
            this TListView listView,
            ImageList imageList
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (imageList);

        listView.SmallImageList = imageList;

        return listView;
    }

    /// <summary>
    /// Задание сортировки для элементов.
    /// </summary>
    public static TListView Sorting<TListView>
        (
            this TListView listView,
            SortOrder sortOrder = SortOrder.Ascending
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.Defined (sortOrder);

        listView.Sorting = sortOrder;

        return listView;
    }

    /// <summary>
    /// Картинки, отображающие состояние элементов.
    /// </summary>
    public static TListView StateImageList<TListView>
        (
            this TListView listView,
            ImageList imageList
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.NotNull (imageList);

        listView.StateImageList = imageList;

        return listView;
    }

    /// <summary>
    /// Размер черепицы.
    /// </summary>
    public static TListView TileSize<TListView>
        (
            this TListView listView,
            Size size
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.TileSize = size;

        return listView;
    }

    /// <summary>
    /// Размер черепицы.
    /// </summary>
    public static TListView TileSize<TListView>
        (
            this TListView listView,
            int width,
            int height
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.Positive (width);
        Sure.Positive (height);

        listView.TileSize = new Size (width, height);

        return listView;
    }

    /// <summary>
    /// Размер виртуального списка.
    /// </summary>
    public static TListView VirtualListSize<TListView>
        (
            this TListView listView,
            int size
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);
        Sure.Positive (size);

        listView.VirtualListSize = size;

        return listView;
    }

    /// <summary>
    /// Включение/выключение виртуального режима.
    /// </summary>
    public static TListView VirtualMode<TListView>
        (
            this TListView listView,
            bool virtualMode = true
        )
        where TListView: ListView
    {
        Sure.NotNull (listView);

        listView.VirtualMode = virtualMode;

        return listView;
    }

    #endregion
}
