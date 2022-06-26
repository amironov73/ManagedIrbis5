// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ToolTipExtensions.cs -- методы расширения для ToolTip
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ToolTip"/>.
/// </summary>
public static class ToolTipExtensions
{
    #region Public methods

    /// <summary>
    /// Задание активности для тултипа.
    /// </summary>
    public static TToolTip Active<TToolTip>
        (
            this TToolTip toolTip,
            bool active
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);

        toolTip.Active = active;

        return toolTip;
    }

    /// <summary>
    /// Задание автоматической задержки для тултипа.
    /// </summary>
    public static TToolTip AutomaticDelay<TToolTip>
        (
            this TToolTip toolTip,
            int delay
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);
        Sure.NonNegative (delay);

        toolTip.AutomaticDelay = delay;

        return toolTip;
    }

    /// <summary>
    /// Задание автоматической задержки для тултипа.
    /// </summary>
    public static TToolTip AutoPopDelay<TToolTip>
        (
            this TToolTip toolTip,
            int delay
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);
        Sure.NonNegative (delay);

        toolTip.AutoPopDelay = delay;

        return toolTip;
    }

    /// <summary>
    /// Задание начальной задержки для тултипа.
    /// </summary>
    public static TToolTip InitialDelay<TToolTip>
        (
            this TToolTip toolTip,
            int delay
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);
        Sure.NonNegative (delay);

        toolTip.InitialDelay = delay;

        return toolTip;
    }

    /// <summary>
    /// Задание баллонности для тултипа.
    /// </summary>
    public static TToolTip IsBalloon<TToolTip>
        (
            this TToolTip toolTip,
            bool isBalloon = true
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);

        toolTip.IsBalloon = isBalloon;

        return toolTip;
    }

    /// <summary>
    /// Подписка на событие <see cref="ToolTip.Draw"/>.
    /// </summary>
    public static TToolTip OnDraw<TToolTip>
        (
            this TToolTip toolTip,
            DrawToolTipEventHandler handler
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);
        Sure.NotNull (handler);

        toolTip.Draw += handler;

        return toolTip;
    }

    /// <summary>
    /// Подписка на событие <see cref="ToolTip.Popup"/>.
    /// </summary>
    public static TToolTip OnPopup<TToolTip>
        (
            this TToolTip toolTip,
            PopupEventHandler handler
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);
        Sure.NotNull (handler);

        toolTip.Popup += handler;

        return toolTip;
    }

    /// <summary>
    /// Задание задержки перед повторным показом для тултипа.
    /// </summary>
    public static TToolTip ReshowDelay<TToolTip>
        (
            this TToolTip toolTip,
            int delay
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);
        Sure.NonNegative (delay);

        toolTip.ReshowDelay = delay;

        return toolTip;
    }

    /// <summary>
    /// Задание постоянного показа для тултипа.
    /// </summary>
    public static TToolTip ShowAlways<TToolTip>
        (
            this TToolTip toolTip,
            bool showAlways = true
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);

        toolTip.ShowAlways = showAlways;

        return toolTip;
    }

    /// <summary>
    /// Задание удаление амперсандов для тултипа.
    /// </summary>
    public static TToolTip StripAmpersands<TToolTip>
        (
            this TToolTip toolTip,
            bool strip = true
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);

        toolTip.StripAmpersands = strip;

        return toolTip;
    }

    /// <summary>
    /// Задание произвольной метки для тултипа.
    /// </summary>
    public static TToolTip Tag<TToolTip>
        (
            this TToolTip toolTip,
            string tag
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);

        toolTip.Tag = tag;

        return toolTip;
    }

    /// <summary>
    /// Задание иконки для тултипа.
    /// </summary>
    public static TToolTip ToolTipIcon<TToolTip>
        (
            this TToolTip toolTip,
            ToolTipIcon icon
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);

        toolTip.ToolTipIcon = icon;

        return toolTip;
    }

    /// <summary>
    /// Задание заголовка для тултипа.
    /// </summary>
    public static TToolTip ToolTipTitle<TToolTip>
        (
            this TToolTip toolTip,
            string title
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);
        Sure.NotNullNorEmpty (title);

        toolTip.ToolTipTitle = title;

        return toolTip;
    }

    /// <summary>
    /// Разрешение анимации для тултипа.
    /// </summary>
    public static TToolTip UseAnimation<TToolTip>
        (
            this TToolTip toolTip,
            bool useAnimation = true
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);

        toolTip.UseAnimation = useAnimation;

        return toolTip;
    }

    /// <summary>
    /// Использование затухания для тултипа.
    /// </summary>
    public static TToolTip UseFading<TToolTip>
        (
            this TToolTip toolTip,
            bool useFading = true
        )
        where TToolTip : ToolTip
    {
        Sure.NotNull (toolTip);

        toolTip.UseFading = useFading;

        return toolTip;
    }

    #endregion
}
