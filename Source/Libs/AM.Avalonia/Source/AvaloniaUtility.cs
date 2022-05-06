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
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* AvaloniaUtility.cs -- полезные расширения для Avalonia UI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

#endregion

#nullable enable

namespace AM.Avalonia.Source;

/// <summary>
/// Полезные расширения для Avalonia UI.
/// </summary>
public static class AvaloniaUtility
{
    #region Public methods

    /// <summary>
    /// Установка жирного начертания для текстового блока.
    /// </summary>
    public static T Bold<T>
        (
            this T block,
            bool bold = true
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.FontWeight = bold ? FontWeight.Bold : FontWeight.Regular;

        return block;
    }

    /// <summary>
    /// Центрирование содержимого контрола.
    /// </summary>
    public static T CenterContent<T>
        (
            this T control
        )
        where T: IContentControl
    {
        ArgumentNullException.ThrowIfNull (control);

        control.HorizontalContentAlignment = HorizontalAlignment.Center;
        control.VerticalContentAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Центрирование самого контрола.
    /// </summary>
    public static T CenterControl<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.HorizontalAlignment = HorizontalAlignment.Center;
        control.VerticalAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Установка наклонного начертания для текстового блока.
    /// </summary>
    public static T Italic<T>
        (
            this T block,
            bool italic = true
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.FontStyle = italic ? FontStyle.Italic : FontStyle.Normal;

        return block;
    }

    /// <summary>
    /// Прижатие содержимого контрола влево по центру.
    /// </summary>
    public static T LeftContent<T>
        (
            this T control
        )
        where T: IContentControl
    {
        ArgumentNullException.ThrowIfNull (control);

        control.HorizontalContentAlignment = HorizontalAlignment.Left;
        control.VerticalContentAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            Thickness thickness
        )
        where T: ContentControl
    {
        Sure.NotNull (control);

        control.Margin = thickness;

        return control;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double thickness
        )
        where T: ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (thickness);

        return control;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double horizontal,
            double vertical
        )
        where T: ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (horizontal, vertical);

        return control;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double left,
            double top,
            double right,
            double bottom
        )
        where T: ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (left, top, right, bottom);

        return control;
    }

    /// <summary>
    /// Установка полей вокруг текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            Thickness thickness
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.Padding = thickness;

        return block;
    }

    /// <summary>
    /// Установка полей вокруг текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double thickness
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (thickness);

        return block;
    }

    /// <summary>
    /// Установка полей вокруг текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double horizontal,
            double vertical
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (horizontal, vertical);

        return block;
    }

    /// <summary>
    /// Установка полей вокруг текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double left,
            double top,
            double right,
            double bottom
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (left, top, right, bottom);

        return block;
    }

    #endregion
}
