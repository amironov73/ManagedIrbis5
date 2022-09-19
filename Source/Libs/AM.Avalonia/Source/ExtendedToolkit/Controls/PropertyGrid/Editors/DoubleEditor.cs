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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.ExtendedToolkit.Controls.PropertyGrid.PropertyEditing;
using Avalonia.Input;
using Avalonia.Interactivity;

using ReactiveUI;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Editors;

//
// ported from https://github.com/DenisVuyka/WPG
//

/// <summary>
/// Simple Expression Blend like double editor.
/// </summary>
[DebuggerDisplay ("[DoubleEditor] PropertyDescriptor: {PropertyDescriptor}")]
public class DoubleEditor : TemplatedControl
{
    private Point _dragStartPoint;
    private Point _lastDragPoint;

    private double _changeValue;
    private double _changeOffset;
    private bool _isMouseDown;
    private KeyModifiers _currentKeyModifiers;
    private const double DragTolerance = 2.0;

    /// <summary>
    /// style key for this control
    /// </summary>
    public Type StyleKey => typeof (DoubleEditor);

    /// <summary>
    /// increase command
    /// </summary>
    public ICommand Increase { get; }

    /// <summary>
    /// Decrease command
    /// </summary>
    public ICommand Decrease { get; }

    //[TypeConverter(typeof(LengthConverter))]
    /// <summary>
    /// get/sets Value
    /// </summary>
    public double Value
    {
        get => GetValue (ValueProperty);
        set => SetValue (ValueProperty, value);
    }

    /// <summary>
    /// <see cref="Value"/>
    /// </summary>
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<DoubleEditor, double> (nameof (Value), defaultValue: 0d);

    /// <summary>
    /// get/sets SmallChange
    /// </summary>
    public double SmallChange
    {
        get => GetValue (SmallChangeProperty);
        set => SetValue (SmallChangeProperty, value);
    }

    /// <summary>
    /// <see cref="SmallChange"/>
    /// </summary>
    public static readonly StyledProperty<double> SmallChangeProperty =
        AvaloniaProperty.Register<DoubleEditor, double> (nameof (SmallChange), defaultValue: 1.0d);

    /// <summary>
    /// get/sets LargeChange
    /// </summary>
    public double LargeChange
    {
        get => GetValue (LargeChangeProperty);
        set => SetValue (LargeChangeProperty, value);
    }

    /// <summary>
    /// <see cref="LargeChange"/>
    /// </summary>
    public static readonly StyledProperty<double> LargeChangeProperty =
        AvaloniaProperty.Register<DoubleEditor, double> (nameof (LargeChange), defaultValue: 1.0d);

    /// <summary>
    /// get/sets DefaultChange
    /// </summary>
    public double DefaultChange
    {
        get => GetValue (DefaultChangeProperty);
        set => SetValue (DefaultChangeProperty, value);
    }

    /// <summary>
    /// <see cref="DefaultChange"/>
    /// </summary>
    public static readonly StyledProperty<double> DefaultChangeProperty =
        AvaloniaProperty.Register<DoubleEditor, double> (nameof (DefaultChange), defaultValue: 1.0d);

    /// <summary>
    /// get/sets Minimum
    /// </summary>
    public double Minimum
    {
        get => GetValue (MinimumProperty);
        set => SetValue (MinimumProperty, value);
    }

    /// <summary>
    /// <see cref="Minimum"/>
    /// </summary>
    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<DoubleEditor, double> (nameof (Minimum), defaultValue: 0.0d);

    /// <summary>
    /// get/sets Maximum
    /// </summary>
    public double Maximum
    {
        get => GetValue (MaximumProperty);
        set => SetValue (MaximumProperty, value);
    }

    /// <summary>
    /// <see cref="Maximum"/>
    /// </summary>
    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<DoubleEditor, double> (nameof (Maximum), defaultValue: double.MaxValue);

    /// <summary>
    /// get/sets MaxPrecision
    /// </summary>
    public int MaxPrecision
    {
        get => GetValue (MaxPrecisionProperty);
        set => SetValue (MaxPrecisionProperty, value);
    }

    /// <summary>
    /// <see cref="MaxPrecision"/>
    /// </summary>
    public static readonly StyledProperty<int> MaxPrecisionProperty =
        AvaloniaProperty.Register<DoubleEditor, int> (nameof (MaxPrecision), defaultValue: 0);

    /// <summary>
    /// get/sets IsDragging
    /// </summary>
    public bool IsDragging
    {
        get => GetValue (IsDraggingProperty);
        set => SetValue (IsDraggingProperty, value);
    }

    /// <summary>
    /// <see cref="IsDragging"/>
    /// </summary>
    public static readonly StyledProperty<bool> IsDraggingProperty =
        AvaloniaProperty.Register<DoubleEditor, bool> (nameof (IsDragging));

    /// <summary>
    /// get/sets PropertyDescriptor
    /// </summary>
    public PropertyDescriptor PropertyDescriptor
    {
        get => GetValue (PropertyDescriptorProperty);
        set => SetValue (PropertyDescriptorProperty, value);
    }

    /// <summary>
    /// <see cref="PropertyDescriptor"/>
    /// </summary>
    public static readonly StyledProperty<PropertyDescriptor> PropertyDescriptorProperty =
        AvaloniaProperty.Register<DoubleEditor, PropertyDescriptor> (nameof (PropertyDescriptor));

    /// <summary>
    /// <see cref="PropertyEditingStarted"/>
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> PropertyEditingStartedEvent =
        RoutedEvent.Register<DoubleEditor, RoutedEventArgs>
            (nameof (PropertyEditingStartedEvent), RoutingStrategies.Bubble);

    /// <summary>
    /// Occurs when property editing is started.
    /// </summary>
    public event EventHandler PropertyEditingStarted
    {
        add => AddHandler (PropertyEditingStartedEvent, value);
        remove => RemoveHandler (PropertyEditingStartedEvent, value);
    }

    /// <summary>
    /// <see cref="PropertyEditingFinished"/>
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> PropertyEditingFinishedEvent =
        RoutedEvent.Register<DoubleEditor, RoutedEventArgs> (nameof (PropertyEditingFinishedEvent),
            RoutingStrategies.Bubble);

    /// <summary>
    /// Occurs when property editing is finished.
    /// </summary>
    public event EventHandler PropertyEditingFinished
    {
        add => AddHandler (PropertyEditingFinishedEvent, value);
        remove => RemoveHandler (PropertyEditingFinishedEvent, value);
    }

    /// <summary>
    /// initialize sone handlers commands
    /// </summary>
    public DoubleEditor()
    {
        Increase = ReactiveCommand.Create<object> (OnIncrease, outputScheduler: RxApp.MainThreadScheduler);

        Decrease = ReactiveCommand.Create<object> (OnDecrease, outputScheduler: RxApp.MainThreadScheduler);

        ValueProperty.Changed.AddClassHandler<DoubleEditor> (ValueChanged);
        MinimumProperty.Changed.AddClassHandler<DoubleEditor> (OnMinimumChanged);
        MaximumProperty.Changed.AddClassHandler<DoubleEditor> (OnMaximumChanged);
        IsDraggingProperty.Changed.AddClassHandler<DoubleEditor> (OnIsDraggingChanged);
    }

    /// <inheritdoc cref="InputElement.OnPointerPressed"/>
    protected override void OnPointerPressed
        (
            PointerPressedEventArgs eventArgs
        )
    {
        base.OnPointerPressed (eventArgs);

        var prop = eventArgs.GetCurrentPoint (this).Properties;

        if (prop.IsLeftButtonPressed)
        {
            _isMouseDown = true;
            _dragStartPoint = eventArgs.GetPosition (this);

            Focus();
            eventArgs.Pointer.Capture (this);

            eventArgs.Handled = true;
        }
    }

    /// <inheritdoc cref="InputElement.OnPointerMoved"/>
    protected override void OnPointerMoved
        (
            PointerEventArgs eventArgs
        )
    {
        base.OnPointerMoved (eventArgs);

        var position = eventArgs.GetPosition (this);
        Vector vector = position - _dragStartPoint;

        if (_isMouseDown)
        {
            if (!IsDragging)
            {
                if (vector.Length > DragTolerance)
                {
                    IsDragging = true;
                    eventArgs.Handled = true;

                    _dragStartPoint = position;

                    _lastDragPoint = _dragStartPoint;
                    _changeValue = Value;
                    _changeOffset = 0;
                }
            }
            else
            {
                Vector offset = position - _lastDragPoint;
                var offsetLength = Math.Round (offset.Length);

                if (offsetLength != 0)
                {
                    CalculateValue ((offset.X > offset.Y) ? offsetLength : -offsetLength);
                    _lastDragPoint = position;
                }
            }

            eventArgs.Handled = true;
        }
    }

    /// <inheritdoc cref="Control.OnPointerReleased"/>
    protected override void OnPointerReleased
        (
            PointerReleasedEventArgs eventArgs
        )
    {
        base.OnPointerReleased (eventArgs);
        if (IsDragging || _isMouseDown)
        {
            eventArgs.Handled = true;
            IsDragging = false;
            _isMouseDown = false;
        }

        eventArgs.Pointer.Capture (null);
    }

    /// <inheritdoc cref="InputElement.OnKeyDown"/>
    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        base.OnKeyDown (eventArgs);

        _currentKeyModifiers = eventArgs.KeyModifiers;
    }

    private void OnIsDraggingChanged (DoubleEditor doubleEditor, AvaloniaPropertyChangedEventArgs e)
    {
        doubleEditor.OnIsDraggingChanged();
    }

    private void OnMaximumChanged (DoubleEditor doubleEditor, AvaloniaPropertyChangedEventArgs e)
    {
    }

    private void OnMinimumChanged (DoubleEditor doubleEditor, AvaloniaPropertyChangedEventArgs e)
    {
    }

    private void ValueChanged (DoubleEditor doubleEditor, AvaloniaPropertyChangedEventArgs e)
    {
        if (!doubleEditor.IsInitialized)
        {
            return;
        }

        doubleEditor.Value = doubleEditor.EnforceLimitsAndPrecision ((double) e.NewValue);
    }

    /// <summary>
    /// if isdragging OnPropertyEditingStarted is calles
    /// else OnPropertyEditingFinished
    /// </summary>
    protected virtual void OnIsDraggingChanged()
    {
        if (IsDragging)
        {
            OnPropertyEditingStarted();
        }
        else
        {
            OnPropertyEditingFinished();
        }
    }

    private void OnDecrease (object x)
    {
        _changeValue = Value;
        _changeOffset = 0;
        CalculateValue (-DefaultChange);
    }

    private void OnIncrease (object x)
    {
        _changeValue = Value;
        _changeOffset = 0;
        CalculateValue (DefaultChange);
    }

    private void CalculateValue (double chageValue)
    {
        //
        // Calculate the base ammount of chage based on...
        //
        // On Mouse Click & Control Key Press
        if ((_currentKeyModifiers & KeyModifiers.Control) != KeyModifiers.None)
        {
            chageValue *= SmallChange;
        }

        // On Mouse Click & Shift Key Press
        else if ((_currentKeyModifiers & KeyModifiers.Shift) != KeyModifiers.None)
        {
            chageValue *= LargeChange;
        }
        else
        {
            chageValue *= DefaultChange;
        }

        _changeOffset += chageValue;
        var newValue = _changeValue + _changeOffset;

        // Make sure the change is line up with Max/Min Limits and set the precission as specified.
        Value = EnforceLimitsAndPrecision (newValue);
    }

    private double EnforceLimitsAndPrecision (double value)
    {
        return Math.Round (Math.Max (Minimum, Math.Min (Maximum, value)), MaxPrecision);
    }

    /// <summary>
    /// Raises the <see cref="PropertyEditingStarted"/> event.
    /// </summary>
    protected virtual void OnPropertyEditingStarted()
    {
        RaiseEvent (new PropertyEditingEventArgs (PropertyEditingStartedEvent, this, PropertyDescriptor));
    }

    /// <summary>
    /// Raises the <see cref="PropertyEditingFinished"/> event.
    /// </summary>
    protected virtual void OnPropertyEditingFinished()
    {
        RaiseEvent (new PropertyEditingEventArgs (PropertyEditingFinishedEvent, this, PropertyDescriptor));
    }
}
