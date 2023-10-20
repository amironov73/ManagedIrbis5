// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* LedIndicator.cs -- простейший индикатор "вкл-выкл".
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Простейший индикатор "вкл-выкл".
/// </summary>
[PublicAPI]
public sealed class LedIndicator
    : Control
{
    #region Properties

    /// <summary>
    /// Описание свойства <see cref="IsOn"/>.
    /// </summary>
    public static readonly DirectProperty<LedIndicator, bool> IsOnProperty
        = AvaloniaProperty.RegisterDirect<LedIndicator, bool>
            (
                nameof (IsOn),
                x => x.IsOn,
                (x, v) => x.IsOn = v
            );

    /// <summary>
    /// Включено?
    /// </summary>
    public bool IsOn
    {
        get => _isOn;
        set => SetAndRaise (IsOnProperty, ref _isOn, value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static LedIndicator()
    {
        AffectsRender<LedIndicator> (IsOnProperty);
    }

    #endregion

    #region Private members

    private bool _isOn;

    #endregion

    #region Control members

    /// <inheritdoc cref="Visual.Render"/>
    public override void Render
        (
            DrawingContext context
        )
    {
        Sure.NotNull (context);

        var radius = Math.Min (Bounds.Width, Bounds.Height) / 2.0;
        var brush = new RadialGradientBrush
        {
            GradientOrigin = new RelativePoint (0.3, 0.3, RelativeUnit.Relative),
            GradientStops = new GradientStops
            {
                new (_isOn ? Colors.LightGreen : Colors.DarkGray, 0.0),
                new (_isOn ? Colors.DarkGreen : Colors.DarkSlateGray, 1.0),
            }
        };

        var pen = new Pen (Brushes.Gray);
        var center = new Point (radius, radius);
        context.DrawEllipse (brush, pen, center, radius, radius);
    }

    #endregion
}
