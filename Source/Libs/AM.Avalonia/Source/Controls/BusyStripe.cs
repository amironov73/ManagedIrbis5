// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

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

namespace AM.Avalonia.Controls;

/// <summary>
/// Полоска "приложение занято чем-то важным" с бегающим градиентом.
/// </summary>
public sealed class BusyStripe
    : Control, IBusyState
{
    #region Properties

    /// <summary>
    /// Описание свойства "Активность".
    /// </summary>
    public static readonly DirectProperty<BusyStripe, bool> ActiveProperty
        = AvaloniaProperty.RegisterDirect<BusyStripe, bool>
            (
                nameof (Active),
                x => x.Active,
                (x, v) => x.Active = v
            );

    /// <summary>
    /// Описание свойства "Позиция".
    /// </summary>
    public static readonly DirectProperty<BusyStripe, double> PositionProperty
        = AvaloniaProperty.RegisterDirect<BusyStripe, double>
            (
                nameof (Position),
                x => x.Position,
                (x, v) => x.Position = v
            );

    /// <summary>
    /// Описание свойства "Текст".
    /// </summary>
    public static readonly DirectProperty<BusyStripe, string?> TextProperty
        = AvaloniaProperty.RegisterDirect<BusyStripe, string?>
            (
                nameof (Text),
                x => x.Text,
                (x, v) => x.Text = v
            );

    /// <summary>
    /// Описание свойства "Состояние".
    /// </summary>
    public static readonly DirectProperty<BusyStripe, IBusyState> StatePropery
        = AvaloniaProperty.RegisterDirect<BusyStripe, IBusyState>
            (
                nameof (State),
                x => x.State,
                (_, _) => { /* Do nothing */ }
            );

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

    /// <summary>
    /// Состояние.
    /// </summary>
    public IBusyState State => this;

    #endregion

    #region Construction

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static BusyStripe()
    {
        AffectsArrange<BusyStripe> (ActiveProperty);
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

    private async void SetActiveState (bool value)
    {
        await Dispatcher.UIThread.InvokeAsync(() => IsVisible = Active = value);
    }

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

    #region IBusyState members

    /// <inheritdoc cref="IBusyState.IsBusy"/>
    bool IBusyState.IsBusy
    {
        get => Active;
        set => SetActiveState (value);
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
