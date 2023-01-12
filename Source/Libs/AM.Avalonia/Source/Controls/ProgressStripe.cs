// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ProgressStripe.cs -- простая полоска прогресса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Полоска прогресса.
/// </summary>
public class ProgressStripe
    : Control
{
    #region Properties

    /// <summary>
    /// Описание свойства <see cref="Active"/>.
    /// </summary>
    public static readonly DirectProperty<BusyStripe, bool> ActiveProperty
        = AvaloniaProperty.RegisterDirect<BusyStripe, bool>
            (
                nameof (Active),
                x => x.Active,
                (x, v) => x.Active = v
            );

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
        }
    }

    /// <summary>
    /// Описание свойства <see cref="Value"/>.
    /// </summary>
    public static readonly DirectProperty<ProgressStripe, double> ValueProperty =
        AvaloniaProperty.RegisterDirect<ProgressStripe, double>
            (
                nameof (Value),
                x => x.Value,
                (x, v) => x.Value = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true
            );

    /// <summary>
    /// Значение прогресса.
    /// </summary>
    public double Value { get; set; }

    #endregion

    #region Construction

    static ProgressStripe()
    {
        AffectsArrange<BusyStripe> (ActiveProperty);
        AffectsRender<BusyStripe> (ValueProperty);
    }

    #endregion

    #region Private members

    private bool _active;

    #endregion

    #region Control members

    /// <inheritdoc cref="Visual.Render"/>
    public override void Render
        (
            DrawingContext context
        )
    {
        Sure.NotNull (context);

        var value = Math.Clamp (Value, 0, 100);
        var width = Bounds.Width * value / 100;
        context.FillRectangle (Brushes.LightGreen, new Rect (0, 0, Bounds.Width, Bounds.Height));
        context.FillRectangle (Brushes.Green, new Rect (0, 0, width, Bounds.Height));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Установка активности.
    /// </summary>
    public void SetActive
        (
            bool value
        )
    {
        Dispatcher.UIThread.InvokeAsync (() => Active = value);
    }

    /// <summary>
    /// Установка значения прогресса.
    /// </summary>
    public void SetProgress
        (
            double value
        )
    {
        Dispatcher.UIThread.InvokeAsync (() =>
        {
            Value = value;
            InvalidateVisual();
        });
    }

    #endregion
}
