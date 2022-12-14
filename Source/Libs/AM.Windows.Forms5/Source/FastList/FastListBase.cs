// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* FastListBase.cs -- базовый класс для списка и дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Базовый класс для списка и дерева.
/// </summary>
[ToolboxItem (false)]
public class FastListBase
    : UserControl
{
    #region Properties

    /// <summary>
    /// Индексы выбранных элементов.
    /// </summary>
    [Browsable (false)]
    public HashSet<int> SelectedItemIndexes { get; }

    /// <summary>
    /// Индекс первого выбранного элемента.
    /// </summary>
    [Browsable (false)]
    public int SelectedItemIndex => SelectedItemIndexes.FirstOrDefault (-1);

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public HashSet<int> CheckedItemIndex { get; }

    /// <summary>
    /// Показывать тултипы?
    /// </summary>
    [DefaultValue (true)]
    public bool ShowToolTips { get; set; }

    /// <summary>
    /// Высота элемента по умолчанию.
    /// </summary>
    [DefaultValue (17)]
    public virtual int ItemHeightDefault { get; set; }

    /// <summary>
    /// Выравнивание элемента по горизонтали по умолчанию.
    /// </summary>
    [DefaultValue (StringAlignment.Near)]
    public virtual StringAlignment ItemLineAlignmentDefault { get; set; }

    /// <summary>
    /// Отступ элемента по умолчанию.
    /// </summary>
    [DefaultValue (10)]
    public virtual int ItemIndentDefault { get; set; }

    /// <summary>
    /// Размер иконки в пикселах.
    /// </summary>
    [DefaultValue (typeof (Size), "16, 16")]
    public Size IconSize { get; set; }

    /// <summary>
    /// В настоящее время происходит редактирование элемента?
    /// </summary>
    [Browsable (false)]
    public bool IsEditMode { get; set; }

    /// <summary>
    /// Только для чтения?
    /// </summary>
    [Browsable (true), DefaultValue (false)]
    public bool Readonly { get; set; }

    /// <summary>
    /// Показывать иконки?
    /// </summary>
    [DefaultValue (false)]
    public bool ShowIcons { get; set; }

    /// <summary>
    /// Показывать чекбоксы?
    /// </summary>
    [DefaultValue (false)]
    public bool ShowCheckBoxes { get; set; }

    /// <summary>
    /// Показывать чекбоксы, позволяющие раскрыть узел?
    /// </summary>
    [DefaultValue (false)]
    public bool ShowExpandBoxes { get; set; }

    /// <summary>
    /// Показывать пустые боксы для раскрытия?
    /// </summary>
    [DefaultValue (true)]
    public bool ShowEmptyExpandBoxes { get; set; }

    /// <summary>
    /// Картинка для отмеченного чекбокса.
    /// </summary>
    public virtual Image? ImageCheckBoxOn { get; set; }

    /// <summary>
    /// Картинка для неотмеченного текстбокса.
    /// </summary>
    public virtual Image? ImageCheckBoxOff { get; set; }

    /// <summary>
    /// Картинка для схлопнутого региона.
    /// </summary>
    public virtual Image? ImageCollapse { get; set; }

    /// <summary>
    /// Картинка для развернутого региона.
    /// </summary>
    public virtual Image? ImageExpand { get; set; }

    /// <summary>
    /// Картинка для пустого развернутого региона.
    /// </summary>
    public virtual Image? ImageEmptyExpand { get; set; }

    /// <summary>
    /// Картинка по умолчанию.
    /// </summary>
    public virtual Image? ImageDefaultIcon { get; set; }

    /// <summary>
    /// Цвет выделенных элементов.
    /// </summary>
    [DefaultValue (typeof (Color), "33, 53, 80")]
    public Color SelectionColor { get; set; }

    /// <summary>
    /// Уровень непрозрачности.
    /// </summary>
    [DefaultValue (100)]
    public int SelectionColorOpaque { get; set; }

    /// <summary>
    /// Разрешить множественное выделение?
    /// </summary>
    [DefaultValue (false)]
    public bool MultiSelect { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (2)]
    public int ItemInterval { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (false)]
    public bool FullItemSelect { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (false)]
    public bool AllowDragItems { get; set; }

    /// <summary>
    /// Разрешено выбирать элементы?
    /// </summary>
    [DefaultValue (true)]
    public bool AllowSelectItems { get; set; }

    /// <summary>
    /// "Горячее отслеживание" элементов разрешено?
    /// </summary>
    [DefaultValue (false)]
    public bool HotTracking { get; set; }

    /// <summary>
    /// Цвет "горячих" элементов.
    /// </summary>
    [DefaultValue (typeof (Color), "255, 192, 128")]
    public Color HotTrackingColor { get; set; }

    /// <summary>
    /// Показывать скроллбар?
    /// </summary>
    [Browsable (true)]
    [DefaultValue (true)]
    [Description ("Scollbar visibility.")]
    public bool ShowScrollBar
    {
        get => _showScrollBar;
        set
        {
            if (value == _showScrollBar)
            {
                return;
            }

            _showScrollBar = value;
            buildNeeded = true;
            Invalidate();
        }
    }

    /// <summary>
    /// Количество элементов.
    /// </summary>
    public virtual int ItemCount
    {
        get => _itemCount;
        set
        {
            _itemCount = value;
            if (!IsHandleCreated)
            {
                BuildNeeded();
            }
            else
            {
                Build();
            }
        }
    }

    /// <inheritdoc cref="ScrollableControl.AutoScroll"/>
    [Browsable (false)]
    public override bool AutoScroll => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FastListBase()
    {
        SetStyle
            (
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable,
                true
            );

        _toolTip = new ToolTip() { UseAnimation = false };

        SelectedItemIndexes = new HashSet<int>();
        CheckedItemIndex = new HashSet<int>();
        InitPropertiesWithDefaultValues();
    }

    #endregion

    #region Private members

    /// <summary>
    /// Инициализация свойств значениями по умолчанию.
    /// </summary>
    protected virtual void InitPropertiesWithDefaultValues()
    {
        IconSize = new Size (16, 16);
        ItemHeightDefault = 17;
        VerticalScroll.SmallChange = ItemHeightDefault;

        SelectionColor = Color.FromArgb (33, 53, 80);
        SelectionColorOpaque = 100;
        ItemInterval = 2;
        BackColor = SystemColors.Window;
        ItemIndentDefault = 10;
        ShowToolTips = true;
        AllowSelectItems = true;
        ShowEmptyExpandBoxes = true;
        HotTrackingColor = Color.FromArgb (255, 192, 128);
        _showScrollBar = true;
    }

    private readonly List<int> _yOfItems = new ();
    private readonly ToolTip _toolTip;
    private int _itemCount;
    private bool _showScrollBar;
    private Size _localAutoScrollMinSize;

    /// <summary>
    ///
    /// </summary>
    protected int currentHotTrackingIndex;

    #endregion

    #region Drag&Drop

    private DragOverItemEventArgs? _lastDragAndDropEffect;

    /// <inheritdoc cref="Control.OnDragOver"/>
    protected override void OnDragOver
        (
            DragEventArgs eventArgs
        )
    {
        var p = new Point (eventArgs.X, eventArgs.Y);
        p = PointToClient (p);

        var itemIndex = YToIndexAround (p.Y + VerticalScroll.Value);
        var rect = CalcItemRect (itemIndex);

        var textRect = rect;
        if (visibleItemInfos.ContainsKey (itemIndex))
        {
            var info = visibleItemInfos[itemIndex];
            textRect = new Rectangle (info.X_Text, rect.Y, info.X_EndText - info.X_Text + 1, rect.Height);
        }

        var ea = new DragOverItemEventArgs (eventArgs.Data, eventArgs.KeyState, p.X, p.Y, eventArgs.AllowedEffect, eventArgs.Effect, rect, textRect)
            { ItemIndex = itemIndex };

        OnDragOverItem (ea);

        _lastDragAndDropEffect = ea.Effect != DragDropEffects.None ? ea : null;

        eventArgs.Effect = ea.Effect;

        //scroll
        if (ea.ItemIndex >= 0 && ea.ItemIndex < ItemCount && itemIndex != ea.ItemIndex)
        {
            rect = CalcItemRect (ea.ItemIndex);
            rect.Inflate (0, 2);
            rect.Offset (HorizontalScroll.Value, VerticalScroll.Value);
            ScrollToRectangle (rect);
        }
        else
        {
            if (p.Y <= Padding.Top + ItemHeightDefault / 2)
            {
                ScrollUp();
            }
            else if (p.Y >= ClientSize.Height - Padding.Bottom - +ItemHeightDefault / 2)
            {
                ScrollDown();
            }
        }

        Invalidate();

        //base.OnDragOver(e);
    }

    /// <summary>
    /// Мышь тащится над элементом списка.
    /// </summary>
    protected virtual void OnDragOverItem
        (
            DragOverItemEventArgs eventArgs
        )
    {
        eventArgs.InsertEffect = eventArgs.Y < eventArgs.ItemRect.Y + eventArgs.ItemRect.Height / 2
            ? InsertEffect.InsertBefore
            : InsertEffect.InsertAfter;
    }

    /// <inheritdoc cref="Control.OnDragDrop"/>
    protected override void OnDragDrop (DragEventArgs e)
    {
        base.OnDragDrop (e);

        if (_lastDragAndDropEffect != null)
        {
            OnDropOverItem (_lastDragAndDropEffect);
        }

        _lastDragAndDropEffect = null;
        Invalidate();
    }

    /// <summary>
    /// Мышь отпустили над элементом списка.
    /// </summary>
    protected virtual void OnDropOverItem
        (
            DragOverItemEventArgs eventArgs
        )
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    protected override void OnDragLeave (EventArgs e)
    {
        base.OnDragLeave (e);
        _lastDragAndDropEffect = null;
        Invalidate();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Rectangle CalcItemRect (int index)
    {
        Rectangle res;

        var i = index;
        if (i >= ItemCount)
        {
            i = ItemCount - 1;
        }

        if (i < 0)
        {
            res = Rectangle.FromLTRB (ClientRectangle.Left + Padding.Left, ClientRectangle.Top + Padding.Top - 2,
                ClientRectangle.Right - Padding.Right, ClientRectangle.Top + Padding.Top - 1);
        }
        else
        {
            var y = GetItemY (i);
            var h = GetItemY (i + 1) - y;

            res = Rectangle.FromLTRB (ClientRectangle.Left + Padding.Left, y, ClientRectangle.Right - Padding.Right,
                y + h);

            if (index >= _itemCount)
            {
                res.Offset (0, (index - _itemCount + 1) * ItemHeightDefault);
            }
        }

        res.Offset (-HorizontalScroll.Value, -VerticalScroll.Value);

        return res;
    }

    #endregion Drop

    #region Keyboard

    /// <summary>
    ///
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool IsInputKey (Keys keyData)
    {
        if (!IsEditMode)
        {
            switch (keyData)
            {
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Down:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                case Keys.Enter:
                case Keys.Space:
                case Keys.A | Keys.Control:
                    return true;

                default:
                    return false;
            }
        }
        else
        {
            return base.IsInputKey (keyData);
        }
    }

    protected override void OnKeyDown (KeyEventArgs e)
    {
        base.OnKeyDown (e);

        if (e.Handled)
        {
            return;
        }

        CancelDelayedAction();

        switch (e.KeyCode)
        {
            case Keys.Up:
                if (e.Control)
                {
                    ScrollUp();
                }
                else
                {
                    SelectPrev();
                }

                break;

            case Keys.Down:
                if (e.Control)
                {
                    ScrollDown();
                }
                else
                {
                    SelectNext();
                }

                break;

            case Keys.PageUp:
                if (e.Control)
                {
                    ScrollPageUp();
                }
                else if (SelectedItemIndexes.Count > 0)
                {
                    var i = SelectedItemIndexes.First();
                    var y = GetItemY (i) - ClientRectMinusPaddings.Height;
                    i = YToIndex (y) + 1;
                    SelectItem (Math.Max (0, Math.Min (ItemCount - 1, i)));
                }

                break;

            case Keys.PageDown:
                if (e.Control)
                {
                    ScrollPageDown();
                }
                else if (SelectedItemIndexes.Count > 0)
                {
                    var i = SelectedItemIndexes.First();
                    var y = GetItemY (i) + ClientRectMinusPaddings.Height;
                    i = YToIndex (y);
                    SelectItem (i < 0 ? ItemCount - 1 : i);
                }

                break;

            case Keys.Home:
                if (e.Control)
                {
                    ScrollToItem (0);
                }
                else
                {
                    SelectItem (0);
                }

                break;

            case Keys.End:
                if (e.Control)
                {
                    ScrollToItem (ItemCount - 1);
                }
                else
                {
                    SelectItem (ItemCount - 1);
                }

                break;

            case Keys.Enter:
            case Keys.Space:
                if (ShowCheckBoxes)
                {
                    if (SelectedItemIndexes.Count > 0)
                    {
                        var val = GetItemChecked (SelectedItemIndexes.First());
                        if (val)
                        {
                            UncheckSelected();
                        }
                        else
                        {
                            CheckSelected();
                        }
                    }
                }
                else if (ShowExpandBoxes)
                {
                    if (SelectedItemIndexes.Count > 0)
                    {
                        var itemIndex = SelectedItemIndexes.First();
                        if (GetItemExpanded (itemIndex))
                        {
                            CollapseItem (itemIndex);
                        }
                        else
                        {
                            ExpandItem (itemIndex);
                        }
                    }
                }

                break;

            case Keys.A:
                if (e.Control)
                {
                    SelectAll();
                }

                break;
        }
    }

    #endregion

    #region Dealyed Actions

    private System.Threading.Timer? delayedActionTimer;

    /// <summary>
    ///
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delayInterval"></param>
    protected void CreateDelayedAction (Action action, int delayInterval)
    {
        CancelDelayedAction();
        delayedActionTimer =
            new System.Threading.Timer ((_) => this.Invoke (action), null, delayInterval, Timeout.Infinite);
    }

    /// <summary>
    ///
    /// </summary>
    protected void CancelDelayedAction()
    {
        if (delayedActionTimer != null)
        {
            delayedActionTimer.Change (Timeout.Infinite, Timeout.Infinite);
            delayedActionTimer.Dispose();
        }

        delayedActionTimer = null;
    }

    #endregion

    #region Mouse

    private int startDiapasonSelectedItemIndex;
    private bool mouseCanSelectArea;
    private Point startMouseSelectArea;
    private Rectangle mouseSelectArea;
    private Point lastMouseClick;

    protected override void OnMouseDown (MouseEventArgs e)
    {
        base.OnMouseDown (e);

        Focus();
        mouseCanSelectArea = false;
        mouseSelectArea = Rectangle.Empty;
        lastMouseClick = e.Location;
        CancelDelayedAction();

        var item = PointToItemInfo (e.Location);

        if (item == null)
        {
            return;
        }

        if (e.Button == MouseButtons.Left && item.X_Text <= e.Location.X)
        {
            if (SelectedItemIndexes.Count == 1 && SelectedItemIndexes.Contains (item.ItemIndex))
            {
                if (!Readonly && CanEditItem (item.ItemIndex))
                {
                    CreateDelayedAction (() => OnStartEdit (item.ItemIndex), 500);
                }
            }
        }

        if (AllowSelectItems)
        {
            if (e.Button == MouseButtons.Left && item.X_Icon <= e.Location.X)
            {
                //Select
                if (MultiSelect)
                {
                    startMouseSelectArea = e.Location;
                    startMouseSelectArea.Offset (HorizontalScroll.Value, VerticalScroll.Value);
                    mouseCanSelectArea = item.X_EndText < e.Location.X || !AllowDragItems;
                }

                if (!AllowDragItems || !MultiSelect)
                {
                    OnMouseSelectItem (e, item);
                }
            }
        }

        if (ShowCheckBoxes && e.Button == MouseButtons.Left)
        {
            if ((item.X_CheckBox <= e.Location.X && item.X_Icon > e.Location.X) || (!AllowSelectItems))
            {
                //Checkbox
                OnCheckboxClick (item);
                Invalidate();
            }
        }

        if (ShowExpandBoxes)
        {
            if (e.Button == MouseButtons.Left && item.X_ExpandBox <= e.Location.X && item.X_CheckBox > e.Location.X)
            {
                //Expand
                OnExpandBoxClick (item);
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Выделение элемента мышью.
    /// </summary>
    protected virtual void OnMouseSelectItem
        (
            MouseEventArgs eventArgs,
            VisibleItemInfo item
        )
    {
        if (MultiSelect)
        {
            startMouseSelectArea = eventArgs.Location;
            startMouseSelectArea.Offset (HorizontalScroll.Value, VerticalScroll.Value);
            mouseCanSelectArea = item.X_EndText < eventArgs.Location.X || !AllowDragItems;

            if (ModifierKeys == Keys.Control)
            {
                if (SelectedItemIndexes.Contains (item.ItemIndex) && SelectedItemIndexes.Count > 1)
                {
                    UnselectItem (item.ItemIndex);
                }
                else
                {
                    SelectItem (item.ItemIndex, false);
                }

                startDiapasonSelectedItemIndex = -1;
            }
            else if (Control.ModifierKeys == Keys.Shift)
            {
                if (SelectedItemIndexes.Count == 1)
                {
                    startDiapasonSelectedItemIndex = SelectedItemIndexes.First();
                }

                if (startDiapasonSelectedItemIndex >= 0)
                {
                    SelectItems (Math.Min (startDiapasonSelectedItemIndex, item.ItemIndex),
                        Math.Max (startDiapasonSelectedItemIndex, item.ItemIndex));
                }
            }
        }

        if (!MultiSelect || ModifierKeys == Keys.None)
        {
            if (!SelectedItemIndexes.Contains (item.ItemIndex) || SelectedItemIndexes.Count > 1)
            {
                SelectItem (item.ItemIndex);
            }
        }

        Invalidate();
    }

    /// <inheritdoc cref="Control.OnMouseMove"/>
    protected override void OnMouseMove
        (
            MouseEventArgs e
        )
    {
        base.OnMouseMove (e);

        if (e.Button != MouseButtons.None)
        {
            CancelDelayedAction();
        }

        if (e.Button == MouseButtons.Left && mouseCanSelectArea)
        {
            if (Math.Abs (e.Location.X - startMouseSelectArea.X) > 0)
            {
                var pos = e.Location;
                pos.Offset (HorizontalScroll.Value, VerticalScroll.Value);
                mouseSelectArea = new Rectangle (Math.Min (startMouseSelectArea.X, pos.X),
                    Math.Min (startMouseSelectArea.Y, pos.Y), Math.Abs (startMouseSelectArea.X - pos.X),
                    Math.Abs (startMouseSelectArea.Y - pos.Y));

                var i1 = YToIndex (startMouseSelectArea.Y);
                var i2 = YToIndex (pos.Y);
                if (i1 >= 0 && i2 >= 0)
                {
                    SelectItems (Math.Min (i1, i2), Math.Max (i1, i2));
                }

                if (e.Location.Y <= Padding.Top + ItemHeightDefault / 2)
                {
                    ScrollUp();
                }
                else if (e.Location.Y >= ClientSize.Height - Padding.Bottom - +ItemHeightDefault / 2)
                {
                    ScrollDown();
                }

                Invalidate();
            }
            else
            {
                mouseSelectArea = Rectangle.Empty;
            }
        }
        else if (e.Button == MouseButtons.Left && AllowDragItems &&
                 (Math.Abs (lastMouseClick.X - e.Location.X) > 2 || Math.Abs (lastMouseClick.Y - e.Location.Y) > 2))
        {
            var p = PointToClient (MousePosition);
            var info = PointToItemInfo (p);
            if (info != null)
            {
                if (!SelectedItemIndexes.Contains (info.ItemIndex))
                {
                    SelectItem (info.ItemIndex);
                }

                OnItemDrag (new HashSet<int> (SelectedItemIndexes));
            }
        }
        else if (e.Button == MouseButtons.None)
        {
            var p = PointToClient (MousePosition);
            var info = PointToItemInfo (p);

            if (HotTracking)
            {
                var i = -1;
                if (info != null)
                {
                    i = info.ItemIndex;
                }

                if (currentHotTrackingIndex != i)
                {
                    currentHotTrackingIndex = i;
                    Invalidate();
                }
            }

            if (info != null && info.X_EndText == info.X_End)
            {
                if ((string?)_toolTip.Tag != info.Text && ShowToolTips)
                {
                    _toolTip.Show (info.Text, this, info.X_Text - 3, info.Y - 2, 2000);
                }

                _toolTip.Tag = info.Text;
            }
            else
            {
                _toolTip.Tag = null;
                _toolTip.Hide (this);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected virtual void OnItemDrag (HashSet<int> itemIndex)
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="Control.OnMouseUp"/>
    protected override void OnMouseUp
        (
            MouseEventArgs e
        )
    {
        base.OnMouseUp (e);

        var item = PointToItemInfo (e.Location);

        if (item != null)
        {
            if (AllowSelectItems)
            {
                if (e.Button == MouseButtons.Left && item.X_Icon <= e.Location.X)
                {
                    if (AllowDragItems && MultiSelect && mouseSelectArea == Rectangle.Empty)
                    {
                        OnMouseSelectItem (e, item);
                    }
                }
            }
        }

        mouseCanSelectArea = false;

        Invalidate();
    }

    /// <inheritdoc cref="Control.OnMouseDoubleClick"/>
    protected override void OnMouseDoubleClick
        (
            MouseEventArgs e
        )
    {
        base.OnMouseDoubleClick (e);

        CancelDelayedAction();

        var item = PointToItemInfo (e.Location);

        if (item != null)
        {
            if (e.Button == MouseButtons.Left && item.X_Icon <= e.Location.X)
            {
                if (GetItemExpanded (item.ItemIndex))
                {
                    CollapseItem (item.ItemIndex);
                }
                else
                {
                    ExpandItem (item.ItemIndex);
                }
            }
        }
    }

    #endregion mouse

    #region Check, Expand

    /// <summary>
    ///
    /// </summary>
    /// <param name="info"></param>
    protected virtual void OnExpandBoxClick (VisibleItemInfo info)
    {
        if (info.Expanded)
        {
            CollapseItem (info.ItemIndex);
        }
        else
        {
            ExpandItem (info.ItemIndex);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public virtual bool CollapseItem (int itemIndex)
    {
        Invalidate();

        if (CanCollapseItem (itemIndex))
        {
            OnItemCollapsed (itemIndex);
            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public virtual bool ExpandItem (int itemIndex)
    {
        Invalidate();

        if (CanExpandItem (itemIndex))
        {
            OnItemExpanded (itemIndex);
            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected virtual void OnItemCollapsed (int itemIndex)
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected virtual void OnItemExpanded (int itemIndex)
    {
        // пустое тело метода
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="info"></param>
    protected virtual void OnCheckboxClick (VisibleItemInfo info)
    {
        if (GetItemChecked (info.ItemIndex))
        {
            UncheckItem (info.ItemIndex);
        }
        else
        {
            CheckItem (info.ItemIndex);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public virtual bool CheckItem (int itemIndex)
    {
        if (GetItemChecked (itemIndex))
        {
            return true;
        }

        Invalidate();

        if (CanCheckItem (itemIndex))
        {
            CheckedItemIndex.Add (itemIndex);
            OnItemChecked (itemIndex);
            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckAll()
    {
        var result = true;
        for (var i = 0; i < ItemCount; i++)
        {
            result &= CheckItem (i);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public virtual bool UncheckItem (int itemIndex)
    {
        if (!GetItemChecked (itemIndex))
        {
            return true;
        }

        Invalidate();

        if (CanUncheckItem (itemIndex))
        {
            CheckedItemIndex.Remove (itemIndex);
            OnItemUnchecked (itemIndex);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Снятие отметки со всех элементов.
    /// </summary>
    /// <returns>Признак успешного выполнения операции.</returns>
    public virtual bool UncheckAll()
    {
        foreach (var i in CheckedItemIndex)
        {
            if (!CanUncheckItem (i))
            {
                return false;
            }
        }

        var list = new List<int> (CheckedItemIndex);

        CheckedItemIndex.Clear();

        foreach (var i in list)
        {
            OnItemUnchecked (i);
        }

        Invalidate();

        return true;
    }

    /// <summary>
    /// Элемент по указанному индексу был отмечен.
    /// </summary>
    protected virtual void OnItemChecked
        (
            int itemIndex
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// С элемента по указанному индексу была снята отметка.
    /// </summary>
    protected virtual void OnItemUnchecked
        (
            int itemIndex
        )
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void CheckSelected()
    {
        foreach (var i in SelectedItemIndexes)
        {
            CheckItem (i);
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void UncheckSelected()
    {
        foreach (var i in SelectedItemIndexes)
        {
            UncheckItem (i);
        }
    }

    #endregion Check, Expand

    #region Selection

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public virtual bool UnselectItem (int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= ItemCount)
        {
            return false;
        }

        if (!SelectedItemIndexes.Contains (itemIndex))
        {
            return true;
        }

        if (!CanUnselectItem (itemIndex))
        {
            return false;
        }

        SelectedItemIndexes.Remove (itemIndex);
        OnItemUnselected (itemIndex);

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual bool UnselectAll()
    {
        foreach (var i in SelectedItemIndexes)
        {
            if (!CanUnselectItem (i))
            {
                return false;
            }
        }

        var list = new List<int> (SelectedItemIndexes);

        SelectedItemIndexes.Clear();

        foreach (var i in list)
        {
            OnItemUnselected (i);
        }

        Invalidate();

        return true;
    }

    public virtual bool SelectItem (int itemIndex, bool unselectOtherItems = true)
    {
        if (itemIndex < 0 || itemIndex >= ItemCount)
        {
            return false;
        }

        if (!CanSelectItem (itemIndex))
        {
            return false;
        }

        var contains = SelectedItemIndexes.Contains (itemIndex);

        if (unselectOtherItems)
        {
            foreach (var i in SelectedItemIndexes)
            {
                if (i != itemIndex)
                {
                    if (!CanUnselectItem (i))
                    {
                        return false;
                    }
                }
            }

            var list = new List<int> (SelectedItemIndexes);

            //SelectedItemIndexes.Clear();

            foreach (var i in list)
            {
                if (i != itemIndex)
                {
                    SelectedItemIndexes.Remove (i);
                    OnItemUnselected (i);
                }
            }
        }

        SelectedItemIndexes.Add (itemIndex);

        if (!contains)
        {
            OnItemSelected (itemIndex);
        }

        ScrollToItem (itemIndex);

        return true;
    }

    public virtual bool SelectItems (int from, int to)
    {
        foreach (var i in SelectedItemIndexes)
        {
            if (i < from || i > to)
            {
                if (!CanUnselectItem (i))
                {
                    return false;
                }
            }
        }

        var list = new List<int> (SelectedItemIndexes);

        //SelectedItemIndexes.RemoveWhere(i=> i < from | i > to);

        foreach (var i in list)
        {
            if (i < from || i > to)
            {
                SelectedItemIndexes.Remove (i);
                OnItemUnselected (i);
            }
        }

        for (var i = from; i <= to; i++)
        {
            if (!SelectedItemIndexes.Contains (i))
            {
                if (CanSelectItem (i))
                {
                    SelectedItemIndexes.Add (i);
                    OnItemSelected (i);
                }
            }
        }

        Invalidate();

        return SelectedItemIndexes.Count > 0;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual bool SelectAll()
    {
        return SelectItems (0, ItemCount - 1);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="unselectOtherItems"></param>
    /// <returns></returns>
    public bool SelectNext (bool unselectOtherItems = true)
    {
        if (SelectedItemIndexes.Count == 0)
        {
            return false;
        }

        var index = SelectedItemIndexes.Max() + 1;
        if (index >= ItemCount)
        {
            return false;
        }

        var res = SelectItem (index, unselectOtherItems);
        if (res)
        {
            ScrollToItem (index);
        }

        return res;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="unselectOtherItems"></param>
    /// <returns></returns>
    public bool SelectPrev (bool unselectOtherItems = true)
    {
        if (SelectedItemIndexes.Count == 0)
        {
            return false;
        }

        var index = SelectedItemIndexes.Min() - 1;
        if (index < 0)
        {
            return false;
        }

        var res = SelectItem (index, unselectOtherItems);
        if (res)
        {
            ScrollToItem (index);
        }

        return res;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected virtual void OnItemSelected (int itemIndex)
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    protected virtual void OnItemUnselected (int itemIndex)
    {
        // пустое тело метода
    }

    #endregion

    #region Paint

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint (PaintEventArgs e)
    {
        //was build request
        if (buildNeeded)
        {
            Build();
            buildNeeded = false;
        }

        //
        e.Graphics.SetClip (ClientRectMinusPaddings);
        DrawItems (e.Graphics);

        if (_lastDragAndDropEffect == null)
        {
            DrawMouseSelectedArea (e.Graphics);
        }
        else
        {
            DrawDragOverInsertEffect (e.Graphics, _lastDragAndDropEffect);
        }

        base.OnPaint (e);

        if (!Enabled)
        {
            e.Graphics.SetClip (ClientRectangle);
            var color = Color.FromArgb (50, (BackColor.R + 127) >> 1, (BackColor.G + 127) >> 1,
                (BackColor.B + 127) >> 1);
            using (var brush = new SolidBrush (color))
            {
                e.Graphics.FillRectangle (brush, ClientRectangle);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="eventArgs"></param>
    protected virtual void DrawDragOverInsertEffect
        (
            Graphics graphics,
            DragOverItemEventArgs eventArgs
        )
    {
        var c1 = Color.FromArgb (SelectionColor.A == 255 ? SelectionColorOpaque : SelectionColor.A, SelectionColor);

        if (!visibleItemInfos.ContainsKey (eventArgs.ItemIndex))
        {
            return;
        }

        var info = visibleItemInfos[eventArgs.ItemIndex];
        var rect = new Rectangle (info.X_ExpandBox, info.Y, 1000, info.Height);

        switch (eventArgs.InsertEffect)
        {
            case InsertEffect.Replace:
                using (var brush = new SolidBrush (c1))
                {
                    graphics.FillRectangle (brush, rect);
                }

                break;

            case InsertEffect.InsertBefore:
                if (eventArgs.ItemIndex <= 0)
                {
                    rect.Offset (0, 2);
                }

                using (var pen = new Pen (c1, 2) { DashStyle = DashStyle.Dash })
                {
                    graphics.DrawLine (pen, rect.Left, rect.Top, rect.Right, rect.Top);
                }

                break;

            case InsertEffect.InsertAfter:
                if (eventArgs.ItemIndex < 0)
                {
                    rect.Offset (0, 2);
                }

                using (var pen = new Pen (c1, 2) { DashStyle = DashStyle.Dash })
                {
                    graphics.DrawLine (pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                }

                break;

            case InsertEffect.AddAsChild:
                if (eventArgs.ItemIndex >= 0 && eventArgs.ItemIndex < ItemCount)
                {
                    var dx = GetItemIndent (eventArgs.ItemIndex) + 80;
                    rect.Offset (dx, 0);
                    using (var pen = new Pen (c1, 2) { DashStyle = DashStyle.Dash })
                    {
                        graphics.DrawLine (pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                    }
                }

                break;
        }
    }

    private void DrawMouseSelectedArea (Graphics gr)
    {
        if (mouseCanSelectArea && mouseSelectArea != Rectangle.Empty)
        {
            var c = Color.FromArgb (SelectionColor.A == 255 ? SelectionColorOpaque : SelectionColor.A, SelectionColor);
            var rect = new Rectangle (mouseSelectArea.Left - HorizontalScroll.Value,
                mouseSelectArea.Top - VerticalScroll.Value, mouseSelectArea.Width, mouseSelectArea.Height);
            using (var pen = new Pen (c))
            {
                gr.DrawRectangle (pen, rect);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="graphics"></param>
    protected virtual void DrawItems (Graphics graphics)
    {
        var i = Math.Max (0, PointToIndex (new Point (Padding.Left, Padding.Top)) - 1);

        visibleItemInfos.Clear();

        for (; i < ItemCount; i++)
        {
            var info = visibleItemInfos[i] = CalcVisibleItemInfo (graphics, i);
            if (info.Y > ClientSize.Height)
            {
                break;
            }

            DrawItem (graphics, info);
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected readonly Dictionary<int, VisibleItemInfo> visibleItemInfos = new ();

    /// <summary>
    ///
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="info"></param>
    protected virtual void DrawItem (Graphics gr, VisibleItemInfo info)
    {
        DrawItemWhole (gr, info);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="info"></param>
    public void DrawItemWhole (Graphics gr, VisibleItemInfo info)
    {
        DrawItemBackgound (gr, info);

        if (_lastDragAndDropEffect == null) //do not draw selection when drag&drop over the control
        {
            if (!IsEditMode) //do not draw selection when edit mode
            {
                if (SelectedItemIndexes.Contains (info.ItemIndex))
                {
                    DrawSelection (gr, info);
                }
            }
        }

        if (HotTracking && info.ItemIndex == currentHotTrackingIndex)
        {
            DrawItemHotTracking (gr, info);
        }


        DrawItemIcons (gr, info);
        DrawItemContent (gr, info);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="info"></param>
    protected virtual void DrawItemHotTracking (Graphics gr, VisibleItemInfo info)
    {
        var c1 = HotTrackingColor;
        var rect = info.TextAndIconRect;

        if (FullItemSelect)
        {
            var cr = ClientRectMinusPaddings;
            rect = new Rectangle (cr.Left, rect.Top, cr.Width - 1, rect.Height);
        }

        if (rect is { Width: > 0, Height: > 0 })
        {
            using (var pen = new Pen (c1))
            {
                gr.DrawRectangle (pen, rect);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public Rectangle ClientRectMinusPaddings
    {
        get
        {
            var rect = ClientRectangle;
            return new Rectangle (rect.Left + Padding.Left, rect.Top + Padding.Top,
                rect.Width - Padding.Left - Padding.Right,
                rect.Height - Padding.Top - Padding.Bottom);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="info"></param>
    public virtual void DrawSelection (Graphics gr, VisibleItemInfo info)
    {
        var c1 = Color.FromArgb (SelectionColor.A == 255 ? SelectionColorOpaque : SelectionColor.A, SelectionColor);
        var c2 = Color.FromArgb (c1.A / 2, SelectionColor);
        var rect = info.TextAndIconRect;

        if (FullItemSelect)
        {
            var cr = ClientRectMinusPaddings;
            rect = new Rectangle (cr.Left, rect.Top, cr.Width - 1, rect.Height);
        }

        if (rect is { Width: > 0, Height: > 0 })
        {
            using (var brush = new LinearGradientBrush (rect, c2, c1, LinearGradientMode.Vertical))
            {
                using (var pen = new Pen (c1))
                {
                    gr.FillRectangle (brush, Rectangle.FromLTRB (rect.Left, rect.Top, rect.Right + 1, rect.Bottom + 1));
                    gr.DrawRectangle (pen, rect);
                }
            }
        }
    }

    /// <summary>
    /// Draws Expand box, Check box and Icon of the Item
    /// </summary>
    public virtual void DrawItemIcons
        (
            Graphics graphics,
            VisibleItemInfo info
        )
    {
        if (info.ExpandBoxType > 0)
        {
            var img = (Bitmap?)(info.ExpandBoxType == 2
                ? ImageEmptyExpand
                : (info.Expanded ? ImageCollapse : ImageExpand));
            if (img != null)
            {
                img.SetResolution (graphics.DpiX, graphics.DpiY);
                graphics.DrawImage (img, info.X_ExpandBox, info.Y + 1);
            }
        }

        if (info.CheckBoxVisible)
        {
            var img = (Bitmap?)(GetItemChecked (info.ItemIndex) ? ImageCheckBoxOn : ImageCheckBoxOff);
            if (img != null)
            {
                img.SetResolution (graphics.DpiX, graphics.DpiY);
                graphics.DrawImage (img, info.X_CheckBox, info.Y + 1);
            }
        }

        if (ShowIcons && info.Icon != null)
        {
            var img = (Bitmap?)info.Icon;
            if (img != null)
            {
                img.SetResolution (graphics.DpiX, graphics.DpiY);
                graphics.DrawImage (img, info.X_Icon, info.Y + 1);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="info"></param>
    public virtual void DrawItemContent (Graphics gr, VisibleItemInfo info)
    {
        using (var sf = new StringFormat() { LineAlignment = info.LineAlignment })
        {
            using (var brush = new SolidBrush (info.ForeColor))
            {
                var rect = new Rectangle (info.X_Text, info.Y + 1, info.X_EndText - info.X_Text + 1, info.Height - 1);

                //gr.DrawString(info.Text, Font, brush, info.X_Text, info.Y + 1, sf);
                gr.DrawString (info.Text, Font, brush, rect, sf);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="info"></param>
    public virtual void DrawItemBackgound (Graphics gr, VisibleItemInfo info)
    {
        var backColor = info.BackColor;

        if (backColor != Color.Empty)
        {
            using (var brush = new SolidBrush (backColor))
            {
                gr.FillRectangle (brush, info.TextAndIconRect);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected virtual VisibleItemInfo CalcVisibleItemInfo (Graphics gr, int itemIndex)
    {
        var result = new VisibleItemInfo();
        result.Calc (this, itemIndex, gr);
        return result;
    }

    #endregion

    #region Coordinates

    /// <summary>
    /// Высота элементов фиксированная?
    /// </summary>
    protected virtual bool IsItemHeightFixed => true;

    /// <summary>
    /// Absolute Y coordinate of the control to item index
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public int YToIndex (int y)
    {
        if (y < Padding.Top)
        {
            return -1;
        }

        if (ItemCount <= 0)
        {
            return -1;
        }

        int i;
        if (IsItemHeightFixed)
        {
            i = (y - Padding.Top) / (ItemHeightDefault + ItemInterval);
        }
        else
        {
            i = _yOfItems.BinarySearch (y + 1);
            if (i < 0)
            {
                i = ~i;
                i -= 1;
            }
        }

        if (i >= ItemCount)
        {
            return -1;
        }

        return i;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected virtual int GetItemY
        (
            int index
        )
    {
        return IsItemHeightFixed
            ? Padding.Top + index * (ItemHeightDefault + ItemInterval)
            : _yOfItems[index];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    protected int YToIndexAround (int y)
    {
        if (ItemCount <= 0)
        {
            return -1;
        }

        int i;

        if (IsItemHeightFixed)
        {
            i = (y - Padding.Top) / (ItemHeightDefault + ItemInterval);
        }
        else
        {
            i = _yOfItems.BinarySearch (y + 1);
            if (i < 0)
            {
                i = ~i;
                i -= 1;
            }
        }

        if (i < 0)
        {
            i = 0;
        }

        if (i >= ItemCount)
        {
            i = ItemCount - 1;
        }

        return i;
    }

    /// <summary>
    /// Control visible rect coordinates to item index
    /// </summary>
    public int PointToIndex (Point p)
    {
        if (p.X < Padding.Left || p.X > ClientRectangle.Right - Padding.Right)
        {
            return -1;
        }

        var y = p.Y + VerticalScroll.Value;

        return YToIndex (y);
    }

    /// <summary>
    /// Control visible rect coordinates to item info
    /// </summary>
    public virtual VisibleItemInfo? PointToItemInfo (Point p)
    {
        var index = PointToIndex (p);
        visibleItemInfos.TryGetValue (index, out var info);

        return info;
    }

    /// <summary>
    ///   x0  x1  x2  x3      x4     x5
    ///   |   |   |   |       |      |
    ///   □   □   □   ItemText
    /// </summary>
    public class VisibleItemInfo
    {
        /// <summary>
        /// Индекс элемента.
        /// </summary>
        public int ItemIndex;

        /// <summary>
        ///
        /// </summary>
        public int Y;

        /// <summary>
        /// Высота элемента.
        /// </summary>
        public int Height;

        /// <summary>
        /// Текст, соответствующий элементу.
        /// </summary>
        public string? Text;

        /// <summary>
        ///
        /// </summary>
        public int X;

        /// <summary>
        ///
        /// </summary>
        public int X_ExpandBox;

        /// <summary>
        ///
        /// </summary>
        public int X_CheckBox;

        /// <summary>
        ///
        /// </summary>
        public int X_Icon;

        /// <summary>
        ///
        /// </summary>
        public int X_Text;

        /// <summary>
        ///
        /// </summary>
        public int X_EndText;

        /// <summary>
        ///
        /// </summary>
        public int X_End;

        /// <summary>
        ///
        /// </summary>
        public bool CheckBoxVisible;

        /// <summary>
        ///
        /// </summary>
        public int ExpandBoxType;

        /// <summary>
        ///
        /// </summary>
        public Image? Icon;

        /// <summary>
        ///
        /// </summary>
        public bool Expanded;

        /// <summary>
        /// Цвет текста.
        /// </summary>
        public Color ForeColor;

        /// <summary>
        /// Цвет фона.
        /// </summary>
        public Color BackColor;

        /// <summary>
        /// Выравнивание по горизонтали.
        /// </summary>
        public StringAlignment LineAlignment = StringAlignment.Near;

        /// <summary>
        /// Полный прямоугольник.
        /// </summary>
        public Rectangle FullRect => new (X, Y, X_End - X + 1, Height);

        /// <summary>
        /// Описывающй прямоугольник.
        /// </summary>
        public Rectangle Rect => new (X_ExpandBox, Y, X_EndText - X + 1, Height);

        /// <summary>
        /// Прямоугольник, соответствующий тексту и иконке.
        /// </summary>
        public Rectangle TextAndIconRect => new (X_Icon, Y, X_EndText - X_Icon + 1, Height);

        /// <summary>
        /// Прямоугольник, соответствующий тексту.
        /// </summary>
        public Rectangle TextRect => new (X_Text, Y, X_EndText - X_Text + 1, Height);

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="itemIndex"></param>
        /// <param name="gr"></param>
        public virtual void Calc (FastListBase list, int itemIndex, Graphics gr)
        {
            //var vertScroll = list.VerticalScroll.Visible ? list.VerticalScroll.Value : 0;
            var vertScroll = list.VerticalScroll.Value; //!!!!!!!!!!!!

            ItemIndex = itemIndex;
            CheckBoxVisible = list.ShowCheckBoxes && list.GetItemCheckBoxVisible (itemIndex);
            Icon = list.GetItemIcon (itemIndex);
            LineAlignment = list.GetItemLineAlignment (itemIndex);
            var y = list.GetItemY (itemIndex);
            Y = y - vertScroll;
            Height = list.GetItemY (itemIndex + 1) - y - list.ItemInterval;
            Text = list.GetItemText (itemIndex) ?? "";
            Expanded = list.GetItemExpanded (itemIndex);
            var temp = list.ShowEmptyExpandBoxes ? 2 : 0;
            ExpandBoxType = list.ShowExpandBoxes
                ? (Expanded
                    ? (list.CanCollapseItem (itemIndex) ? 1 : temp)
                    : (list.CanExpandItem (itemIndex) ? 1 : temp))
                : 0;
            BackColor = list.GetItemBackColor (itemIndex);
            ForeColor = list.GetItemForeColor (itemIndex);

            X = list.Padding.Left;
            var x = list.GetItemIndent (itemIndex) + list.Padding.Left;
            X_ExpandBox = x;
            if (list.ShowExpandBoxes)
            {
                x += list.ImageExpand!.Width + 2;
            }

            X_CheckBox = x;
            if (list.ShowCheckBoxes)
            {
                x += list.ImageCheckBoxOn!.Width + 2;
            }

            X_Icon = x;
            if (list.ShowIcons)
            {
                x += list.IconSize.Width + 2;
            }

            X_Text = x;
            x += (int)gr.MeasureString (Text, list.Font).Width + 1;
            X_End = list.ClientSize.Width - list.Padding.Right - 2;
            X_EndText = Math.Min (x, X_End);
        }
    }

    #endregion

    #region Build

    /// <summary>
    /// Построение списка элементов.
    /// </summary>
    protected virtual void Build()
    {
        _yOfItems.Clear();

        var y = Padding.Top;

        if (!IsItemHeightFixed)
        {
            for (var i = 0; i < _itemCount; i++)
            {
                _yOfItems.Add (y);
                y += GetItemHeight (i) + ItemInterval;
            }

            _yOfItems.Add (y);
        }
        else
        {
            y += _itemCount * (ItemHeightDefault + ItemInterval);
        }


        SelectedItemIndexes.RemoveWhere (i => i >= _itemCount);
        CheckedItemIndex.RemoveWhere (i => i >= _itemCount);

        AutoScrollMinSize = new Size (AutoScrollMinSize.Width, y + Padding.Bottom + 2);
        Invalidate();
    }

    bool buildNeeded;

    /// <summary>
    /// Установка признака "требуется перестроение элементов".
    /// </summary>
    public virtual void BuildNeeded()
    {
        buildNeeded = true;
    }

    #endregion

    #region Get item info methods

    /// <summary>
    /// Получение высоты элемента по его индексу.
    /// </summary>
    protected virtual int GetItemHeight
        (
            int itemIndex
        )
    {
        return ItemHeightDefault;
    }

    /// <summary>
    /// Получение отступа элемента по его индексу.
    /// </summary>
    public virtual int GetItemIndent
        (
            int itemIndex
        )
    {
        return ItemIndentDefault;
    }

    /// <summary>
    /// Получение текста элемента по его индексу.
    /// </summary>
    protected virtual string? GetItemText
        (
            int itemIndex
        )
    {
        return string.Empty;
    }

    /// <summary>
    /// Реакция на изменение текста в элементе.
    /// </summary>
    protected virtual void OnItemTextPushed
        (
            int itemIndex,
            string text
        )
    {
        Invalidate();
    }

    /// <summary>
    /// Получение видимости чекбокса для элемента.
    /// </summary>
    protected virtual bool GetItemCheckBoxVisible
        (
            int itemIndex
        )
    {
        return ShowCheckBoxes;
    }

    /// <summary>
    /// Получение состояния отметки для элемента по его индексу.
    /// </summary>
    protected virtual bool GetItemChecked
        (
            int itemIndex
        )
    {
        return CheckedItemIndex.Contains (itemIndex);
    }

    /// <summary>
    /// Получение иконки для элемента по его индексу.
    /// </summary>
    protected virtual Image? GetItemIcon
        (
            int itemIndex
        )
    {
        return null;
    }

    /// <summary>
    /// Получение выравнивания для элемента по его индексу.
    /// </summary>
    protected virtual StringAlignment GetItemLineAlignment
        (
            int itemIndex
        )
    {
        return StringAlignment.Near;
    }

    /// <summary>
    /// Получение цвета фона элемента по его индексу.
    /// </summary>
    protected virtual Color GetItemBackColor
        (
            int itemIndex
        )
    {
        return Color.Empty;
    }

    /// <summary>
    /// Получение цвета текста элемента по его индексу.
    /// </summary>
    protected virtual Color GetItemForeColor
        (
            int itemIndex
        )
    {
        return ForeColor;
    }

    /// <summary>
    /// Получение состояния элемента "свернут/развернут" по его индексу.
    /// </summary>
    protected virtual bool GetItemExpanded
        (
            int itemIndex
        )
    {
        return false;
    }

    /// <summary>
    /// Можно ли снять выделение с указанного элемента?
    /// </summary>
    protected virtual bool CanUnselectItem
        (
            int itemIndex
        )
    {
        return true;
    }

    /// <summary>
    /// Получение возможности выделения элемента по его индексу.
    /// </summary>
    protected virtual bool CanSelectItem
        (
            int itemIndex
        )
    {
        return AllowSelectItems;
    }

    /// <summary>
    /// Получение возможности снятия выделения с элемента по его индексу.
    /// </summary>
    protected virtual bool CanUncheckItem
        (
            int itemIndex
        )
    {
        return true;
    }

    /// <summary>
    /// Получение возможности отметки элемента по его индексу.
    /// </summary>
    protected virtual bool CanCheckItem
        (
            int itemIndex
        )
    {
        return true;
    }

    /// <summary>
    /// Получение возможности разворачивания элемента по его индексу.
    /// </summary>
    protected virtual bool CanExpandItem
        (
            int itemIndex
        )
    {
        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected virtual bool CanCollapseItem (int itemIndex)
    {
        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected virtual bool CanEditItem (int itemIndex)
    {
        return true;
    }

    #endregion

    #region Scroll

    /// <summary>
    /// Скроллинг вверх.
    /// </summary>
    protected virtual void ScrollUp()
    {
        OnScrollVertical (VerticalScroll.Value - VerticalScroll.SmallChange);
    }

    /// <summary>
    /// Скроллинг вниз.
    /// </summary>
    protected virtual void ScrollDown()
    {
        OnScrollVertical (VerticalScroll.Value + VerticalScroll.SmallChange);
    }

    /// <summary>
    /// Скроллинг на одну страницу вверх.
    /// </summary>
    protected virtual void ScrollPageUp()
    {
        OnScrollVertical (VerticalScroll.Value - VerticalScroll.LargeChange);
    }

    /// <summary>
    /// Скроллинг на одну страницу вниз.
    /// </summary>
    protected virtual void ScrollPageDown()
    {
        OnScrollVertical (VerticalScroll.Value + VerticalScroll.LargeChange);
    }

    /// <summary>
    ///
    /// </summary>
    public new Size AutoScrollMinSize
    {
        set
        {
            if (_showScrollBar)
            {
                if (!base.AutoScroll)
                {
                    base.AutoScroll = true;
                }

                var newSize = value;
                base.AutoScrollMinSize = newSize;
            }
            else
            {
                if (base.AutoScroll)
                {
                    base.AutoScroll = false;
                }

                base.AutoScrollMinSize = new Size (0, 0);
                VerticalScroll.Visible = false;
                HorizontalScroll.Visible = false;
                VerticalScroll.Maximum = Math.Max (0, value.Height - ClientSize.Height);
                HorizontalScroll.Maximum = Math.Max (0, value.Width - ClientSize.Width);
                _localAutoScrollMinSize = value;
            }
        }

        get => _showScrollBar ? base.AutoScrollMinSize : _localAutoScrollMinSize;
    }

    /// <summary>
    /// Updates scrollbar position after Value changed
    /// </summary>
    public void UpdateScrollbars()
    {
        if (ShowScrollBar)
        {
            //some magic for update scrolls
            base.AutoScrollMinSize -= new Size (1, 0);
            base.AutoScrollMinSize += new Size (1, 0);
        }
        else
        {
            AutoScrollMinSize = AutoScrollMinSize;
        }

        if (IsHandleCreated)
        {
            BeginInvoke ((MethodInvoker) OnScrollbarsUpdated);
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnScrollbarsUpdated()
    {
    }

    private const int WM_HSCROLL = 0x114;
    private const int WM_VSCROLL = 0x115;
    private const int SB_ENDSCROLL = 0x8;
    private const int WM_MOUSEWHEEL = 0x20A;

    /// <inheritdoc cref="UserControl.WndProc"/>
    protected override void WndProc (ref Message m)
    {
        if (m.Msg == WM_HSCROLL || m.Msg == WM_VSCROLL)
        {
            if (m.WParam.ToInt32() != SB_ENDSCROLL)
            {
                Focus();
                Invalidate();
            }
        }

        if (m.Msg == WM_MOUSEWHEEL)
        {
            //var step = 3 * ItemHeightDefault * Math.Sign(-m.WParam.ToInt64());
            var step = -3 * ItemHeightDefault * Math.Sign ((short)(m.WParam.ToInt64() >> 16));
            if (VerticalScroll.Visible)
            {
                OnScroll (new ScrollEventArgs (ScrollEventType.ThumbPosition, VerticalScroll.Value + step,
                    ScrollOrientation.VerticalScroll));
            }

            Focus();
            return;
        }

        base.WndProc (ref m);
    }

    /// <summary>
    /// Скроллинг вплоть до указанного прямоугольника.
    /// </summary>
    /// <param name="rect"></param>
    public void ScrollToRectangle
        (
            Rectangle rect
        )
    {
        rect = new Rectangle (rect.X - HorizontalScroll.Value, rect.Y - VerticalScroll.Value, rect.Width, rect.Height);

        var v = VerticalScroll.Value;
        var h = HorizontalScroll.Value;

        if (rect.Bottom > ClientRectangle.Height)
        {
            v += rect.Bottom - ClientRectangle.Height;
        }
        else if (rect.Top < Padding.Top)
        {
            v += rect.Top - Padding.Top;
        }

        if (rect.Right > ClientRectangle.Width)
        {
            h += rect.Right - ClientRectangle.Width;
        }
        else if (rect.Left < Padding.Left)
        {
            h += rect.Left - Padding.Left;
        }

        //
        v = Math.Max (VerticalScroll.Minimum, v);
        h = Math.Max (HorizontalScroll.Minimum, h);

        //
        try
        {
            OnScrollVertical (v);
        }
        catch (ArgumentOutOfRangeException exception)
        {
            Debug.WriteLine (exception.Message);
        }
    }

    /// <summary>
    /// Скроллинг вплоть до элемента с указанным индексом.
    /// </summary>
    public void ScrollToItem
        (
            int itemIndex
        )
    {
        if (itemIndex < 0 || itemIndex >= ItemCount)
        {
            return;
        }

        var y = GetItemY (itemIndex);
        var height = GetItemHeight (itemIndex);
        ScrollToRectangle (new Rectangle (0, y, ClientRectangle.Width, height));
    }

    /// <summary>
    /// Вертикальный скроллинг.
    /// </summary>
    public void OnScrollVertical
        (
            int newVerticalScrollBarValue
        )
    {
        OnScroll (new ScrollEventArgs (ScrollEventType.ThumbPosition, newVerticalScrollBarValue,
            ScrollOrientation.VerticalScroll));
    }

    /// <inheritdoc cref="ScrollableControl.OnScroll"/>
    protected override void OnScroll (ScrollEventArgs se)
    {
        if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
        {
            VerticalScroll.Value = Math.Max (VerticalScroll.Minimum, Math.Min (VerticalScroll.Maximum, se.NewValue));
        }

        UpdateScrollbars();
        Invalidate();

        //
        base.OnScroll (se);
    }

    #endregion

    #region Edit

    /// <summary>
    /// Индекс редактируемого элемента.
    /// </summary>
    protected int EditItemIndex;

    /// <summary>
    /// Текущий редактор для элемента.
    /// </summary>
    protected Control? EditControl;

    /// <summary>
    /// Счетчик режима обновления.
    /// </summary>
    protected int editUpdating;

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="startValue"></param>
    public virtual void OnStartEdit (int itemIndex, string? startValue = null)
    {
        if (!visibleItemInfos.ContainsKey (itemIndex))
        {
            return;
        }

        var info = visibleItemInfos[itemIndex];

        EditItemIndex = itemIndex;
        IsEditMode = true;
        var ctrl = new TextBox()
            { Text = GetItemText (itemIndex), AcceptsTab = true, Multiline = false, WordWrap = false };
        ctrl.Bounds = new Rectangle (info.X_Text, info.Y, info.X_End - info.X_Text, info.Height);
        ctrl.Parent = this;
        ctrl.Focus();
        if (startValue != null)
        {
            ctrl.Text = startValue;
            ctrl.SelectionStart = startValue.Length;
        }

        ctrl.LostFocus += (_, _) => OnEndEdit();
        ctrl.PreviewKeyDown += (_, a) => a.IsInputKey = true;
        ctrl.KeyDown += (_, a) =>
        {
            switch (a.KeyCode)
            {
                case Keys.Escape:
                    OnEndEdit (null);
                    a.Handled = true;
                    a.SuppressKeyPress = true;
                    break;

                case Keys.Enter:
                    OnEndEdit();
                    a.Handled = true;
                    a.SuppressKeyPress = true;
                    break;
            }
        };
        EditControl = ctrl;
        Invalidate();
    }

    /// <summary>
    /// Окончание редактирования.
    /// </summary>
    public virtual void OnEndEdit()
    {
        string? val = null;

        if (EditControl != null)
        {
            val = EditControl.Text;
        }

        OnEndEdit (val);
    }

    /// <summary>
    /// Окончание редактирования.
    /// </summary>
    /// <param name="newValue">Новое значение.</param>
    public virtual void OnEndEdit
        (
            string? newValue
        )
    {
        if (editUpdating > 0)
        {
            return;
        }

        try
        {
            editUpdating++;

            if (newValue is not null)
            {
                OnItemTextPushed (EditItemIndex, newValue);
            }

            if (EditControl is not null)
            {
                EditControl.Parent = null;
            }

            EditControl = null;
            IsEditMode = false;

            Invalidate();
        }
        finally
        {
            editUpdating--;
        }
    }

    #endregion

    #region Routines

    /// <inheritdoc cref="Control.OnGotFocus"/>
    protected override void OnGotFocus
        (
            EventArgs e
        )
    {
        base.OnGotFocus (e);
        Invalidate();
    }

    #endregion

    #region ISupportInitialize

    /// <summary>
    /// В настоящий момент происходит инициализация?
    /// </summary>
    protected bool IsInitializing;

    /// <summary>
    /// Начало инициализации.
    /// </summary>
    public void BeginInit()
    {
        IsInitializing = true;
    }

    /// <summary>
    /// Окончание инициализации.
    /// </summary>
    public void EndInit()
    {
        IsInitializing = false;
        ItemCount = ItemCount;
    }

    #endregion
}
