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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;

using ReactiveUI;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/jogibear9988/OdysseyWPF.git

/// <summary>
/// outlookbar control
/// </summary>
public partial class OutlookBar : HeaderedItemsControl
{
    /// <summary>
    /// calls GetLogicalChildren to get logical children
    /// </summary>
    protected new IAvaloniaList<ILogical> LogicalChildren
    {
        get
        {
            if (_logicalChildren.Count > 0)
            {
                _logicalChildren.Clear();
            }

            _logicalChildren.AddRange (GetLogicalChildren().OfType<ILogical>().ToList());

            return _logicalChildren;
        }
    }

    private IEnumerable<ILogical> GetLogicalChildren()
    {
        foreach (var section in Items)
        {
            yield return section as ILogical;
        }

        if (SelectedSection != null)
        {
            yield return SelectedSection.Content as ILogical;
        }
    }

    private void ApplyOverflowMenu()
    {
        var overflowItems = new ObservableCollection<object>();
        if (OverflowMenuItems.Count > 0)
        {
            foreach (var item in OverflowMenuItems)
            {
                overflowItems.Add (item);
            }
        }

        var separatorAdded = false;
        var visibleButtons = _maximizedSections.Count + (IsMaximized ? _minimizedSections.Count : 0);

        var items = Items.OfType<OutlookSection>().ToList();
        if (items.Count > 0)
        {
            for (var i = visibleButtons; i < items.Count; i++)
            {
                var section = items[i];

                if (overflowItems.OfType<MenuItem>().Any (x => x.Tag == section))
                {
                    continue;
                }

                if (!separatorAdded)
                {
                    overflowItems.Add (new Separator());
                    separatorAdded = true;
                }

                var item = new MenuItem();
                item.Width = section.Width;
                item.Header = section.Header;
                item.Foreground = new SolidColorBrush (Colors.Red);
                var image = new Image();
                image.Source = section.Image;
                item.Icon = image;
                item.Tag = section;
                item.Click += Item_Click;
                overflowItems.Add (item);
            }
        }

        //SetValue(OutlookBar.OverflowMenuItemsProperty, overflowItems);

        OverflowMenuItems = overflowItems;
    }

    private int GetNumberOfMinimizedButtons()
    {
        if (_minimizedButtonContainer != null)
        {
            const double width = 32;
            const double overflowWidth = 18;

            var result = _minimizedButtonContainer.DesiredSize;

            var fraction = ((result.Width + width) - overflowWidth) / width;

            var minimizedButtons = (int)Math.Truncate (fraction);
            var visibleButtons = MaxNumberOfButtons + minimizedButtons;
            return visibleButtons;
        }

        return 0;
    }

    /// <summary>
    /// OverflowMenuCreated event
    /// </summary>
    public event EventHandler<OverflowMenuCreatedEventArgs>? OverflowMenuCreated;

    /// <summary>
    /// fires OverflowMenuCreated if not null
    /// </summary>
    /// <param name="menuItems"></param>
    protected virtual void OnOverflowMenuCreated (Collection<object> menuItems)
    {
        if (OverflowMenuCreated != null)
        {
            var e = new OverflowMenuCreatedEventArgs (menuItems);
            OverflowMenuCreated (this, e);
        }
    }

    private void Item_Click
        (
            object? sender,
            RoutedEventArgs e
        )
    {
        var item = e.Source as MenuItem;
        var section = item.Tag as OutlookSection;
        SelectedSection = section;
    }

    private void SectionsCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
    {
        ApplySections();
    }

    /// <inheritdoc cref="TemplatedControl.OnApplyTemplate"/>
    protected override void OnApplyTemplate
        (
            TemplateAppliedEventArgs eventArgs
        )
    {
        if (popup != null)
        {
            popup.Closed -= OnPopupClosed;
            popup.Opened -= OnPopupOpened;
        }

        _minimizedButtonContainer = eventArgs.NameScope.Find<Control> (partMinimizedButtonContainer);

        popup = eventArgs.NameScope.Find<Popup> (partPopup);
        if (popup != null)
        {
            popup.Closed += OnPopupClosed;
            popup.Opened += new EventHandler (OnPopupOpened);
        }

        var btn = eventArgs.NameScope.Find<ToggleButton> ("PART_ToggleButton");
        if (btn != null)
        {
            btn.PointerReleased += Btn_PointerReleased;
        }

        btnMenu = eventArgs.NameScope.Find<ToggleButton> ("toggleMenu");

        btnMenu.PointerLeave += (o, e) => { e.Pointer.Capture (null); };

        btnMenu.Click += (o, e) =>
        {
            ApplyOverflowMenu();

            if (OverflowMenuItems.Count == 0)
            {
                return;
            }

            //IsOverflowVisible = !IsOverflowVisible;

            if (btnMenu.ContextMenu.IsOpen == false)
            {
                btnMenu.ContextMenu.Items = OverflowMenuItems;
                try
                {
                    //alway set to null otherwise an exeption is raised on second call
                    ((ISetLogicalParent)btnMenu.ContextMenu).SetParent (null);
                    btnMenu.ContextMenu.Open (btnMenu);
                }
                catch
                {
                    btnMenu.ContextMenu.Close();
                    InvalidateMeasure();
                    InvalidateArrange();
                    InvalidateVisual();
                }
            }
            else
            {
                btnMenu.ContextMenu.Close();
            }
        };

        //Border headerBorder = e.NameScope.Find<Border>("headerBorder");
        //headerBorder.DoubleTapped += (o, e) =>
        //{
        //    IsMaximized = !IsMaximized;
        //};

        eventArgs.NameScope.Find<Button> ("resizeButton").Click += (o, e) => { ResizeCommand.Execute (o); };

        eventArgs.NameScope.Find<Button> ("closeButton").Click += (o, e) => { CloseCommand.Execute (o); };

        eventArgs.NameScope.Find<Button> ("toggleMinimizeButton").Click += (o, e) => { CollapseCommand.Execute (o); };

        eventArgs.NameScope.Find<Button> ("splitter").Click += (o, e) => { StartDraggingCommand.Execute (o); };

        base.OnApplyTemplate (eventArgs);

        RaisePropertyChanged (OdcExpanderClassesProperty, null, (Classes)OdcExpanderClasses);
        ApplySections();

        //RaisePropertyChanged(IsOverflowVisibleProperty, !IsOverflowVisible, IsOverflowVisible);
    }

    private void Btn_PointerReleased
        (
            object? sender,
            PointerReleasedEventArgs eventArgs
        )
    {
        if (eventArgs.InitialPressMouseButton != MouseButton.Left)
        {
            return;
        }

        //          e.Pointer.Capture(null);

        popup.IsLightDismissEnabled = false;
    }

    /// <summary>
    /// set IsPopupVisible to true
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    protected virtual void OnPopupOpened
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        IsPopupVisible = true;

        //Mouse.Capture(this, CaptureMode.SubTree);
    }

    /// <summary>
    /// set IsPopupVisible to false
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    protected virtual void OnPopupClosed
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        IsPopupVisible = false;

        //Mouse.Capture(null);
    }

    /// <summary>
    /// Occurs when the IsMaximized property has changed.
    /// </summary>
    /// <param name="isExpanded"></param>
    protected virtual void OnMaximizedChanged (bool isExpanded)
    {
        if (isExpanded)
        {
            IsPopupVisible = false;
        }

        EnsureSectionContentIsVisible();
        if (isExpanded)
        {
            MaxWidth = MaximizedWidth;
            RaiseEvent (new RoutedEventArgs (ExpandedEvent));
        }
        else
        {
            MaxWidth = MinimizedWidth + (CanResize ? 4 : 0);
            RaiseEvent (new RoutedEventArgs (CollapsedEvent));
        }

        RaisePropertyChanged (IsOverflowVisibleProperty, !IsOverflowVisible, IsOverflowVisible);
    }

    /// <summary>
    /// This code ensures that the section content is visible when the IsExpanded has changed,
    /// since the parent of the content could have changed either.
    /// </summary>
    private void EnsureSectionContentIsVisible()
    {
        var content = SelectedSection != null ? SelectedSection.Content : null;
        SectionContent = null; // set temporarily to null, so resetting to the current content will have an effect.
        CollapsedSectionContent = IsMaximized ? null : content;
        SectionContent = IsMaximized ? content : null;
    }


    /// <summary>
    /// init some properties and registered some changed events
    /// </summary>
    public OutlookBar()
    {
        OverflowMenuItems = _overflowMenu = new ObservableCollection<object>();
        OptionButtons = _optionButtons = new ObservableCollection<Button>();
        _optionButtons.CollectionChanged += OnOptionButtons_CollectionChanged;

        //command bindings missing

        _maximizedSections = new ObservableCollection<OutlookSection>();
        _minimizedSections = new ObservableCollection<OutlookSection>();
        Items = new ObservableCollection<OutlookSection>();
        (Items as ObservableCollection<OutlookSection>).CollectionChanged +=
            new NotifyCollectionChangedEventHandler (SectionsCollectionChanged);

        WidthProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => SizeChanged (o, e));
        HeightProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => SizeChanged (o, e));

        //Collapsed += (o, e) =>
        //{
        //    IsMaximized = false;
        //};
        //Expanded += (o, e) =>
        //{
        //    IsMaximized = true;
        //};

        IsMaximizedProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => MaximizedPropertyChanged (o, e));
        MaxNumberOfButtonsProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => MaxNumberOfButtonsChanged (o, e));
        IsPopupVisibleProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => IsPopupVisibleChanged (o, e));
        SelectedSectionIndexProperty.Changed.AddClassHandler<OutlookBar> ((o, e) =>
            SelectedIndexPropertyChanged (o, e));
        SelectedSectionProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => SelectedSectionPropertyChanged (o, e));
        IsOverflowVisibleProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => OverflowVisiblePropertyChanged (o, e));

        CollapseCommand = ReactiveCommand.Create<object> (x => CollapseCommandExecuted (x),
            outputScheduler: RxApp.MainThreadScheduler);
        ShowPopupCommand = ReactiveCommand.Create<object> (x => ShowPopupCommandExecuted (x),
            outputScheduler: RxApp.MainThreadScheduler);
        ResizeCommand =
            ReactiveCommand.Create<object> (x => ResizeCommandExecuted (x), outputScheduler: RxApp.MainThreadScheduler);
        CloseCommand =
            ReactiveCommand.Create<object> (x => CloseCommandExecuted (x), outputScheduler: RxApp.MainThreadScheduler);
        StartDraggingCommand = ReactiveCommand.Create<object> (x => StartDraggingCommandExecuted (x),
            outputScheduler: RxApp.MainThreadScheduler);

        OdcExpanderClassesProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => OnOdcExpanderClassesChanged (o, e));
        OptionButtonClassesProperty.Changed.AddClassHandler<OutlookBar> ((o, e) => OnOptionButtonClassesChanged (o, e));
        OptionToggleButtonClassesProperty.Changed.AddClassHandler<OutlookBar> ((o, e) =>
            OnOptionToggleButtonClassesChanged (o, e));
    }

    private void OnOptionToggleButtonClassesChanged (OutlookBar o, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is Classes)
        {
            foreach (var item in OptionButtons)
            {
                if ((item is ToggleButton) == false)
                {
                    continue;
                }

                foreach (var itemClass in (e.NewValue as Classes))
                {
                    if (item.Classes.Contains (itemClass) == false)
                    {
                        item.Classes.Add (itemClass);
                    }
                }
            }
        }
    }

    private void OnOptionButtonClassesChanged (OutlookBar o, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is Classes)
        {
            foreach (var item in OptionButtons)
            {
                if (item is ToggleButton)
                {
                    continue;
                }

                foreach (var itemClass in (e.NewValue as Classes))
                {
                    if (item.Classes.Contains (itemClass) == false)
                    {
                        item.Classes.Add (itemClass);
                    }
                }
            }
        }
    }

    private void OnOptionButtons_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
    {
        if (OptionButtonClasses != null)
        {
            foreach (var button in e.NewItems.OfType<Button>())
            {
                foreach (var itemClass in OptionButtonClasses)
                {
                    if (button.Classes.Contains (itemClass) == false)
                    {
                        button.Classes.Add (itemClass);
                    }
                }
            }
        }
    }

    static OutlookBar()
    {
        ItemsPanelProperty.OverrideDefaultValue<OutlookBar> (DefaultPanel);
    }

    private void OnOdcExpanderClassesChanged (OutlookBar o, AvaloniaPropertyChangedEventArgs e)
    {
        if (OdcExpanderClasses != null)
        {
            foreach (var item in Items.OfType<IControl>()
                         .SelectMany (x => x.GetLogicalChildren().OfType<OdcExpander>()))
            {
                foreach (var cls in OdcExpanderClasses.ToList())
                {
                    item.Classes.Add (cls);
                }
            }
        }
    }

    private void OverflowVisiblePropertyChanged (OutlookBar bar, AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = (bool)e.NewValue;
        bar.OnOverflowVisibleChanged (newValue);
    }

    /// <summary>
    /// Occurs when the IsOverflowVisible has changed.
    /// </summary>
    /// <param name="newValue"></param>
    protected virtual void OnOverflowVisibleChanged (bool newValue)
    {
        if (newValue == true)
        {
            ApplyOverflowMenu();
        }
    }

    private void SelectedSectionPropertyChanged (OutlookBar bar, AvaloniaPropertyChangedEventArgs e)
    {
        bar.OnSelectedSectionChanged (e.OldValue as OutlookSection, (OutlookSection)e.NewValue);
    }

    private void SelectedIndexPropertyChanged (OutlookBar bar, AvaloniaPropertyChangedEventArgs e)
    {
        bar.ApplySections();
    }

    private void IsPopupVisibleChanged (OutlookBar o, AvaloniaPropertyChangedEventArgs e)
    {
        o.OnPopupVisibleChanged ((bool)e.NewValue);
    }

    private void MaxNumberOfButtonsChanged (OutlookBar bar, AvaloniaPropertyChangedEventArgs e)
    {
        bar.ApplySections();
    }

    private void MaximizedPropertyChanged (OutlookBar bar, AvaloniaPropertyChangedEventArgs e)
    {
        bar.OnMaximizedChanged ((bool)e.NewValue);
    }

    private void SizeChanged (OutlookBar o, AvaloniaPropertyChangedEventArgs e)
    {
        ApplySections();
    }

    private void CollapseCommandExecuted (object x)
    {
        var temp = IsMaximized;
        IsMaximized ^= true;
    }

    private void ShowPopupCommandExecuted (object x)
    {
        if (!IsMaximized)
        {
            IsPopupVisible = true;
        }
    }

    private void ResizeCommandExecuted (object x)
    {
        var c = x as Control;
        if (c != null)
        {
            c.PointerReleased += DragMouseLeftButtonUp;
        }

        PointerMoved += PreviewMouseMoveResize;
    }

    private void CloseCommandExecuted (object x)
    {
        IsVisible = false;
    }

    private void PreviewMouseMoveResize (object sender, PointerEventArgs e)
    {
        var properties = e.GetCurrentPoint (this).Properties;

        if (properties.IsLeftButtonPressed == true)
        {
            if (DockPosition == HorizontalAlignment.Left)
            {
                ResizeFromRight (e);
            }
            else
            {
                ResizeFromLeft (e);
            }
        }
    }

    private void ResizeFromLeft (PointerEventArgs e)
    {
        var pos = e.GetPosition (this);
        var w = Width - pos.X;

        if (w < 80)
        {
            w = double.NaN;
            IsMaximized = false;
        }
        else
        {
            IsMaximized = true;
        }

        if (!double.IsNaN (MaxWidth) && w > MaxWidth)
        {
            w = MaxWidth;
        }

        Width = w;
    }

    private void ResizeFromRight (PointerEventArgs e)
    {
        var pos = e.GetPosition (this);
        var w = pos.X;

        if (w < 80)
        {
            w = double.NaN;
            IsMaximized = false;
        }
        else
        {
            IsMaximized = true;
        }

        if (double.IsNaN (MaxWidth) == false && w > MaxWidth)
        {
            w = MaxWidth;
        }

        Width = w;
    }

    private void StartDraggingCommandExecuted (object x)
    {
        var c = x as Control;
        if (c != null)
        {
            c.PointerReleased += DragMouseLeftButtonUp;
        }

        PointerMoved += PreviewMouseMoveButtons;
    }

    /// <summary>
    /// Remove all PreviewMouseMove events from the outlookbar that have been possible set at the beginning of a drag command.
    /// </summary>
    private void DragMouseLeftButtonUp (object sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton != MouseButton.Left)
        {
            return;
        }

        var c = e.Source as Control;
        if (c != null)
        {
            c.PointerReleased -= DragMouseLeftButtonUp;
        }

        PointerMoved -= PreviewMouseMoveButtons;
        PointerMoved -= PreviewMouseMoveResize;
    }

    private void PreviewMouseMoveButtons (object sender, PointerEventArgs e)
    {
        var properties = e.GetCurrentPoint (this).Properties;

        if (properties.IsLeftButtonPressed)
        {
            var pos = e.GetPosition (this);

            var h = DesiredSize.Height - 1 - ButtonHeight - pos.Y;
            MaxNumberOfButtons = (int)((double)(h / ButtonHeight));
        }
        else
        {
            PointerMoved -= PreviewMouseMoveButtons;
        }
    }

    //protected override Size ArrangeOverride(Size finalSize)
    //{
    //    _finalSize = finalSize;
    //    _minimizedButtonContainer.Arrange(new Rect(finalSize));
    //    return base.ArrangeOverride(finalSize);
    //}

    /// <inheritdoc cref="Layoutable.MeasureOverride"/>
    protected override Size MeasureOverride
        (
            Size availableSize
        )
    {
        _finalSize = availableSize;
        return base.MeasureOverride (availableSize);
    }

    /// <summary>
    /// Occurs when the SelectedSection has changed.
    /// </summary>
    protected virtual void OnSelectedSectionChanged
        (
            OutlookSection oldSection,
            OutlookSection newSection
        )
    {
        var items = Items.OfType<OutlookSection>().ToList();
        for (var index = 0; index < items.Count; index++)
        {
            var section = items[index];
            var selected = newSection == section;
            section.IsSelected = newSection == section;
            if (selected)
            {
                SelectedSectionIndex = index;
                SectionContent = IsMaximized ? section.Content : null;
                CollapsedSectionContent = IsMaximized ? null : section.Content;
            }
        }

        RaiseEvent (
            new RoutedPropertyChangedEventArgs<OutlookSection> (oldSection, newSection, SelectedSectionChangedEvent));

        //RaiseEvent(new RoutedEventArgs(SelectedSectionChangedEvent));
    }

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (double.IsNaN (Width) == false)
        {
            MaximizedWidth = Width;
        }

        ApplySections();
    }

    /// <summary>
    /// applies the sections
    /// </summary>
    protected virtual void ApplySections()
    {
        if (IsInitialized)
        {
            foreach (var item in _maximizedSections)
            {
                ((ISetLogicalParent)item).SetParent (null);

                //item.IsVisible = false;
            }

            foreach (var item in _minimizedSections)
            {
                ((ISetLogicalParent)item).SetParent (null);

                //item.IsVisible = false;
            }

            if (_maximizedSections == null)
            {
                _maximizedSections = new ObservableCollection<OutlookSection>();
            }
            else
            {
                _maximizedSections.Clear();
            }


            if (_minimizedSections == null)
            {
                _minimizedSections = new ObservableCollection<OutlookSection>();
            }
            else
            {
                _minimizedSections.Clear();
            }

            var max = MaxNumberOfButtons;
            var index = 0;
            var selectedIndex = SelectedSectionIndex;
            OutlookSection selectedContent = null;

            var numberOfMinimizedButtons = GetNumberOfMinimizedButtons();

            foreach (var e in Items.OfType<OutlookSection>().ToList())
            {
                ((ISetInheritanceParent)e).SetParent (null);
                e.OutlookBar = this;
                e.Height = ButtonHeight;
                if (max-- > 0)
                {
                    e.IsMaximized = true;

                    _maximizedSections.Add (e);
                }
                else
                {
                    e.IsMaximized = false;
                    if (_minimizedSections.Count < numberOfMinimizedButtons)
                    {
                        _minimizedSections.Add (e);
                    }
                }

                var selected = index++ == selectedIndex;
                e.IsSelected = selected;
                if (selected)
                {
                    selectedContent = e;
                }
            }

            try
            {
                SetValue (MaximizedSectionsProperty, _maximizedSections);

                //MaximizedSections = maximizedSections;
            }
            catch
            {
                //already has parent exception
                InvalidateVisual();
                InvalidateArrange();
                InvalidateMeasure();
            }

            try
            {
                SetValue (MinimizedSectionsProperty, _minimizedSections);
            }
            catch
            {
                //already has parent exception

                InvalidateVisual();
                InvalidateMeasure();
                InvalidateArrange();
            }

            SetValue (SelectedSectionProperty, selectedContent);

            InvalidateVisual();
            InvalidateMeasure();
            InvalidateArrange();
        }
    }

    /// <summary>
    /// Occurs when the IsPopupVisible has changed.
    /// </summary>
    /// <param name="isPopupVisible"></param>
    protected virtual void OnPopupVisibleChanged (bool isPopupVisible)
    {
        if (popup != null)
        {
            popup.IsLightDismissEnabled = true;
            popup.IsOpen = isPopupVisible;
        }

        if (isPopupVisible)
        {
            RaiseEvent (new RoutedEventArgs (PopupOpenedEvent));
        }
        else
        {
            RaiseEvent (new RoutedEventArgs (PopupClosedEvent));
        }
    }

    /// <inheritdoc cref="ItemsControl.CreateItemContainerGenerator"/>
    protected override IItemContainerGenerator CreateItemContainerGenerator()
    {
        var result = new ItemContainerGenerator<OutlookSection>
            (
                this,
                ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty
            );

        return result;
    }
}
