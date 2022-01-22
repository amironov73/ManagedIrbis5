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
            ItemTextNeeded += (o, a) => a.Result = "Item " + a.ItemIndex;
            SelectedItemIndexes.Add (0);
        }
    }

    #endregion

    public void Build (int itemCount)
    {
        ItemCount = itemCount;
    }

    public void Rebuild()
    {
        Invalidate();
    }

    public void Build (ICollection? list)
    {
        if (list == null)
            ItemCount = 0;
        else
            ItemCount = list.Count;
    }

    #region Overrided methods

    /// <inheritdoc cref="FastListBase.CheckItem"/>
    public override bool CheckItem (int itemIndex)
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
                CheckedItemIndex.Add (itemIndex);
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
    /// Запрос высоты элемента.
    /// </summary>
    public event EventHandler<IntItemEventArgs>? ItemHeightNeeded;
    public event EventHandler<IntItemEventArgs>? ItemIndentNeeded;
    public event EventHandler<StringItemEventArgs>? ItemTextNeeded;
    public event EventHandler<BoolItemEventArgs>? ItemCheckStateNeeded;
    public event EventHandler<ImageItemEventArgs>? ItemIconNeeded;
    public event EventHandler<BoolItemEventArgs>? ItemCheckBoxVisibleNeeded;
    public event EventHandler<ColorItemEventArgs>? ItemBackColorNeeded;
    public event EventHandler<ColorItemEventArgs>? ItemForeColorNeeded;
    public event EventHandler<BoolItemEventArgs>? ItemExpandedNeeded;
    public event EventHandler<BoolItemEventArgs>? CanUnselectItemNeeded;
    public event EventHandler<BoolItemEventArgs>? CanSelectItemNeeded;
    public event EventHandler<BoolItemEventArgs>? CanUncheckItemNeeded;
    public event EventHandler<BoolItemEventArgs>? CanCheckItemNeeded;
    public event EventHandler<BoolItemEventArgs>? CanExpandItemNeeded;
    public event EventHandler<BoolItemEventArgs>? CanCollapseItemNeeded;
    public event EventHandler<BoolItemEventArgs>? CanEditItemNeeded;

    public event EventHandler<ItemCheckedStateChangedEventArgs>? ItemCheckedStateChanged;
    public event EventHandler<ItemExpandedStateChangedEventArgs>? ItemExpandedStateChanged;
    public event EventHandler<ItemSelectedStateChangedEventArgs>? ItemSelectedStateChanged;
    public event EventHandler<ItemTextPushedEventArgs>? ItemTextPushed;

    public event EventHandler<PaintItemContentEventArgs>? PaintItem;
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
    protected override int GetItemHeight (int itemIndex)
    {
        return GetIntItemProperty (itemIndex, ItemHeightNeeded, ItemHeightDefault);
    }

    public override int GetItemIndent (int itemIndex)
    {
        return GetIntItemProperty (itemIndex, ItemIndentNeeded, ItemIndentDefault);
    }

    protected override string GetItemText (int itemIndex)
    {
        return GetStringItemProperty (itemIndex, ItemTextNeeded, string.Empty);
    }

    protected override bool GetItemChecked (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, ItemCheckStateNeeded, CheckedItemIndex.Contains (itemIndex));
    }

    protected override Image GetItemIcon (int itemIndex)
    {
        return GetImageItemProperty (itemIndex, ItemIconNeeded, ImageDefaultIcon);
    }

    protected override bool GetItemCheckBoxVisible (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, ItemCheckBoxVisibleNeeded, ShowCheckBoxes);
    }

    protected override Color GetItemBackColor (int itemIndex)
    {
        return GetColorItemProperty (itemIndex, ItemBackColorNeeded, Color.Empty);
    }

    protected override Color GetItemForeColor (int itemIndex)
    {
        return GetColorItemProperty (itemIndex, ItemForeColorNeeded, ForeColor);
    }

    protected override bool GetItemExpanded (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, ItemExpandedNeeded, false);
    }

    protected override bool CanUnselectItem (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, CanUnselectItemNeeded, true);
    }

    protected override bool CanSelectItem (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, CanSelectItemNeeded, AllowSelectItems);
    }

    protected override bool CanUncheckItem (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, CanUncheckItemNeeded, true);
    }

    protected override bool CanCheckItem (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, CanCheckItemNeeded, true);
    }

    protected override bool CanExpandItem (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, CanExpandItemNeeded, true);
    }

    protected override bool CanCollapseItem (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, CanCollapseItemNeeded, true);
    }

    protected override bool CanEditItem (int itemIndex)
    {
        return GetBoolItemProperty (itemIndex, CanEditItemNeeded, true);
    }

    protected override void OnItemChecked (int itemIndex)
    {
        if (ItemCheckedStateChanged != null)
            ItemCheckedStateChanged (this,
                new ItemCheckedStateChangedEventArgs { ItemIndex = itemIndex, Checked = true });

        base.OnItemChecked (itemIndex);
    }

    protected override void OnItemUnchecked (int itemIndex)
    {
        if (ItemCheckedStateChanged != null)
            ItemCheckedStateChanged (this,
                new ItemCheckedStateChangedEventArgs { ItemIndex = itemIndex, Checked = false });

        base.OnItemUnchecked (itemIndex);
    }

    protected override void OnItemExpanded (int itemIndex)
    {
        if (ItemExpandedStateChanged != null)
            ItemExpandedStateChanged (this,
                new ItemExpandedStateChangedEventArgs { ItemIndex = itemIndex, Expanded = true });

        base.OnItemExpanded (itemIndex);
    }

    protected override void OnItemCollapsed (int itemIndex)
    {
        if (ItemExpandedStateChanged != null)
            ItemExpandedStateChanged (this,
                new ItemExpandedStateChangedEventArgs { ItemIndex = itemIndex, Expanded = false });

        base.OnItemCollapsed (itemIndex);
    }

    protected override void OnItemSelected (int itemIndex)
    {
        if (ItemSelectedStateChanged != null)
            ItemSelectedStateChanged (this,
                new ItemSelectedStateChangedEventArgs { ItemIndex = itemIndex, Selected = true });

        base.OnItemSelected (itemIndex);
    }

    protected override void OnItemTextPushed (int itemIndex, string text)
    {
        if (ItemTextPushed != null)
            ItemTextPushed (this, new ItemTextPushedEventArgs { ItemIndex = itemIndex, Text = text });

        base.OnItemTextPushed (itemIndex, text);
    }

    protected override void OnItemUnselected (int itemIndex)
    {
        if (ItemSelectedStateChanged != null)
            ItemSelectedStateChanged (this,
                new ItemSelectedStateChangedEventArgs { ItemIndex = itemIndex, Selected = false });

        base.OnItemUnselected (itemIndex);
    }

    protected override void OnItemDrag (HashSet<int> itemIndex)
    {
        if (ItemDrag != null)
            ItemDrag (this, new ItemDragEventArgs { ItemIndex = itemIndex });
        else
            DoDragDrop (itemIndex, DragDropEffects.Copy);

        base.OnItemDrag (itemIndex);
    }

    protected override void DrawItem (Graphics gr, VisibleItemInfo info)
    {
        if (PaintItem != null)
            PaintItem (this, new PaintItemContentEventArgs { Graphics = gr, Info = info });
        else
            base.DrawItem (gr, info);
    }

    protected override void OnDragOverItem (DragOverItemEventArgs eventArgs)
    {
        base.OnDragOverItem (eventArgs);

        if (DragOverItem != null)
            DragOverItem (this, eventArgs);
    }

    protected override void OnDropOverItem (DragOverItemEventArgs eventArgs)
    {
        if (DropOverItem != null)
            DropOverItem (this, eventArgs);

        base.OnDropOverItem (eventArgs);
    }

    protected override void OnScrollbarsUpdated()
    {
        if (ScrollbarsUpdated != null)
            ScrollbarsUpdated (this, EventArgs.Empty);
        base.OnScrollbarsUpdated();
    }

    #endregion

    #region Event Helpers

    private IntItemEventArgs intArg = new IntItemEventArgs();
    private BoolItemEventArgs boolArg = new BoolItemEventArgs();
    private StringItemEventArgs stringArg = new StringItemEventArgs();
    private ImageItemEventArgs imageArg = new ImageItemEventArgs();
    private ColorItemEventArgs colorArg = new ColorItemEventArgs();

    int GetIntItemProperty (int itemIndex, EventHandler<IntItemEventArgs>? handler, int defaultValue)
    {
        if (handler is not null)
        {
            intArg.ItemIndex = itemIndex;
            intArg.Result = defaultValue;
            handler (this, intArg);
            return intArg.Result;
        }

        return defaultValue;
    }

    string GetStringItemProperty (int itemIndex, EventHandler<StringItemEventArgs>? handler, string defaultValue)
    {
        if (handler is not null)
        {
            stringArg.ItemIndex = itemIndex;
            stringArg.Result = defaultValue;
            handler (this, stringArg);
            return stringArg.Result;
        }

        return defaultValue;
    }

    bool GetBoolItemProperty (int itemIndex, EventHandler<BoolItemEventArgs>? handler, bool defaultValue)
    {
        if (handler is not null)
        {
            boolArg.ItemIndex = itemIndex;
            boolArg.Result = defaultValue;
            handler (this, boolArg);
            return boolArg.Result;
        }

        return defaultValue;
    }

    Image GetImageItemProperty (int itemIndex, EventHandler<ImageItemEventArgs>? handler, Image defaultValue)
    {
        if (handler is not null)
        {
            imageArg.ItemIndex = itemIndex;
            imageArg.Result = defaultValue;
            handler (this, imageArg);
            return imageArg.Result;
        }

        return defaultValue;
    }

    Color GetColorItemProperty (int itemIndex, EventHandler<ColorItemEventArgs>? handler, Color defaultValue)
    {
        if (handler is not null)
        {
            colorArg.ItemIndex = itemIndex;
            colorArg.Result = defaultValue;
            handler (this, colorArg);
            return colorArg.Result;
        }

        return defaultValue;
    }

    #endregion Helpers
}

public class GenericItemResultEventArgs<T> : EventArgs
{
    public int ItemIndex { get; internal set; }
    public T Result;
}

public class IntItemEventArgs : GenericItemResultEventArgs<int>
{
}

public class StringItemEventArgs : GenericItemResultEventArgs<string>
{
}

public class ImageItemEventArgs : GenericItemResultEventArgs<Image>
{
}

public class ColorItemEventArgs : GenericItemResultEventArgs<Color>
{
}

public class BoolItemEventArgs : GenericItemResultEventArgs<bool>
{
}

public class ItemCheckedStateChangedEventArgs : EventArgs
{
    public int ItemIndex;
    public bool Checked;
}

public class ItemExpandedStateChangedEventArgs : EventArgs
{
    public int ItemIndex;
    public bool Expanded;
}

public class ItemSelectedStateChangedEventArgs : EventArgs
{
    public int ItemIndex;
    public bool Selected;
}

public class PaintItemContentEventArgs : EventArgs
{
    public Graphics Graphics;
    public FastListBase.VisibleItemInfo Info;
}

public class ItemDragEventArgs : EventArgs
{
    public HashSet<int> ItemIndex;
}

public class ItemTextPushedEventArgs : EventArgs
{
    public int ItemIndex;
    public string Text;
}
