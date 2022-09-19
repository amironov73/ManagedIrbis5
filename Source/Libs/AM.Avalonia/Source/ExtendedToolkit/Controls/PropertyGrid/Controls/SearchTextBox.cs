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

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Controls;

//
// ported from https://github.com/DenisVuyka/WPG
//

// http://davidowens.wordpress.com/2009/02/18/wpf-search-text-box/
/// <summary>
/// Property search box control.
/// </summary>
public class SearchTextBox : TextBox
{
    private const string DefaultLabelText = "Search";

    private readonly DispatcherTimer _searchEventDelayTimer;

    /// <summary>
    /// style key of this control
    /// </summary>
    public Type StyleKey => typeof (SearchTextBox);

    /// <summary>
    /// Gets or sets the search mode.
    /// </summary>
    public SearchMode SearchMode
    {
        get => GetValue (SearchModeProperty);
        set => SetValue (SearchModeProperty, value);
    }

    /// <summary>
    /// <see cref="SearchMode"/>
    /// </summary>
    public static readonly StyledProperty<SearchMode> SearchModeProperty =
        AvaloniaProperty.Register<SearchTextBox, SearchMode> (nameof (SearchMode)
            , defaultValue: SearchMode.Instant);

    /// <summary>
    /// Gets a value indicating whether this instance has text.
    /// </summary>
    /// <value><c>true</c> if this instance has text;
    ///     otherwise, <c>false</c>.</value>
    public bool HasText
    {
        get => GetValue (HasTextProperty);
        set => SetValue (HasTextProperty, value);
    }

    /// <summary>
    /// <see cref="HasText"/>
    /// </summary>
    public static readonly StyledProperty<bool> HasTextProperty =
        AvaloniaProperty.Register<SearchTextBox, bool> (nameof (HasText));

    /// <summary>
    /// Gets or sets a value indicating whether mouse left button is down.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if mouse left button is down; otherwise, <c>false</c>.
    /// </value>
    public bool IsMouseLeftButtonDown
    {
        get => GetValue (IsMouseLeftButtonDownProperty);
        set => SetValue (IsMouseLeftButtonDownProperty, value);
    }

    /// <summary>
    /// <see cref="IsMouseLeftButtonDown"/>
    /// </summary>
    public static readonly StyledProperty<bool> IsMouseLeftButtonDownProperty =
        AvaloniaProperty.Register<SearchTextBox, bool> (nameof (IsMouseLeftButtonDown));

    /// <summary>
    /// Gets or sets the search event time delay.
    /// </summary>
    /// <value>The search event time delay.</value>
    public TimeSpan SearchEventTimeDelay
    {
        get => GetValue (SearchEventTimeDelayProperty);
        set => SetValue (SearchEventTimeDelayProperty, value);
    }

    /// <summary>
    /// <see cref="SearchEventTimeDelay"/>
    /// </summary>
    public static readonly StyledProperty<TimeSpan> SearchEventTimeDelayProperty =
        AvaloniaProperty.Register<SearchTextBox, TimeSpan> (nameof (SearchEventTimeDelay)
            , defaultValue: TimeSpan.FromMilliseconds (500));


    /// <summary>
    /// <see cref="Search"/>
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> SearchEvent =
        RoutedEvent.Register<SearchTextBox, RoutedEventArgs> (nameof (SearchEvent),
            RoutingStrategies.Bubble);

    /// <summary>
    /// Occurs when search is performed.
    /// </summary>
    public event EventHandler Search
    {
        add => AddHandler (SearchEvent, value);
        remove => RemoveHandler (SearchEvent, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTextBox"/> class.
    /// </summary>
    public SearchTextBox()
    {
        _searchEventDelayTimer = new DispatcherTimer { Interval = SearchEventTimeDelay };
        _searchEventDelayTimer.Tick += OnSeachEventDelayTimerTick;

        SearchEventTimeDelayProperty.Changed.AddClassHandler<SearchTextBox> (OnSearchEventTimeDelayChanged);
        TextProperty.Changed.AddClassHandler<SearchTextBox> (OnTextChanged);
    }

    /// <summary>
    /// Is called when content in this editing control changes.
    /// </summary>
    private void OnTextChanged (SearchTextBox searchTextBox, AvaloniaPropertyChangedEventArgs e)
    {
        HasText = Text.Length != 0;

        if (SearchMode == SearchMode.Instant)
        {
            _searchEventDelayTimer.Stop();
            _searchEventDelayTimer.Start();
        }
    }

    private void OnSeachEventDelayTimerTick
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        _searchEventDelayTimer.Stop();
        RaiseSearchEvent();
    }

    private void OnSearchEventTimeDelayChanged
        (
            SearchTextBox? searchTextBox,
            AvaloniaPropertyChangedEventArgs e
        )
    {
        if (searchTextBox is not null)
        {
            searchTextBox._searchEventDelayTimer.Interval = (TimeSpan)e.NewValue;
            searchTextBox._searchEventDelayTimer.Stop();
        }
    }

    /// <inheritdoc cref="TextBox.OnApplyTemplate"/>
    protected override void OnApplyTemplate
        (
            TemplateAppliedEventArgs eventArgs
        )
    {
        var iconBorder = eventArgs.NameScope.Find<Border> ("PART_SearchIconBorder");
        if (iconBorder != null)
        {
            iconBorder.PointerPressed += IconBorderMouseLeftButtonDown;
            iconBorder.PointerReleased += IconBorderMouseLeftButtonUp;
            iconBorder.PointerLeave += IconBorderMouseLeave;
        }

        base.OnApplyTemplate (eventArgs);
    }

    private void IconBorderMouseLeave
        (
            object? sender,
            PointerEventArgs eventArgs
        )
    {
        IsMouseLeftButtonDown = false;
    }

    private void IconBorderMouseLeftButtonUp
        (
            object? sender,
            PointerReleasedEventArgs eventArgs
        )
    {
        var prop = eventArgs.GetCurrentPoint (sender as IVisual).Properties;
        if (IsMouseLeftButtonDown == false)
        {
            return;
        }

        if (!IsMouseLeftButtonDown)
        {
            return;
        }

        if (HasText && SearchMode == SearchMode.Instant)
        {
            Text = string.Empty;
        }

        if (HasText && SearchMode == SearchMode.Delayed)
        {
            RaiseSearchEvent();
        }

        IsMouseLeftButtonDown = false;
    }

    private void IconBorderMouseLeftButtonDown
        (
            object? sender,
            PointerPressedEventArgs eventArgs
        )
    {
        var prop = eventArgs.GetCurrentPoint (sender as IVisual).Properties;
        IsMouseLeftButtonDown = prop.IsLeftButtonPressed;
    }

    /// <inheritdoc cref="TextBox.OnKeyDown"/>
    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        if (eventArgs.Key == Key.Escape && SearchMode == SearchMode.Instant)
        {
            Text = string.Empty;
        }
        else if ((eventArgs.Key == Key.Return || eventArgs.Key == Key.Enter) &&
                 SearchMode == SearchMode.Delayed)
        {
            RaiseSearchEvent();
        }
        else
        {
            base.OnKeyDown (eventArgs);
        }
    }

    private void RaiseSearchEvent()
    {
        var args = new RoutedEventArgs (SearchEvent);
        RaiseEvent (args);
    }

    /// <inheritdoc cref="TextBox.OnPropertyChanged{T}"/>
    protected override void OnPropertyChanged<T>
        (
            AvaloniaPropertyChangedEventArgs<T> eventArgs
        )
    {
        if (eventArgs.Property == IsVisibleProperty)
        {
            Text = string.Empty;
        }

        base.OnPropertyChanged (eventArgs);
    }
}
