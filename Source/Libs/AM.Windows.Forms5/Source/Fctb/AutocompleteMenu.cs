// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

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
    #region Events

    /// <summary>
    /// User selects item
    /// </summary>
    public event EventHandler<SelectingEventArgs>? Selecting;

    /// <summary>
    /// It fires after item inserting
    /// </summary>
    public event EventHandler<SelectedEventArgs>? Selected;

    /// <summary>
    /// Occurs when popup menu is opening
    /// </summary>
    public new event EventHandler<CancelEventArgs>? Opening;

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    public ToolStripControlHost host;

    /// <summary>
    ///
    /// </summary>
    public TextRange? Fragment { get; internal set; }

    /// <summary>
    /// Regex pattern for serach fragment around caret
    /// </summary>
    public string SearchPattern { get; set; }

    /// <summary>
    /// Minimum fragment length for popup
    /// </summary>
    public int MinFragmentLength { get; set; }

    /// <summary>
    /// Allow TAB for select menu item
    /// </summary>
    public bool AllowTabKey
    {
        get => _listView.AllowTabKey;
        set => _listView.AllowTabKey = value;
    }

    /// <summary>
    /// Interval of menu appear (ms)
    /// </summary>
    public int AppearInterval
    {
        get => _listView.AppearInterval;
        set => _listView.AppearInterval = value;
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
        get => _listView.SelectedColor;
        set => _listView.SelectedColor = value;
    }

    /// <summary>
    /// Border color of hovered item
    /// </summary>
    [DefaultValue (typeof (Color), "Red")]
    public Color HoveredColor
    {
        get => _listView.HoveredColor;
        set => _listView.HoveredColor = value;
    }

    /// <summary>
    ///
    /// </summary>
    public new Font Font
    {
        get => _listView.Font;
        set => _listView.Font = value;
    }

    /// <summary>
    /// Minimal size of menu
    /// </summary>
    public new Size MinimumSize
    {
        get => Items.MinimumSize;
        set => Items.MinimumSize = value;
    }

    /// <summary>
    /// Image list of menu
    /// </summary>
    public new ImageList? ImageList
    {
        get => Items.ImageList;
        set => Items.ImageList = value;
    }

    /// <summary>
    /// Tooltip duration (ms)
    /// </summary>
    public int ToolTipDuration
    {
        get => Items.ToolTipDuration;
        set => Items.ToolTipDuration = value;
    }

    /// <summary>
    /// Tooltip
    /// </summary>
    public ToolTip ToolTip
    {
        get => Items.toolTip!;
        set => Items.toolTip = value;
    }

    /// <summary>
    ///
    /// </summary>
    public new AutocompleteListView Items => _listView;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AutocompleteMenu
        (
            SyntaxTextBox tb
        )
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

    #endregion

    #region Private members

    private readonly AutocompleteListView _listView;

    /// <summary>
    ///
    /// </summary>
    /// <param name="args"></param>
    internal new void OnOpening
        (
            CancelEventArgs args
        )
    {
        Opening?.Invoke (this, args);
    }

    /// <summary>
    ///
    /// </summary>
    public new void Close()
    {
        _listView.toolTip?.Hide (_listView);
        base.Close();
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void OnSelecting()
    {
        _listView.OnSelecting();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="shift"></param>
    public void SelectNext (int shift)
    {
        _listView.SelectNext (shift);
    }

    internal void CalcSize()
    {
        host.Size = _listView.Size;
        Size = new Size (_listView.Size.Width + 4, _listView.Size.Height + 4);
    }

    internal void OnSelecting
        (
            SelectingEventArgs args
        )
    {
        Selecting?.Invoke (this, args);
    }

    /// <inheritdoc cref="ToolStripDropDown.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        base.Dispose (disposing);
        if (_listView is { IsDisposed: false })
        {
            _listView.Dispose();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public void OnSelected
        (
            SelectedEventArgs args
        )
    {
        Selected?.Invoke (this, args);
    }

    /// <summary>
    /// Shows popup menu immediately
    /// </summary>
    /// <param name="forced">If True - MinFragmentLength will be ignored</param>
    public void Show
        (
            bool forced
        )
    {
        Items.DoAutocomplete (forced);
    }

    #endregion
}
