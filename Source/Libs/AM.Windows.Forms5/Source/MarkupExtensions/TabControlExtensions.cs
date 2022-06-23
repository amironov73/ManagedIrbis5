// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TabControlExtensions.cs -- методы расширения для TabControl
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="TabControl"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TabControlExtensions
{
    #region Public methods

    /// <summary>
    /// Раположение табов (сверху, снизу и т.д.).
    /// </summary>
    public static TTabControl Alignment<TTabControl>
        (
            this TTabControl tabControl,
            TabAlignment alignment
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.Defined (alignment);

        tabControl.Alignment = alignment;

        return tabControl;
    }

    /// <summary>
    /// Визуальное отображение табов.
    /// </summary>
    public static TTabControl Appearance<TTabControl>
        (
            this TTabControl tabControl,
            TabAppearance appearance
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.Defined (appearance);

        tabControl.Appearance = appearance;

        return tabControl;
    }

    /// <summary>
    /// Подписка на событие <see cref="TabControl.DrawItem"/>.
    /// </summary>
    public static TTabControl OnDrawItem<TTabControl>
        (
            this TTabControl tabControl,
            DrawItemEventHandler handler
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.NotNull (handler);

        tabControl.DrawItem += handler;

        return tabControl;
    }

    /// <summary>
    /// Визуальное отслеживание мыши над табами.
    /// </summary>
    public static TTabControl HotTrack<TTabControl>
        (
            this TTabControl tabControl,
            bool hotTrack = true
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);

        tabControl.HotTrack = hotTrack;

        return tabControl;
    }

    /// <summary>
    /// Список картинок.
    /// </summary>
    public static TTabControl ImageList<TTabControl>
        (
            this TTabControl tabControl,
            ImageList imageList
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.NotNull (imageList);

        tabControl.ImageList = imageList;

        return tabControl;
    }

    /// <summary>
    /// Размеры табов.
    /// </summary>
    public static TTabControl ItemSize<TTabControl>
        (
            this TTabControl tabControl,
            Size size
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);

        tabControl.ItemSize = size;

        return tabControl;
    }

    /// <summary>
    /// Включение многострочного режима.
    /// </summary>
    public static TTabControl Multiline<TTabControl>
        (
            this TTabControl tabControl,
            bool multiline = true
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);

        tabControl.Multiline = multiline;

        return tabControl;
    }

    /// <summary>
    /// Подписка на событие <see cref="TabControl.Selected"/>.
    /// </summary>
    public static TTabControl OnSelected<TTabControl>
        (
            this TTabControl tabControl,
            TabControlEventHandler handler
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.NotNull (handler);

        tabControl.Selected += handler;

        return tabControl;
    }

    /// <summary>
    /// Подписка на событие <see cref="TabControl.SelectedIndexChanged"/>.
    /// </summary>
    public static TTabControl OnSelectedIndexChanged<TTabControl>
        (
            this TTabControl tabControl,
            EventHandler handler
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.NotNull (handler);

        tabControl.SelectedIndexChanged += handler;

        return tabControl;
    }

    /// <summary>
    /// Подписка на событие <see cref="TabControl.Selecting"/>.
    /// </summary>
    public static TTabControl OnSelecting<TTabControl>
        (
            this TTabControl tabControl,
            TabControlCancelEventHandler handler
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.NotNull (handler);

        tabControl.Selecting += handler;

        return tabControl;
    }

    /// <summary>
    /// Задание номера выбранного таба.
    /// </summary>
    public static TTabControl SelectedIndex<TTabControl>
        (
            this TTabControl tabControl,
            int index
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.NonNegative (index);

        tabControl.SelectedIndex = index;

        return tabControl;
    }

    /// <summary>
    /// Включение отображения тултипов над табами.
    /// </summary>
    public static TTabControl ShowToolTips<TTabControl>
        (
            this TTabControl tabControl,
            bool show = true
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);

        tabControl.ShowToolTips = show;

        return tabControl;
    }

    /// <summary>
    /// Режим изменения размеров табов.
    /// </summary>
    public static TTabControl SizeMode<TTabControl>
        (
            this TTabControl tabControl,
            TabSizeMode sizeMode
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.Defined (sizeMode);

        tabControl.SizeMode = sizeMode;

        return tabControl;
    }

    /// <summary>
    /// Добавление страниц.
    /// </summary>
    public static TTabControl TabPages<TTabControl>
        (
            this TTabControl tabControl,
            params TabPage[] pages
        )
        where TTabControl: TabControl
    {
        Sure.NotNull (tabControl);
        Sure.NotNull (pages);

        tabControl.TabPages.AddRange (pages);

        return tabControl;
    }

    #endregion
}
