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
using System.Diagnostics;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/MahApps/MahApps.Metro

/// <summary>
/// The MetroThumbContentControl control can be used
/// for titles or something else and
/// enables basic drag movement functionality.
/// </summary>
public class MetroThumbContentControl : ContentControlEx, IMetroThumb
{
    /// <summary>
    /// style key for this control
    /// </summary>
    public new Type StyleKey => typeof (MetroThumbContentControl);

    private Point startDragPoint;
    private PixelPoint startDragScreenPoint;
    private PixelPoint? oldDragScreenPoint;

    /// <summary>
    /// <see cref="DragStarted"/>
    /// </summary>
    public static readonly RoutedEvent<VectorEventArgs> DragStartedEvent =
        RoutedEvent.Register<MetroThumbContentControl, VectorEventArgs> (nameof (DragStartedEvent),
            RoutingStrategies.Bubble);

    /// <summary>
    /// get/set drag started event
    /// </summary>
    public event EventHandler<VectorEventArgs> DragStarted
    {
        add => AddHandler (DragStartedEvent, value);
        remove => RemoveHandler (DragStartedEvent, value);
    }

    /// <summary>
    /// <see cref="DragDelta"/>
    /// </summary>
    public static readonly RoutedEvent<VectorEventArgs> DragDeltaEvent =
        RoutedEvent.Register<MetroThumbContentControl, VectorEventArgs> (nameof (DragDeltaEvent),
            RoutingStrategies.Bubble);

    /// <summary>
    /// get/set drag delta event
    /// </summary>
    public event EventHandler<VectorEventArgs> DragDelta
    {
        add => AddHandler (DragDeltaEvent, value);
        remove => RemoveHandler (DragDeltaEvent, value);
    }

    /// <summary>
    /// <see cref="DragCompleted"/>
    /// </summary>
    public static readonly RoutedEvent<VectorEventArgs> DragCompletedEvent =
        RoutedEvent.Register<MetroThumbContentControl, VectorEventArgs> (nameof (DragCompletedEvent),
            RoutingStrategies.Bubble);

    /// <summary>
    /// get/set dragcompleted event
    /// </summary>
    public event EventHandler<VectorEventArgs> DragCompleted
    {
        add => AddHandler (DragCompletedEvent, value);
        remove => RemoveHandler (DragCompletedEvent, value);
    }

    /// <summary>
    /// Indicates that the left mouse button is pressed and is over the MetroThumbContentControl.
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
        AvaloniaProperty.Register<MetroThumbContentControl, bool> (nameof (IsDragging));

    /// <summary>
    /// cancels the drag
    /// </summary>
    public void CancelDragAction()
    {
        if (!IsDragging)
        {
            return;
        }

        //if (this.IsMouseCaptured)
        //{
        //    this.ReleaseMouseCapture();
        //}

        ClearValue (IsDraggingProperty);
        var horizontalChange = oldDragScreenPoint.Value.X - startDragScreenPoint.X;
        var verticalChange = oldDragScreenPoint.Value.Y - startDragScreenPoint.Y;

        var args = new VectorEventArgs()
        {
            Handled = true,
            RoutedEvent = DragCompletedEvent,
            Vector = new Vector (horizontalChange, verticalChange)
        };

        RaiseEvent (args);
    }

    /// <inheritdoc cref="InputElement.OnPointerPressed"/>
    protected override void OnPointerPressed
        (
            PointerPressedEventArgs eventArgs
        )
    {
        var properties = eventArgs.GetCurrentPoint (this).Properties;

        if (properties.IsLeftButtonPressed == false)
            return;

        if (!IsDragging)
        {
            eventArgs.Handled = true;
            try
            {
                // focus me
                Focus();

                // now capture the mouse for the drag action
                //this.CaptureMouse();
                eventArgs.Pointer.Capture (this);

                // so now we are in dragging mode
                SetValue (IsDraggingProperty, true);

                // get the mouse points
                startDragPoint = eventArgs.GetPosition (this);
                oldDragScreenPoint = startDragScreenPoint = this.PointToScreen (startDragPoint);

                var args = new VectorEventArgs()
                {
                    RoutedEvent = DragStartedEvent,
                    Vector = new Vector (startDragPoint.X, startDragPoint.Y)
                };

                RaiseEvent (args);
            }
            catch (Exception exception)
            {
                Trace.TraceError (
                    $"{this}: Something went wrong here: {exception} {Environment.NewLine} {exception.StackTrace}");
                CancelDragAction();
            }
        }

        base.OnPointerPressed (eventArgs);
    }

    /// <inheritdoc cref="Control.OnPointerReleased"/>
    protected override void OnPointerReleased
        (
            PointerReleasedEventArgs eventArgs
        )
    {
        if ( /*e.Pointer.Captured.*/IsDragging)
        {
            eventArgs.Handled = true;

            // now we are in normal mode
            ClearValue (IsDraggingProperty);

            // release the captured mouse
            eventArgs.Pointer.Capture (null);

            //this.ReleaseMouseCapture();
            // get the current mouse position and call the completed event with the horizontal/vertical change
            var currentMouseScreenPoint = this.PointToScreen (eventArgs.GetPosition (this));
            var horizontalChange = currentMouseScreenPoint.X - startDragScreenPoint.X;
            var verticalChange = currentMouseScreenPoint.Y - startDragScreenPoint.Y;

            var args = new VectorEventArgs()
            {
                Handled = false,
                RoutedEvent = DragCompletedEvent,
                Vector = new Vector (horizontalChange, verticalChange)
            };

            RaiseEvent (args);
        }
    }

    /// <inheritdoc cref="InputElement.OnPointerCaptureLost"/>
    protected override void OnPointerCaptureLost (PointerCaptureLostEventArgs e)
    {
        // Cancel the drag action if we lost capture
        var thumb = (MetroThumbContentControl)e.Source;
        if (e.Pointer.Captured != thumb)
        {
            thumb.CancelDragAction();
        }
    }

    /// <inheritdoc cref="InputElement.OnPointerMoved"/>
    protected override void OnPointerMoved
        (
            PointerEventArgs eventArgs
        )
    {
        base.OnPointerMoved (eventArgs);

        if (!IsDragging)
        {
            return;
        }

        if (oldDragScreenPoint.HasValue)
        {
            var currentDragPoint = eventArgs.GetPosition (this);

            // Get client point and convert it to screen point
            var currentDragScreenPoint = this.PointToScreen (currentDragPoint);
            if (currentDragScreenPoint != oldDragScreenPoint)
            {
                oldDragScreenPoint = currentDragScreenPoint;
                eventArgs.Handled = true;
                var horizontalChange = currentDragPoint.X - startDragPoint.X;
                var verticalChange = currentDragPoint.Y - startDragPoint.Y;

                var ev = new VectorEventArgs
                {
                    RoutedEvent = DragDeltaEvent,
                    Vector = new Vector (horizontalChange, verticalChange)
                };

                RaiseEvent (ev);
            }
        }
        else
        {
            ClearValue (IsDraggingProperty);
            startDragPoint = new Point (0, 0);
        }
    }

    //private void ReleaseCurrentDevice()
    //{
    //    if (this.currentDevice != null)
    //    {
    //        // Set the reference to null so that we don't re-capture in the OnLostTouchCapture() method
    //        var temp = this.currentDevice;
    //        this.currentDevice = null;
    //        //this.ReleaseTouchCapture(temp);
    //    }
    //}

    //private void CaptureCurrentDevice(TouchEventArgs e)
    //{
    //    bool gotTouch = this.CaptureTouch(e.TouchDevice);
    //    if (gotTouch)
    //    {
    //        this.currentDevice = e.TouchDevice;
    //    }
    //}
}
