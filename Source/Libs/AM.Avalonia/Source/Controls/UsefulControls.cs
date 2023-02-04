// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* UsefulControls.cs -- простые, но полезные контролы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Простые, но полезные контролы.
/// </summary>
public static class UsefulControls
{
    #region Public methods

    /// <summary>
    /// Отображает элементы в виде горизонтального перечня,
    /// заворачивающегося на новую строку по мере необходимости.
    /// </summary>
    /// <param name="items">Элементы, подлежащие отображению.</param>
    /// <param name="itemTemplate">Шаблон контрола для отображения
    /// элемента.</param>
    /// <param name="orientation">Ориентация списка
    /// (по умолчанию горизонтальная).</param>
    /// <typeparam name="TItem">Тип отображаемых элементов.</typeparam>
    /// <returns>Созданный список.</returns>
    public static ItemsControl HorizontalWrappingItemsControl<TItem>
        (
            IEnumerable<TItem>? items,
            IDataTemplate? itemTemplate,
            Orientation orientation = Orientation.Horizontal
        )
    {
        return new ItemsControl
        {
            ItemsPanel = new FuncTemplate<Panel> (() => new WrapPanel
            {
                Orientation = orientation
            }),

            Items = items,

            ItemTemplate = itemTemplate
        };
    }

    /// <summary>
    /// Отображает элементы в виде горизонтального перечня,
    /// заворачивающегося на новую строку по мере необходимости.
    /// </summary>
    /// <param name="items">Элементы, подлежащие отображению.</param>
    /// <param name="itemTemplate">Шаблон контрола для отображения
    /// элемента.</param>
    /// <param name="orientation">Ориентация списка
    /// (по умолчанию горизонтальная).</param>
    /// <typeparam name="TItem">Тип отображаемых элементов.</typeparam>
    /// <returns>Созданный список.</returns>
    public static ItemsControl HorizontalWrappingItemsControl<TItem>
        (
            IEnumerable<TItem>? items,
            Func<TItem, INameScope, Control?> itemTemplate,
            Orientation orientation = Orientation.Horizontal
        )
    {
        Sure.NotNull (itemTemplate);

        return new ItemsControl
        {
            ItemsPanel = new FuncTemplate<Panel> (() => new WrapPanel
            {
                Orientation = orientation
            }),

            Items = items,

            ItemTemplate = new FuncDataTemplate<TItem> (itemTemplate)
        };
    }

    /// <summary>
    /// Отображает элементы в виде горизонтального перечня,
    /// заворачивающегося на новую строку по мере необходимости.
    /// </summary>
    /// <param name="items">Элементы, подлежащие отображению.</param>
    /// <param name="itemTemplate">Шаблон контрола для отображения
    /// элемента.</param>
    /// <param name="orientation">Ориентация списка
    /// (по умолчанию горизонтальная).</param>
    /// <typeparam name="TItem">Тип отображаемых элементов.</typeparam>
    /// <returns>Созданный список.</returns>
    public static ListBox HorizontalWrappingListBox<TItem>
        (
            IEnumerable<TItem>? items,
            IDataTemplate? itemTemplate,
            Orientation orientation = Orientation.Horizontal
        )
    {
        return new ListBox
        {
            ItemsPanel = new FuncTemplate<Panel> (() => new WrapPanel
            {
                Orientation = orientation
            }),

            Items = items,

            ItemTemplate = itemTemplate
        };
    }

    /// <summary>
    /// Отображает элементы в виде горизонтального перечня,
    /// заворачивающегося на новую строку по мере необходимости.
    /// </summary>
    /// <param name="items">Элементы, подлежащие отображению.</param>
    /// <param name="itemTemplate">Шаблон контрола для отображения
    /// элемента.</param>
    /// <param name="orientation">Ориентация списка
    /// (по умолчанию горизонтальная).</param>
    /// <typeparam name="TItem">Тип отображаемых элементов.</typeparam>
    /// <returns>Созданный список.</returns>
    public static ListBox HorizontalWrappingListBox<TItem>
        (
            IEnumerable<TItem>? items,
            Func<TItem, INameScope, Control?> itemTemplate,
            Orientation orientation = Orientation.Horizontal
        )
    {
        Sure.NotNull (itemTemplate);

        return new ListBox
        {
            ItemsPanel = new FuncTemplate<Panel> (() => new WrapPanel
            {
                Orientation = orientation
            }),

            Items = items,

            ItemTemplate = new FuncDataTemplate<TItem> (itemTemplate)
        };
    }

    #endregion
}
