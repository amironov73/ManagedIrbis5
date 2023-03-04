// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ProgressStripe.cs -- простая полоска прогресса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Threading;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Полоска прогресса.
/// </summary>
[PublicAPI]
public class ProgressStripe
    : Control
{
    #region Properties

    /// <summary>
    /// Описание свойства <see cref="Active"/>.
    /// </summary>
    public static readonly DirectProperty<ProgressStripe, bool> ActiveProperty
        = AvaloniaProperty.RegisterDirect<ProgressStripe, bool>
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
    /// Описание свойства <see cref="Percentage"/>.
    /// </summary>
    public static readonly DirectProperty<ProgressStripe, double> PercentageProperty =
        AvaloniaProperty.RegisterDirect<ProgressStripe, double>
            (
                nameof (Percentage),
                x => x.Percentage,
                (x, v) => x.Percentage = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true
            );

    /// <summary>
    /// Процент выполненного действия (величина прогресса в процентах).
    /// </summary>
    public double Percentage { get; set; }

    #endregion

    #region Construction

    static ProgressStripe()
    {
        AffectsArrange<ProgressStripe> (ActiveProperty);
        AffectsRender<ProgressStripe> (PercentageProperty);
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

        var value = Math.Clamp (Percentage, 0, 100);
        var boundsWidth = Bounds.Width;
        var width = boundsWidth * value / 100;
        Debug.WriteLine ($"Value={value}, Bounds.Width={boundsWidth}, Width={width}");

        context.FillRectangle (Brushes.LightGreen, new Rect (0, 0, boundsWidth, Bounds.Height));
        context.FillRectangle (Brushes.Blue, new Rect (0, 0, width, Bounds.Height));
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
            Percentage = value;
            InvalidateVisual();
        });
    }

    #endregion
}
