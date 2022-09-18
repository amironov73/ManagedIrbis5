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
using System.Linq;

using AM;

using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.ExtendedToolkit.Extensions;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/MahApps/MahApps.Metro

/// <summary>
/// A sliding panel control that is hosted in a MetroWindow via a FlyoutsControl.
/// <see cref="MetroWindow"/>
/// <seealso cref="FlyoutsControl"/>
/// </summary>
public partial class Flyout : HeaderedContentControl
{
    /// <summary>
    /// initilaze classhandlers
    /// autoclose timer
    /// </summary>
    public Flyout()
    {
        PositionProperty.Changed.AddClassHandler<Flyout> ((o, e) => PositionChanged (o, e));
        AnimateOpacityProperty.Changed.AddClassHandler<Flyout> ((o, e) => AnimateOpacityChanged (o, e));
        FlyoutThemeProperty.Changed.AddClassHandler<Flyout> ((o, e) => ThemeChanged (o, e));
        IsAutoCloseEnabledProperty.Changed.AddClassHandler<Flyout> ((o, e) => IsAutoCloseEnabledChanged (o, e));
        AutoCloseIntervalProperty.Changed.AddClassHandler<Flyout> ((o, e) => AutoCloseIntervalChanged (o, e));
        ThemeManager.Instance.IsThemeChanged += (o, e) => { UpdateFlyoutTheme(); };
        InitializeAutoCloseTimer();
        IsOpenProperty.Changed.AddClassHandler<Flyout> ((o, e) => IsOpenedChanged (o, e));

        WidthProperty.Changed.AddClassHandler<Flyout> ((o, e) => SizeChanged (o, e));
        HeightProperty.Changed.AddClassHandler<Flyout> ((o, e) => SizeChanged (o, e));
    }

    private void SizeChanged (Flyout o, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name == nameof (Width))
        {
            _currentSize = new Size ((double)e.NewValue, _currentSize.Height);
        }
        else
        {
            _currentSize = new Size (_currentSize.Width, (double)e.NewValue);
        }
    }

    private MetroWindow parentWindow;
    private Size _currentSize;

    private MetroWindow ParentWindow => parentWindow ?? (parentWindow = this.TryFindParent<MetroWindow>());

    /// <inheritdoc cref="Layoutable.MeasureOverride"/>
    protected override Size MeasureOverride (Size availableSize)
    {
        _currentSize = availableSize;
        return base.MeasureOverride (availableSize);
    }

    private void InitializeAutoCloseTimer()
    {
        StopAutoCloseTimer();

        autoCloseTimer = new DispatcherTimer();
        autoCloseTimer.Tick += AutoCloseTimerCallback;
        autoCloseTimer.Interval = TimeSpan.FromMilliseconds (AutoCloseInterval);
    }

    private void UpdateFlyoutTheme()
    {
        var flyoutsControl = this.TryFindParent<FlyoutsControl>();

        if (Design.IsDesignMode)
        {
            IsVisible = flyoutsControl != null ? false : true;
        }

        var window = ParentWindow;
        if (window != null)
        {
            Theme windowTheme = DetectTheme (this);

            if (windowTheme != null)
            {
                ChangeFlyoutTheme (windowTheme);
            }

            // we must certain to get the right foreground for window commands and buttons
            if (flyoutsControl != null && IsOpen)
            {
                flyoutsControl.HandleFlyoutStatusChange (this, window);
            }
        }
    }

    internal void ChangeFlyoutTheme (Theme windowTheme)
    {
        //ApplyTemplate();

        // Beware: Über-dumb code ahead!
        switch (FlyoutTheme)
        {
            case FlyoutTheme.Accent:
                ThemeManager.Instance.ApplyThemeResourcesFromTheme (Styles, windowTheme);
                OverrideFlyoutResources (Styles, true);
                break;

            case FlyoutTheme.Adapt:
                ThemeManager.Instance.ApplyThemeResourcesFromTheme (Styles, windowTheme);
                OverrideFlyoutResources (Styles);
                break;

            case FlyoutTheme.Inverse:
                var inverseTheme = ThemeManager.Instance.GetInverseTheme (windowTheme);

                if (inverseTheme == null)
                    throw new InvalidOperationException ("The inverse flyout theme only works if the window" +
                                                         " theme abides the naming convention. " +
                                                         "See ThemeManager.GetInverseAppTheme for more infos");

                ThemeManager.Instance.ApplyThemeResourcesFromTheme (Styles, inverseTheme);
                OverrideFlyoutResources (Styles);
                break;

            case FlyoutTheme.Dark:
                ThemeManager.Instance.ApplyThemeResourcesFromTheme (Styles,
                    ThemeManager.Instance.Themes.First (x => x.BaseColorScheme == ThemeManager.BaseColorDark
                                                             && x.ColorScheme == windowTheme.ColorScheme));

                OverrideFlyoutResources (Styles);
                break;

            case FlyoutTheme.Light:
                ThemeManager.Instance.ApplyThemeResourcesFromTheme (Styles,
                    ThemeManager.Instance.Themes.First (x => x.BaseColorScheme == ThemeManager.BaseColorLight
                                                             && x.ColorScheme == windowTheme.ColorScheme));
                OverrideFlyoutResources (Styles);
                break;
        }
    }

    private void OverrideFlyoutResources (Styles styles, bool accent = false)
    {
        var fromColorKey = accent ? "MahApps.Colors.Highlight" : "MahApps.Colors.Flyout";
        var item = styles.GetThemeStyle();
        styles.Remove (item);

        ResourceDictionary resources = (item.Loaded as Style).Resources as ResourceDictionary;
        item.TryGetResource (fromColorKey, out object result);
        var fromColor = (Color)result;

        resources["MahApps.Colors.White"] = fromColor;
        resources["MahApps.Colors.Flyout"] = fromColor;

        var newBrush = new SolidColorBrush (fromColor);

        //newBrush.Freeze();
        resources["MahApps.Brushes.Flyout.Background"] = newBrush;
        resources["MahApps.Brushes.Control.Background"] = newBrush;
        resources["MahApps.Brushes.White"] = newBrush;
        resources["MahApps.Brushes.WhiteColor"] = newBrush;
        resources["MahApps.Brushes.DisabledWhite"] = newBrush;
        resources["MahApps.Brushes.Window.Background"] = newBrush;
        resources["ThemeBackgroundBrush"] = newBrush;

        if (accent)
        {
            fromColor = (Color)resources["MahApps.Colors.IdealForeground"];
            newBrush = new SolidColorBrush (fromColor);

            //    newBrush.Freeze();
            resources["MahApps.Brushes.Flyout.Foreground"] = newBrush;
            resources["MahApps.Brushes.Text"] = newBrush;
            resources["MahApps.Brushes.Label.Text"] = newBrush;

            if (resources.ContainsKey ("MahApps.Colors.AccentBase"))
            {
                fromColor = (Color)resources["MahApps.Colors.AccentBase"];
            }
            else
            {
                var accentColor = (Color)resources["MahApps.Colors.Accent"];
                fromColor = Color.FromArgb (255, accentColor.R, accentColor.G, accentColor.B);
            }

            newBrush = new SolidColorBrush (fromColor);

            //    newBrush.Freeze();
            resources["MahApps.Colors.Highlight"] = fromColor;
            resources["MahApps.Brushes.Highlight"] = newBrush;
            resources["MahApps.Brushes.Highlight"] = newBrush;
        }

        styles.Add (item);

        //resources.EndInit();
    }

    private static Theme DetectTheme (Flyout flyout)
    {
        if (flyout == null)
            return null;

        // first look for owner
        var window = flyout.ParentWindow;
        var theme = window != null ? ThemeManager.Instance.DetectTheme (window) : null;
        if (theme != null)
        {
            return theme;
        }

        // second try, look for main window
        if (Application.Current != null)
        {
            var mainWindow = (Application.Current as
                IClassicDesktopStyleApplicationLifetime)?.MainWindow as MetroWindow;
            theme = mainWindow != null ? ThemeManager.Instance.DetectTheme (mainWindow) : null;
            if (theme != null)
            {
                return theme;
            }

            // oh no, now look at application resource
            theme = ThemeManager.Instance.DetectTheme (Application.Current);
            if (theme != null)
            {
                return theme;
            }
        }

        return null;
    }

    private void UpdateOpacityChange()
    {
        if (flyoutRoot == null || Design.IsDesignMode)
        {
            return;
        }

        if (!AnimateOpacity)
        {
            FadeOutFrameOpacity = 1;
            flyoutRoot.Opacity = 1;
        }
        else
        {
            FadeOutFrameOpacity = 0;
            if (!IsOpen) flyoutRoot.Opacity = 0;
        }
    }

    /// <summary>
    /// gets the controls from the style
    /// </summary>
    /// <param name="e"></param>
    protected override void OnApplyTemplate (TemplateAppliedEventArgs e)
    {
        flyoutRoot = e.NameScope.Find<Border> ("PART_Root");
        if (flyoutRoot == null)
        {
            return;
        }

        flyoutHeader = e.NameScope.Find<MetroThumbContentControl> ("PART_Header");
        flyoutHeader?.ApplyTemplate();
        flyoutContent = e.NameScope.Find<ContentPresenter> ("PART_Content");

        var thumbContentControl = flyoutHeader as IMetroThumb;
        if (thumbContentControl != null)
        {
            thumbContentControl.DragStarted -= WindowTitleThumbOnDragStarted;
            thumbContentControl.DragCompleted -= WindowTitleThumbOnDragCompleted;

            //thumbContentControl.PreviewMouseLeftButtonUp -= this.WindowTitleThumbOnPreviewMouseLeftButtonUp;
            thumbContentControl.DragDelta -= WindowTitleThumbMoveOnDragDelta;
            thumbContentControl.DoubleTapped -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
            thumbContentControl.PointerReleased -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;

            var flyoutsControl = this.TryFindParent<FlyoutsControl>();
            if (flyoutsControl != null)
            {
                thumbContentControl.DragStarted += WindowTitleThumbOnDragStarted;
                thumbContentControl.DragCompleted += WindowTitleThumbOnDragCompleted;

                //thumbContentControl.PreviewMouseLeftButtonUp += this.WindowTitleThumbOnPreviewMouseLeftButtonUp;
                thumbContentControl.DragDelta += WindowTitleThumbMoveOnDragDelta;
                thumbContentControl.DoubleTapped += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                thumbContentControl.PointerReleased += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }
        }

        //this.hideStoryboard = this.GetTemplateChild("HideStoryboard") as Storyboard;
        //this.hideFrame = e.NameScope.Find<KeyFrameExt>("hideFrame") as KeyFrameExt;
        //this.hideFrameY = this.GetTemplateChild("hideFrameY") as SplineDoubleKeyFrame;
        //this.showFrame = this.GetTemplateChild("showFrame") as SplineDoubleKeyFrame;
        //this.showFrameY = this.GetTemplateChild("showFrameY") as SplineDoubleKeyFrame;
        //this.fadeOutFrame = this.GetTemplateChild("fadeOutFrame") as SplineDoubleKeyFrame;

        //if (this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null || this.fadeOutFrame == null)
        //{
        //    return;
        //}

        base.OnApplyTemplate (e);
        UpdateOpacityChange();
        UpdateFlyoutTheme();
        ApplyAnimation (Position, AnimateOpacity);
    }

    private void WindowTitleThumbOnDragCompleted (object sender, VectorEventArgs e)
    {
        dragStartedMousePos = null;
    }

    private void WindowTitleThumbOnDragStarted (object sender, VectorEventArgs e)
    {
        var window = ParentWindow;
        if (window != null && Position != Position.Bottom)
        {
            //this.dragStartedMousePos = Mouse.GetPosition((IInputElement)sender);
        }
        else
        {
            dragStartedMousePos = null;
        }
    }

    /// <summary>
    /// cleanups the events
    /// </summary>
    /// <param name="flyoutsControl"></param>
    protected internal void CleanUp (FlyoutsControl flyoutsControl)
    {
        var thumbContentControl = flyoutHeader as IMetroThumb;
        if (thumbContentControl != null)
        {
            thumbContentControl.DragStarted -= WindowTitleThumbOnDragStarted;
            thumbContentControl.DragCompleted -= WindowTitleThumbOnDragCompleted;

            //thumbContentControl.PreviewMouseLeftButtonUp -= this.WindowTitleThumbOnPreviewMouseLeftButtonUp;
            thumbContentControl.DragDelta -= WindowTitleThumbMoveOnDragDelta;
            thumbContentControl.DoubleTapped -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
            thumbContentControl.PointerReleased -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
        }

        parentWindow = null;
    }

    private void WindowTitleThumbMoveOnDragDelta (object sender, VectorEventArgs dragDeltaEventArgs)
    {
        var window = ParentWindow;

        //if (window != null && this.Position != Position.Bottom && (this.Position == Position.Top || (this.dragStartedMousePos.GetValueOrDefault().Y <= window.TitleBarHeight && window.TitleBarHeight > 0)))
        //if (window != null && this.Position != Position.Bottom && this.dragStartedMousePos.GetValueOrDefault().Y <= window.TitleBarHeight && window.TitleBarHeight > 0)
        if (window != null && Position != Position.Bottom)
        {
            MetroWindow.DoWindowTitleThumbMoveOnDragDelta (sender as IMetroThumb, window, dragDeltaEventArgs);
        }
    }

    private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick (object sender,
        RoutedEventArgs mouseButtonEventArgs)
    {
        var window = ParentWindow;
        if (window != null &&
            Position != Position
                .Bottom /*&& Mouse.GetPosition((IInputElement)sender).Y <= window.TitleBarHeight && window.TitleBarHeight > 0*/
           )
        {
            MetroWindow.DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick (window, mouseButtonEventArgs);
        }
    }

    private void WindowTitleThumbSystemMenuOnMouseRightButtonUp (object sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton != MouseButton.Right)
            return;

        var window = ParentWindow;

        if (window != null &&
            Position != Position
                .Bottom /*&& Mouse.GetPosition((IInputElement)sender).Y <= window.TitleBarHeight && window.TitleBarHeight > 0*/
           )
        {
            //MetroWindow.DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(window, e);
        }
    }

    internal void ApplyAnimation (Position position, bool animateOpacity, bool resetShowFrame = true)
    {
        if (flyoutRoot ==
            null /*|| this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null || this.fadeOutFrame == null*/
           )
            return;

        if (Position == Position.Left || Position == Position.Right)
        {
            ShowFrameTranslateTransformX = 0;
        }

        if (Position == Position.Top || Position == Position.Bottom)
        {
            ShowFrameTranslateTransformY = 0;
        }

        // I mean, we don't need this anymore, because we use Width and Height of the flyoutRoot
        flyoutRoot.Measure (new Size (_currentSize.Width, _currentSize.Height));
        if (double.IsNaN (flyoutRoot.Height))
        {
            flyoutRoot.Height = _currentSize.Height;
        }

        if (double.IsNaN (flyoutRoot.Width))
        {
            flyoutRoot.Width = _currentSize.Width;
        }

        if (!animateOpacity)
        {
            FadeOutFrameOpacity = 1;
            flyoutRoot.Opacity = 1;
        }
        else
        {
            FadeOutFrameOpacity = 0;
            if (!IsOpen) flyoutRoot.Opacity = 0;
        }

        switch (position)
        {
            default:
                HorizontalAlignment = Margin.Right <= 0
                    ? (HorizontalContentAlignment != HorizontalAlignment.Stretch
                        ? HorizontalAlignment.Left
                        : HorizontalContentAlignment)
                    : HorizontalAlignment.Stretch; //HorizontalAlignment.Left;
                VerticalAlignment = VerticalAlignment.Stretch;
                HideFrameTranslateTransformX = -flyoutRoot.Width - Margin.Left;
                if (resetShowFrame)
                    flyoutRoot.RenderTransform = new TranslateTransform (-flyoutRoot.Width, 0);
                break;

            case Position.Right:
                HorizontalAlignment = Margin.Left <= 0
                    ? (HorizontalContentAlignment != HorizontalAlignment.Stretch
                        ? HorizontalAlignment.Right
                        : HorizontalContentAlignment)
                    : HorizontalAlignment.Stretch; //HorizontalAlignment.Right;
                VerticalAlignment = VerticalAlignment.Stretch;
                HideFrameTranslateTransformX = flyoutRoot.Width + Margin.Right;
                if (resetShowFrame)
                    flyoutRoot.RenderTransform = new TranslateTransform (flyoutRoot.Width, 0);
                break;

            case Position.Top:
                HorizontalAlignment = HorizontalAlignment.Stretch;
                VerticalAlignment = Margin.Bottom <= 0
                    ? (VerticalContentAlignment !=
                       VerticalAlignment.Stretch
                        ? VerticalAlignment.Top
                        : VerticalContentAlignment)
                    : VerticalAlignment.Stretch; //VerticalAlignment.Top;
                HideFrameTranslateTransformY = -flyoutRoot.Height - 1 - Margin.Top;
                if (resetShowFrame)
                    flyoutRoot.RenderTransform = new TranslateTransform (0, -flyoutRoot.Height - 1);
                break;

            case Position.Bottom:
                HorizontalAlignment = HorizontalAlignment.Stretch;
                VerticalAlignment = Margin.Top <= 0
                    ? (VerticalContentAlignment !=
                       VerticalAlignment.Stretch
                        ? VerticalAlignment.Bottom
                        : VerticalContentAlignment)
                    : VerticalAlignment.Stretch; //VerticalAlignment.Bottom;
                HideFrameTranslateTransformY = flyoutRoot.Height + Margin.Bottom;
                if (resetShowFrame)
                    flyoutRoot.RenderTransform = new TranslateTransform (0, flyoutRoot.Height);
                break;
        }
    }

    /// <summary>
    /// arrange the control
    /// </summary>
    /// <param name="finalRect"></param>
    protected override void ArrangeCore (Rect finalRect)
    {
        base.ArrangeCore (finalRect);
        if (!IsOpen) return; // no changes for invisible flyouts, ApplyAnimation is called now in visible changed event

        //if (!sizeInfo.WidthChanged && !sizeInfo.HeightChanged) return;
        if (flyoutRoot ==
            null /*|| this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null*/
           )
            return; // don't bother checking IsOpen and calling ApplyAnimation

        if (Position == Position.Left || Position == Position.Right)
            ShowFrameTranslateTransformX = 0;
        if (Position == Position.Top || Position == Position.Bottom)
            ShowFrameTranslateTransformY = 0;

        switch (Position)
        {
            default:
                HideFrameTranslateTransformX = -flyoutRoot.Width - Margin.Left;
                break;

            case Position.Right:
                HideFrameTranslateTransformX = flyoutRoot.Width + Margin.Right;
                break;

            case Position.Top:
                HideFrameTranslateTransformY = -flyoutRoot.Height - 1 - Margin.Top;
                break;

            case Position.Bottom:
                HideFrameTranslateTransformY = flyoutRoot.Height + Margin.Bottom;
                break;
        }
    }

    private void AutoCloseTimerCallback (object sender, EventArgs e)
    {
        StopAutoCloseTimer();

        //if the flyout is open and autoclose is still enabled then close the flyout
        if ((IsOpen) && (IsAutoCloseEnabled))
        {
            IsOpen = false;
        }
    }

    private void StopAutoCloseTimer()
    {
        if ((autoCloseTimer != null) && (autoCloseTimer.IsEnabled))
        {
            autoCloseTimer.Stop();
        }
    }

    private void AutoCloseIntervalChanged (Flyout flyout, AvaloniaPropertyChangedEventArgs e)
    {
        Action autoCloseIntervalChangedAction = () =>
        {
            if (e.NewValue != e.OldValue)
            {
                flyout.InitializeAutoCloseTimer();
                if (flyout.IsAutoCloseEnabled && flyout.IsOpen)
                {
                    flyout.StartAutoCloseTimer();
                }
            }
        };

        Dispatcher.UIThread.InvokeAsync (autoCloseIntervalChangedAction, DispatcherPriority.Background);

        //flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, autoCloseIntervalChangedAction);
    }

    private void StartAutoCloseTimer()
    {
        //in case it is already running
        StopAutoCloseTimer();
        if (!Design.IsDesignMode)
        {
            autoCloseTimer.Start();
        }
    }

    private void IsAutoCloseEnabledChanged
        (
            Flyout flyout,
            AvaloniaPropertyChangedEventArgs eventArgs
        )
    {
        Action autoCloseEnabledChangedAction = () =>
        {
            if (eventArgs.NewValue != eventArgs.OldValue)
            {
                if ((bool)eventArgs.NewValue)
                {
                    if (flyout.IsOpen)
                    {
                        flyout.StartAutoCloseTimer();
                    }
                }
                else
                {
                    flyout.StopAutoCloseTimer();
                }
            }
        };

        Dispatcher.UIThread.InvokeAsync (autoCloseEnabledChangedAction, DispatcherPriority.Background);

        //flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, autoCloseEnabledChangedAction);
    }

    private void ThemeChanged
        (
            Flyout flyout,
            AvaloniaPropertyChangedEventArgs eventArgs
        )
    {
        eventArgs.NotUsed();

        flyout.UpdateFlyoutTheme();
        flyout.RaiseEvent (new RoutedEventArgs (FlyoutThemeChangedEvent));
    }

    private void AnimateOpacityChanged (Flyout flyout, AvaloniaPropertyChangedEventArgs e)
    {
        flyout.UpdateOpacityChange();
    }

    private void IsOpenedChanged (Flyout flyout, AvaloniaPropertyChangedEventArgs e)
    {
        Action openedChangedAction = () =>
        {
            if (e.NewValue != e.OldValue)
            {
                if (flyout.AreAnimationsEnabled)
                {
                    if ((bool)e.NewValue)
                    {
                        //if (flyout.hideStoryboard != null)
                        //{
                        //    // don't let the storyboard end it's completed event
                        //    // otherwise it could be hidden on start
                        //    flyout.hideStoryboard.Completed -= flyout.HideStoryboardCompleted;
                        //}
                        flyout.IsVisible = true;
                        flyout.ApplyAnimation (flyout.Position, flyout.AnimateOpacity);
                        flyout.TryFocusElement();
                        if (flyout.IsAutoCloseEnabled)
                        {
                            flyout.StartAutoCloseTimer();
                        }
                    }
                    else
                    {
                        flyout.StopAutoCloseTimer();

                        //if (flyout.hideStoryboard != null)
                        //{
                        //    flyout.hideStoryboard.Completed += flyout.HideStoryboardCompleted;
                        //}
                        //else
                        //{
                        //    flyout.Hide();
                        //}
                    }

                    FlyoutVisualStates = (bool)e.NewValue == false ? FlyoutVisualState.Hide : FlyoutVisualState.Show;

                    //VisualStateManager.GoToState(flyout, (bool)e.NewValue == false ? "Hide" : "Show", true);
                }
                else
                {
                    if ((bool)e.NewValue)
                    {
                        flyout.IsVisible = true;
                        flyout.TryFocusElement();
                        if (flyout.IsAutoCloseEnabled)
                        {
                            flyout.StartAutoCloseTimer();
                        }
                    }
                    else
                    {
                        flyout.StopAutoCloseTimer();
                        flyout.Hide();
                    }

                    FlyoutVisualStates = (bool)e.NewValue == false
                        ? FlyoutVisualState.HideDirect
                        : FlyoutVisualState.ShowDirect;

                    //VisualStateManager.GoToState(flyout, (bool)e.NewValue == false ? "HideDirect" : "ShowDirect", true);
                }
            }

            flyout.RaiseEvent (new RoutedEventArgs (IsOpenChangedEvent));
        };

        ////flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, openedChangedAction);
        Dispatcher.UIThread.InvokeAsync (openedChangedAction, DispatcherPriority.Render);
    }

    private void Hide()
    {
        // hide the flyout, we should get better performance and prevent showing the flyout on any resizing events
        IsVisible = false;
        RaiseEvent (new RoutedEventArgs (ClosingFinishedEvent));
    }

    private void TryFocusElement()
    {
        if (AllowFocusElement)
        {
            // first focus itself
            Focus();

            if (FocusedElement != null)
            {
                FocusedElement.Focus();
            }
            else if
                (flyoutContent == null ||
                 !flyoutContent
                     .IsFocused) //this.flyoutContent.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)))
            {
                //this.flyoutHeader?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                flyoutHeader?.Focus();
            }
        }
    }

    private void PositionChanged
        (
            Flyout flyout,
            AvaloniaPropertyChangedEventArgs eventArgs
        )
    {
        var wasOpen = flyout.IsOpen;
        if (wasOpen && flyout.AnimateOnPositionChange)
        {
            flyout.ApplyAnimation ((Position)eventArgs.NewValue, flyout.AnimateOpacity);

            //VisualStateManager.GoToState(flyout, "Hide", true);
            FlyoutVisualStates = FlyoutVisualState.Hide;
        }
        else
        {
            flyout.ApplyAnimation ((Position)eventArgs.NewValue, flyout.AnimateOpacity, false);
        }

        if (wasOpen && flyout.AnimateOnPositionChange)
        {
            flyout.ApplyAnimation ((Position)eventArgs.NewValue, flyout.AnimateOpacity);

            //VisualStateManager.GoToState(flyout, "Show", true);
            FlyoutVisualStates = FlyoutVisualState.Show;
        }
    }
}
