// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* BusyStripe.cs -- полоска "приложение занято чем-то важным" с бегающим градиентом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

using AM.Avalonia.Media;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Полоска "приложение занято чем-то важным" с бегающим градиентом.
/// </summary>
public sealed class BusyStripe
    : Control
{
    #region Properties

    /// <summary>
    /// Описание свойства "Активность".
    /// </summary>
    public static readonly StyledProperty<bool> ActiveProperty
        = AvaloniaProperty.Register<BusyStripe, bool> (nameof (Active));

    /// <summary>
    /// Описание свойства "Позиция".
    /// </summary>
    public static readonly StyledProperty<double> PositionProperty
        = AvaloniaProperty.Register<BusyStripe, double> (nameof (Position));

    /// <summary>
    /// Описание свойства "Текст".
    /// </summary>
    public static readonly StyledProperty<string?> TextProperty
        = AvaloniaProperty.Register<BusyStripe, string?> (nameof (Text));

    /// <summary>
    /// Позиция.
    /// </summary>
    public double Position
    {
        get => _position;
        set => SetAndRaise (PositionProperty, ref _position, value);
    }

    /// <summary>
    /// Текст.
    /// </summary>
    public string? Text
    {
        get => _text;
        set => SetAndRaise (TextProperty, ref _text, value);
    }

    /// <summary>
    /// Активно?
    /// </summary>
    public bool Active
    {
        get => _active;
        set
        {
            SetAndRaise (ActiveProperty, ref _active, value);
            IsVisible = value;
            _timer.IsEnabled = value;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static BusyStripe()
    {
        AffectsRender<BusyStripe> (PositionProperty, TextProperty);
    }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public BusyStripe()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds (50)
        };
        _timer.Tick += _Advance;
    }

    #endregion

    #region Private members

    private bool _active;
    private double _position = 0.3;
    private double _delta = 0.05;
    private string? _text;
    private readonly DispatcherTimer _timer;

    private void _Advance
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        if (_position is <= 0.1 or >= 0.9)
        {
            _delta = -_delta;
        }

        Position = _position + _delta;
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="Visual.Render"/>
    public override void Render
        (
            DrawingContext context
        )
    {
        Sure.NotNull (context);

        var brush = new LinearGradientBrush
        {
            StartPoint = new RelativePoint (0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint (1, 1, RelativeUnit.Relative),
            GradientStops =
            {
                new GradientStop (Colors.LightBlue, 0.0),
                new GradientStop (Colors.Green, _position),
                new GradientStop (Colors.LightBlue, 1.0)
            }
        };

        context.FillRectangle (brush, new Rect (0, 0, Bounds.Width, Bounds.Height));

        if (!string.IsNullOrEmpty (_text))
        {
            var formatted = new FormattedText
                (
                    _text,
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    Typeface.Default,
                    12.0,
                    Brushes.White
                );
            formatted.DrawCentered (context, Bounds);
        }
    }

    #endregion
}
