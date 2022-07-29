// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* AutocompleteListView.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
// ReSharper disable RedundantNameQualifier
[System.ComponentModel.ToolboxItem (false)]
// ReSharper restore RedundantNameQualifier
public class AutocompleteListView
    : UserControl
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    public event EventHandler? FocusedItemIndexChanged;

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    public ImageList? ImageList { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color SelectedColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color HoveredColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int FocusedItemIndex
    {
        get => _focusedItemIndex;
        set
        {
            if (_focusedItemIndex != value)
            {
                _focusedItemIndex = value;
                FocusedItemIndexChanged?.Invoke (this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public AutocompleteItem? FocusedItem
    {
        get => FocusedItemIndex >= 0 && _focusedItemIndex < visibleItems.Count
                ? visibleItems[_focusedItemIndex]
                : null;
        set => FocusedItemIndex = visibleItems.IndexOf (value!);
    }

    /// <summary>
    ///
    /// </summary>
    public int Count => visibleItems.Count;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal AutocompleteListView
        (
            SyntaxTextBox textBox
        )
    {
        Sure.NotNull (textBox);

        SetStyle
            (
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint,
                true
            );
        base.Font = new Font (FontFamily.GenericSansSerif, 9);
        visibleItems = new List<AutocompleteItem>();
        VerticalScroll.SmallChange = ItemHeight;
        MaximumSize = new Size (Size.Width, 180);
        if (toolTip != null)
        {
            toolTip.ShowAlways = false;
            toolTip.Popup += toolTip_Popup;
        }

        AppearInterval = 500;
        _timer.Tick += timer_Tick;
        SelectedColor = Color.Orange;
        HoveredColor = Color.Red;
        ToolTipDuration = 3000;

        _textBox = textBox;

        textBox.KeyDown += tb_KeyDown;
        textBox.SelectionChanged += textBox_SelectionChanged;
        textBox.KeyPressed += textBox_KeyPressed;

        var form = textBox.FindForm();
        if (form != null)
        {
            form.LocationChanged += delegate { SafeClose(); };
            form.ResizeBegin += delegate { SafeClose(); };
            form.FormClosing += delegate { SafeClose(); };
            form.LostFocus += delegate { SafeClose(); };
        }

        textBox.LostFocus += (_, _) =>
        {
            if (Menu is { IsDisposed: false, Focused: false })
            {
                SafeClose();
            }
        };

        textBox.Scroll += (_, _) => SafeClose();

        this.VisibleChanged += (_, _) =>
        {
            if (Visible)
            {
                DoSelectedVisible();
            }
        };
    }

    #endregion

    #region Private members

    private readonly SyntaxTextBox _textBox;

    internal List<AutocompleteItem> visibleItems;

    private IEnumerable<AutocompleteItem> _sourceItems = new List<AutocompleteItem>();

    private int _focusedItemIndex;

    private int _hoveredItemIndex = -1;

    private int _oldItemCount;

    private int ItemHeight => Font.Height + 2;

    AutocompleteMenu? Menu => Parent as AutocompleteMenu;

    internal ToolTip? toolTip = new ();

    private readonly Timer _timer = new ();

    internal bool AllowTabKey { get; set; }

    internal int ToolTipDuration { get; set; }

    internal Size MaxToolTipSize { get; set; }

    internal int AppearInterval
    {
        get => _timer.Interval;
        set => _timer.Interval = value;
    }

    internal bool AlwaysShowTooltip
    {
        get => toolTip!.ShowAlways;
        set => toolTip!.ShowAlways = value;
    }

    private void AdjustScroll()
    {
        if (_oldItemCount == visibleItems.Count)
        {
            return;
        }

        var needHeight = ItemHeight * visibleItems.Count + 1;
        Height = Math.Min (needHeight, MaximumSize.Height);
        Menu?.CalcSize();

        AutoScrollMinSize = new Size (0, needHeight);
        _oldItemCount = visibleItems.Count;
    }

    /// <inheritdoc cref="ScrollableControl.OnScroll"/>
    protected override void OnScroll
        (
            ScrollEventArgs se
        )
    {
        base.OnScroll (se);
        Invalidate();
    }

    /// <inheritdoc cref="Control.OnMouseClick"/>
    protected override void OnMouseClick
        (
            MouseEventArgs e
        )
    {
        base.OnMouseClick (e);

        if (e.Button == MouseButtons.Left)
        {
            FocusedItemIndex = PointToItemIndex (e.Location);
            DoSelectedVisible();
            Invalidate();
        }
    }

    /// <inheritdoc cref="Control.OnMouseDoubleClick"/>
    protected override void OnMouseDoubleClick (MouseEventArgs e)
    {
        base.OnMouseDoubleClick (e);
        FocusedItemIndex = PointToItemIndex (e.Location);
        Invalidate();
        OnSelecting();
    }

    internal virtual void OnSelecting()
    {
        if (FocusedItemIndex < 0 || FocusedItemIndex >= visibleItems.Count)
        {
            return;
        }

        _textBox.TextSource.Manager.BeginAutoUndoCommands();
        try
        {
            var item = FocusedItem;
            var args = new SelectingEventArgs()
            {
                Item = item!,
                SelectedIndex = FocusedItemIndex
            };

            Menu?.OnSelecting (args);

            if (args.Cancel)
            {
                FocusedItemIndex = args.SelectedIndex;
                Invalidate();
                return;
            }

            if (!args.Handled)
            {
                var fragment = Menu?.Fragment;
                if (fragment != null)
                {
                    DoAutocomplete (item!, fragment);
                }
            }

            Menu?.Close();

            //
            if (Menu != null)
            {
                var args2 = new SelectedEventArgs()
                {
                    Item = item!,
                    Tb = Menu.Fragment._textBox
                };
                item?.OnSelected (Menu, args2);
                Menu.OnSelected (args2);
            }
        }
        finally
        {
            _textBox.TextSource.Manager.EndAutoUndoCommands();
        }
    }

    private void DoAutocomplete
        (
            AutocompleteItem item,
            TextRange fragment
        )
    {
        var newText = item.GetTextForReplace();

        //replace text of fragment
        var tb = fragment._textBox;

        tb.BeginAutoUndo();
        tb.TextSource.Manager.ExecuteCommand (new SelectCommand (tb.TextSource));
        if (tb.Selection.ColumnSelectionMode)
        {
            var start = tb.Selection.Start;
            var end = tb.Selection.End;
            start.Column = fragment.Start.Column;
            end.Column = fragment.End.Column;
            tb.Selection.Start = start;
            tb.Selection.End = end;
        }
        else
        {
            tb.Selection.Start = fragment.Start;
            tb.Selection.End = fragment.End;
        }

        tb.InsertText (newText);
        tb.TextSource.Manager.ExecuteCommand (new SelectCommand (tb.TextSource));
        tb.EndAutoUndo();
        tb.Focus();
    }

    private void SetToolTip
        (
            AutocompleteItem autocompleteItem
        )
    {
        var title = autocompleteItem.ToolTipTitle;
        var text = autocompleteItem.ToolTipText;

        if (string.IsNullOrEmpty (title))
        {
            toolTip!.ToolTipTitle = null;
            toolTip.SetToolTip (this, null);
            return;
        }

        if (Parent != null)
        {
            IWin32Window window = Parent ?? this;
            var location = (PointToScreen (Location).X + MaxToolTipSize.Width + 105) <
                           Screen.FromControl (Parent!).WorkingArea.Right
                ? new Point (Right + 5, 0)
                : new Point (Left - 105 - MaximumSize.Width, 0);

            if (string.IsNullOrEmpty (text))
            {
                toolTip!.ToolTipTitle = null;
                toolTip.Show (title, window, location.X, location.Y, ToolTipDuration);
            }
            else
            {
                toolTip!.ToolTipTitle = title;
                toolTip.Show (text, window, location.X, location.Y, ToolTipDuration);
            }
        }
    }

    private int PointToItemIndex
        (
            Point p
        )
    {
        return (p.Y + VerticalScroll.Value) / ItemHeight;
    }

    /// <inheritdoc cref="ContainerControl.ProcessCmdKey"/>
    protected override bool ProcessCmdKey
        (
            ref Message msg,
            Keys keyData
        )
    {
        ProcessKey (keyData, Keys.None);

        return base.ProcessCmdKey (ref msg, keyData);
    }

    private bool ProcessKey
        (
            Keys keyData,
            Keys keyModifiers
        )
    {
        if (keyModifiers == Keys.None)
        {
            switch (keyData)
            {
                case Keys.Down:
                    SelectNext (+1);
                    return true;

                case Keys.PageDown:
                    SelectNext (+10);
                    return true;

                case Keys.Up:
                    SelectNext (-1);
                    return true;

                case Keys.PageUp:
                    SelectNext (-10);
                    return true;

                case Keys.Enter:
                    OnSelecting();
                    return true;

                case Keys.Tab:
                    if (!AllowTabKey)
                    {
                        break;
                    }

                    OnSelecting();
                    return true;

                case Keys.Escape:
                    Menu?.Close();
                    return true;
            }
        }

        return false;
    }

    private void DoSelectedVisible()
    {
        if (FocusedItem != null)
        {
            SetToolTip (FocusedItem);
        }

        var y = FocusedItemIndex * ItemHeight - VerticalScroll.Value;
        if (y < 0)
        {
            VerticalScroll.Value = FocusedItemIndex * ItemHeight;
        }

        if (y > ClientSize.Height - ItemHeight)
        {
            VerticalScroll.Value = Math.Min
                (
                    VerticalScroll.Maximum,
                    FocusedItemIndex * ItemHeight - ClientSize.Height + ItemHeight
                );
        }

        //some magic for update scrolls
        AutoScrollMinSize -= new Size (1, 0);
        AutoScrollMinSize += new Size (1, 0);
    }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs e
        )
    {
        AdjustScroll();

        var itemHeight = ItemHeight;
        var startI = VerticalScroll.Value / itemHeight - 1;
        var finishI = (VerticalScroll.Value + ClientSize.Height) / itemHeight + 1;
        startI = Math.Max (startI, 0);
        finishI = Math.Min (finishI, visibleItems.Count);
        var leftPadding = 18;
        for (var i = startI; i < finishI; i++)
        {
            var y = i * itemHeight - VerticalScroll.Value;

            var item = visibleItems[i];

            if (item.BackColor != Color.Transparent)
            {
                using var brush = new SolidBrush (item.BackColor);
                e.Graphics.FillRectangle (brush, 1, y, ClientSize.Width - 1 - 1, itemHeight - 1);
            }

            if (ImageList != null && visibleItems[i].ImageIndex >= 0)
            {
                e.Graphics.DrawImage (ImageList.Images[item.ImageIndex], 1, y);
            }

            if (i == FocusedItemIndex)
            {
                using var selectedBrush = new LinearGradientBrush (new Point (0, y - 3), new Point (0, y + itemHeight),
                    Color.Transparent, SelectedColor);
                using var pen = new Pen (SelectedColor);
                e.Graphics.FillRectangle (selectedBrush, leftPadding, y, ClientSize.Width - 1 - leftPadding,
                    itemHeight - 1);
                e.Graphics.DrawRectangle (pen, leftPadding, y, ClientSize.Width - 1 - leftPadding, itemHeight - 1);
            }

            if (i == _hoveredItemIndex)
            {
                using var pen = new Pen (HoveredColor);
                e.Graphics.DrawRectangle (pen, leftPadding, y, ClientSize.Width - 1 - leftPadding, itemHeight - 1);
            }

            using (var brush = new SolidBrush (item.ForeColor != Color.Transparent ? item.ForeColor : ForeColor))
            {
                e.Graphics.DrawString (item.ToString(), Font, brush, leftPadding, y);
            }
        }
    }

    private void tb_KeyDown
        (
            object? sender,
            KeyEventArgs e
        )
    {
        var textBox =(SyntaxTextBox) sender!;

        if (Menu!.Visible)
        {
            if (ProcessKey (e.KeyCode, e.Modifiers))
            {
                e.Handled = true;
            }
        }

        if (!Menu.Visible)
        {
            if (textBox.HotkeyMapping.ContainsKey (e.KeyData) &&
                textBox.HotkeyMapping[e.KeyData] == ActionCode.AutocompleteMenu)
            {
                DoAutocomplete();
                e.Handled = true;
            }
            else
            {
                if (e.KeyCode == Keys.Escape && _timer.Enabled)
                {
                    _timer.Stop();
                }
            }
        }
    }

    private void toolTip_Popup
        (
            object? sender,
            PopupEventArgs e
        )
    {
        if (MaxToolTipSize.Height > 0 && MaxToolTipSize.Width > 0)
        {
            e.ToolTipSize = MaxToolTipSize;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose
        (
            bool disposing
        )
    {
        if (toolTip != null)
        {
            toolTip.Popup -= toolTip_Popup;
            toolTip.Dispose();
        }

        if (_textBox != null)
        {
            _textBox.KeyDown -= tb_KeyDown;
            _textBox.KeyPressed -= textBox_KeyPressed;
            _textBox.SelectionChanged -= textBox_SelectionChanged;
        }

        if (_timer != null)
        {
            _timer.Stop();
            _timer.Tick -= timer_Tick;
            _timer.Dispose();
        }

        base.Dispose (disposing);
    }

    private void SafeClose()
    {
        if (Menu is { IsDisposed: false })
        {
            Menu.Close();
        }
    }

    private void textBox_KeyPressed
        (
            object? sender,
            KeyPressEventArgs e
        )
    {
        var backspaceORdel = e.KeyChar == '\b' || e.KeyChar == 0xff;

        if (Menu is { Visible: true } && !backspaceORdel)
        {
            DoAutocomplete();
        }
        else
        {
            ResetTimer (_timer);
        }
    }

    private void timer_Tick
        (
            object? sender,
            EventArgs e
        )
    {
        _timer.Stop();
        DoAutocomplete();
    }

    private void ResetTimer
        (
            Timer timer
        )
    {
        timer.Stop();
        timer.Start();
    }

    internal void DoAutocomplete
        (
            bool forced = false
        )
    {
        if (Menu is { Enabled: false })
        {
            Menu.Close();
            return;
        }

        visibleItems.Clear();
        FocusedItemIndex = 0;
        VerticalScroll.Value = 0;

        //some magic for update scrolls
        AutoScrollMinSize -= new Size (1, 0);
        AutoScrollMinSize += new Size (1, 0);

        //get fragment around caret
        var fragment = _textBox.Selection.GetFragment (Menu!.SearchPattern);
        var text = fragment.Text;

        //calc screen point for popup menu
        var point = _textBox.PlaceToPoint (fragment.End);
        point.Offset (2, _textBox.CharHeight);

        //
        if (forced || (text.Length >= Menu.MinFragmentLength
                       && _textBox.Selection.IsEmpty /*pops up only if selected range is empty*/
                       && (_textBox.Selection.Start > fragment.Start ||
                           text.Length == 0 /*pops up only if caret is after first letter*/)))
        {
            Menu.Fragment = fragment;
            var foundSelected = false;

            //build popup menu
            foreach (var item in _sourceItems)
            {
                item.Parent = Menu;
                var res = item.Compare (text);
                if (res != CompareResult.Hidden)
                {
                    visibleItems.Add (item);
                }

                if (res == CompareResult.VisibleAndSelected && !foundSelected)
                {
                    foundSelected = true;
                    FocusedItemIndex = visibleItems.Count - 1;
                }
            }

            if (foundSelected)
            {
                AdjustScroll();
                DoSelectedVisible();
            }
        }

        //show popup menu
        if (Count > 0)
        {
            if (!Menu.Visible)
            {
                var args = new CancelEventArgs();
                Menu.OnOpening (args);
                if (!args.Cancel)
                {
                    Menu.Show (_textBox, point);
                }
            }

            DoSelectedVisible();
            Invalidate();
        }
        else
        {
            Menu.Close();
        }
    }

    private void textBox_SelectionChanged
        (
            object? sender,
            EventArgs e
        )
    {
        if (Menu is { Visible: true })
        {
            var needClose = false;

            if (!_textBox.Selection.IsEmpty)
            {
                needClose = true;
            }
            else if (!Menu.Fragment.Contains (_textBox.Selection.Start))
            {
                if (_textBox.Selection.Start.Line == Menu.Fragment.End.Line &&
                    _textBox.Selection.Start.Column == Menu.Fragment.End.Column + 1)
                {
                    //user press key at end of fragment
                    var c = _textBox.Selection.CharBeforeStart;
                    if (!Regex.IsMatch (c.ToString(), Menu.SearchPattern)) //check char
                    {
                        needClose = true;
                    }
                }
                else
                {
                    needClose = true;
                }
            }

            if (needClose)
            {
                Menu.Close();
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public void SetAutocompleteItems
        (
            ICollection<string> items
        )
    {
        var list = new List<AutocompleteItem> (items.Count);
        foreach (var item in items)
        {
            list.Add (new AutocompleteItem (item));
        }

        SetAutocompleteItems (list);
    }

    /// <summary>
    ///
    /// </summary>
    public void SetAutocompleteItems
        (
            IEnumerable<AutocompleteItem> items
        )
    {
        _sourceItems = items;
    }

    /// <summary>
    ///
    /// </summary>
    public void SelectNext
        (
            int shift
        )
    {
        FocusedItemIndex = Math.Max (0, Math.Min (FocusedItemIndex + shift, visibleItems.Count - 1));
        DoSelectedVisible();

        //
        Invalidate();
    }

    #endregion

}
