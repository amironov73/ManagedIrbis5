// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ProgressRing.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// progress status ring
/// </summary>
public class ProgressRing
    : TemplatedControl
{
    #region Constants

    private const string PseudoClassActive = ":active";
    private const string PseudoClassInactive = ":inactive";

    #endregion

    /// <summary>
    /// get /set BindableWidth
    /// </summary>
    public double BindableWidth
    {
        get => GetValue (BindableWidthProperty);
        set => SetValue (BindableWidthProperty, value);
    }

    /// <summary>
    /// <see cref="BindableWidth"/>
    /// </summary>
    public static readonly StyledProperty<double> BindableWidthProperty =
        AvaloniaProperty.Register<ProgressRing, double> (nameof (BindableWidth));

    /// <summary>
    /// get/sets IsActive
    /// </summary>
    public bool IsActive
    {
        get => GetValue (IsActiveProperty);
        set => SetValue (IsActiveProperty, value);
    }

    /// <summary>
    /// <see cref="IsActive"/>
    /// </summary>
    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<ProgressRing, bool> (nameof (IsActive), defaultValue: true);

    /// <summary>
    /// get/sets MaxSideLength
    /// </summary>
    public double MaxSideLength
    {
        get => GetValue (MaxSideLengthProperty);
        set => SetValue (MaxSideLengthProperty, value);
    }

    /// <summary>
    /// <see cref="MaxSideLength"/>
    /// </summary>
    public static readonly StyledProperty<double> MaxSideLengthProperty =
        AvaloniaProperty.Register<ProgressRing, double> (nameof (MaxSideLength), defaultValue: default (double));

    /// <summary>
    /// get/sets EllipseDiameter
    /// </summary>
    public double EllipseDiameter
    {
        get => GetValue (EllipseDiameterProperty);
        set => SetValue (EllipseDiameterProperty, value);
    }

    /// <summary>
    /// <see cref="EllipseDiameter"/>
    /// </summary>
    public static readonly StyledProperty<double> EllipseDiameterProperty =
        AvaloniaProperty.Register<ProgressRing, double> (nameof (EllipseDiameter));

    /// <summary>
    /// get/sets EllipseOffset
    /// </summary>
    public Thickness EllipseOffset
    {
        get { return (Thickness)GetValue (EllipseOffsetProperty); }
        set { SetValue (EllipseOffsetProperty, value); }
    }

    /// <summary>
    /// <see cref="EllipseOffset"/>
    /// </summary>
    public static readonly StyledProperty<Thickness> EllipseOffsetProperty =
        AvaloniaProperty.Register<ProgressRing, Thickness> (nameof (EllipseOffset));

    /// <summary>
    /// get/sets EllipseDiameterScale
    /// </summary>
    public double EllipseDiameterScale
    {
        get { return (double)GetValue (EllipseDiameterScaleProperty); }
        set { SetValue (EllipseDiameterScaleProperty, value); }
    }

    /// <summary>
    /// <see cref="EllipseDiameterScale"/>
    /// </summary>
    public static readonly StyledProperty<double> EllipseDiameterScaleProperty =
        AvaloniaProperty.Register<ProgressRing, double> (nameof (EllipseDiameterScale), defaultValue: 1D);

    /// <summary>
    /// add some changed listeners
    /// </summary>
    static ProgressRing()
    {
        BindableWidthProperty.Changed.AddClassHandler<ProgressRing> ((o, e) => BindableWidthCallback (o, e));
        IsActiveProperty.Changed.AddClassHandler<ProgressRing> ((o, e) => IsActiveChanged (o, e));
        IsVisibleProperty.Changed.AddClassHandler<ProgressRing> ((o, e) => IsVisibleChanged (o, e));
        WidthProperty.Changed.AddClassHandler<ProgressRing> ((o, e) => OnSizeChanged (o, e));
        HeightProperty.Changed.AddClassHandler<ProgressRing> ((o, e) => OnSizeChanged (o, e));
    }

    private static void OnSizeChanged (ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
    {
        ring.SetValue (BindableWidthProperty, ring.Width);
    }

    private static void IsVisibleChanged (ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue != e.OldValue)
        {
            if ((bool) e.NewValue == false)
            {
                ring.SetValue (IsActiveProperty, false);
            }
            else
            {
                ring.SetValue (IsActiveProperty, true);
            }
        }
    }

    private static void IsActiveChanged (ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
    {
        ring.UpdatePseudoClasses();
    }

    private static void BindableWidthCallback (ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
    {
        ring.SetEllipseDiameter ((double) e.NewValue!);
        ring.SetEllipseOffset ((double) e.NewValue);
        ring.SetMaxSideLength ((double) e.NewValue);
    }

    private void SetMaxSideLength (double width)
    {
        SetValue (MaxSideLengthProperty, width <= 20 ? 20 : width);
    }

    private void SetEllipseDiameter (double width)
    {
        SetValue (EllipseDiameterProperty, (width / 8) * EllipseDiameterScale);
    }

    private void SetEllipseOffset (double width)
    {
        SetValue (EllipseOffsetProperty, new Thickness (0, width / 2, 0, 0));
    }

    /// <summary>
    /// UpdateLargeState, UpdateActiveState
    /// </summary>
    /// <param name="e"></param>
    protected override void OnApplyTemplate (TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate (e);

        //make sure the states get updated
        UpdatePseudoClasses();
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Remove (PseudoClassActive);
        PseudoClasses.Remove (PseudoClassInactive);

        PseudoClasses.Add (IsActive ? PseudoClassActive : PseudoClassInactive);
    }


    /// <summary>
    /// sets the pseudo classes
    /// </summary>
    /// <param name="context"></param>
    public override void Render (DrawingContext context)
    {
        UpdatePseudoClasses();
        base.Render (context);
    }
}
