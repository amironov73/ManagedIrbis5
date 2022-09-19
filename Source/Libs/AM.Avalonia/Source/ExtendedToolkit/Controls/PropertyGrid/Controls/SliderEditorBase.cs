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

using AM;

using Avalonia.Controls;
using Avalonia.ExtendedToolkit.Controls.PropertyGrid.PropertyEditing;
using Avalonia.Input;
using Avalonia.Interactivity;

#endregion

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Controls;

//
// ported from https://github.com/DenisVuyka/WPG
//

/// <summary>
/// Base class for Slider-like editor implementations.
/// </summary>
public abstract class SliderEditorBase
    : Slider
{
    /// <summary>
    /// style key of this control
    /// </summary>
    public Type StyleKey => typeof (SliderEditorBase);


    /// <summary>
    /// <see cref="PropertyEditingStarted"/>
    /// </summary>
    public static readonly RoutedEvent<PropertyEditingEventArgs> PropertyEditingStartedEvent =
        RoutedEvent.Register<SliderEditorBase, PropertyEditingEventArgs>
            (nameof (PropertyEditingStartedEvent), RoutingStrategies.Bubble);

    /// <summary>
    /// Occurs when property editing started.
    /// </summary>
    public event PropertyEditingEventHandler PropertyEditingStarted
    {
        add => AddHandler (PropertyEditingStartedEvent, value);
        remove => RemoveHandler (PropertyEditingStartedEvent, value);
    }

    /// <summary>
    /// <see cref="PropertyEditingFinished"/>
    /// </summary>
    public static readonly RoutedEvent<PropertyEditingEventArgs> PropertyEditingFinishedEvent =
        RoutedEvent.Register<SliderEditorBase, PropertyEditingEventArgs>
            (nameof (PropertyEditingFinishedEvent), RoutingStrategies.Bubble);

    /// <summary>
    /// Occurs when property editing is finished.
    /// </summary>
    public event PropertyEditingEventHandler PropertyEditingFinished
    {
        add => AddHandler (PropertyEditingFinishedEvent, value);
        remove => RemoveHandler (PropertyEditingFinishedEvent, value);
    }

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
        AvaloniaProperty.Register<SliderEditorBase, PropertyDescriptor> (nameof (PropertyDescriptor));

    #region Construction

    /// <summary>
    /// add some class handler for the property grid
    /// </summary>
    protected SliderEditorBase()
    {
        PropertyGrid.PropertyEditingStartedEvent.AddClassHandler (
            typeof (SliderEditorBase), (_, e) =>
            {
                var args = (e as PropertyEditingEventArgs).ThrowIfNull();

                var evt = new PropertyEditingEventArgs (PropertyEditingStartedEvent,
                    this, args.PropertyDescriptor);

                RaiseEvent (evt);
            });

        PropertyGrid.PropertyEditingFinishedEvent.AddClassHandler (
            typeof (SliderEditorBase), (_, e) =>
            {
                var args = (e as PropertyEditingEventArgs).ThrowIfNull();

                var evt = new PropertyEditingEventArgs (PropertyEditingFinishedEvent,
                    this, args.PropertyDescriptor);

                RaiseEvent (evt);
            });
    }

    #endregion

    /// <inheritdoc cref="Slider.OnThumbDragStarted"/>
    protected override void OnThumbDragStarted
        (
            VectorEventArgs eventArgs
        )
    {
        OnPropertyEditingStarted();
        base.OnThumbDragStarted (eventArgs);
    }

    /// <inheritdoc cref="Slider.OnThumbDragCompleted"/>
    protected override void OnThumbDragCompleted
        (
            VectorEventArgs eventArgs
        )
    {
        OnPropertyEditingFinished();
        base.OnThumbDragCompleted (eventArgs);
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
