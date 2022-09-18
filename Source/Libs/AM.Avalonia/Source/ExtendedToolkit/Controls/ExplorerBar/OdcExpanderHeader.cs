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
using Avalonia.Media;
using Avalonia.Media.Imaging;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/jogibear9988/OdysseyWPF.git

/// <summary>
/// A helper class to specify the header of an OdcExpander.
/// </summary>
public class OdcExpanderHeader : ToggleButton
{
    /// <summary>
    /// style key of this control
    /// </summary>
    public Type StyleKey => typeof (OdcExpanderHeader);

    /// <summary>
    /// Gets whether the expand geometry is not null.
    /// </summary>
    public bool HasExpandGeometry
    {
        get => GetValue (HasExpandGeometryProperty);
        set => SetValue (HasExpandGeometryProperty, value);
    }

    /// <summary>
    /// <see cref="HasExpandGeometry"/>
    /// </summary>
    public static readonly StyledProperty<bool> HasExpandGeometryProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, bool> (nameof (HasExpandGeometry));

    /// <summary>
    /// Gets or sets the geometry for the collapse symbol.
    /// </summary>
    public Geometry CollapseGeometry
    {
        get => GetValue (CollapseGeometryProperty);
        set => SetValue (CollapseGeometryProperty, value);
    }

    /// <summary>
    /// <see cref="CollapseGeometry"/>
    /// </summary>
    public static readonly StyledProperty<Geometry> CollapseGeometryProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, Geometry> (nameof (CollapseGeometry));

    /// <summary>
    /// Gets or sets the geometry for the expand symbol.
    /// </summary>
    public Geometry ExpandGeometry
    {
        get => GetValue (ExpandGeometryProperty);
        set => SetValue (ExpandGeometryProperty, value);
    }

    /// <summary>
    /// <see cref="ExpandGeometry"/>
    /// </summary>
    public static readonly StyledProperty<Geometry> ExpandGeometryProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, Geometry> (nameof (ExpandGeometry)
            );

    /// <summary>
    /// Gets or sets the corner radius for the header.
    /// </summary>
    public new CornerRadius CornerRadius
    {
        get => GetValue (CornerRadiusProperty);
        set => SetValue (CornerRadiusProperty, value);
    }

    /// <summary>
    /// <see cref="CornerRadius"/>
    /// </summary>
    public new static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, CornerRadius> (nameof (CornerRadius));

    /// <summary>
    /// Gets or sets whether to display the ellipse arround the collapse/expand symbol.
    /// </summary>
    public bool ShowEllipse
    {
        get => GetValue (ShowEllipseProperty);
        set => SetValue (ShowEllipseProperty, value);
    }

    /// <summary>
    /// <see cref="ShowEllipse"/>
    /// </summary>
    public static readonly StyledProperty<bool> ShowEllipseProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, bool> (nameof (ShowEllipse));

    /// <summary>
    /// for styling the cirecle button
    /// </summary>
    public IBrush CircleButtonStroke
    {
        get => GetValue (CircleButtonStrokeProperty);
        set => SetValue (CircleButtonStrokeProperty, value);
    }

    /// <summary>
    /// <see cref="CircleButtonStroke"/>
    /// </summary>
    public static readonly StyledProperty<IBrush> CircleButtonStrokeProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, IBrush> (nameof (CircleButtonStroke));

    /// <summary>
    /// for styling the cirecle button
    /// </summary>
    public IBrush CircleButtonFill
    {
        get => GetValue (CircleButtonFillProperty);
        set => SetValue (CircleButtonFillProperty, value);
    }

    /// <summary>
    /// <see cref="CircleButtonFill"/>
    /// </summary>
    public static readonly StyledProperty<IBrush> CircleButtonFillProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, IBrush> (nameof (CircleButtonFill));

    /// <summary>
    /// for styling the circle button
    /// </summary>
    public IBrush CircleButtonForeground
    {
        get => GetValue (CircleButtonForegroundProperty);
        set => SetValue (CircleButtonForegroundProperty, value);
    }

    /// <summary>
    /// <see cref="CircleButtonForeground"/>
    /// </summary>
    public static readonly StyledProperty<IBrush> CircleButtonForegroundProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, IBrush> (nameof (CircleButtonForeground));

    /// <summary>
    /// Gets or sets the Image to display on the header.
    /// </summary>
    public IBitmap Image
    {
        get => GetValue (ImageProperty);
        set => SetValue (ImageProperty, value);
    }

    /// <summary>
    /// <see cref="Image"/>
    /// </summary>
    public static readonly StyledProperty<IBitmap> ImageProperty =
        AvaloniaProperty.Register<OdcExpanderHeader, IBitmap> (nameof (Image));

    static OdcExpanderHeader()
    {
        ExpandGeometryProperty.Changed.AddClassHandler<OdcExpanderHeader> (CollapseGeometryChangedCallback);
    }

    private static void CollapseGeometryChangedCallback
        (
            OdcExpanderHeader eh,
            AvaloniaPropertyChangedEventArgs eventArgs
        )
    {
        eh.HasExpandGeometry = eventArgs.NewValue != null;
    }

    /// <inheritdoc cref="Button.OnAttachedToVisualTree"/>
    protected override void OnAttachedToVisualTree
        (
            VisualTreeAttachmentEventArgs eventArgs
        )
    {
        base.OnAttachedToVisualTree (eventArgs);
        RaisePropertyChanged (ExpandGeometryProperty, null!, ExpandGeometry);
    }
}
