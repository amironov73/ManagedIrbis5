// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.ExtendedToolkit.Extensions;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Styling;
using Avalonia.VisualTree;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls
{
    //ported from https://github.com/MahApps/MahApps.Metro

    /// <summary>
    /// Interaction logic for <see cref="MetroWindow"/> xaml.
    /// </summary>
    public partial class MetroWindow : Window, IStyleable
    {
        private static Pointer dummyMovePointer = null;
        private static PointerPressedEventArgs dummyPointerPressedEventArgs = null;

#warning check commented code

        private void ToggleWindowState()
        {
            var oldValue = WindowState;
            switch (WindowState)
            {
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;

                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
            }

            RaisePropertyChanged (WindowStateProperty, oldValue, WindowState, Data.BindingPriority.TemplatedParent);
        }

        internal void HandleFlyoutStatusChange (Flyout flyout, List<Flyout> visibleFlyouts)
        {
            //checks a recently opened flyout's position.
            //if (flyout.Position == Position.Left || flyout.Position == Position.Right || flyout.Position == Position.Top)
            {
                //get it's zindex
                var zIndex = flyout.IsOpen ? flyout.ZIndex + 3 : visibleFlyouts.Count + 2;

                //if the the corresponding behavior has the right flag, set the window commands' and icon zIndex to a number that is higher than the flyout's.
                _icon?.SetValue (ZIndexProperty,
                    flyout.IsModal && flyout.IsOpen
                        ? 0
                        : (IconOverlayBehavior.HasFlag (OverlayBehavior.Flyouts) ? zIndex : 1));
                LeftWindowCommandsPresenter?.SetValue (ZIndexProperty, flyout.IsModal && flyout.IsOpen ? 0 : 1);
                RightWindowCommandsPresenter?.SetValue (ZIndexProperty, flyout.IsModal && flyout.IsOpen ? 0 : 1);
                WindowButtonCommandsPresenter?.SetValue (ZIndexProperty,
                    flyout.IsModal && flyout.IsOpen
                        ? 0
                        : (WindowButtonCommandsOverlayBehavior.HasFlag (OverlayBehavior.Flyouts) ? zIndex : 1));
                this.HandleWindowCommandsForFlyouts (visibleFlyouts);
            }

            if (flyoutModal != null)
            {
                flyoutModal.IsVisible = visibleFlyouts.Any (x => x.IsModal) ? true : false;
            }

            //flyout.IsVisible = true;

            RaiseEvent (new FlyoutStatusChangedRoutedEventArgs (FlyoutsStatusChangedEvent, this)
                { ChangedFlyout = flyout });
        }

        /// <inheritdoc cref="InputElement.OnPointerPressed"/>
        protected override void OnPointerPressed
            (
                PointerPressedEventArgs eventArgs
            )
        {
            if (Design.IsDesignMode)
            {
                return;
            }

            if (_topHorizontalGrip is { IsPointerOver: true })
            {
                BeginResizeDrag (WindowEdge.North, eventArgs);
            }
            else if (_bottomHorizontalGrip is { IsPointerOver: true })
            {
                BeginResizeDrag (WindowEdge.South, eventArgs);
            }
            else if (_leftVerticalGrip is { IsPointerOver: true })
            {
                BeginResizeDrag (WindowEdge.West, eventArgs);
            }
            else if (_rightVerticalGrip is { IsPointerOver: true })
            {
                BeginResizeDrag (WindowEdge.East, eventArgs);
            }
            else if (_topLeftGrip is { IsPointerOver: true })
            {
                BeginResizeDrag (WindowEdge.NorthWest, eventArgs);
            }
            else if (_bottomLeftGrip is { IsPointerOver: true })
            {
                BeginResizeDrag (WindowEdge.SouthWest, eventArgs);
            }
            else if (_topRightGrip is { IsPointerOver: true })
            {
                BeginResizeDrag (WindowEdge.NorthEast, eventArgs);
            }
            else if (_bottomRightGrip is { IsPointerOver: true })
            {
                BeginResizeDrag (WindowEdge.SouthEast, eventArgs);
            }
            else if (_titleBar is { IsPointerOver: true })
            {
                _mouseDown = true;
                _mouseDownPosition = eventArgs.GetPosition (this);
            }
            else
            {
                _mouseDown = false;
            }

            base.OnPointerPressed (eventArgs);
        }

        /// <inheritdoc cref="Control.OnPointerReleased"/>
        protected override void OnPointerReleased
            (
                PointerReleasedEventArgs eventArgs
            )
        {
            if (Design.IsDesignMode)
            {
                return;
            }

            _mouseDown = false;
            base.OnPointerReleased (eventArgs);
        }

        /// <inheritdoc/>

        //protected override void OnPointerMoved(PointerEventArgs e)
        //{
        //    if (_titleBar != null && _titleBar.IsPointerOver && _mouseDown)
        //    {
        //        WindowState = WindowState.Normal;
        //        BeginMoveDrag(e);
        //        _mouseDown = false;
        //    }
        //    base.OnPointerMoved(e);
        //}
        public MetroWindow()
        {
            if (Design.IsDesignMode == false)
            {
                ShowIconOnTitleBarProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    OnShowIconOnTitleBarPropertyChangedCallback (o, e));
                ShowTitleBarProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    OnShowTitleBarPropertyChangedCallback (o, e));
                TitleBarHeightProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    TitleBarHeightPropertyChangedCallback (o, e));

                //TitleCharacterCasingProperty.Changed.AddClassHandler<MetroWindow>((o, e) => TitleCharacterCasingChangedCallback(o, e));
                TitleAlignmentProperty.Changed.AddClassHandler<MetroWindow> ((o, e) => OnTitleAlignmentChanged (o, e));

                FlyoutsProperty.Changed.AddClassHandler<MetroWindow> ((o, e) => UpdateLogicalChilds (o, e));
                LeftWindowCommandsProperty.Changed.AddClassHandler<MetroWindow> ((o, e) => UpdateLogicalChilds (o, e));
                RightWindowCommandsProperty.Changed.AddClassHandler<MetroWindow> ((o, e) => UpdateLogicalChilds (o, e));
                WindowButtonCommandsProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    UpdateLogicalChilds (o, e));
                LeftWindowCommandsOverlayBehaviorProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    OnShowTitleBarPropertyChangedCallback (o, e));
                RightWindowCommandsOverlayBehaviorProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    OnShowTitleBarPropertyChangedCallback (o, e));
                WindowButtonCommandsOverlayBehaviorProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    OnShowTitleBarPropertyChangedCallback (o, e));
                IconOverlayBehaviorProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    OnShowTitleBarPropertyChangedCallback (o, e));
                UseNoneWindowStyleProperty.Changed.AddClassHandler<MetroWindow> ((o, e) =>
                    OnUseNoneWindowStylePropertyChangedCallback (o, e));

                WidthProperty.Changed.AddClassHandler<MetroWindow> ((o, e) => OnWidthChanged (o, e));
                HeightProperty.Changed.AddClassHandler<MetroWindow> ((o, e) => OnHeightChanged (o, e));
                ThemeManager.Instance.IsThemeChanged += ThemeManagerOnIsThemeChanged;

                SetVisibiltyForAllTitleElements();

                if (Flyouts == null)
                {
                    Flyouts = new FlyoutsControl();
                }
            }
        }

        /// <inheritdoc cref="TopLevel.OnOpened"/>
        protected override void OnOpened
            (
                EventArgs eventArgs
            )
        {
            base.OnOpened (eventArgs);

            // HACK so the controls are drawn correctly
            // TODO remove this hack if drawing is fixed
            //workaround for so the window is correctly displayed

            var tempState = WindowState;
            WindowState = WindowState.Maximized;
            WindowState = tempState;
        }

        private void OnWidthChanged (MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            RaiseEvent (new RoutedEventArgs (SizeChangedEvent));
        }

        private void OnHeightChanged (MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            RaiseEvent (new RoutedEventArgs (SizeChangedEvent));
        }

        private void OnTitleAlignmentChanged (MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        {
            o.SizeChanged -= o.MetroWindow_SizeChanged;
            if (e.NewValue is HorizontalAlignment && (HorizontalAlignment)e.NewValue == HorizontalAlignment.Center &&
                o._titleBar != null)
            {
                o.SizeChanged += o.MetroWindow_SizeChanged;
            }
        }

        //private void TitleCharacterCasingChangedCallback(MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        //{
        //    if(e.NewValue is CharacterCasing)
        //    {
        //    }

        //    //value => CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper

        //}

        private void TitleBarHeightPropertyChangedCallback (MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                o.SetVisibiltyForAllTitleElements();
            }
        }

        private void OnShowIconOnTitleBarPropertyChangedCallback (MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                SetVisibiltyForIcon();
            }
        }

        private void SetVisibiltyForIcon()
        {
            if (_icon != null)
            {
                var isVisible = (IconOverlayBehavior.HasFlag (OverlayBehavior.HiddenTitleBar) && !ShowTitleBar)
                                || (ShowIconOnTitleBar && ShowTitleBar);

                _icon.IsVisible = isVisible;
            }
        }

        private void OnUseNoneWindowStylePropertyChangedCallback (MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                // if UseNoneWindowStyle = true no title bar should be shown
                var useNoneWindowStyle = (bool)e.NewValue;

                // UseNoneWindowStyle means no title bar, window commands or min, max, close buttons
                if (useNoneWindowStyle)
                {
                    o.SetValue (ShowTitleBarProperty, false);
                }
            }
        }

        private void OnShowTitleBarPropertyChangedCallback (MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                o.SetVisibiltyForAllTitleElements();
            }
        }

        private void ThemeManagerOnIsThemeChanged (object sender, OnThemeChangedEventArgs e)
        {
            if (e.Theme != null)
            {
                var flyouts = Flyouts.GetFlyouts().ToList();

                // since we disabled the ThemeManager OnThemeChanged part, we must change all children flyouts too
                // e.g if the FlyoutsControl is hosted in a UserControl
                var allChildFlyouts = (Content as IVisual)?.GetSelfAndVisualDescendants().OfType<FlyoutsControl>()
                    .ToList();
                if (allChildFlyouts?.Any() == true)
                {
                    flyouts.AddRange (allChildFlyouts.SelectMany (flyoutsControl => flyoutsControl.GetFlyouts()));
                }

                if (!flyouts.Any())
                {
                    // we must update the window command brushes!!!
                    this.ResetAllWindowCommandsBrush();
                    return;
                }

                foreach (var flyout in flyouts)
                {
                    flyout.ChangeFlyoutTheme (e.Theme);
                }

                this.HandleWindowCommandsForFlyouts (flyouts);
            }
        }

        private void SetVisibiltyForAllTitleElements()
        {
            if (Design.IsDesignMode)
            {
                return;
            }

            SetVisibiltyForIcon();
            var newVisibility = TitleBarHeight > 0 && ShowTitleBar && !UseNoneWindowStyle ? true : false;

            _titleBar?.SetValue (IsVisibleProperty, newVisibility);
            _titleBarBackground?.SetValue (IsVisibleProperty, newVisibility);

            var leftWindowCommandsVisibility =
                LeftWindowCommandsOverlayBehavior.HasFlag (WindowCommandsOverlayBehavior.HiddenTitleBar) &&
                !UseNoneWindowStyle
                    ? true
                    : newVisibility;
            LeftWindowCommandsPresenter?.SetValue (IsVisibleProperty, leftWindowCommandsVisibility);

            var rightWindowCommandsVisibility =
                RightWindowCommandsOverlayBehavior.HasFlag (WindowCommandsOverlayBehavior.HiddenTitleBar) &&
                !UseNoneWindowStyle
                    ? true
                    : newVisibility;
            RightWindowCommandsPresenter?.SetValue (IsVisibleProperty, rightWindowCommandsVisibility);

            var windowButtonCommandsVisibility =
                WindowButtonCommandsOverlayBehavior.HasFlag (OverlayBehavior.HiddenTitleBar) ? true : newVisibility;
            WindowButtonCommandsPresenter?.SetValue (IsVisibleProperty, windowButtonCommandsVisibility);

            SetWindowEvents();
        }

        private void SetWindowEvents()
        {
            // clear all event handlers first
            ClearWindowEvents();

            // set mouse down/up for icon
            if (_icon is { IsVisible: true })
            {
                _icon.PointerPressed += IconMouseDown;
            }

            if (_windowTitleThumb != null)
            {
                //this.windowTitleThumb.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
                _windowTitleThumb.DragDelta += WindowTitleThumbMoveOnDragDelta;
                _windowTitleThumb.DoubleTapped += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                _windowTitleThumb.PointerReleased += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            var thumbContentControl = _titleBar as IMetroThumb;
            if (thumbContentControl != null)
            {
                //thumbContentControl.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
                thumbContentControl.DragDelta += WindowTitleThumbMoveOnDragDelta;
                thumbContentControl.DoubleTapped += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                thumbContentControl.PointerReleased += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            if (_flyoutModalDragMoveThumb != null)
            {
                //this.flyoutModalDragMoveThumb.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
                _flyoutModalDragMoveThumb.DragDelta += WindowTitleThumbMoveOnDragDelta;
                _flyoutModalDragMoveThumb.DoubleTapped += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                _flyoutModalDragMoveThumb.PointerReleased += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            // handle size if we have a Grid for the title (e.g. clean window have a centered title)
            //if (titleBar != null && titleBar.GetType() == typeof(Grid))
            if (_titleBar != null && TitleAlignment == HorizontalAlignment.Center)
            {
                SizeChanged += MetroWindow_SizeChanged;
            }
        }

        private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick (object sender, RoutedEventArgs e)
        {
            DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick (this, e);
        }

        private void MetroWindow_SizeChanged (object sender, EventArgs e)
        {
            // this all works only for centered title
            if (TitleAlignment != HorizontalAlignment.Center)
            {
                return;
            }

            // Half of this MetroWindow
            var halfDistance = Width / 2;

            // Distance between center and left/right
            var margin = (Thickness)_titleBar.GetValue (MarginProperty);
            var distanceToCenter = (_titleBar.DesiredSize.Width - margin.Left - margin.Right) / 2;

            var iconWidth = _icon?.Width ?? 0;
            var leftWindowCommandsWidth = LeftWindowCommands?.Width ?? 0;
            var rightWindowCommandsWidth = RightWindowCommands?.Width ?? 0;
            var windowButtonCommandsWith = WindowButtonCommands?.Width ?? 0;

            // Distance between right edge from LeftWindowCommands to left window side
            var distanceFromLeft = iconWidth + leftWindowCommandsWidth;

            // Distance between left edge from RightWindowCommands to right window side
            var distanceFromRight = rightWindowCommandsWidth + windowButtonCommandsWith;

            // Margin
            const double horizontalMargin = 5.0;

            var dLeft = distanceFromLeft + distanceToCenter + horizontalMargin;
            var dRight = distanceFromRight + distanceToCenter + horizontalMargin;
            if ((dLeft < halfDistance) && (dRight < halfDistance))
            {
                _titleBar.SetValue (MarginProperty, default (Thickness));
                Grid.SetColumn (_titleBar, 0);
                Grid.SetColumnSpan (_titleBar, 5);
            }
            else
            {
                _titleBar.SetValue (MarginProperty,
                    new Thickness (leftWindowCommandsWidth, 0, rightWindowCommandsWidth, 0));
                Grid.SetColumn (_titleBar, 2);
                Grid.SetColumnSpan (_titleBar, 1);
            }
        }

        private void WindowTitleThumbSystemMenuOnMouseRightButtonUp (object sender, PointerReleasedEventArgs e)
        {
            if (e.InitialPressMouseButton != MouseButton.Right)
            {
                return;
            }

            DoWindowTitleThumbSystemMenuOnMouseRightButtonUp (this, e);
        }

        internal void DoWindowTitleThumbSystemMenuOnMouseRightButtonUp (MetroWindow metroWindow,
            PointerReleasedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        internal static void DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick (MetroWindow metroWindow,
            RoutedEventArgs mouseButtonEventArgs)
        {
            // restore/maximize only with left button
            //if (mouseButtonEventArgs.ChangedButton == MouseButton.Left)
            {
                // we can maximize or restore the window if the title bar height is set (also if title bar is hidden)
                var canResize =
                    metroWindow
                        .CanResize; //.ResizeMode == ResizeMode.CanResizeWithGrip || metroWindow.ResizeMode == ResizeMode.CanResize;

                //var mousePos = Mouse.GetPosition(window);
                //var isMouseOnTitlebar = mousePos.Y <= window.TitleBarHeight && window.TitleBarHeight > 0;
                if (canResize /*&& isMouseOnTitlebar*/)
                {
                    metroWindow.ToggleWindowState();

                    mouseButtonEventArgs.Handled = true;
                }
            }
        }

        private void WindowTitleThumbMoveOnDragDelta (object sender, VectorEventArgs e)
        {
            DoWindowTitleThumbMoveOnDragDelta (sender as IMetroThumb, this, e);
        }

        private void IconMouseDown (object sender, PointerPressedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ClearWindowEvents()
        {
            // clear all event handlers first:
            if (_windowTitleThumb != null)
            {
                //this.windowTitleThumb.PreviewMouseLeftButtonUp -= this.WindowTitleThumbOnPreviewMouseLeftButtonUp;
                _windowTitleThumb.DragDelta -= WindowTitleThumbMoveOnDragDelta;
                _windowTitleThumb.PointerPressed -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                _windowTitleThumb.PointerReleased -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            var thumbContentControl = _titleBar as IMetroThumb;
            if (thumbContentControl != null)
            {
                //thumbContentControl.PreviewMouseLeftButtonUp -= this.WindowTitleThumbOnPreviewMouseLeftButtonUp;
                thumbContentControl.DragDelta -= WindowTitleThumbMoveOnDragDelta;
                thumbContentControl.PointerPressed -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                thumbContentControl.PointerReleased -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            if (_flyoutModalDragMoveThumb != null)
            {
                //this.flyoutModalDragMoveThumb.PreviewMouseLeftButtonUp -= this.WindowTitleThumbOnPreviewMouseLeftButtonUp;
                _flyoutModalDragMoveThumb.DragDelta -= WindowTitleThumbMoveOnDragDelta;
                _flyoutModalDragMoveThumb.PointerPressed -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                _flyoutModalDragMoveThumb.PointerReleased -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            if (_icon != null)
            {
                _icon.PointerPressed -= IconMouseDown;
            }

            SizeChanged -= MetroWindow_SizeChanged;
        }

        private void UpdateLogicalChilds (MetroWindow o, AvaloniaPropertyChangedEventArgs e)
        {
            var oldChild = e.OldValue as StyledElement;
            if (oldChild != null)
            {
                //this.RemoveLogicalChild(oldChild);
                LogicalChildren.Remove (oldChild);
            }

            var newChild = e.NewValue as StyledElement;
            if (newChild != null)
            {
                LogicalChildren.Add (newChild);

                // Yes, that's crazy. But we must do this to enable all possible scenarios for setting DataContext
                // in a Window. Without set the DataContext at this point it can happen that e.g. a Flyout
                // doesn't get the same DataContext.
                // So now we can type
                //
                // this.InitializeComponent();
                // this.DataContext = new MainViewModel();
                //
                // or
                //
                // this.DataContext = new MainViewModel();
                // this.InitializeComponent();
                //
                newChild.DataContext = DataContext;
            }
        }

        internal static void DoWindowTitleThumbMoveOnDragDelta (IMetroThumb thumb, MetroWindow window,
            VectorEventArgs dragDeltaEventArgs)
        {
            if (thumb == null)
            {
                throw new ArgumentNullException (nameof (thumb));
            }

            if (window == null)
            {
                throw new ArgumentNullException (nameof (window));
            }

            // drag only if IsWindowDraggable is set to true
            if (!window.IsWindowDraggable ||
                (!(Math.Abs (dragDeltaEventArgs.Vector.Y) > 2) && !(Math.Abs (dragDeltaEventArgs.Vector.X) > 2)))
            {
                return;
            }

            // tage from DragMove internal code
            window.VerifyAccess();

            //var cursorPos = WinApiHelper.GetPhysicalCursorPos();

            // if the window is maximized dragging is only allowed on title bar (also if not visible)
            var windowIsMaximized = window.WindowState == WindowState.Maximized;

            if (window._titleBar is { IsPointerOver: true } /*&& window._mouseDown*/)
            {
                window.WindowState = WindowState.Normal;

                try
                {
                    if (dummyMovePointer == null)
                    {
                        dummyMovePointer =
                            new Pointer (0, PointerType.Mouse, true);
                    }

                    if (dummyPointerPressedEventArgs == null)
                    {
                        //just create a dummy PointerPressedEventArgs
                        dummyPointerPressedEventArgs = new PointerPressedEventArgs
                            (
                                window,
                                dummyMovePointer,
                                window,
                                new Point (1, 1),
                                0,
                                new PointerPointProperties (RawInputModifiers.None,
                                    PointerUpdateKind.LeftButtonPressed),
                                KeyModifiers.None
                            );
                    }

                    window.PlatformImpl.BeginMoveDrag (dummyPointerPressedEventArgs);
                }
                catch
                {
                }

                window._mouseDown = false;
            }

            //var isMouseOnTitlebar = Mouse.GetPosition(thumb).Y <= window.TitleBarHeight && window.TitleBarHeight > 0;
            //if (!isMouseOnTitlebar && windowIsMaximized)
            //{
            //    return;
            //}

#pragma warning disable 618

            // for the touch usage
            //UnsafeNativeMethods.ReleaseCapture();
#pragma warning restore 618

            if (windowIsMaximized)
            {
                //var cursorXPos = cursorPos.x;
                EventHandler windowOnStateChanged = null;
                windowOnStateChanged = (sender, args) =>
                {
                    //window.Top = 2;
                    //window.Left = Math.Max(cursorXPos - window.RestoreBounds.Width / 2, 0);

                    //window.StateChanged -= windowOnStateChanged;
                    //if (window.WindowState == WindowState.Normal)
                    //{
                    //    Mouse.Capture(thumb, CaptureMode.Element);
                    //}
                };

                //window.StateChanged -= windowOnStateChanged;
                //window.StateChanged += windowOnStateChanged;
            }

            //            var criticalHandle = window.CriticalHandle;
            //#pragma warning disable 618
            //            // these lines are from DragMove
            //            // NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr)SC.MOUSEMOVE, IntPtr.Zero);
            //            // NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);

            //            var wpfPoint = window.PointToScreen(Mouse.GetPosition(window));
            //            var x = (int)wpfPoint.X;
            //            var y = (int)wpfPoint.Y;
            //            NativeMethods.SendMessage(criticalHandle, WM.NCLBUTTONDOWN, (IntPtr)HT.CAPTION, new IntPtr(x | (y << 16)));
            //#pragma warning restore 618
        }

        /// <inheritdoc cref="TopLevel.OnApplyTemplate"/>
        protected override void OnApplyTemplate
            (
                TemplateAppliedEventArgs eventArgs
            )
        {
            base.OnApplyTemplate (eventArgs);

            LeftWindowCommandsPresenter = eventArgs.NameScope.Find<ContentPresenter> (PART_LeftWindowCommands);
            RightWindowCommandsPresenter = eventArgs.NameScope.Find<ContentPresenter> (PART_RightWindowCommands);
            WindowButtonCommandsPresenter = eventArgs.NameScope.Find<ContentPresenter> (PART_WindowButtonCommands);

            if (LeftWindowCommands == null)
            {
                LeftWindowCommands = new WindowCommands();
            }

            if (RightWindowCommands == null)
            {
                RightWindowCommands = new WindowCommands();
            }

            if (WindowButtonCommands == null)
            {
                WindowButtonCommands = new WindowButtonCommands();
            }

            LeftWindowCommands.ParentWindow = this;
            RightWindowCommands.ParentWindow = this;
            WindowButtonCommands.ParentWindow = this;
            overlayBox = eventArgs.NameScope.Find<Grid> (PART_OverlayBox);
            metroActiveDialogContainer = eventArgs.NameScope.Find<Grid> (PART_MetroActiveDialogContainer);
            metroInactiveDialogContainer = eventArgs.NameScope.Find<Grid> (PART_MetroInactiveDialogsContainer);
            flyoutModal = eventArgs.NameScope.Find<Rectangle> (PART_FlyoutModal);
            flyoutModal.PointerPressed += FlyoutsPreviewMouseDown;

            //flyoutModal.MouseDown += FlyoutsPreviewMouseDown;
            PointerPressed += FlyoutsPreviewMouseDown;

            _icon = eventArgs.NameScope.Find<ContentControl> (PART_Icon);
            _titleBar = eventArgs.NameScope.Find<ContentControl> (PART_TitleBar);
            _titleBarBackground = eventArgs.NameScope.Find<Rectangle> (PART_WindowTitleBackground);
            _windowTitleThumb = eventArgs.NameScope.Find<Thumb> (PART_WindowTitleThumb);
            _flyoutModalDragMoveThumb = eventArgs.NameScope.Find<Thumb> (PART_FlyoutModalDragMoveThumb);
            SetVisibiltyForAllTitleElements();

            var metroContentControl = eventArgs.NameScope.Find<MetroContentControl> (PART_Content);
            if (metroContentControl != null)
            {
                if (Design.IsDesignMode == false)
                {
                    metroContentControl.TransitionCompleted += (sender, args) =>
                        RaiseEvent (new RoutedEventArgs (WindowTransitionCompletedEvent));
                }
            }

            _topHorizontalGrip = eventArgs.NameScope.Find<Grid> (PART_TopHorizontalGrip);
            _bottomHorizontalGrip = eventArgs.NameScope.Find<Grid> (PART_BottomHorizontalGrip);
            _leftVerticalGrip = eventArgs.NameScope.Find<Grid> (PART_LeftVerticalGrip);
            _rightVerticalGrip = eventArgs.NameScope.Find<Grid> (PART_RightVerticalGrip);

            _topLeftGrip = eventArgs.NameScope.Find<Grid> (PART_TopLeftGrip);
            _bottomLeftGrip = eventArgs.NameScope.Find<Grid> (PART_BottomLeftGrip);
            _topRightGrip = eventArgs.NameScope.Find<Grid> (PART_TopRightGrip);
            _bottomRightGrip = eventArgs.NameScope.Find<Grid> (PART_BottomRightGrip);
        }

        private void FlyoutsPreviewMouseDown (object sender, PointerPressedEventArgs e)
        {
            var element = (e.Source as IControl);
            if (element != null)
            {
                // no preview if we just clicked these elements
                if (element.TryFindParent<Flyout>() != null
                    || Equals (element, overlayBox)

                    //|| element.TryFindParent<BaseMetroDialog>() != null
                    || Equals (element.TryFindParent<ContentControl>(), Icon)
                    || element.TryFindParent<WindowCommands>() != null
                    || element.TryFindParent<WindowButtonCommands>() != null)
                {
                    return;
                }
            }

            //don't know how to do this in avalonia e.ChangedButton
            //miaaing in the event.

            //if (Flyouts.OverrideExternalCloseButton == null)
            //{
            //    foreach (var flyout in Flyouts.GetFlyouts().
            //        Where(x => x.IsOpen && x.ExternalCloseButton == e.ChangedButton
            //        && (!x.IsPinned || Flyouts.OverrideIsPinned)))
            //    {
            //        flyout.IsOpen = false;
            //    }
            //}
            //else if (Flyouts.OverrideExternalCloseButton == e.ChangedButton)
            //{
            //    foreach (var flyout in Flyouts.GetFlyouts().Where(x => x.IsOpen &&
            //    (!x.IsPinned || Flyouts.OverrideIsPinned)))
            //    {
            //        flyout.IsOpen = false;
            //    }
            //}
        }
    }
}
