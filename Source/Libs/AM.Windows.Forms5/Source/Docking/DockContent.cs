// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DockContent.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public class DockContent
    : Form, IDockContent
{
    /// <summary>
    ///
    /// </summary>
    public DockContent()
    {
        _tabText = null!;

        _dockHandler = new DockContentHandler (this, GetPersistString);
        _dockHandler.DockStateChanged += DockHandler_DockStateChanged;
        if (PatchController.EnableFontInheritanceFix != true)
        {
            //Suggested as a fix by bensty regarding form resize
            ParentChanged += DockContent_ParentChanged;
        }
    }

    //Suggested as a fix by bensty regarding form resize
    private void DockContent_ParentChanged
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        if (Parent != null)
        {
            Font = Parent.Font;
        }
    }

    private DockContentHandler _dockHandler;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public DockContentHandler DockHandler => _dockHandler;

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_AllowEndUserDocking_Description")]
    [DefaultValue (true)]
    public bool AllowEndUserDocking
    {
        get => DockHandler.AllowEndUserDocking;
        set => DockHandler.AllowEndUserDocking = value;
    }

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_DockAreas_Description")]
    [DefaultValue (DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom |
                   DockAreas.Document | DockAreas.Float)]
    public DockAreas DockAreas
    {
        get => DockHandler.DockAreas;
        set => DockHandler.DockAreas = value;
    }

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_AutoHidePortion_Description")]
    [DefaultValue (0.25)]
    public double AutoHidePortion
    {
        get => DockHandler.AutoHidePortion;
        set => DockHandler.AutoHidePortion = value;
    }

    private string _tabText;

    /// <summary>
    ///
    /// </summary>
    [Localizable (true)]
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_TabText_Description")]
    [DefaultValue (null)]
    public string TabText
    {
        get => _tabText;
        set => DockHandler.TabText = _tabText = value;
    }

    private bool ShouldSerializeTabText()
    {
        return (_tabText != null!);
    }

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_CloseButton_Description")]
    [DefaultValue (true)]
    public bool CloseButton
    {
        get => DockHandler.CloseButton;
        set => DockHandler.CloseButton = value;
    }

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_CloseButtonVisible_Description")]
    [DefaultValue (true)]
    public bool CloseButtonVisible
    {
        get => DockHandler.CloseButtonVisible;
        set => DockHandler.CloseButtonVisible = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public DockPanel DockPanel
    {
        get => DockHandler.DockPanel;
        set => DockHandler.DockPanel = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public DockState DockState
    {
        get => DockHandler.DockState;
        set => DockHandler.DockState = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public DockPane Pane
    {
        get => DockHandler.Pane;
        set => DockHandler.Pane = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public bool IsHidden
    {
        get => DockHandler.IsHidden;
        set => DockHandler.IsHidden = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public DockState VisibleState
    {
        get => DockHandler.VisibleState;
        set => DockHandler.VisibleState = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public bool IsFloat
    {
        get => DockHandler.IsFloat;
        set => DockHandler.IsFloat = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public DockPane PanelPane
    {
        get => DockHandler.PanelPane;
        set => DockHandler.PanelPane = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public DockPane FloatPane
    {
        get => DockHandler.FloatPane;
        set => DockHandler.FloatPane = value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [SuppressMessage ("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    protected virtual string GetPersistString()
    {
        return GetType().ToString();
    }

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_HideOnClose_Description")]
    [DefaultValue (false)]
    public bool HideOnClose
    {
        get => DockHandler.HideOnClose;
        set => DockHandler.HideOnClose = value;
    }

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_ShowHint_Description")]
    [DefaultValue (DockState.Unknown)]
    public DockState ShowHint
    {
        get => DockHandler.ShowHint;
        set => DockHandler.ShowHint = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public bool IsActivated => DockHandler.IsActivated;

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockState"></param>
    /// <returns></returns>
    public bool IsDockStateValid (DockState dockState)
    {
        return DockHandler.IsDockStateValid (dockState);
    }

    /// <summary>
    /// Context menu strip.
    /// </summary>
    [LocalizedCategory ("Category_Docking")]
    [LocalizedDescription ("DockContent_TabPageContextMenuStrip_Description")]
    [DefaultValue (null)]
    public ContextMenuStrip TabPageContextMenuStrip
    {
        get => DockHandler.TabPageContextMenuStrip;
        set => DockHandler.TabPageContextMenuStrip = value;
    }

    void IContextMenuStripHost.ApplyTheme()
    {
        DockHandler.ApplyTheme();

        if (DockPanel != null!)
        {
            if (MainMenuStrip != null)
            {
                DockPanel.Theme.ApplyTo (MainMenuStrip);
            }

            if (ContextMenuStrip != null)
            {
                DockPanel.Theme.ApplyTo (ContextMenuStrip);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Localizable (true)]
    [Category ("Appearance")]
    [LocalizedDescription ("DockContent_ToolTipText_Description")]
    [DefaultValue (null)]
    public string ToolTipText
    {
        get => DockHandler.ToolTipText;
        set => DockHandler.ToolTipText = value;
    }

    /// <summary>
    ///
    /// </summary>
    public new void Activate()
    {
        DockHandler.Activate();
    }

    /// <summary>
    ///
    /// </summary>
    public new void Hide()
    {
        DockHandler.Hide();
    }

    /// <summary>
    ///
    /// </summary>
    public new void Show()
    {
        DockHandler.Show();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockPanel"></param>
    public void Show (DockPanel dockPanel)
    {
        DockHandler.Show (dockPanel);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockPanel"></param>
    /// <param name="dockState"></param>
    public void Show (DockPanel dockPanel, DockState dockState)
    {
        DockHandler.Show (dockPanel, dockState);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockPanel"></param>
    /// <param name="floatWindowBounds"></param>
    [SuppressMessage ("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
    public void Show (DockPanel dockPanel, Rectangle floatWindowBounds)
    {
        DockHandler.Show (dockPanel, floatWindowBounds);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pane"></param>
    /// <param name="beforeContent"></param>
    public void Show (DockPane pane, IDockContent beforeContent)
    {
        DockHandler.Show (pane, beforeContent);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="previousPane"></param>
    /// <param name="alignment"></param>
    /// <param name="proportion"></param>
    public void Show (DockPane previousPane, DockAlignment alignment, double proportion)
    {
        DockHandler.Show (previousPane, alignment, proportion);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="floatWindowBounds"></param>
    [SuppressMessage ("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
    public void FloatAt (Rectangle floatWindowBounds)
    {
        DockHandler.FloatAt (floatWindowBounds);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="paneTo"></param>
    /// <param name="dockStyle"></param>
    /// <param name="contentIndex"></param>
    public void DockTo (DockPane paneTo, DockStyle dockStyle, int contentIndex)
    {
        DockHandler.DockTo (paneTo, dockStyle, contentIndex);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="dockStyle"></param>
    public void DockTo (DockPanel panel, DockStyle dockStyle)
    {
        DockHandler.DockTo (panel, dockStyle);
    }

    #region IDockContent Members

    void IDockContent.OnActivated (EventArgs e)
    {
        OnActivated (e);
    }

    void IDockContent.OnDeactivate (EventArgs e)
    {
        OnDeactivate (e);
    }

    #endregion

    #region Events

    private void DockHandler_DockStateChanged
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        OnDockStateChanged (eventArgs);
    }

    private static readonly object _dockStateChangedEvent = new ();

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_PropertyChanged")]
    [LocalizedDescription ("Pane_DockStateChanged_Description")]
    public event EventHandler DockStateChanged
    {
        add => Events.AddHandler (_dockStateChangedEvent, value);
        remove => Events.RemoveHandler (_dockStateChangedEvent, value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnDockStateChanged (EventArgs e)
    {
        ((EventHandler?) Events[_dockStateChangedEvent])?.Invoke (this, e);
    }

    #endregion

    /// <summary>
    /// Overridden to avoid resize issues with nested controls
    /// </summary>
    /// <remarks>
    /// http://blogs.msdn.com/b/alejacma/archive/2008/11/20/controls-won-t-get-resized-once-the-nesting-hierarchy-of-windows-exceeds-a-certain-depth-x64.aspx
    /// http://support.microsoft.com/kb/953934
    /// </remarks>
    protected override void OnSizeChanged (EventArgs e)
    {
        if (DockPanel is { SupportDeeplyNestedContent: true } && IsHandleCreated)
        {
            BeginInvoke ((MethodInvoker)delegate { base.OnSizeChanged (e); });
        }
        else
        {
            base.OnSizeChanged (e);
        }
    }
}
