// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* FastList.cs -- список элементов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Список элементов.
/// </summary>
[DefaultEvent ("ItemTextNeeded")]
[ToolboxItem (true)]
public class FastList
    : FastListBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FastList()
    {
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            ItemCount = 100;
            ItemTextNeeded += (_, a) => a.Result = "Item " + a.ItemIndex;
            SelectedItemIndexes.Add (0);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение списка с указанным числом элементов.
    /// </summary>
    /// <param name="itemCount"></param>
    public void Build
        (
            int itemCount
        )
    {
        ItemCount = itemCount;
    }

    /// <summary>
    /// Перестроение списка.
    /// </summary>
    public void Rebuild()
    {
        Invalidate();
    }

    /// <summary>
    /// Построение списка на основе указанной коллекции элеементов.
    /// </summary>
    /// <param name="list"></param>
    public void Build
        (
            ICollection? list
        )
    {
        ItemCount = list?.Count ?? 0;
    }

    #endregion

    #region Overrided methods

    /// <inheritdoc cref="FastListBase.CheckItem"/>
    public override bool CheckItem
        (
            int itemIndex
        )
    {
        if (GetItemChecked (itemIndex))
        {
            return true;
        }

        Invalidate();

        if (CanCheckItem (itemIndex))
        {
            if (ItemCheckStateNeeded ==
                null) //add to CheckedItemIndex only if handler of ItemCheckStateNeeded is not assigned
            {
                CheckedItemIndex.Add (itemIndex);
            }

            OnItemChecked (itemIndex);
            return true;
        }

        return false;
    }

    /// <inheritdoc cref="FastListBase.IsItemHeightFixed"/>
    protected override bool IsItemHeightFixed => ItemHeightNeeded is null;

    #endregion Overrided methods

    #region Events

    /// <summary>
    /// Запрос высоты для указанного элемента.
    /// </summary>
    public event EventHandler<Int32ItemEventArgs>? ItemHeightNeeded;

    /// <summary>
    /// Запрос величины отступа для указанного элемента.
    /// </summary>
    public event EventHandler<Int32ItemEventArgs>? ItemIndentNeeded;

    /// <summary>
    /// Запрос текста для указанного элемента.
    /// </summary>
    public event EventHandler<StringItemEventArgs>? ItemTextNeeded;

    /// <summary>
    /// Запрос состояния отметки для указанного элемента.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? ItemCheckStateNeeded;

    /// <summary>
    /// Запрос иконки для указанного элемента.
    /// </summary>
    public event EventHandler<ImageItemEventArgs>? ItemIconNeeded;

    /// <summary>
    /// Запрос, нужно ли показывать чекбокс для указанного элемента.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? ItemCheckBoxVisibleNeeded;

    /// <summary>
    /// Запрос цвета фона для указанного элемента.
    /// </summary>
    public event EventHandler<ColorItemEventArgs>? ItemBackColorNeeded;

    /// <summary>
    /// Запрос цвета текста дял указанного элемента.
    /// </summary>
    public event EventHandler<ColorItemEventArgs>? ItemForeColorNeeded;

    /// <summary>
    /// Запрос состояния "свернут/развернут" для указанного элемента.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? ItemExpandedNeeded;

    /// <summary>
    /// Запрос возможности снять выделение с указанного элемента.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? CanUnselectItemNeeded;

    /// <summary>
    /// Запрос возможности установить выделение для указанного элемента.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? CanSelectItemNeeded;

    /// <summary>
    /// Запрос возможности снять отметку с указанного элемента.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? CanUncheckItemNeeded;

    /// <summary>
    /// Запрос возможности установить отметку для указанного элемента.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? CanCheckItemNeeded;

    /// <summary>
    /// Запрос возможности развернуть указанный элемент.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? CanExpandItemNeeded;

    /// <summary>
    /// Запрос возможности свернуть указанный элемент.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? CanCollapseItemNeeded;

    /// <summary>
    /// Запрос возможности редактирования текста для указанного элемента.
    /// </summary>
    public event EventHandler<BoolItemEventArgs>? CanEditItemNeeded;

    /// <summary>
    /// Произошло изменение состояния "отмечен" для указанного элемента.
    /// </summary>
    public event EventHandler<ItemCheckedStateChangedEventArgs>? ItemCheckedStateChanged;

    /// <summary>
    /// Произошло зменение состояния "развернут" для указанного элемента.
    /// </summary>
    public event EventHandler<ItemExpandedStateChangedEventArgs>? ItemExpandedStateChanged;

    /// <summary>
    /// Произошло изменение состояния "выбран" для указанного элемента.
    /// </summary>
    public event EventHandler<ItemSelectedStateChangedEventArgs>? ItemSelectedStateChanged;

    /// <summary>
    /// Произошло изменение текста для указанного элемента.
    /// </summary>
    public event EventHandler<ItemTextPushedEventArgs>? ItemTextPushed;

    /// <summary>
    /// Отрисовка элемента.
    /// </summary>
    public event EventHandler<PaintItemContentEventArgs>? PaintItem;

    /// <summary>
    /// Произошло обновление состояния полос прокрутки.
    /// </summary>
    public event EventHandler? ScrollbarsUpdated;

    /// <summary>
    /// Occurs when user start to drag items
    /// </summary>
    public event EventHandler<ItemDragEventArgs>? ItemDrag;

    /// <summary>
    /// Occurs when user drag object over node
    /// </summary>
    public event EventHandler<DragOverItemEventArgs>? DragOverItem;

    /// <summary>
    /// Occurs when user drop object on given item
    /// </summary>
    public event EventHandler<DragOverItemEventArgs>? DropOverItem;

    /// <inheritdoc cref="FastListBase.GetItemHeight"/>
    protected override int GetItemHeight
        (
            int itemIndex
        )
    {
        return GetIntItemProperty (itemIndex, ItemHeightNeeded, ItemHeightDefault);
    }

    /// <inheritdoc cref="FastListBase.GetItemIndent"/>
    public override int GetItemIndent
        (
            int itemIndex
        )
    {
        return GetIntItemProperty (itemIndex, ItemIndentNeeded, ItemIndentDefault);
    }

    /// <inheritdoc cref="FastListBase.GetItemText"/>
    protected override string GetItemText
        (
            int itemIndex
        )
    {
        return GetStringItemProperty (itemIndex, ItemTextNeeded, string.Empty);
    }

    /// <inheritdoc cref="FastListBase.GetItemChecked"/>
    protected override bool GetItemChecked
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, ItemCheckStateNeeded, CheckedItemIndex.Contains (itemIndex));
    }

    /// <inheritdoc cref="FastListBase.GetItemIcon"/>
    protected override Image GetItemIcon
        (
            int itemIndex
        )
    {
        return GetImageItemProperty (itemIndex, ItemIconNeeded, ImageDefaultIcon);
    }

    /// <inheritdoc cref="FastListBase.GetItemCheckBoxVisible"/>
    protected override bool GetItemCheckBoxVisible
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, ItemCheckBoxVisibleNeeded, ShowCheckBoxes);
    }

    /// <inheritdoc cref="FastListBase.GetItemBackColor"/>
    protected override Color GetItemBackColor
        (
            int itemIndex
        )
    {
        return GetColorItemProperty (itemIndex, ItemBackColorNeeded, Color.Empty);
    }

    /// <inheritdoc cref="FastListBase.GetItemForeColor"/>
    protected override Color GetItemForeColor
        (
            int itemIndex
        )
    {
        return GetColorItemProperty (itemIndex, ItemForeColorNeeded, ForeColor);
    }

    /// <inheritdoc cref="FastListBase.GetItemExpanded"/>
    protected override bool GetItemExpanded
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, ItemExpandedNeeded, false);
    }

    /// <inheritdoc cref="FastListBase.CanUnselectItem"/>
    protected override bool CanUnselectItem
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, CanUnselectItemNeeded, true);
    }

    /// <inheritdoc cref="FastListBase.CanSelectItem"/>
    protected override bool CanSelectItem
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, CanSelectItemNeeded, AllowSelectItems);
    }

    /// <inheritdoc cref="FastListBase.CanUncheckItem"/>
    protected override bool CanUncheckItem
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, CanUncheckItemNeeded, true);
    }

    /// <inheritdoc cref="FastListBase.CanCheckItem"/>
    protected override bool CanCheckItem
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, CanCheckItemNeeded, true);
    }

    /// <inheritdoc cref="FastListBase.CanExpandItem"/>
    protected override bool CanExpandItem
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, CanExpandItemNeeded, true);
    }

    /// <inheritdoc cref="FastListBase.CanCollapseItem"/>
    protected override bool CanCollapseItem
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, CanCollapseItemNeeded, true);
    }

    /// <inheritdoc cref="FastListBase.CanEditItem"/>
    protected override bool CanEditItem
        (
            int itemIndex
        )
    {
        return GetBoolItemProperty (itemIndex, CanEditItemNeeded, true);
    }

    /// <inheritdoc cref="FastListBase.OnItemChecked"/>
    protected override void OnItemChecked
        (
            int itemIndex
        )
    {
        ItemCheckedStateChanged?.Invoke
            (
                this,
                new ItemCheckedStateChangedEventArgs { ItemIndex = itemIndex, Checked = true }
            );

        base.OnItemChecked (itemIndex);
    }

    /// <inheritdoc cref="FastListBase.OnItemUnchecked"/>
    protected override void OnItemUnchecked (int itemIndex)
    {
        ItemCheckedStateChanged?.Invoke
            (
                this,
                new ItemCheckedStateChangedEventArgs { ItemIndex = itemIndex, Checked = false }
            );

        base.OnItemUnchecked (itemIndex);
    }

    /// <inheritdoc cref="FastListBase.OnItemExpanded"/>
    protected override void OnItemExpanded (int itemIndex)
    {
        if (ItemExpandedStateChanged != null)
        {
            ItemExpandedStateChanged (this,
                new ItemExpandedStateChangedEventArgs { ItemIndex = itemIndex, Expanded = true });
        }

        base.OnItemExpanded (itemIndex);
    }

    /// <inheritdoc cref="FastListBase.OnItemCollapsed"/>
    protected override void OnItemCollapsed (int itemIndex)
    {
        if (ItemExpandedStateChanged != null)
        {
            ItemExpandedStateChanged (this,
                new ItemExpandedStateChangedEventArgs { ItemIndex = itemIndex, Expanded = false });
        }

        base.OnItemCollapsed (itemIndex);
    }

    /// <inheritdoc cref="FastListBase.OnItemSelected"/>
    protected override void OnItemSelected (int itemIndex)
    {
        if (ItemSelectedStateChanged != null)
        {
            ItemSelectedStateChanged (this,
                new ItemSelectedStateChangedEventArgs { ItemIndex = itemIndex, Selected = true });
        }

        base.OnItemSelected (itemIndex);
    }

    /// <inheritdoc cref="FastListBase.OnItemTextPushed"/>
    protected override void OnItemTextPushed (int itemIndex, string text)
    {
        if (ItemTextPushed != null)
        {
            ItemTextPushed (this, new ItemTextPushedEventArgs { ItemIndex = itemIndex, Text = text });
        }

        base.OnItemTextPushed (itemIndex, text);
    }

    /// <inheritdoc cref="FastListBase.OnItemUnselected"/>
    protected override void OnItemUnselected (int itemIndex)
    {
        if (ItemSelectedStateChanged != null)
        {
            ItemSelectedStateChanged (this,
                new ItemSelectedStateChangedEventArgs { ItemIndex = itemIndex, Selected = false });
        }

        base.OnItemUnselected (itemIndex);
    }

    /// <inheritdoc cref="FastListBase.OnItemDrag"/>
    protected override void OnItemDrag (HashSet<int> itemIndex)
    {
        if (ItemDrag != null)
        {
            ItemDrag (this, new ItemDragEventArgs { ItemIndex = itemIndex });
        }
        else
        {
            DoDragDrop (itemIndex, DragDropEffects.Copy);
        }

        base.OnItemDrag (itemIndex);
    }

    /// <inheritdoc cref="FastListBase.DrawItem"/>
    protected override void DrawItem (Graphics gr, VisibleItemInfo info)
    {
        if (PaintItem != null)
        {
            PaintItem (this, new PaintItemContentEventArgs { Graphics = gr, Info = info });
        }
        else
        {
            base.DrawItem (gr, info);
        }
    }

    /// <inheritdoc cref="FastListBase.OnDragOverItem"/>
    protected override void OnDragOverItem (DragOverItemEventArgs eventArgs)
    {
        base.OnDragOverItem (eventArgs);

        DragOverItem?.Invoke (this, eventArgs);
    }

    /// <inheritdoc cref="FastListBase.OnDropOverItem"/>
    protected override void OnDropOverItem (DragOverItemEventArgs eventArgs)
    {
        DropOverItem?.Invoke (this, eventArgs);

        base.OnDropOverItem (eventArgs);
    }

    /// <inheritdoc cref="FastListBase.OnScrollbarsUpdated"/>
    protected override void OnScrollbarsUpdated()
    {
        ScrollbarsUpdated?.Invoke (this, EventArgs.Empty);
        base.OnScrollbarsUpdated();
    }

    #endregion

    #region Event Helpers

    private readonly Int32ItemEventArgs _int32Arg = new ();
    private readonly BoolItemEventArgs _boolArg = new ();
    private readonly StringItemEventArgs _stringArg = new ();
    private readonly ImageItemEventArgs _imageArg = new ();
    private readonly ColorItemEventArgs _colorArg = new ();

    private int GetIntItemProperty
        (
            int itemIndex,
            EventHandler<Int32ItemEventArgs>? handler,
            int defaultValue
        )
    {
        if (handler is not null)
        {
            _int32Arg.ItemIndex = itemIndex;
            _int32Arg.Result = defaultValue;
            handler (this, _int32Arg);
            return _int32Arg.Result;
        }

        return defaultValue;
    }

    private string GetStringItemProperty
        (
            int itemIndex,
            EventHandler<StringItemEventArgs>? handler,
            string defaultValue
        )
    {
        if (handler is not null)
        {
            _stringArg.ItemIndex = itemIndex;
            _stringArg.Result = defaultValue;
            handler (this, _stringArg);
            return _stringArg.Result;
        }

        return defaultValue;
    }

    private bool GetBoolItemProperty
        (
            int itemIndex,
            EventHandler<BoolItemEventArgs>? handler,
            bool defaultValue
        )
    {
        if (handler is not null)
        {
            _boolArg.ItemIndex = itemIndex;
            _boolArg.Result = defaultValue;
            handler (this, _boolArg);
            return _boolArg.Result;
        }

        return defaultValue;
    }

    private Image GetImageItemProperty
        (
            int itemIndex,
            EventHandler<ImageItemEventArgs>? handler,
            Image defaultValue
        )
    {
        if (handler is not null)
        {
            _imageArg.ItemIndex = itemIndex;
            _imageArg.Result = defaultValue;
            handler (this, _imageArg);
            return _imageArg.Result;
        }

        return defaultValue;
    }

    private Color GetColorItemProperty
        (
            int itemIndex,
            EventHandler<ColorItemEventArgs>? handler,
            Color defaultValue
        )
    {
        if (handler is not null)
        {
            _colorArg.ItemIndex = itemIndex;
            _colorArg.Result = defaultValue;
            handler (this, _colorArg);
            return _colorArg.Result;
        }

        return defaultValue;
    }

    #endregion Helpers
}
