// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AnalogClock.cs -- простые аналоговые часы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Простые аналоговые часы
/// </summary>
public sealed class AnalogClock
    : Control
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AnalogClock()
    {
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds (1_000),
            IsEnabled = true
        };
        timer.Tick += _Advance;
    }

    #endregion

    #region Private members

    private void _Advance
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        InvalidateVisual();
    }

    private void DrawClockHand
        (
            DrawingContext context,
            Rect bounds,
            double angle,
            double size,
            IBrush brush,
            double thickness
        )
    {
        var centerX = bounds.Width / 2.0f;
        var centerY = bounds.Height / 2.0f;
        var pointX = centerX + Math.Sin (angle) * size;
        var pointY = centerY - Math.Cos (angle) * size;

        var pen = new Pen (brush, thickness);
        context.DrawLine (pen, new Point (centerX, centerY), new Point (pointX, pointY));
    }

    private void DrawDigit
        (
            DrawingContext context,
            Rect  bounds,
            Typeface typeface,
            IBrush brush,
            int digit
        )
    {
        var angle = FractionToAngle (digit / 12.0);
        var size = bounds.Width / 2.0 * 0.85;
        var centerX = bounds.Width / 2.0;
        var centerY = bounds.Height / 2.0;
        var x = centerX + Math.Sin (angle) * size - 5.0;
        var y= centerY - Math.Cos (angle) * size - 10.0;

        var text = digit.ToString (CultureInfo.InvariantCulture);
        var formatted = new FormattedText
            (
                text,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                12.0,
                brush
            );
        context.DrawText (formatted, new Point (x, y));
    }

    private double FractionToAngle (double fraction) => fraction * Math.PI * 2.0;

    #endregion

    #region Control members

    /// <inheritdoc cref="Visual.Render"/>
    public override void Render
        (
            DrawingContext context
        )
    {
        Sure.NotNull (context);

        var bounds = Bounds.Deflate (1);
        var centerX = bounds.Width / 2.0;
        var centerY = bounds.Height / 2.0;
        var center = new Point (centerX, centerY);
        var size = bounds.Width / 2.0;
        context.DrawEllipse
            (
                Brushes.White,
                new Pen(Brushes.Black),
                center,
                centerX - 1,
                centerY - 1
            );

        var now = DateTime.Now;
        var hour = now.Hour;
        while (hour >= 12)
        {
            hour -= 12;
        }

        var minute = now.Minute;
        var second = now.Second;
        DrawClockHand (context, bounds,
            FractionToAngle ((hour + minute / 60.0) / 12.0),
            0.5 * size, Brushes.Black, 6.0);
        DrawClockHand (context, bounds, FractionToAngle (minute / 60.0),
            0.65 * size, Brushes.Black, 3.0);

        DrawClockHand (context, bounds, FractionToAngle (second / 60.0),
            0.8 * size, Brushes.Red, 1.0);

        for (var digit = 1; digit < 13; digit++)
        {
            DrawDigit (context, bounds, Typeface.Default, Brushes.Blue, digit);
        }
    }

    #endregion
}
