// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantCheckBeforeAssignment
// ReSharper disable UnusedMember.Global

/* DockContentHandler.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public delegate string GetPersistStringCallback();

/// <summary>
///
/// </summary>
public class DockContentHandler
    : IDisposable, IDockDragSource
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="form"></param>
    public DockContentHandler (Form form)
        : this (form, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="form"></param>
    /// <param name="getPersistStringCallback"></param>
    /// <exception cref="ArgumentException"></exception>
    public DockContentHandler
        (
            Form form,
            GetPersistStringCallback? getPersistStringCallback
        )
    {
        if (form is not IDockContent)
        {
            throw new ArgumentException (Strings.DockContent_Constructor_InvalidForm, nameof (form));
        }

        _tab = null!;
        _autoHideTab = null!;

        PreviousActive = null!;
        NextActive = null!;
        ToolTipText = null!;
        m_tabPageContextMenuStrip = null!;

        Form = form;
        GetPersistStringCallback = getPersistStringCallback;

        Events = new EventHandlerList();
        Form.Disposed += Form_Disposed;
        Form.TextChanged += Form_TextChanged;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Dispose (true);
        GC.SuppressFinalize (this);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose (bool disposing)
    {
        if (disposing)
        {
            DockPanel = null!;
            if (_autoHideTab != null!)
            {
                _autoHideTab.Dispose();
            }

            if (_tab != null!)
            {
                _tab.Dispose();
            }

            Form.Disposed -= Form_Disposed;
            Form.TextChanged -= Form_TextChanged;
            Events.Dispose();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public Form Form { get; }

    /// <summary>
    ///
    /// </summary>
    public IDockContent? Content => Form as IDockContent;

    /// <summary>
    ///
    /// </summary>
    public IDockContent PreviousActive { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public IDockContent NextActive { get; internal set; }

    private EventHandlerList Events { get; }

    /// <summary>
    ///
    /// </summary>
    public bool AllowEndUserDocking { get; set; } = true;

    internal bool SuspendAutoHidePortionUpdates { get; set; } = false;

    private double _autoHidePortion = 0.25;


    /// <summary>
    ///
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public double AutoHidePortion
    {
        get => _autoHidePortion;

        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException (Strings.DockContentHandler_AutoHidePortion_OutOfRange);
            }

            if (SuspendAutoHidePortionUpdates)
            {
                return;
            }

            if (Math.Abs (_autoHidePortion - value) < double.Epsilon)
            {
                return;
            }

            _autoHidePortion = value;

            if (DockPanel == null!)
            {
                return;
            }

            if (DockPanel.ActiveAutoHideContent == Content)
            {
                DockPanel.PerformLayout();
            }
        }
    }

    private bool _closeButton = true;

    /// <summary>
    ///
    /// </summary>
    public bool CloseButton
    {
        get => _closeButton;

        set
        {
            if (_closeButton == value)
            {
                return;
            }

            _closeButton = value;
            if (IsActiveContentHandler)
            {
                Pane.RefreshChanges();
            }
        }
    }

    private bool _closeButtonVisible = true;

    /// <summary>
    /// Determines whether the close button is visible on the content
    /// </summary>
    public bool CloseButtonVisible
    {
        get => _closeButtonVisible;

        set
        {
            if (_closeButtonVisible == value)
            {
                return;
            }

            _closeButtonVisible = value;
            if (IsActiveContentHandler)
            {
                Pane.RefreshChanges();
            }
        }
    }

    private bool IsActiveContentHandler => Pane is { ActiveContent: { } } && Pane.ActiveContent.DockHandler == this;

    private DockState DefaultDockState
    {
        get
        {
            if (ShowHint != DockState.Unknown && ShowHint != DockState.Hidden)
            {
                return ShowHint;
            }

            if ((DockAreas & DockAreas.Document) != 0)
            {
                return DockState.Document;
            }

            if ((DockAreas & DockAreas.DockRight) != 0)
            {
                return DockState.DockRight;
            }

            if ((DockAreas & DockAreas.DockLeft) != 0)
            {
                return DockState.DockLeft;
            }

            if ((DockAreas & DockAreas.DockBottom) != 0)
            {
                return DockState.DockBottom;
            }

            if ((DockAreas & DockAreas.DockTop) != 0)
            {
                return DockState.DockTop;
            }

            return DockState.Unknown;
        }
    }

    private DockState DefaultShowState
    {
        get
        {
            if (ShowHint != DockState.Unknown)
            {
                return ShowHint;
            }

            if ((DockAreas & DockAreas.Document) != 0)
            {
                return DockState.Document;
            }

            if ((DockAreas & DockAreas.DockRight) != 0)
            {
                return DockState.DockRight;
            }

            if ((DockAreas & DockAreas.DockLeft) != 0)
            {
                return DockState.DockLeft;
            }

            if ((DockAreas & DockAreas.DockBottom) != 0)
            {
                return DockState.DockBottom;
            }

            if ((DockAreas & DockAreas.DockTop) != 0)
            {
                return DockState.DockTop;
            }

            if ((DockAreas & DockAreas.Float) != 0)
            {
                return DockState.Float;
            }

            return DockState.Unknown;
        }
    }

    private DockAreas _allowedAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop |
                                       DockAreas.DockBottom | DockAreas.Document | DockAreas.Float;

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public DockAreas DockAreas
    {
        get => _allowedAreas;
        set
        {
            if (_allowedAreas == value)
            {
                return;
            }

            if (!DockHelper.IsDockStateValid (DockState, value))
            {
                throw new InvalidOperationException (Strings.DockContentHandler_DockAreas_InvalidValue);
            }

            _allowedAreas = value;

            if (!DockHelper.IsDockStateValid (ShowHint, _allowedAreas))
            {
                ShowHint = DockState.Unknown;
            }
        }
    }

    private DockState _dockState = DockState.Unknown;

    /// <summary>
    ///
    /// </summary>
    public DockState DockState
    {
        get => _dockState;

        set
        {
            if (_dockState == value)
            {
                return;
            }

            DockPanel.SuspendLayout (true);

            if (value == DockState.Hidden)
            {
                IsHidden = true;
            }
            else
            {
                SetDockState (false, value, Pane);
            }

            DockPanel.ResumeLayout (true, true);
        }
    }

    private DockPanel _dockPanel = null!;

    /// <summary>
    ///
    /// </summary>
    public DockPanel DockPanel
    {
        get => _dockPanel;

        set
        {
            if (_dockPanel == value)
            {
                return;
            }

            Pane = null!;

            if (_dockPanel != null!)
            {
                _dockPanel.RemoveContent (Content!);
            }

            if (_tab != null!)
            {
                _tab.Dispose();
                _tab = null!;
            }

            if (_autoHideTab != null!)
            {
                _autoHideTab.Dispose();
                _autoHideTab = null!;
            }

            _dockPanel = value;
            if (_dockPanel != null!)
            {
                _dockPanel.AddContent (Content!);
                Form.TopLevel = false;
                Form.FormBorderStyle = FormBorderStyle.None;
                Form.ShowInTaskbar = false;
                Form.WindowState = FormWindowState.Normal;
                Content!.ApplyTheme();
                if (Win32Helper.IsRunningOnMono)
                {
                    return;
                }

                Win32.NativeMethods.SetWindowPos (Form.Handle, IntPtr.Zero, 0, 0, 0, 0,
                    Win32.FlagsSetWindowPos.SWP_NOACTIVATE |
                    Win32.FlagsSetWindowPos.SWP_NOMOVE |
                    Win32.FlagsSetWindowPos.SWP_NOSIZE |
                    Win32.FlagsSetWindowPos.SWP_NOZORDER |
                    Win32.FlagsSetWindowPos.SWP_NOOWNERZORDER |
                    Win32.FlagsSetWindowPos.SWP_FRAMECHANGED);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public Icon Icon => Form.Icon!;

    /// <summary>
    ///
    /// </summary>
    public DockPane Pane
    {
        get => IsFloat ? FloatPane : PanelPane;

        set
        {
            if (Pane == value)
            {
                return;
            }

            DockPanel.SuspendLayout (true);

            var oldPane = Pane;

            SuspendSetDockState();
            FloatPane = (value == null! ? null : value.IsFloat ? value : FloatPane)!;
            PanelPane = (value == null ? null : value.IsFloat ? PanelPane : value)!;
            ResumeSetDockState (IsHidden, value?.DockState ?? DockState.Unknown, oldPane);

            DockPanel.ResumeLayout (true, true);
        }
    }

    private bool _isHidden = true;

    /// <summary>
    ///
    /// </summary>
    public bool IsHidden
    {
        get => _isHidden;

        set
        {
            if (_isHidden == value)
            {
                return;
            }

            SetDockState (value, VisibleState, Pane);
        }
    }

    private string _tabText = null!;

    /// <summary>
    ///
    /// </summary>
    public string TabText
    {
        get => _tabText is null or "" ? Form.Text : _tabText;

        set
        {
            if (_tabText == value)
            {
                return;
            }

            _tabText = value;
            if (Pane != null!)
            {
                Pane.RefreshChanges();
            }
        }
    }

    private DockState _visibleState = DockState.Unknown;

    /// <summary>
    ///
    /// </summary>
    public DockState VisibleState
    {
        get => _visibleState;

        set
        {
            if (_visibleState == value)
            {
                return;
            }

            SetDockState (IsHidden, value, Pane);
        }
    }

    private bool _isFloat;

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsFloat
    {
        get => _isFloat;

        set
        {
            if (_isFloat == value)
            {
                return;
            }

            var visibleState = CheckDockState (value);

            if (visibleState == DockState.Unknown)
            {
                throw new InvalidOperationException (Strings.DockContentHandler_IsFloat_InvalidValue);
            }

            SetDockState (IsHidden, visibleState, Pane);
            if (PatchController.EnableFloatSplitterFix == true)
            {
                if (PanelPane is { IsHidden: true })
                {
                    PanelPane.NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild (PanelPane);
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="isFloat"></param>
    /// <returns></returns>
    [SuppressMessage ("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
    public DockState CheckDockState (bool isFloat)
    {
        DockState dockState;

        if (isFloat)
        {
            dockState = !IsDockStateValid (DockState.Float) ? DockState.Unknown : DockState.Float;
        }
        else
        {
            dockState = PanelPane != null! ? PanelPane.DockState : DefaultDockState;
            if (dockState != DockState.Unknown && !IsDockStateValid (dockState))
            {
                dockState = DockState.Unknown;
            }
        }

        return dockState;
    }

    private DockPane _panelPane = null!;

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public DockPane PanelPane
    {
        get => _panelPane;

        set
        {
            if (_panelPane == value)
            {
                return;
            }

            if (value != null!)
            {
                if (value.IsFloat || value.DockPanel != DockPanel)
                {
                    throw new InvalidOperationException (Strings.DockContentHandler_DockPane_InvalidValue);
                }
            }

            var oldPane = Pane;

            if (_panelPane != null!)
            {
                RemoveFromPane (_panelPane);
            }

            _panelPane = value!;
            if (_panelPane != null!)
            {
                _panelPane.AddContent (Content!);
                SetDockState (IsHidden, IsFloat ? DockState.Float : _panelPane.DockState, oldPane);
            }
            else
            {
                SetDockState (IsHidden, DockState.Unknown, oldPane);
            }
        }
    }

    private void RemoveFromPane (DockPane pane)
    {
        pane.RemoveContent (Content!);
        SetPane (null!);
        if (pane.Contents.Count == 0)
        {
            pane.Dispose();
        }
    }

    private DockPane _floatPane = null!;

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public DockPane FloatPane
    {
        get => _floatPane;

        set
        {
            if (_floatPane == value)
            {
                return;
            }

            if (value != null!)
            {
                if (!value.IsFloat || value.DockPanel != DockPanel)
                {
                    throw new InvalidOperationException (Strings.DockContentHandler_FloatPane_InvalidValue);
                }
            }

            var oldPane = Pane;

            if (_floatPane != null!)
            {
                RemoveFromPane (_floatPane);
            }

            _floatPane = value!;
            if (_floatPane != null!)
            {
                _floatPane.AddContent (Content!);
                SetDockState (IsHidden, IsFloat ? DockState.Float : VisibleState, oldPane);
            }
            else
            {
                SetDockState (IsHidden, DockState.Unknown, oldPane);
            }
        }
    }

    private int _countSetDockState;

    private void SuspendSetDockState()
    {
        _countSetDockState++;
    }

    private void ResumeSetDockState()
    {
        _countSetDockState--;
        if (_countSetDockState < 0)
        {
            _countSetDockState = 0;
        }
    }

    internal bool IsSuspendSetDockState => _countSetDockState != 0;

    private void ResumeSetDockState (bool isHidden, DockState visibleState, DockPane oldPane)
    {
        ResumeSetDockState();
        SetDockState (isHidden, visibleState, oldPane);
    }

    internal void SetDockState (bool isHidden, DockState visibleState, DockPane oldPane)
    {
        if (IsSuspendSetDockState)
        {
            return;
        }

        if (DockPanel == null && visibleState != DockState.Unknown)
        {
            throw new InvalidOperationException (Strings.DockContentHandler_SetDockState_NullPanel);
        }

        if (visibleState == DockState.Hidden ||
            visibleState != DockState.Unknown && !IsDockStateValid (visibleState))
        {
            throw new InvalidOperationException (Strings.DockContentHandler_SetDockState_InvalidState);
        }

        var dockPanel = DockPanel!;
        if (dockPanel != null!)
        {
            dockPanel.SuspendLayout (true);
        }

        SuspendSetDockState();

        var oldDockState = DockState;

        if (_isHidden != isHidden || oldDockState == DockState.Unknown)
        {
            _isHidden = isHidden;
        }

        _visibleState = visibleState;
        _dockState = isHidden ? DockState.Hidden : visibleState;

        //Remove hidden content (shown content is added last so removal is done first to invert the operation)
        var hidingContent = DockState is DockState.Hidden or DockState.Unknown ||
                            DockHelper.IsDockStateAutoHide (DockState);
        if (PatchController.EnableContentOrderFix == true && oldDockState != DockState)
        {
            if (hidingContent)
            {
                if (!Win32Helper.IsRunningOnMono)
                {
                    DockPanel!.ContentFocusManager.RemoveFromList (Content!);
                }
            }
        }

        if (visibleState == DockState.Unknown)
        {
            Pane = null!;
        }
        else
        {
            _isFloat = _visibleState == DockState.Float;

            if (Pane == null!)
            {
                Pane = DockPanel!.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, visibleState, true);
            }
            else if (Pane.DockState != visibleState)
            {
                if (Pane.Contents.Count == 1)
                {
                    Pane.SetDockState (visibleState);
                }
                else
                {
                    Pane = DockPanel!.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, visibleState, true);
                }
            }
        }

        if (Form.ContainsFocus)
        {
            if (DockState is DockState.Hidden or DockState.Unknown)
            {
                if (!Win32Helper.IsRunningOnMono)
                {
                    DockPanel!.ContentFocusManager.GiveUpFocus (Content!);
                }
            }
        }

        SetPaneAndVisible (Pane);

        if (oldPane is { IsDisposed: false } && oldDockState == oldPane.DockState)
        {
            RefreshDockPane (oldPane);
        }

        if (Pane != null! && DockState == Pane.DockState)
        {
            if (Pane != oldPane ||
                Pane == oldPane && oldDockState != oldPane.DockState)
            {
                // Avoid early refresh of hidden AutoHide panes
                if ((Pane.DockWindow == null! || Pane.DockWindow.Visible || Pane.IsHidden) && !Pane.IsAutoHide)
                {
                    RefreshDockPane (Pane);
                }
            }
        }

        if (oldDockState != DockState)
        {
            if (PatchController.EnableContentOrderFix == true)
            {
                //Add content that is being shown
                if (!hidingContent)
                {
                    if (!Win32Helper.IsRunningOnMono)
                    {
                        DockPanel!.ContentFocusManager.AddToList (Content!);
                    }
                }
            }
            else
            {
                if (DockState is DockState.Hidden or DockState.Unknown ||
                    DockHelper.IsDockStateAutoHide (DockState))
                {
                    if (!Win32Helper.IsRunningOnMono)
                    {
                        DockPanel!.ContentFocusManager.RemoveFromList (Content!);
                    }
                }
                else if (!Win32Helper.IsRunningOnMono)
                {
                    DockPanel!.ContentFocusManager.AddToList (Content!);
                }
            }

            ResetAutoHidePortion (oldDockState, DockState);
            OnDockStateChanged (EventArgs.Empty);
        }

        ResumeSetDockState();

        if (dockPanel != null)
        {
            dockPanel.ResumeLayout (true, true);
        }
    }

    private void ResetAutoHidePortion (DockState oldState, DockState newState)
    {
        if (oldState == newState || DockHelper.ToggleAutoHideState (oldState) == newState)
        {
            return;
        }

        switch (newState)
        {
            case DockState.DockTop:
            case DockState.DockTopAutoHide:
                AutoHidePortion = DockPanel.DockTopPortion;
                break;
            case DockState.DockLeft:
            case DockState.DockLeftAutoHide:
                AutoHidePortion = DockPanel.DockLeftPortion;
                break;
            case DockState.DockBottom:
            case DockState.DockBottomAutoHide:
                AutoHidePortion = DockPanel.DockBottomPortion;
                break;
            case DockState.DockRight:
            case DockState.DockRightAutoHide:
                AutoHidePortion = DockPanel.DockRightPortion;
                break;
        }
    }

    private static void RefreshDockPane (DockPane pane)
    {
        pane.RefreshChanges();
        pane.ValidateActiveContent();
    }

    internal string PersistString => GetPersistStringCallback == null ? Form.GetType().ToString() : GetPersistStringCallback();

    /// <summary>
    ///
    /// </summary>
    public GetPersistStringCallback? GetPersistStringCallback { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool HideOnClose { get; set; }

    private DockState _showHint = DockState.Unknown;

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public DockState ShowHint
    {
        get => _showHint;

        set
        {
            if (!DockHelper.IsDockStateValid (value, DockAreas))
            {
                throw new InvalidOperationException (Strings.DockContentHandler_ShowHint_InvalidValue);
            }

            if (_showHint == value)
            {
                return;
            }

            _showHint = value;
        }
    }

    private bool _isActivated;

    /// <summary>
    ///
    /// </summary>
    public bool IsActivated
    {
        get => _isActivated;

        internal set
        {
            if (_isActivated == value)
            {
                return;
            }

            _isActivated = value;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockState"></param>
    /// <returns></returns>
    public bool IsDockStateValid (DockState dockState)
    {
        if (DockPanel != null! && dockState == DockState.Document &&
            DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
        {
            return false;
        }
        else
        {
            return DockHelper.IsDockStateValid (dockState, DockAreas);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public string ToolTipText { get; set; }

    public void Activate()
    {
        if (DockPanel == null!)
        {
            Form.Activate();
        }
        else if (Pane == null!)
        {
            Show (DockPanel);
        }
        else
        {
            IsHidden = false;
            Pane.ActiveContent = Content;
            if (DockState == DockState.Document && DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                Form.Activate();
                return;
            }
            else if (DockHelper.IsDockStateAutoHide (DockState))
            {
                if (DockPanel.ActiveAutoHideContent != Content)
                {
                    DockPanel.ActiveAutoHideContent = null;
                    return;
                }
            }

            if (Form.ContainsFocus)
            {
                return;
            }

            if (Win32Helper.IsRunningOnMono)
            {
                return;
            }

            DockPanel.ContentFocusManager.Activate (Content!);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void GiveUpFocus()
    {
        if (!Win32Helper.IsRunningOnMono)
        {
            DockPanel.ContentFocusManager.GiveUpFocus (Content!);
        }
    }

    internal IntPtr ActiveWindowHandle { get; set; } = IntPtr.Zero;

    /// <summary>
    ///
    /// </summary>
    public void Hide()
    {
        IsHidden = true;
    }

    internal void SetPaneAndVisible (DockPane pane)
    {
        SetPane (pane);
        SetVisible();
    }

    private void SetPane (DockPane pane)
    {
        if (pane is { DockState: DockState.Document } &&
            DockPanel.DocumentStyle == DocumentStyle.DockingMdi)
        {
            if (Form.Parent is DockPane)
            {
                SetParent (null!);
            }

            if (Form.MdiParent != DockPanel.ParentForm)
            {
                FlagClipWindow = true;

                // The content form should inherit the font of the dock panel, not the font of
                // the dock panel's parent form. However, the content form's font value should
                // not be overwritten if it has been explicitly set to a non-default value.
                if (PatchController.EnableFontInheritanceFix == true && Form.Font == Control.DefaultFont)
                {
                    Form.MdiParent = DockPanel.ParentForm;
                    Form.Font = DockPanel.Font;
                }
                else
                {
                    Form.MdiParent = DockPanel.ParentForm;
                }
            }
        }
        else
        {
            FlagClipWindow = true;
            if (Form.MdiParent != null)
            {
                Form.MdiParent = null;
            }

            if (Form.TopLevel)
            {
                Form.TopLevel = false;
            }

            SetParent (pane);
        }
    }

    internal void SetVisible()
    {
        bool visible;

        if (IsHidden)
        {
            visible = false;
        }
        else if (Pane is { DockState: DockState.Document } &&
                 DockPanel.DocumentStyle == DocumentStyle.DockingMdi)
        {
            visible = true;
        }
        else if (Pane != null! && Pane.ActiveContent == Content)
        {
            visible = true;
        }
        else if (Pane != null && Pane.ActiveContent != Content)
        {
            visible = false;
        }
        else
        {
            visible = Form.Visible;
        }

        if (Form.Visible != visible)
        {
            Form.Visible = visible;
        }
    }

    private void SetParent (Control value)
    {
        if (Form.Parent == value)
        {
            return;
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Workaround of .Net Framework bug:
        // Change the parent of a control with focus may result in the first
        // MDI child form get activated.
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        var bRestoreFocus = false;
        if (Form.ContainsFocus)
        {
            // Suggested as a fix for a memory leak by bugreports
            if (value == null! && !IsFloat)
            {
                if (!Win32Helper.IsRunningOnMono)
                {
                    DockPanel.ContentFocusManager.GiveUpFocus (Content!);
                }
            }
            else
            {
                DockPanel.SaveFocus();
                bRestoreFocus = true;
            }
        }

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        var parentChanged = value != Form.Parent;
        Form.Parent = value;
        if (PatchController.EnableMainWindowFocusLostFix == true && parentChanged)
        {
            Form.Focus();
        }

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Workaround of .Net Framework bug:
        // Change the parent of a control with focus may result in the first
        // MDI child form get activated.
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (bRestoreFocus && !Win32Helper.IsRunningOnMono)
        {
            Activate();
        }

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    /// <summary>
    ///
    /// </summary>
    public void Show()
    {
        if (DockPanel == null!)
        {
            Form.Show();
        }
        else
        {
            Show (DockPanel);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockPanel"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Show (DockPanel dockPanel)
    {
        if (dockPanel == null)
        {
            throw new ArgumentNullException (Strings.DockContentHandler_Show_NullDockPanel);
        }

        if (DockState == DockState.Unknown)
        {
            Show (dockPanel, DefaultShowState);
        }
        else if (DockPanel != dockPanel)
        {
            Show (dockPanel, DockState == DockState.Hidden ? _visibleState : DockState);
        }
        else
        {
            Activate();
        }
    }

    public void Show (DockPanel dockPanel, DockState dockState)
    {
        if (dockPanel == null)
        {
            throw new ArgumentNullException (Strings.DockContentHandler_Show_NullDockPanel);
        }

        if (dockState is DockState.Unknown or DockState.Hidden)
        {
            throw new ArgumentException (Strings.DockContentHandler_Show_InvalidDockState);
        }

        if (dockPanel.Theme.GetType() == typeof (DefaultTheme))
        {
            throw new ArgumentException (Strings.Theme_NoTheme);
        }

        dockPanel.SuspendLayout (true);

        DockPanel = dockPanel;

        if (dockState == DockState.Float)
        {
            if (FloatPane == null!)
            {
                Pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, DockState.Float, true);
            }
        }
        else if (PanelPane == null!)
        {
            DockPane? paneExisting = null;
            foreach (var pane in DockPanel.Panes)
            {
                if (pane.DockState == dockState)
                {
                    if (paneExisting == null || pane.IsActivated)
                    {
                        paneExisting = pane;
                    }

                    if (pane.IsActivated)
                    {
                        break;
                    }
                }
            }

            Pane = paneExisting ?? DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, dockState, true);
        }

        DockState = dockState;
        dockPanel.ResumeLayout (true, true); //we'll resume the layout before activating to ensure that the position
        Activate(); //and size of the form are finally processed before the form is shown
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockPanel"></param>
    /// <param name="floatWindowBounds"></param>
    /// <exception cref="ArgumentNullException"></exception>
    [SuppressMessage ("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
    public void Show (DockPanel dockPanel, Rectangle floatWindowBounds)
    {
        if (dockPanel == null)
        {
            throw new ArgumentNullException (Strings.DockContentHandler_Show_NullDockPanel);
        }

        dockPanel.SuspendLayout (true);

        DockPanel = dockPanel;
        if (FloatPane == null!)
        {
            IsHidden = true; // to reduce the screen flicker
            FloatPane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, DockState.Float, false);
            FloatPane.FloatWindow!.StartPosition = FormStartPosition.Manual;
        }

        FloatPane.FloatWindow!.Bounds = floatWindowBounds;

        Show (dockPanel, DockState.Float);
        Activate();

        dockPanel.ResumeLayout (true, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pane"></param>
    /// <param name="beforeContent"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void Show (DockPane pane, IDockContent beforeContent)
    {
        if (pane == null)
        {
            throw new ArgumentNullException (Strings.DockContentHandler_Show_NullPane);
        }

        if (beforeContent != null! && pane.Contents.IndexOf (beforeContent) == -1)
        {
            throw new ArgumentException (Strings.DockContentHandler_Show_InvalidBeforeContent);
        }

        pane.DockPanel.SuspendLayout (true);

        DockPanel = pane.DockPanel;
        Pane = pane;
        pane.SetContentIndex (Content!, pane.Contents.IndexOf (beforeContent!));
        Show();

        pane.DockPanel.ResumeLayout (true, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="previousPane"></param>
    /// <param name="alignment"></param>
    /// <param name="proportion"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Show (DockPane previousPane, DockAlignment alignment, double proportion)
    {
        if (previousPane == null)
        {
            throw new ArgumentException (Strings.DockContentHandler_Show_InvalidPrevPane);
        }

        if (DockHelper.IsDockStateAutoHide (previousPane.DockState))
        {
            throw new ArgumentException (Strings.DockContentHandler_Show_InvalidPrevPane);
        }

        previousPane.DockPanel.SuspendLayout (true);

        DockPanel = previousPane.DockPanel;
        DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, previousPane, alignment, proportion,
            true);
        Show();

        previousPane.DockPanel.ResumeLayout (true, true);
    }

    /// <summary>
    ///
    /// </summary>
    public void Close()
    {
        var dockPanel = DockPanel;
        if (dockPanel != null!)
        {
            dockPanel.SuspendLayout (true);
        }

        Form.Close();
        if (dockPanel != null)
        {
            dockPanel.ResumeLayout (true, true);
        }
    }

    private DockPaneStripBase.Tab _tab;

    internal DockPaneStripBase.Tab GetTab (DockPaneStripBase dockPaneStrip)
    {
        if (_tab == null!)
        {
            _tab = dockPaneStrip.CreateTab (Content!);
        }

        return _tab;
    }

    private IDisposable _autoHideTab;

    internal IDisposable AutoHideTab
    {
        get => _autoHideTab;
        set => _autoHideTab = value;
    }

    #region Events

    private static readonly object DockStateChangedEvent = new object();

    /// <summary>
    ///
    /// </summary>
    public event EventHandler DockStateChanged
    {
        add => Events.AddHandler (DockStateChangedEvent, value);
        remove => Events.RemoveHandler (DockStateChangedEvent, value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventArgs"></param>
    protected virtual void OnDockStateChanged (EventArgs eventArgs)
    {
        var handler = (EventHandler?) Events[DockStateChangedEvent];
        if (handler != null)
        {
            handler (this, eventArgs);
        }
    }

    #endregion

    private void Form_Disposed (object? sender, EventArgs e)
    {
        Dispose();
    }

    private void Form_TextChanged (object? sender, EventArgs e)
    {
        if (DockHelper.IsDockStateAutoHide (DockState))
        {
            DockPanel.RefreshAutoHideStrip();
        }
        else if (Pane != null!)
        {
            if (Pane.FloatWindow != null)
            {
                Pane.FloatWindow.SetText();
            }

            Pane.RefreshChanges();
        }
    }

    private bool _flagClipWindow;

    internal bool FlagClipWindow
    {
        get => _flagClipWindow;

        set
        {
            if (_flagClipWindow == value)
            {
                return;
            }

            _flagClipWindow = value;
            Form.Region = _flagClipWindow
                ? new Region (Rectangle.Empty)
                : null;
        }
    }

    private ContextMenuStrip m_tabPageContextMenuStrip;

    /// <summary>
    ///
    /// </summary>
    public ContextMenuStrip TabPageContextMenuStrip
    {
        get => m_tabPageContextMenuStrip;

        set
        {
            if (value == m_tabPageContextMenuStrip)
            {
                return;
            }

            m_tabPageContextMenuStrip = value;
            ApplyTheme();
        }
    }

    internal void ApplyTheme()
    {
        if (m_tabPageContextMenuStrip != null! && DockPanel != null!)
        {
            DockPanel.Theme.ApplyTo (m_tabPageContextMenuStrip);
        }
    }

    #region IDockDragSource Members

    Control IDragSource.DragControl => Form;

    bool IDockDragSource.CanDockTo (DockPane pane)
    {
        if (!IsDockStateValid (pane.DockState))
        {
            return false;
        }

        if (Pane == pane && pane.DisplayingContents.Count == 1)
        {
            return false;
        }

        return true;
    }

    Rectangle IDockDragSource.BeginDrag (Point ptMouse)
    {
        Size size;
        var floatPane = FloatPane;
        if (DockState == DockState.Float
            || floatPane == null!
            || floatPane.FloatWindow!.NestedPanes.Count != 1)
        {
            size = DockPanel.DefaultFloatWindowSize;
        }
        else
        {
            size = floatPane.FloatWindow.Size;
        }

        Point location;
        var rectPane = Pane.ClientRectangle;
        if (DockState == DockState.Document)
        {
            location = Pane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom
                ? new Point (rectPane.Left, rectPane.Bottom - size.Height)
                : new Point (rectPane.Left, rectPane.Top);
        }
        else
        {
            location = new Point (rectPane.Left, rectPane.Bottom);
            location.Y -= size.Height;
        }

        location = Pane.PointToScreen (location);

        if (ptMouse.X > location.X + size.Width)
        {
            location.X += ptMouse.X - (location.X + size.Width) + DockPanel.Theme.Measures.SplitterSize;
        }

        return new Rectangle (location, size);
    }

    void IDockDragSource.EndDrag()
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="floatWindowBounds"></param>
    public void FloatAt (Rectangle floatWindowBounds)
    {
        // TODO: where is the pane used?
        var pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, floatWindowBounds, true);
    }

    public void DockTo (DockPane pane, DockStyle dockStyle, int contentIndex)
    {
        if (dockStyle == DockStyle.Fill)
        {
            var samePane = Pane == pane;
            if (!samePane)
            {
                Pane = pane;
            }

            var visiblePanes = 0;
            var convertedIndex = 0;
            while (visiblePanes <= contentIndex && convertedIndex < Pane.Contents.Count)
            {
                if (Pane.Contents[convertedIndex] is DockContent { IsHidden: false } window)
                {
                    ++visiblePanes;
                }

                ++convertedIndex;
            }

            contentIndex = Math.Min (Math.Max (0, convertedIndex - 1), Pane.Contents.Count - 1);

            if (contentIndex == -1 || !samePane)
            {
                pane.SetContentIndex (Content!, contentIndex);
            }
            else
            {
                var contents = pane.Contents;
                var oldIndex = contents.IndexOf (Content!);
                var newIndex = contentIndex;
                if (oldIndex < newIndex)
                {
                    newIndex += 1;
                    if (newIndex > contents.Count - 1)
                    {
                        newIndex = -1;
                    }
                }

                pane.SetContentIndex (Content!, newIndex);
            }
        }
        else
        {
            var paneFrom =
                DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, pane.DockState, true);
            var container = pane.NestedPanesContainer;
            if (dockStyle == DockStyle.Left)
            {
                paneFrom.DockTo (container!, pane, DockAlignment.Left, 0.5);
            }
            else if (dockStyle == DockStyle.Right)
            {
                paneFrom.DockTo (container!, pane, DockAlignment.Right, 0.5);
            }
            else if (dockStyle == DockStyle.Top)
            {
                paneFrom.DockTo (container!, pane, DockAlignment.Top, 0.5);
            }
            else if (dockStyle == DockStyle.Bottom)
            {
                paneFrom.DockTo (container!, pane, DockAlignment.Bottom, 0.5);
            }

            paneFrom.DockState = pane.DockState;
        }

        if (PatchController.EnableActivateOnDockFix == true)
        {
            Pane.ActiveContent = Content;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="dockStyle"></param>
    /// <exception cref="ArgumentException"></exception>
    public void DockTo (DockPanel panel, DockStyle dockStyle)
    {
        if (panel != DockPanel)
        {
            throw new ArgumentException (Strings.IDockDragSource_DockTo_InvalidPanel, nameof (panel));
        }

        DockPane pane;

        if (dockStyle == DockStyle.Top)
        {
            pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, DockState.DockTop, true);
        }
        else if (dockStyle == DockStyle.Bottom)
        {
            pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, DockState.DockBottom, true);
        }
        else if (dockStyle == DockStyle.Left)
        {
            pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, DockState.DockLeft, true);
        }
        else if (dockStyle == DockStyle.Right)
        {
            pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, DockState.DockRight, true);
        }
        else if (dockStyle == DockStyle.Fill)
        {
            pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane (Content!, DockState.Document, true);
        }
        else
        {
            return;
        }
    }

    #endregion
}
