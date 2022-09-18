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

using Avalonia.Controls.Primitives;
using Avalonia.Input;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/MahApps/MahApps.Metro

/// <summary>
/// metro thumb
/// </summary>
public class MetroThumb : Thumb, IMetroThumb
{
    /// <summary>
    /// style key for this control
    /// </summary>
    public Type StyleKey => typeof (MetroThumb);

    /// <summary>
    /// Indicates that the left mouse button is
    /// pressed and is over the MetroThumbContentControl.
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
        AvaloniaProperty.Register<MetroThumb, bool> (nameof (IsDragging));

    /// <inheritdoc cref="Thumb.OnDragStarted"/>
    protected override void OnDragStarted
        (
            VectorEventArgs eventArgs
        )
    {
        IsDragging = true;
        base.OnDragStarted (eventArgs);
    }

    /// <inheritdoc cref="Thumb.OnDragCompleted"/>
    protected override void OnDragCompleted
        (
            VectorEventArgs eventArgs
        )
    {
        IsDragging = false;
        base.OnDragCompleted (eventArgs);
    }
}
