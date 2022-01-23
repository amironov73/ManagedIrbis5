// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AutocompleteMenu.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Popup menu for autocomplete
/// </summary>
[Browsable (false)]
public class AutocompleteMenu
    : ToolStripDropDown
{
    AutocompleteListView _listView;
    public ToolStripControlHost host;
    public TextRange Fragment { get; internal set; }

    /// <summary>
    /// Regex pattern for serach fragment around caret
    /// </summary>
    public string SearchPattern { get; set; }

    /// <summary>
    /// Minimum fragment length for popup
    /// </summary>
    public int MinFragmentLength { get; set; }

    /// <summary>
    /// User selects item
    /// </summary>
    public event EventHandler<SelectingEventArgs> Selecting;

    /// <summary>
    /// It fires after item inserting
    /// </summary>
    public event EventHandler<SelectedEventArgs> Selected;

    /// <summary>
    /// Occurs when popup menu is opening
    /// </summary>
    public new event EventHandler<CancelEventArgs> Opening;

    /// <summary>
    /// Allow TAB for select menu item
    /// </summary>
    public bool AllowTabKey
    {
        get { return _listView.AllowTabKey; }
        set { _listView.AllowTabKey = value; }
    }

    /// <summary>
    /// Interval of menu appear (ms)
    /// </summary>
    public int AppearInterval
    {
        get { return _listView.AppearInterval; }
        set { _listView.AppearInterval = value; }
    }

    /// <summary>
    /// Sets the max tooltip window size
    /// </summary>
    public Size MaxTooltipSize
    {
        get => _listView.MaxToolTipSize;
        set => _listView.MaxToolTipSize = value;
    }

    /// <summary>
    /// Tooltip will perm show and duration will be ignored
    /// </summary>
    public bool AlwaysShowTooltip
    {
        get => _listView.AlwaysShowTooltip;
        set => _listView.AlwaysShowTooltip = value;
    }

    /// <summary>
    /// Back color of selected item
    /// </summary>
    [DefaultValue (typeof (Color), "Orange")]
    public Color SelectedColor
    {
        get { return _listView.SelectedColor; }
        set { _listView.SelectedColor = value; }
    }

    /// <summary>
    /// Border color of hovered item
    /// </summary>
    [DefaultValue (typeof (Color), "Red")]
    public Color HoveredColor
    {
        get { return _listView.HoveredColor; }
        set { _listView.HoveredColor = value; }
    }

    public AutocompleteMenu (SyntaxTextBox tb)
    {
        // create a new popup and add the list view to it
        AutoClose = false;
        AutoSize = false;
        Margin = Padding.Empty;
        Padding = Padding.Empty;
        BackColor = Color.White;
        _listView = new AutocompleteListView (tb);
        host = new ToolStripControlHost (_listView);
        host.Margin = new Padding (2, 2, 2, 2);
        host.Padding = Padding.Empty;
        host.AutoSize = false;
        host.AutoToolTip = false;
        CalcSize();
        base.Items.Add (host);
        _listView.Parent = this;
        SearchPattern = @"[\w\.]";
        MinFragmentLength = 2;
    }

    public new Font Font
    {
        get { return _listView.Font; }
        set { _listView.Font = value; }
    }

    new internal void OnOpening (CancelEventArgs args)
    {
        if (Opening != null)
            Opening (this, args);
    }

    public new void Close()
    {
        _listView.toolTip.Hide (_listView);
        base.Close();
    }

    internal void CalcSize()
    {
        host.Size = _listView.Size;
        Size = new System.Drawing.Size (_listView.Size.Width + 4, _listView.Size.Height + 4);
    }

    public virtual void OnSelecting()
    {
        _listView.OnSelecting();
    }

    public void SelectNext (int shift)
    {
        _listView.SelectNext (shift);
    }

    internal void OnSelecting (SelectingEventArgs args)
    {
        if (Selecting != null)
            Selecting (this, args);
    }

    public void OnSelected (SelectedEventArgs args)
    {
        if (Selected != null)
            Selected (this, args);
    }

    public new AutocompleteListView Items
    {
        get { return _listView; }
    }

    /// <summary>
    /// Shows popup menu immediately
    /// </summary>
    /// <param name="forced">If True - MinFragmentLength will be ignored</param>
    public void Show (bool forced)
    {
        Items.DoAutocomplete (forced);
    }

    /// <summary>
    /// Minimal size of menu
    /// </summary>
    public new Size MinimumSize
    {
        get { return Items.MinimumSize; }
        set { Items.MinimumSize = value; }
    }

    /// <summary>
    /// Image list of menu
    /// </summary>
    public new ImageList ImageList
    {
        get { return Items.ImageList; }
        set { Items.ImageList = value; }
    }

    /// <summary>
    /// Tooltip duration (ms)
    /// </summary>
    public int ToolTipDuration
    {
        get { return Items.ToolTipDuration; }
        set { Items.ToolTipDuration = value; }
    }

    /// <summary>
    /// Tooltip
    /// </summary>
    public ToolTip ToolTip
    {
        get { return Items.toolTip; }
        set { Items.toolTip = value; }
    }

    protected override void Dispose (bool disposing)
    {
        base.Dispose (disposing);
        if (_listView != null && !_listView.IsDisposed)
            _listView.Dispose();
    }
}
