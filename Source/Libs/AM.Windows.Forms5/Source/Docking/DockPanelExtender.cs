// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* DockPanelExtender.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

using static AM.Windows.Forms.Docking.DockPanel;
using static AM.Windows.Forms.Docking.DockPanel.DockDragHandler;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public sealed class DockPanelExtender
{
    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    public interface IDockPaneFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <param name="visibleState"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        DockPane CreateDockPane (IDockContent content, DockState visibleState, bool show);

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <param name="floatWindow"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        [SuppressMessage ("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "1#")]
        DockPane CreateDockPane (IDockContent content, FloatWindow floatWindow, bool show);

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <param name="previousPane"></param>
        /// <param name="alignment"></param>
        /// <param name="proportion"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        DockPane CreateDockPane (IDockContent content, DockPane previousPane, DockAlignment alignment,
            double proportion, bool show);

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <param name="floatWindowBounds"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        [SuppressMessage ("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "1#")]
        DockPane CreateDockPane (IDockContent content, Rectangle floatWindowBounds, bool show);
    }

    /// <summary>
    ///
    /// </summary>
    public interface IDockPaneSplitterControlFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="pane"></param>
        /// <returns></returns>
        DockPane.SplitterControlBase CreateSplitterControl (DockPane pane);
    }

    /// <summary>
    ///
    /// </summary>
    public interface IWindowSplitterControlFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        SplitterBase CreateSplitterControl (ISplitterHost host);
    }

    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    public interface IFloatWindowFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="pane"></param>
        /// <returns></returns>
        FloatWindow CreateFloatWindow (DockPanel dockPanel, DockPane pane);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="pane"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        FloatWindow CreateFloatWindow (DockPanel dockPanel, DockPane pane, Rectangle bounds);
    }

    /// <summary>
    ///
    /// </summary>
    public interface IDockWindowFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="dockState"></param>
        /// <returns></returns>
        DockWindow CreateDockWindow (DockPanel dockPanel, DockState dockState);
    }

    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    public interface IDockPaneCaptionFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="pane"></param>
        /// <returns></returns>
        DockPaneCaptionBase CreateDockPaneCaption (DockPane pane);
    }

    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    public interface IDockPaneStripFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="pane"></param>
        /// <returns></returns>
        DockPaneStripBase CreateDockPaneStrip (DockPane pane);
    }

    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    public interface IAutoHideStripFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        AutoHideStripBase CreateAutoHideStrip (DockPanel panel);
    }

    /// <summary>
    ///
    /// </summary>
    public interface IAutoHideWindowFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        AutoHideWindowControl CreateAutoHideWindow (DockPanel panel);
    }

    /// <summary>
    ///
    /// </summary>
    public interface IPaneIndicatorFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        IPaneIndicator CreatePaneIndicator (ThemeBase theme);
    }

    /// <summary>
    ///
    /// </summary>
    public interface IPanelIndicatorFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="style"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        IPanelIndicator CreatePanelIndicator (DockStyle style, ThemeBase theme);
    }

    /// <summary>
    ///
    /// </summary>
    public interface IDockOutlineFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DockOutlineBase CreateDockOutline();
    }

    /// <summary>
    ///
    /// </summary>
    public interface IDockIndicatorFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dockDragHandler"></param>
        /// <returns></returns>
        DockIndicator CreateDockIndicator (DockDragHandler dockDragHandler);
    }

    #region DefaultDockPaneFactory

    private class DefaultDockPaneFactory
        : IDockPaneFactory
    {
        public DockPane CreateDockPane (IDockContent content, DockState visibleState, bool show)
        {
            return new DockPane (content, visibleState, show);
        }

        public DockPane CreateDockPane (IDockContent content, FloatWindow floatWindow, bool show)
        {
            return new DockPane (content, floatWindow, show);
        }

        public DockPane CreateDockPane (IDockContent content, DockPane prevPane, DockAlignment alignment,
            double proportion, bool show)
        {
            return new DockPane (content, prevPane, alignment, proportion, show);
        }

        public DockPane CreateDockPane (IDockContent content, Rectangle floatWindowBounds, bool show)
        {
            return new DockPane (content, floatWindowBounds, show);
        }
    }

    #endregion

    #region DefaultFloatWindowFactory

    private class DefaultFloatWindowFactory
        : IFloatWindowFactory
    {
        public FloatWindow CreateFloatWindow (DockPanel dockPanel, DockPane pane)
        {
            return new FloatWindow (dockPanel, pane);
        }

        public FloatWindow CreateFloatWindow (DockPanel dockPanel, DockPane pane, Rectangle bounds)
        {
            return new FloatWindow (dockPanel, pane, bounds);
        }
    }

    #endregion

    private IDockPaneFactory? _dockPaneFactory;

    /// <summary>
    ///
    /// </summary>
    public IDockPaneFactory DockPaneFactory
    {
        get => _dockPaneFactory ??= new DefaultDockPaneFactory();
        set => _dockPaneFactory = value;
    }

    /// <summary>
    ///
    /// </summary>
    public IDockPaneSplitterControlFactory? DockPaneSplitterControlFactory { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IWindowSplitterControlFactory? WindowSplitterControlFactory { get; set; }

    private IFloatWindowFactory? _floatWindowFactory;

    /// <summary>
    ///
    /// </summary>
    public IFloatWindowFactory FloatWindowFactory
    {
        get => _floatWindowFactory ??= new DefaultFloatWindowFactory();
        set => _floatWindowFactory = value;
    }

    /// <summary>
    ///
    /// </summary>
    public IDockWindowFactory? DockWindowFactory { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IDockPaneCaptionFactory? DockPaneCaptionFactory { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IDockPaneStripFactory? DockPaneStripFactory { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IAutoHideStripFactory AutoHideStripFactory { get; set; } = null!;

    /// <summary>
    ///
    /// </summary>
    public IAutoHideWindowFactory AutoHideWindowFactory { get; set; } = null!;

    /// <summary>
    ///
    /// </summary>
    public IPaneIndicatorFactory? PaneIndicatorFactory { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IPanelIndicatorFactory? PanelIndicatorFactory { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IDockOutlineFactory? DockOutlineFactory { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IDockIndicatorFactory? DockIndicatorFactory { get; set; }
}
