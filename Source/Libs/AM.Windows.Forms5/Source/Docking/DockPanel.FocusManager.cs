// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Local
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Global

/* DockPanel.FocusManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

internal interface IContentFocusManager
{
    void Activate (IDockContent content);
    void GiveUpFocus (IDockContent content);
    void AddToList (IDockContent content);
    void RemoveFromList (IDockContent content);
}

partial class DockPanel
{
    private interface IFocusManager
    {
        void SuspendFocusTracking();
        void ResumeFocusTracking();
        bool IsFocusTrackingSuspended { get; }
        IDockContent ActiveContent { get; }
        DockPane? ActivePane { get; }
        IDockContent ActiveDocument { get; }
        DockPane ActiveDocumentPane { get; }
    }

    private class FocusManagerImpl : Component, IContentFocusManager, IFocusManager
    {
        private class HookEventArgs : EventArgs
        {
            [SuppressMessage ("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            public int HookCode;

            [SuppressMessage ("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            public IntPtr wParam;

            public IntPtr lParam;
        }

        private class LocalWindowsHook : IDisposable
        {
            // Internal properties
            private IntPtr m_hHook = IntPtr.Zero;
            private AM.Windows.Forms.Docking.Win32.NativeMethods.HookProc? _filterFunc;
            private Win32.HookType m_hookType;

            // Event delegate
            public delegate void HookEventHandler (object sender, HookEventArgs e);

            // Event: HookInvoked
            public event HookEventHandler? HookInvoked;

            /// <summary>
            ///
            /// </summary>
            /// <param name="e"></param>
            protected void OnHookInvoked (HookEventArgs e)
            {
                if (HookInvoked != null!)
                {
                    HookInvoked (this, e);
                }
            }

            public LocalWindowsHook (Win32.HookType hook)
            {
                m_hookType = hook;
                _filterFunc = new AM.Windows.Forms.Docking.Win32.NativeMethods.HookProc (this.CoreHookProc);
            }

            // Default filter function
            public IntPtr CoreHookProc (int code, IntPtr wParam, IntPtr lParam)
            {
                if (code < 0)
                {
                    return AM.Windows.Forms.Docking.Win32.NativeMethods.CallNextHookEx (m_hHook, code, wParam,
                        lParam);
                }

                // Let clients determine what to do
                var e = new HookEventArgs();
                e.HookCode = code;
                e.wParam = wParam;
                e.lParam = lParam;
                OnHookInvoked (e);

                // Yield to the next hook in the chain
                return Win32.NativeMethods.CallNextHookEx (m_hHook, code, wParam, lParam);
            }

            // Install the hook
            public void Install()
            {
                if (m_hHook != IntPtr.Zero)
                {
                    Uninstall();
                }

                var threadId = Win32.NativeMethods.GetCurrentThreadId();
                m_hHook = Win32.NativeMethods.SetWindowsHookEx (m_hookType, _filterFunc!, IntPtr.Zero, threadId);
            }

            /// <summary>
            /// Uninstall the hook
            /// </summary>
            public void Uninstall()
            {
                if (m_hHook != IntPtr.Zero)
                {
                    Win32.NativeMethods.UnhookWindowsHookEx (m_hHook);
                    m_hHook = IntPtr.Zero;
                }
            }

            ~LocalWindowsHook()
            {
                Dispose (false);
            }

            public void Dispose()
            {
                Dispose (true);
                GC.SuppressFinalize (this);
            }

            protected virtual void Dispose (bool disposing)
            {
                Uninstall();
            }
        }

        // Use a static instance of the windows hook to prevent stack overflows in the windows kernel.
        [ThreadStatic] private static LocalWindowsHook? _localWindowsHook;
        [ThreadStatic] private static int _referenceCount;

        private readonly LocalWindowsHook.HookEventHandler? m_hookEventHandler;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dockPanel"></param>
        public FocusManagerImpl (DockPanel dockPanel)
        {
            DockPanel = dockPanel;
            if (Win32Helper.IsRunningOnMono)
            {
                return;
            }

            m_hookEventHandler = new LocalWindowsHook.HookEventHandler (HookEventHandler);

            // Ensure the windows hook has been created for this thread
            if (_localWindowsHook == null)
            {
                _localWindowsHook = new LocalWindowsHook (Win32.HookType.WH_CALLWNDPROCRET);
                _localWindowsHook.Install();
            }

            _localWindowsHook.HookInvoked += m_hookEventHandler;
            ++_referenceCount;
        }

        /// <summary>
        ///
        /// </summary>
        public DockPanel DockPanel { get; }

        private bool m_disposed;

        protected override void Dispose (bool disposing)
        {
            if (!m_disposed && disposing)
            {
                if (!Win32Helper.IsRunningOnMono)
                {
                    _localWindowsHook!.HookInvoked -= m_hookEventHandler;
                }

                --_referenceCount;

                if (_referenceCount == 0 && _localWindowsHook != null)
                {
                    _localWindowsHook.Dispose();
                    _localWindowsHook = null;
                }

                m_disposed = true;
            }

            base.Dispose (disposing);
        }

        private IDockContent ContentActivating { get; set; }

        public void Activate (IDockContent content)
        {
            if (IsFocusTrackingSuspended)
            {
                ContentActivating = content;
                return;
            }

            if (content == null!)
            {
                return;
            }

            var handler = content.DockHandler;
            if (handler.Form.IsDisposed)
            {
                return; // Should not reach here, but better than throwing an exception
            }

            if (ContentContains (content, handler.ActiveWindowHandle))
            {
                if (!Win32Helper.IsRunningOnMono)
                {
                    Win32.NativeMethods.SetFocus (handler.ActiveWindowHandle);
                }
            }

            if (handler.Form.ContainsFocus)
            {
                return;
            }

            if (handler.Form.SelectNextControl (handler.Form.ActiveControl, true, true, true, true))
            {
                return;
            }

            if (Win32Helper.IsRunningOnMono)
            {
                return;
            }

            // Since DockContent Form is not selectalbe, use Win32 SetFocus instead
            Win32.NativeMethods.SetFocus (handler.Form.Handle);
        }

        private List<IDockContent> ListContent { get; } = new ();

        public void AddToList (IDockContent content)
        {
            if (ListContent.Contains (content) || IsInActiveList (content))
            {
                return;
            }

            ListContent.Add (content);
        }

        public void RemoveFromList (IDockContent content)
        {
            if (IsInActiveList (content))
            {
                RemoveFromActiveList (content);
            }

            if (ListContent.Contains (content))
            {
                ListContent.Remove (content);
            }
        }

        private IDockContent LastActiveContent { get; set; }

        private bool IsInActiveList (IDockContent content)
        {
            return !(content.DockHandler.NextActive == null && LastActiveContent != content);
        }

        private void AddLastToActiveList (IDockContent content)
        {
            var last = LastActiveContent;
            if (last == content)
            {
                return;
            }

            var handler = content.DockHandler;

            if (IsInActiveList (content))
            {
                RemoveFromActiveList (content);
            }

            handler.PreviousActive = last;
            handler.NextActive = null;
            LastActiveContent = content;
            if (last != null!)
            {
                last.DockHandler.NextActive = LastActiveContent;
            }
        }

        private void RemoveFromActiveList (IDockContent content)
        {
            if (LastActiveContent == content)
            {
                LastActiveContent = content.DockHandler.PreviousActive!;
            }

            var prev = content.DockHandler.PreviousActive;
            var next = content.DockHandler.NextActive;
            if (prev != null!)
            {
                prev.DockHandler.NextActive = next;
            }

            if (next != null!)
            {
                next.DockHandler.PreviousActive = prev!;
            }

            content.DockHandler.PreviousActive = null;
            content.DockHandler.NextActive = null;
        }

        public void GiveUpFocus (IDockContent content)
        {
            var handler = content.DockHandler;
            if (!handler.Form.ContainsFocus)
            {
                return;
            }

            if (IsFocusTrackingSuspended)
            {
                DockPanel.DummyControl.Focus();
            }

            if (LastActiveContent == content)
            {
                var prev = handler.PreviousActive;
                if (prev != null)
                {
                    Activate (prev);
                }
                else if (ListContent.Count > 0)
                {
                    Activate (ListContent[^1]);
                }
            }
            else if (LastActiveContent != null!)
            {
                Activate (LastActiveContent);
            }
            else if (ListContent.Count > 0)
            {
                Activate (ListContent[^1]);
            }
        }

        private static bool ContentContains (IDockContent content, IntPtr hWnd)
        {
            var control = Control.FromChildHandle (hWnd);
            for (var parent = control; parent != null; parent = parent.Parent)
                if (parent == content.DockHandler.Form)
                {
                    return true;
                }

            return false;
        }

        private uint m_countSuspendFocusTracking;

        public void SuspendFocusTracking()
        {
            if (m_disposed)
            {
                return;
            }

            if (m_countSuspendFocusTracking++ == 0)
            {
                if (!Win32Helper.IsRunningOnMono)
                {
                    _localWindowsHook!.HookInvoked -= m_hookEventHandler;
                }
            }
        }

        public void ResumeFocusTracking()
        {
            if (m_disposed || m_countSuspendFocusTracking == 0)
            {
                return;
            }

            if (--m_countSuspendFocusTracking == 0)
            {
                if (ContentActivating != null!)
                {
                    Activate (ContentActivating);
                    ContentActivating = null!;
                }

                if (!Win32Helper.IsRunningOnMono)
                {
                    _localWindowsHook!.HookInvoked += m_hookEventHandler;
                }

                if (!InRefreshActiveWindow)
                {
                    RefreshActiveWindow();
                }
            }
        }

        public bool IsFocusTrackingSuspended => m_countSuspendFocusTracking != 0;

        // Windows hook event handler
        private void HookEventHandler (object sender, HookEventArgs e)
        {
            var msg = (Win32.Msgs)Marshal.ReadInt32 (e.lParam, IntPtr.Size * 3);

            if (msg == Win32.Msgs.WM_KILLFOCUS)
            {
                var wParam = Marshal.ReadIntPtr (e.lParam, IntPtr.Size * 2);
                var pane = GetPaneFromHandle (wParam);
                if (pane == null)
                {
                    RefreshActiveWindow();
                }
            }
            else if (msg == Win32.Msgs.WM_SETFOCUS || msg == Win32.Msgs.WM_MDIACTIVATE)
            {
                RefreshActiveWindow();
            }
        }

        private DockPane? GetPaneFromHandle (IntPtr hWnd)
        {
            var control = Control.FromChildHandle (hWnd);

            DockPane? pane = null;
            for (; control != null; control = control.Parent)
            {
                var content = control as IDockContent;
                if (content != null)
                {
                    content.DockHandler.ActiveWindowHandle = hWnd;
                }

                if (content != null && content.DockHandler.DockPanel == DockPanel)
                {
                    return content.DockHandler.Pane;
                }

                pane = control as DockPane;
                if (pane != null && pane.DockPanel == DockPanel)
                {
                    break;
                }
            }

            return pane;
        }

        private bool m_inRefreshActiveWindow;

        private bool InRefreshActiveWindow => m_inRefreshActiveWindow;

        private void RefreshActiveWindow()
        {
            if (DockPanel.Theme == null!)
            {
                return;
            }

            SuspendFocusTracking();
            m_inRefreshActiveWindow = true;

            var oldActivePane = ActivePane;
            var oldActiveContent = ActiveContent;
            var oldActiveDocument = ActiveDocument;

            SetActivePane();
            SetActiveContent();
            SetActiveDocumentPane();
            SetActiveDocument();
            DockPanel.AutoHideWindow.RefreshActivePane();

            ResumeFocusTracking();
            m_inRefreshActiveWindow = false;

            if (oldActiveContent != ActiveContent)
            {
                DockPanel.OnActiveContentChanged (EventArgs.Empty);
            }

            if (oldActiveDocument != ActiveDocument)
            {
                DockPanel.OnActiveDocumentChanged (EventArgs.Empty);
            }

            if (oldActivePane != ActivePane)
            {
                DockPanel.OnActivePaneChanged (EventArgs.Empty);
            }
        }

        public DockPane? ActivePane { get; private set; }

        private void SetActivePane()
        {
            var value = Win32Helper.IsRunningOnMono
                ? null
                : GetPaneFromHandle (Win32.NativeMethods.GetFocus());
            if (ActivePane == value)
            {
                return;
            }

            if (ActivePane != null)
            {
                ActivePane.SetIsActivated (false);
            }

            ActivePane = value;

            if (ActivePane != null)
            {
                ActivePane.SetIsActivated (true);
            }
        }

        private IDockContent m_activeContent;

        public IDockContent ActiveContent => m_activeContent;

        internal void SetActiveContent()
        {
            var value = ActivePane?.ActiveContent;

            if (m_activeContent == value)
            {
                return;
            }

            if (m_activeContent != null!)
            {
                m_activeContent.DockHandler.IsActivated = false;
            }

            m_activeContent = value!;

            if (m_activeContent != null!)
            {
                m_activeContent.DockHandler.IsActivated = true;
                if (!DockHelper.IsDockStateAutoHide ((m_activeContent.DockHandler.DockState)))
                {
                    AddLastToActiveList (m_activeContent);
                }
            }
        }

        private DockPane m_activeDocumentPane;

        public DockPane ActiveDocumentPane => m_activeDocumentPane;

        private void SetActiveDocumentPane()
        {
            DockPane? value = null;

            if (ActivePane != null && ActivePane.DockState == DockState.Document)
            {
                value = ActivePane;
            }

            if (value == null && DockPanel.DockWindows != null!)
            {
                if (ActiveDocumentPane == null!)
                {
                    value = DockPanel.DockWindows[DockState.Document].DefaultPane;
                }
                else if (ActiveDocumentPane.DockPanel != DockPanel ||
                         ActiveDocumentPane.DockState != DockState.Document)
                {
                    value = DockPanel.DockWindows[DockState.Document].DefaultPane;
                }
                else
                {
                    value = ActiveDocumentPane;
                }
            }

            if (m_activeDocumentPane == value)
            {
                return;
            }

            if (m_activeDocumentPane != null!)
            {
                m_activeDocumentPane.SetIsActiveDocumentPane (false);
            }

            m_activeDocumentPane = value!;

            if (m_activeDocumentPane != null!)
            {
                m_activeDocumentPane.SetIsActiveDocumentPane (true);
            }
        }

        private IDockContent m_activeDocument;

        public IDockContent ActiveDocument => m_activeDocument;

        private void SetActiveDocument()
        {
            var value = ActiveDocumentPane == null! ? null : ActiveDocumentPane.ActiveContent;

            if (m_activeDocument == value)
            {
                return;
            }

            m_activeDocument = value!;
        }
    }

    private IFocusManager FocusManager => m_focusManager;

    internal IContentFocusManager ContentFocusManager => m_focusManager;

    internal void SaveFocus()
    {
        DummyControl.Focus();
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public IDockContent ActiveContent => FocusManager.ActiveContent;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public DockPane ActivePane => FocusManager.ActivePane!;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public IDockContent ActiveDocument => FocusManager.ActiveDocument;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    public DockPane ActiveDocumentPane => FocusManager.ActiveDocumentPane;

    private static readonly object ActiveDocumentChangedEvent = new ();

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_PropertyChanged")]
    [LocalizedDescription ("DockPanel_ActiveDocumentChanged_Description")]
    public event EventHandler ActiveDocumentChanged
    {
        add => Events.AddHandler (ActiveDocumentChangedEvent, value);
        remove => Events.RemoveHandler (ActiveDocumentChangedEvent, value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnActiveDocumentChanged (EventArgs e)
    {
        var handler = (EventHandler?) Events[ActiveDocumentChangedEvent];
        handler?.Invoke (this, e);
    }

    private static readonly object ActiveContentChangedEvent = new ();

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_PropertyChanged")]
    [LocalizedDescription ("DockPanel_ActiveContentChanged_Description")]
    public event EventHandler ActiveContentChanged
    {
        add => Events.AddHandler (ActiveContentChangedEvent, value);
        remove => Events.RemoveHandler (ActiveContentChangedEvent, value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    protected void OnActiveContentChanged (EventArgs e)
    {
        var handler = (EventHandler?) Events[ActiveContentChangedEvent];
        handler?.Invoke (this, e);
    }

    private static readonly object DocumentDraggedEvent = new ();

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_PropertyChanged")]
    [LocalizedDescription ("DockPanel_ActiveContentChanged_Description")]
    public event EventHandler DocumentDragged
    {
        add => Events.AddHandler (DocumentDraggedEvent, value);
        remove => Events.RemoveHandler (DocumentDraggedEvent, value);
    }

    /// <summary>
    ///
    /// </summary>
    internal void OnDocumentDragged()
    {
        var handler = (EventHandler?) Events[DocumentDraggedEvent];
        handler?.Invoke (this, EventArgs.Empty);
    }

    private static readonly object ActivePaneChangedEvent = new ();

    /// <summary>
    ///
    /// </summary>
    [LocalizedCategory ("Category_PropertyChanged")]
    [LocalizedDescription ("DockPanel_ActivePaneChanged_Description")]
    public event EventHandler? ActivePaneChanged
    {
        add => Events.AddHandler (ActivePaneChangedEvent, value);
        remove => Events.RemoveHandler (ActivePaneChangedEvent, value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnActivePaneChanged (EventArgs e)
    {
        var handler = (EventHandler?) Events[ActivePaneChangedEvent];
        handler?.Invoke (this, e);
    }
}
