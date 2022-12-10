// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DockPanel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

#endregion

#nullable enable

// To simplify the process of finding the toolbox bitmap resource:
// #1 Create an internal class called "resfinder" outside of the root namespace.
// #2 Use "resfinder" in the toolbox bitmap attribute instead of the control name.
// #3 use the "<default namespace>.<resourcename>" string to locate the resource.
// See: http://www.bobpowell.net/toolboxbitmap.htm
internal class ResFinder
{
}

namespace AM.Windows.Forms.Docking
{
    /// <summary>
    /// Deserialization handler of layout file/stream.
    /// </summary>
    /// <param name="persistString">Strings stored in layout file/stream.</param>
    /// <returns>Dock content deserialized from layout/stream.</returns>
    /// <remarks>
    /// The deserialization handler method should handle all possible exceptions.
    ///
    /// If any exception happens during deserialization and is not handled, the program might crash or experience other issues.
    /// </remarks>
    [SuppressMessage ("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#")]
    public delegate IDockContent DeserializeDockContent (string persistString);

    /// <summary>
    ///
    /// </summary>
    [LocalizedDescription ("DockPanel_Description")]
    [Designer ("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [ToolboxBitmap (typeof (ResFinder), "WeifenLuo.WinFormsUI.Docking.DockPanel.bmp")]
    [DefaultProperty ("DocumentStyle")]
    [DefaultEvent ("ActiveContentChanged")]
    public partial class DockPanel : Panel
    {
        private readonly FocusManagerImpl m_focusManager;
        private readonly DockPaneCollection m_panes;
        private readonly FloatWindowCollection m_floatWindows;
        private AutoHideWindowControl? m_autoHideWindow;
        private DockWindowCollection m_dockWindows;
        private readonly DockContent m_dummyContent;
        private readonly Control m_dummyControl;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DockPanel()
        {
            m_dockWindows = null!;

            ShowAutoHideContentOnHover = true;

            m_focusManager = new FocusManagerImpl (this);
            m_panes = new DockPaneCollection();
            m_floatWindows = new FloatWindowCollection();

            SuspendLayout();

            m_dummyControl = new DummyControl();
            m_dummyControl.Bounds = new Rectangle (0, 0, 1, 1);
            Controls.Add (m_dummyControl);

            Theme.ApplyTo (this);

            m_autoHideWindow = Theme.Extender.AutoHideWindowFactory.CreateAutoHideWindow (this);
            m_autoHideWindow.Visible = false;
            m_autoHideWindow.ActiveContentChanged += m_autoHideWindow_ActiveContentChanged;
            SetAutoHideWindowParent();

            LoadDockWindows();

            m_dummyContent = new DockContent();
            ResumeLayout();
        }

        internal void ResetDummy()
        {
            DummyControl.ResetBackColor();
        }

        internal void SetDummy()
        {
            DummyControl.BackColor = DockBackColor;
        }

        private Color m_BackColor;

        /// <summary>
        /// Determines the color with which the client rectangle will be drawn.
        /// If this property is used instead of the BackColor it will not have any influence on the borders to the surrounding controls (DockPane).
        /// The BackColor property changes the borders of surrounding controls (DockPane).
        /// Alternatively both properties may be used (BackColor to draw and define the color of the borders and DockBackColor to define the color of the client rectangle).
        /// For Backgroundimages: Set your prefered Image, then set the DockBackColor and the BackColor to the same Color (Control)
        /// </summary>
        [Description ("Determines the color with which the client rectangle will be drawn.\r\n" +
                      "If this property is used instead of the BackColor it will not have any influence on the borders to the surrounding controls (DockPane).\r\n" +
                      "The BackColor property changes the borders of surrounding controls (DockPane).\r\n" +
                      "Alternatively both properties may be used (BackColor to draw and define the color of the borders and DockBackColor to define the color of the client rectangle).\r\n" +
                      "For Backgroundimages: Set your prefered Image, then set the DockBackColor and the BackColor to the same Color (Control).")]
        public Color DockBackColor
        {
            get => !m_BackColor.IsEmpty ? m_BackColor : base.BackColor;

            set
            {
                if (m_BackColor != value)
                {
                    m_BackColor = value;
                    Refresh();
                }
            }
        }

        private bool ShouldSerializeDockBackColor()
        {
            return !m_BackColor.IsEmpty;
        }

        private AutoHideStripBase? m_autoHideStripControl;

        internal AutoHideStripBase AutoHideStripControl
        {
            get
            {
                if (m_autoHideStripControl == null)
                {
                    m_autoHideStripControl = Theme.Extender.AutoHideStripFactory.CreateAutoHideStrip (this);
                    Controls.Add (m_autoHideStripControl);
                }

                return m_autoHideStripControl;
            }
        }

        internal void ResetAutoHideStripControl()
        {
            if (m_autoHideStripControl != null)
            {
                m_autoHideStripControl.Dispose();
            }

            m_autoHideStripControl = null;
        }

        private void MdiClientHandleAssigned (object? sender, EventArgs e)
        {
            SetMdiClient();
            PerformLayout();
        }

        private void MdiClient_Layout (object? sender, LayoutEventArgs e)
        {
            if (DocumentStyle != DocumentStyle.DockingMdi)
            {
                return;
            }

            foreach (var pane in Panes)
            {
                if (pane.DockState == DockState.Document)
                {
                    pane.SetContentBounds();
                }
            }

            InvalidateWindowRegion();
        }

        private bool m_disposed;

        /// <inheritdoc cref="Control.Dispose(bool)"/>
        protected override void Dispose (bool disposing)
        {
            if (!m_disposed && disposing)
            {
                m_focusManager.Dispose();
                if (m_mdiClientController != null!)
                {
                    m_mdiClientController.HandleAssigned -= MdiClientHandleAssigned;
                    m_mdiClientController.MdiChildActivate -= ParentFormMdiChildActivate;
                    m_mdiClientController.Layout -= MdiClient_Layout;
                    m_mdiClientController.Dispose();
                }

                FloatWindows.Dispose();
                Panes.Dispose();
                DummyContent.Dispose();

                m_disposed = true;
            }

            base.Dispose (disposing);
        }

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public IDockContent? ActiveAutoHideContent
        {
            get => AutoHideWindow.ActiveContent;
            set => AutoHideWindow.ActiveContent = value;
        }

        private bool m_allowEndUserDocking = !Win32Helper.IsRunningOnMono;

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_AllowEndUserDocking_Description")]
        [DefaultValue (true)]
        public bool AllowEndUserDocking
        {
            get
            {
                if (Win32Helper.IsRunningOnMono && m_allowEndUserDocking)
                {
                    m_allowEndUserDocking = false;
                }

                return m_allowEndUserDocking;
            }

            set
            {
                if (Win32Helper.IsRunningOnMono && value)
                {
                    throw new InvalidOperationException ("AllowEndUserDocking can only be false if running on Mono");
                }

                m_allowEndUserDocking = value;
            }
        }

        private bool m_allowEndUserNestedDocking = !Win32Helper.IsRunningOnMono;

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_AllowEndUserNestedDocking_Description")]
        [DefaultValue (true)]
        public bool AllowEndUserNestedDocking
        {
            get
            {
                if (Win32Helper.IsRunningOnMono && m_allowEndUserDocking)
                {
                    m_allowEndUserDocking = false;
                }

                return m_allowEndUserNestedDocking;
            }

            set
            {
                if (Win32Helper.IsRunningOnMono && value)
                {
                    throw new InvalidOperationException (
                        "AllowEndUserNestedDocking can only be false if running on Mono");
                }

                m_allowEndUserNestedDocking = value;
            }
        }

        private readonly DockContentCollection _contents = new ();

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public DockContentCollection Contents => _contents;

        internal DockContent DummyContent => m_dummyContent;

        private bool m_rightToLeftLayout;

        /// <summary>
        ///
        /// </summary>
        [DefaultValue (false)]
        [LocalizedCategory ("Appearance")]
        [LocalizedDescription ("DockPanel_RightToLeftLayout_Description")]
        public bool RightToLeftLayout
        {
            get => m_rightToLeftLayout;

            set
            {
                if (m_rightToLeftLayout == value)
                {
                    return;
                }

                m_rightToLeftLayout = value;
                foreach (var floatWindow in FloatWindows)
                {
                    floatWindow.RightToLeftLayout = value;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRightToLeftChanged (EventArgs e)
        {
            base.OnRightToLeftChanged (e);
            foreach (var floatWindow in FloatWindows)
            {
                floatWindow.RightToLeft = RightToLeft;
            }
        }

        private bool m_showDocumentIcon;

        /// <summary>
        ///
        /// </summary>
        [DefaultValue (false)]
        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_ShowDocumentIcon_Description")]
        public bool ShowDocumentIcon
        {
            get => m_showDocumentIcon;
            set
            {
                if (m_showDocumentIcon == value)
                {
                    return;
                }

                m_showDocumentIcon = value;
                Refresh();
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DefaultValue (DocumentTabStripLocation.Top)]
        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_DocumentTabStripLocation")]
        public DocumentTabStripLocation DocumentTabStripLocation { get; set; } = DocumentTabStripLocation.Top;

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        [Obsolete ("Use Theme.Extender instead.")]
        public DockPanelExtender? Extender => null;

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        [Obsolete ("Use Theme.Extender instead.")]
        public DockPanelExtender.IDockPaneFactory? DockPaneFactory => null;

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        [Obsolete ("Use Theme.Extender instead.")]
        public DockPanelExtender.IFloatWindowFactory? FloatWindowFactory => null;

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        [Obsolete ("Use Theme.Extender instead.")]
        public DockPanelExtender.IDockWindowFactory DockWindowFactory => null!;

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public DockPaneCollection Panes => m_panes;

        /// <summary>
        /// Dock area.
        /// </summary>
        /// <remarks>
        /// This <see cref="Rectangle"/> is the center rectangle of <see cref="DockPanel"/> control.
        ///
        /// Excluded spaces are for the following visual elements,
        /// * Auto hide strips on four sides.
        /// * Necessary paddings defined in themes.
        ///
        /// Therefore, all dock contents mainly fall into this area (except auto hide window, which might slightly move beyond this area).
        /// </remarks>
        public Rectangle DockArea =>
            new (DockPadding.Left, DockPadding.Top,
                ClientRectangle.Width - DockPadding.Left - DockPadding.Right,
                ClientRectangle.Height - DockPadding.Top - DockPadding.Bottom);

        private double m_dockBottomPortion = 0.25;

        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_DockBottomPortion_Description")]
        [DefaultValue (0.25)]
        public double DockBottomPortion
        {
            get => m_dockBottomPortion;

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException (nameof (value));
                }

                if (Math.Abs (value - m_dockBottomPortion) < double.Epsilon)
                {
                    return;
                }

                m_dockBottomPortion = value;

                if (m_dockBottomPortion < 1 && m_dockTopPortion < 1)
                {
                    if (m_dockTopPortion + m_dockBottomPortion > 1)
                    {
                        m_dockTopPortion = 1 - m_dockBottomPortion;
                    }
                }

                PerformLayout();
            }
        }

        private double m_dockLeftPortion = 0.25;

        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_DockLeftPortion_Description")]
        [DefaultValue (0.25)]
        public double DockLeftPortion
        {
            get => m_dockLeftPortion;

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException (nameof (value));
                }

                if (Math.Abs (value - m_dockLeftPortion) < double.Epsilon)
                {
                    return;
                }

                m_dockLeftPortion = value;

                if (m_dockLeftPortion < 1 && m_dockRightPortion < 1)
                {
                    if (m_dockLeftPortion + m_dockRightPortion > 1)
                    {
                        m_dockRightPortion = 1 - m_dockLeftPortion;
                    }
                }

                PerformLayout();
            }
        }

        private double m_dockRightPortion = 0.25;

        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_DockRightPortion_Description")]
        [DefaultValue (0.25)]
        public double DockRightPortion
        {
            get => m_dockRightPortion;

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException (nameof (value));
                }

                if (Math.Abs (value - m_dockRightPortion) < double.Epsilon)
                {
                    return;
                }

                m_dockRightPortion = value;

                if (m_dockLeftPortion < 1 && m_dockRightPortion < 1)
                {
                    if (m_dockLeftPortion + m_dockRightPortion > 1)
                    {
                        m_dockLeftPortion = 1 - m_dockRightPortion;
                    }
                }

                PerformLayout();
            }
        }

        private double m_dockTopPortion = 0.25;

        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_DockTopPortion_Description")]
        [DefaultValue (0.25)]
        public double DockTopPortion
        {
            get => m_dockTopPortion;

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException (nameof (value));
                }

                if (Math.Abs (value - m_dockTopPortion) < double.Epsilon)
                {
                    return;
                }

                m_dockTopPortion = value;

                if (m_dockTopPortion < 1 && m_dockBottomPortion < 1)
                {
                    if (m_dockTopPortion + m_dockBottomPortion > 1)
                    {
                        m_dockBottomPortion = 1 - m_dockTopPortion;
                    }
                }

                PerformLayout();
            }
        }

        [Browsable (false)]
        public DockWindowCollection DockWindows => m_dockWindows;

        public void UpdateDockWindowZOrder (DockStyle dockStyle, bool fullPanelEdge)
        {
            if (dockStyle == DockStyle.Left)
            {
                if (fullPanelEdge)
                {
                    DockWindows[DockState.DockLeft].SendToBack();
                }
                else
                {
                    DockWindows[DockState.DockLeft].BringToFront();
                }
            }
            else if (dockStyle == DockStyle.Right)
            {
                if (fullPanelEdge)
                {
                    DockWindows[DockState.DockRight].SendToBack();
                }
                else
                {
                    DockWindows[DockState.DockRight].BringToFront();
                }
            }
            else if (dockStyle == DockStyle.Top)
            {
                if (fullPanelEdge)
                {
                    DockWindows[DockState.DockTop].SendToBack();
                }
                else
                {
                    DockWindows[DockState.DockTop].BringToFront();
                }
            }
            else if (dockStyle == DockStyle.Bottom)
            {
                if (fullPanelEdge)
                {
                    DockWindows[DockState.DockBottom].SendToBack();
                }
                else
                {
                    DockWindows[DockState.DockBottom].BringToFront();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public int DocumentsCount
        {
            get
            {
                var count = 0;
                foreach (var content in Documents)
                {
                    count++;
                }

                return count;
            }
        }

        public IDockContent[] DocumentsToArray()
        {
            var count = DocumentsCount;
            var documents = new IDockContent[count];
            var i = 0;
            foreach (var content in Documents)
            {
                documents[i] = content;
                i++;
            }

            return documents;
        }

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public IEnumerable<IDockContent> Documents
        {
            get
            {
                foreach (var content in Contents)
                {
                    if (content.DockHandler.DockState == DockState.Document)
                    {
                        yield return content;
                    }
                }
            }
        }

        private Control DummyControl => m_dummyControl;

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public FloatWindowCollection FloatWindows => m_floatWindows;

        /// <summary>
        ///
        /// </summary>
        [Category ("Layout")]
        [LocalizedDescription ("DockPanel_DefaultFloatWindowSize_Description")]
        public Size DefaultFloatWindowSize { get; set; } = new (300, 300);

        private bool ShouldSerializeDefaultFloatWindowSize()
        {
            return DefaultFloatWindowSize != new Size (300, 300);
        }

        private void ResetDefaultFloatWindowSize()
        {
            DefaultFloatWindowSize = new Size (300, 300);
        }

        private DocumentStyle m_documentStyle = DocumentStyle.DockingWindow;

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_DocumentStyle_Description")]
        [DefaultValue (DocumentStyle.DockingWindow)]
        public DocumentStyle DocumentStyle
        {
            get => m_documentStyle;
            set
            {
                if (value == m_documentStyle)
                {
                    return;
                }

                if (!Enum.IsDefined (typeof (DocumentStyle), value))
                {
                    throw new InvalidEnumArgumentException();
                }

                if (value == DocumentStyle.SystemMdi && DockWindows[DockState.Document].VisibleNestedPanes.Count > 0)
                {
                    throw new InvalidEnumArgumentException();
                }

                m_documentStyle = value;

                SuspendLayout (true);

                SetAutoHideWindowParent();
                SetMdiClient();
                InvalidateWindowRegion();

                foreach (var content in Contents)
                {
                    if (content.DockHandler.DockState == DockState.Document)
                    {
                        content.DockHandler.SetPaneAndVisible (content.DockHandler.Pane);
                    }
                }

                PerformMdiClientLayout();

                ResumeLayout (true, true);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [LocalizedCategory ("Category_Performance")]
        [LocalizedDescription ("DockPanel_SupportDeeplyNestedContent_Description")]
        [DefaultValue (false)]
        public bool SupportDeeplyNestedContent { get; set; }

        /// <summary>
        /// Flag to show autohide content on mouse hover. Default value is <code>true</code>.
        /// </summary>
        /// <remarks>
        /// This flag is ignored in VS2012/2013 themes. Such themes assume it is always <code>false</code>.
        /// </remarks>
        [LocalizedCategory ("Category_Docking")]
        [LocalizedDescription ("DockPanel_ShowAutoHideContentOnHover_Description")]
        [DefaultValue (true)]
        public bool ShowAutoHideContentOnHover { get; set; }

        public int GetDockWindowSize (DockState dockState)
        {
            if (dockState == DockState.DockLeft || dockState == DockState.DockRight)
            {
                var width = ClientRectangle.Width - DockPadding.Left - DockPadding.Right;
                var dockLeftSize = m_dockLeftPortion >= 1 ? (int)m_dockLeftPortion : (int)(width * m_dockLeftPortion);
                var dockRightSize = m_dockRightPortion >= 1
                    ? (int)m_dockRightPortion
                    : (int)(width * m_dockRightPortion);

                if (dockLeftSize < MeasurePane.MinSize)
                {
                    dockLeftSize = MeasurePane.MinSize;
                }

                if (dockRightSize < MeasurePane.MinSize)
                {
                    dockRightSize = MeasurePane.MinSize;
                }

                if (dockLeftSize + dockRightSize > width - MeasurePane.MinSize)
                {
                    var adjust = (dockLeftSize + dockRightSize) - (width - MeasurePane.MinSize);
                    dockLeftSize -= adjust / 2;
                    dockRightSize -= adjust / 2;
                }

                return dockState == DockState.DockLeft ? dockLeftSize : dockRightSize;
            }

            if (dockState == DockState.DockTop || dockState == DockState.DockBottom)
            {
                var height = ClientRectangle.Height - DockPadding.Top - DockPadding.Bottom;
                var dockTopSize = m_dockTopPortion >= 1 ? (int)m_dockTopPortion : (int)(height * m_dockTopPortion);
                var dockBottomSize = m_dockBottomPortion >= 1
                    ? (int)m_dockBottomPortion
                    : (int)(height * m_dockBottomPortion);

                if (dockTopSize < MeasurePane.MinSize)
                {
                    dockTopSize = MeasurePane.MinSize;
                }

                if (dockBottomSize < MeasurePane.MinSize)
                {
                    dockBottomSize = MeasurePane.MinSize;
                }

                if (dockTopSize + dockBottomSize > height - MeasurePane.MinSize)
                {
                    var adjust = (dockTopSize + dockBottomSize) - (height - MeasurePane.MinSize);
                    dockTopSize -= adjust / 2;
                    dockBottomSize -= adjust / 2;
                }

                return dockState == DockState.DockTop ? dockTopSize : dockBottomSize;
            }

            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="levent"></param>
        protected override void OnLayout (LayoutEventArgs levent)
        {
            SuspendLayout (true);

            AutoHideStripControl.Bounds = ClientRectangle;

            CalculateDockPadding();

            DockWindows[DockState.DockLeft].Width = GetDockWindowSize (DockState.DockLeft);
            DockWindows[DockState.DockRight].Width = GetDockWindowSize (DockState.DockRight);
            DockWindows[DockState.DockTop].Height = GetDockWindowSize (DockState.DockTop);
            DockWindows[DockState.DockBottom].Height = GetDockWindowSize (DockState.DockBottom);

            AutoHideWindow.Bounds = GetAutoHideWindowBounds (AutoHideWindowRectangle);

            var documentDockWindow = DockWindows[DockState.Document];

            if (ReferenceEquals (documentDockWindow.Parent, AutoHideWindow.Parent))
            {
                AutoHideWindow.Parent!.Controls.SetChildIndex (AutoHideWindow, 0);
                documentDockWindow.Parent!.Controls.SetChildIndex (documentDockWindow, 1);
            }
            else
            {
                documentDockWindow.BringToFront();
                AutoHideWindow.BringToFront();
            }

            base.OnLayout (levent);

            if (DocumentStyle == DocumentStyle.SystemMdi && MdiClientExists)
            {
                SetMdiClientBounds (SystemMdiClientBounds);
                InvalidateWindowRegion();
            }
            else if (DocumentStyle == DocumentStyle.DockingMdi)
            {
                InvalidateWindowRegion();
            }

            ResumeLayout (true, true);
        }

        internal Rectangle GetTabStripRectangle (DockState dockState)
        {
            return AutoHideStripControl.GetTabStripRectangle (dockState);
        }

        /// <inheritdoc cref="Control.OnPaint"/>
        protected override void OnPaint (PaintEventArgs e)
        {
            base.OnPaint (e);

            if (DockBackColor.ToArgb() == BackColor.ToArgb())
            {
                return;
            }

            var g = e.Graphics;
            var bgBrush = new SolidBrush (DockBackColor);
            g.FillRectangle (bgBrush, ClientRectangle);
        }

        internal void AddContent (IDockContent content)
        {
            if (content == null)
            {
                throw (new ArgumentNullException());
            }

            if (!Contents.Contains (content))
            {
                Contents.Add (content);
                OnContentAdded (new DockContentEventArgs (content));
            }
        }

        internal void AddPane (DockPane pane)
        {
            if (Panes.Contains (pane))
            {
                return;
            }

            Panes.Add (pane);
        }

        internal void AddFloatWindow (FloatWindow floatWindow)
        {
            if (FloatWindows.Contains (floatWindow))
            {
                return;
            }

            FloatWindows.Add (floatWindow);
        }

        private void CalculateDockPadding()
        {
            DockPadding.All = Theme.Measures.DockPadding;
            var standard = AutoHideStripControl.MeasureHeight();
            if (AutoHideStripControl.GetNumberOfPanes (DockState.DockLeftAutoHide) > 0)
            {
                DockPadding.Left = standard;
            }

            if (AutoHideStripControl.GetNumberOfPanes (DockState.DockRightAutoHide) > 0)
            {
                DockPadding.Right = standard;
            }

            if (AutoHideStripControl.GetNumberOfPanes (DockState.DockTopAutoHide) > 0)
            {
                DockPadding.Top = standard;
            }

            if (AutoHideStripControl.GetNumberOfPanes (DockState.DockBottomAutoHide) > 0)
            {
                DockPadding.Bottom = standard;
            }
        }

        internal void RemoveContent (IDockContent content)
        {
            if (content == null)
            {
                throw (new ArgumentNullException());
            }

            if (Contents.Contains (content))
            {
                Contents.Remove (content);
                OnContentRemoved (new DockContentEventArgs (content));
            }
        }

        internal void RemovePane (DockPane pane)
        {
            if (!Panes.Contains (pane))
            {
                return;
            }

            Panes.Remove (pane);
        }

        internal void RemoveFloatWindow (FloatWindow floatWindow)
        {
            if (!FloatWindows.Contains (floatWindow))
            {
                return;
            }

            FloatWindows.Remove (floatWindow);
            if (FloatWindows.Count != 0)
            {
                return;
            }

            if (ParentForm == null!)
            {
                return;
            }

            ParentForm.Focus();
        }

        public void SetPaneIndex (DockPane pane, int index)
        {
            var oldIndex = Panes.IndexOf (pane);
            if (oldIndex == -1)
            {
                throw (new ArgumentException (Strings.DockPanel_SetPaneIndex_InvalidPane));
            }

            if (index < 0 || index > Panes.Count - 1)
            {
                if (index != -1)
                {
                    throw (new ArgumentOutOfRangeException (Strings.DockPanel_SetPaneIndex_InvalidIndex));
                }
            }

            if (oldIndex == index)
            {
                return;
            }

            if (oldIndex == Panes.Count - 1 && index == -1)
            {
                return;
            }

            Panes.Remove (pane);
            if (index == -1)
            {
                Panes.Add (pane);
            }
            else if (oldIndex < index)
            {
                Panes.AddAt (pane, index - 1);
            }
            else
            {
                Panes.AddAt (pane, index);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="allWindows"></param>
        public void SuspendLayout (bool allWindows)
        {
            FocusManager.SuspendFocusTracking();
            SuspendLayout();
            if (allWindows)
            {
                SuspendMdiClientLayout();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="performLayout"></param>
        /// <param name="allWindows"></param>
        public void ResumeLayout (bool performLayout, bool allWindows)
        {
            FocusManager.ResumeFocusTracking();
            ResumeLayout (performLayout);
            if (allWindows)
            {
                ResumeMdiClientLayout (performLayout);
            }
        }

        internal Form ParentForm
        {
            get
            {
                if (!IsParentFormValid())
                {
                    throw new InvalidOperationException (Strings.DockPanel_ParentForm_Invalid);
                }

                return GetMdiClientController().ParentForm!;
            }
        }

        private bool IsParentFormValid()
        {
            if (DocumentStyle == DocumentStyle.DockingSdi || DocumentStyle == DocumentStyle.DockingWindow)
            {
                return true;
            }

            if (!MdiClientExists)
            {
                GetMdiClientController().RenewMdiClient();
            }

            return (MdiClientExists);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>

        protected override void OnParentChanged (EventArgs e)
        {
            SetAutoHideWindowParent();
            GetMdiClientController().ParentForm = (Parent as Form);
            base.OnParentChanged (e);
        }

        private void SetAutoHideWindowParent()
        {
            Control? parent;
            parent = DocumentStyle == DocumentStyle.DockingMdi ||
                     DocumentStyle == DocumentStyle.SystemMdi
                ? Parent
                : this;

            if (AutoHideWindow.Parent != parent)
            {
                AutoHideWindow.Parent = parent;
                AutoHideWindow.BringToFront();
            }
        }

        /// <inheritdoc cref="ScrollableControl.OnVisibleChanged"/>
        protected override void OnVisibleChanged (EventArgs e)
        {
            base.OnVisibleChanged (e);

            if (Visible)
            {
                SetMdiClient();
            }
        }

        private Rectangle SystemMdiClientBounds
        {
            get
            {
                if (!IsParentFormValid() || !Visible)
                {
                    return Rectangle.Empty;
                }

                var rect = ParentForm.RectangleToClient (RectangleToScreen (DocumentWindowBounds));
                return rect;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Rectangle DocumentWindowBounds
        {
            get
            {
                var rectDocumentBounds = DisplayRectangle;
                if (DockWindows[DockState.DockLeft].Visible)
                {
                    rectDocumentBounds.X += DockWindows[DockState.DockLeft].Width;
                    rectDocumentBounds.Width -= DockWindows[DockState.DockLeft].Width;
                }

                if (DockWindows[DockState.DockRight].Visible)
                {
                    rectDocumentBounds.Width -= DockWindows[DockState.DockRight].Width;
                }

                if (DockWindows[DockState.DockTop].Visible)
                {
                    rectDocumentBounds.Y += DockWindows[DockState.DockTop].Height;
                    rectDocumentBounds.Height -= DockWindows[DockState.DockTop].Height;
                }

                if (DockWindows[DockState.DockBottom].Visible)
                {
                    rectDocumentBounds.Height -= DockWindows[DockState.DockBottom].Height;
                }

                return rectDocumentBounds;
            }
        }

        private PaintEventHandler? m_dummyControlPaintEventHandler;

        private void InvalidateWindowRegion()
        {
            if (DesignMode)
            {
                return;
            }

            if (m_dummyControlPaintEventHandler == null!)
            {
                m_dummyControlPaintEventHandler = DummyControl_Paint;
            }

            DummyControl.Paint += m_dummyControlPaintEventHandler;
            DummyControl.Invalidate();
        }

        void DummyControl_Paint (object? sender, PaintEventArgs eventArgs)
        {
            DummyControl.Paint -= m_dummyControlPaintEventHandler;
            UpdateWindowRegion();
        }

        private void UpdateWindowRegion()
        {
            if (DocumentStyle == DocumentStyle.DockingMdi)
            {
                UpdateWindowRegion_ClipContent();
            }
            else if (DocumentStyle == DocumentStyle.DockingSdi ||
                     DocumentStyle == DocumentStyle.DockingWindow)
            {
                UpdateWindowRegion_FullDocumentArea();
            }
            else if (DocumentStyle == DocumentStyle.SystemMdi)
            {
                UpdateWindowRegion_EmptyDocumentArea();
            }
        }

        private void UpdateWindowRegion_FullDocumentArea()
        {
            SetRegion (null);
        }

        private void UpdateWindowRegion_EmptyDocumentArea()
        {
            var rect = DocumentWindowBounds;
            SetRegion (new Rectangle[] { rect });
        }

        private void UpdateWindowRegion_ClipContent()
        {
            var count = 0;
            foreach (var pane in Panes)
            {
                if (!pane.Visible || pane.DockState != DockState.Document)
                {
                    continue;
                }

                count++;
            }

            if (count == 0)
            {
                SetRegion (null);
                return;
            }

            var rects = new Rectangle[count];
            var i = 0;
            foreach (var pane in Panes)
            {
                if (!pane.Visible || pane.DockState != DockState.Document)
                {
                    continue;
                }

                rects[i] = RectangleToClient (pane.RectangleToScreen (pane.ContentRectangle));
                i++;
            }

            SetRegion (rects);
        }

        private Rectangle[]? _clipRects;

        private void SetRegion
            (
                Rectangle[]? clipRects
            )
        {
            if (!IsClipRectsChanged (clipRects))
            {
                return;
            }

            _clipRects = clipRects;

            if (_clipRects == null || _clipRects.GetLength (0) == 0)
            {
                Region = null;
            }
            else
            {
                var region = new Region (new Rectangle (0, 0, Width, Height));
                foreach (var rect in _clipRects)
                {
                    region.Exclude (rect);
                }

                if (Region != null)
                {
                    Region.Dispose();
                }

                Region = region;
            }
        }

        private bool IsClipRectsChanged (Rectangle[]? clipRects)
        {
            if (clipRects == null && _clipRects == null)
            {
                return false;
            }
            else if ((clipRects == null) != (_clipRects == null))
            {
                return true;
            }

            foreach (var rect in clipRects!)
            {
                var matched = false;
                foreach (var rect2 in _clipRects!)
                {
                    if (rect == rect2)
                    {
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    return true;
                }
            }

            foreach (var rect2 in _clipRects!)
            {
                var matched = false;
                foreach (var rect in clipRects)
                {
                    if (rect == rect2)
                    {
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    return true;
                }
            }

            return false;
        }

        private static readonly object ActiveAutoHideContentChangedEvent = new ();

        /// <summary>
        ///
        /// </summary>
        [LocalizedCategory ("Category_DockingNotification")]
        [LocalizedDescription ("DockPanel_ActiveAutoHideContentChanged_Description")]
        public event EventHandler ActiveAutoHideContentChanged
        {
            add => Events.AddHandler (ActiveAutoHideContentChangedEvent, value);
            remove => Events.RemoveHandler (ActiveAutoHideContentChangedEvent, value);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnActiveAutoHideContentChanged (EventArgs e)
        {
            var handler = (EventHandler?) Events[ActiveAutoHideContentChangedEvent];
            if (handler != null)
            {
                handler (this, e);
            }
        }

        private void m_autoHideWindow_ActiveContentChanged
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            OnActiveAutoHideContentChanged (eventArgs);
        }


        private static readonly object ContentAddedEvent = new ();

        /// <summary>
        ///
        /// </summary>
        [LocalizedCategory ("Category_DockingNotification")]
        [LocalizedDescription ("DockPanel_ContentAdded_Description")]
        public event EventHandler<DockContentEventArgs> ContentAdded
        {
            add => Events.AddHandler (ContentAddedEvent, value);
            remove => Events.RemoveHandler (ContentAddedEvent, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnContentAdded (DockContentEventArgs e)
        {
            var handler = (EventHandler<DockContentEventArgs>?) Events[ContentAddedEvent];
            if (handler != null)
            {
                handler (this, e);
            }
        }

        private static readonly object ContentRemovedEvent = new ();

        /// <summary>
        ///
        /// </summary>
        [LocalizedCategory ("Category_DockingNotification")]
        [LocalizedDescription ("DockPanel_ContentRemoved_Description")]
        public event EventHandler<DockContentEventArgs> ContentRemoved
        {
            add => Events.AddHandler (ContentRemovedEvent, value);
            remove => Events.RemoveHandler (ContentRemovedEvent, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventArgs"></param>
        protected virtual void OnContentRemoved
            (
                DockContentEventArgs eventArgs
            )
        {
            var handler = (EventHandler<DockContentEventArgs>?) Events[ContentRemovedEvent];
            if (handler != null)
            {
                handler (this, eventArgs);
            }
        }

        internal void ResetDockWindows()
        {
            if (m_autoHideWindow == null)
            {
                return;
            }

            var old = m_dockWindows;
            LoadDockWindows();
            foreach (var dockWindow in old)
            {
                Controls.Remove (dockWindow);
                dockWindow.Dispose();
            }
        }

        internal void LoadDockWindows()
        {
            m_dockWindows = new DockWindowCollection (this);
            foreach (var dockWindow in DockWindows)
            {
                Controls.Add (dockWindow);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ResetAutoHideStripWindow()
        {
            var old = m_autoHideWindow;
            m_autoHideWindow = Theme.Extender.AutoHideWindowFactory.CreateAutoHideWindow (this);
            m_autoHideWindow.Visible = false;
            SetAutoHideWindowParent();

            old!.Visible = false;
            old.Parent = null;
            old.Dispose();
        }
    }
}
