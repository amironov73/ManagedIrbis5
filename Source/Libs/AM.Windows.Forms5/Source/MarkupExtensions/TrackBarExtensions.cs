// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TrackBarExtensions.cs -- методы расширения для TrackBar
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="TrackBar"/>.
/// </summary>
public static class TrackBarExtensions
{
    #region Public methods

    /// <summary>
    /// Задание большого приращения для трекбара.
    /// </summary>
    public static TTrackBar LargeChange<TTrackBar>
        (
            this TTrackBar trackBar,
            int change
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.Positive (change);

        trackBar.LargeChange = change;

        return trackBar;
    }

    /// <summary>
    /// Задание максимального значения для трекбара.
    /// </summary>
    public static TTrackBar Maximum<TTrackBar>
        (
            this TTrackBar trackBar,
            int maximum
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);

        trackBar.Maximum = maximum;

        return trackBar;
    }

    /// <summary>
    /// Задание минимального значения для трекбара.
    /// </summary>
    public static TTrackBar Minimum<TTrackBar>
        (
            this TTrackBar trackBar,
            int minimum
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);

        trackBar.Minimum = minimum;

        return trackBar;
    }

    /// <summary>
    /// Подписка на событие <see cref="TrackBar.Click"/>.
    /// </summary>
    public static TTrackBar OnClick<TTrackBar>
        (
            this TTrackBar trackBar,
            EventHandler handler
        )
        where TTrackBar: TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.NotNull (handler);

        trackBar.Click += handler;

        return trackBar;
    }

    /// <summary>
    /// Подписка на событие <see cref="TrackBar.DoubleClick"/>.
    /// </summary>
    public static TTrackBar OnDoubleClick<TTrackBar>
        (
            this TTrackBar trackBar,
            EventHandler handler
        )
        where TTrackBar: TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.NotNull (handler);

        trackBar.DoubleClick += handler;

        return trackBar;
    }

    /// <summary>
    /// Подписка на событие <see cref="TrackBar.Scroll"/>.
    /// </summary>
    public static TTrackBar OnScroll<TTrackBar>
        (
            this TTrackBar trackBar,
            EventHandler handler
        )
        where TTrackBar: TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.NotNull (handler);

        trackBar.Scroll += handler;

        return trackBar;
    }

    /// <summary>
    /// Подписка на событие <see cref="TrackBar.ValueChanged"/>.
    /// </summary>
    public static TTrackBar OnValueChanged<TTrackBar>
        (
            this TTrackBar trackBar,
            EventHandler handler
        )
        where TTrackBar: TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.NotNull (handler);

        trackBar.ValueChanged += handler;

        return trackBar;
    }

    /// <summary>
    /// Задание ориентации для трекбара.
    /// </summary>
    public static TTrackBar Orientation<TTrackBar>
        (
            this TTrackBar trackBar,
            Orientation orientation
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.Defined (orientation);

        trackBar.Orientation = orientation;

        return trackBar;
    }

    /// <summary>
    /// Задание вертикальной ориентации для трекбара.
    /// </summary>
    public static TTrackBar OrientationVertical<TTrackBar>
        (
            this TTrackBar trackBar
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);

        trackBar.Orientation = System.Windows.Forms.Orientation.Vertical;

        return trackBar;
    }

    /// <summary>
    /// Задание мелкого шага для трекбара.
    /// </summary>
    public static TTrackBar SmallChange<TTrackBar>
        (
            this TTrackBar trackBar,
            int change
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.Positive (change);

        trackBar.SmallChange = change;

        return trackBar;
    }

    /// <summary>
    /// Задание частоты тиков для трекбара.
    /// </summary>
    public static TTrackBar TickFrequency<TTrackBar>
        (
            this TTrackBar trackBar,
            int frequency
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.Positive (frequency);

        trackBar.TickFrequency = frequency;

        return trackBar;
    }

    /// <summary>
    /// Задание стиля тиков для трекбара.
    /// </summary>
    public static TTrackBar TickStyle<TTrackBar>
        (
            this TTrackBar trackBar,
            TickStyle style
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);
        Sure.Defined (style);

        trackBar.TickStyle = style;

        return trackBar;
    }

    /// <summary>
    /// Задание текущего значения для трекбара.
    /// </summary>
    public static TTrackBar Value<TTrackBar>
        (
            this TTrackBar trackBar,
            int value
        )
        where TTrackBar : TrackBar
    {
        Sure.NotNull (trackBar);

        trackBar.Value = value;

        return trackBar;
    }

    #endregion
}
