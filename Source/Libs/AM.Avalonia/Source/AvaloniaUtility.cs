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
using System.Diagnostics;
using System.IO;
using System.Reflection;

using AM.Text.Output;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

#endregion

#nullable enable

namespace AM.Avalonia;

/// <summary>
/// Полезные расширения для Avalonia UI.
/// </summary>
public static class AvaloniaUtility
{
    #region Public methods

    /// <summary>
    /// Выполнение произвольных побочных действий.
    /// </summary>
    public static TControl Also<TControl>
        (
            this TControl control,
            Action<TControl> action
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (action);

        action (control);

        return control;
    }

    /// <summary>
    /// Присваивание указанной переменной.
    /// </summary>
    public static TControl Assign<TControl>
        (
            this TControl control,
            out TControl variable
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        variable = control;

        return control;
    }

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
    /// Центрирование контрола по горизонтали.
    /// </summary>
    public static T CenterHorizontally<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.HorizontalAlignment = HorizontalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Центрирование контрола по вертикали.
    /// </summary>
    public static T CenterVertically<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.VerticalAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Размещение контрола внизу в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockBottom<T>
        (
            this T control

        )
        where T: Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Bottom);

        return control;
    }

    /// <summary>
    /// Размещение контрола слева в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockLeft<T>
        (
            this T control

        )
        where T: Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Left);

        return control;
    }

    /// <summary>
    /// Размещение контрола справа в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockRight<T>
        (
            this T control

        )
        where T: Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Right);

        return control;
    }

    /// <summary>
    /// Размещение контрола наверху в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockTop<T>
        (
            this T control

        )
        where T: Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Top);

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
        control.HorizontalContentAlignment = HorizontalAlignment.Left;
        control.VerticalContentAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Установка обработчика событий нажатия на кнопку.
    /// </summary>
    public static T OnClick<T>
        (
            this T button,
            EventHandler<RoutedEventArgs> handler
        )
        where T: Button
    {
        Sure.NotNull (button);
        Sure.NotNull (handler);

        button.Click += handler;

        return button;
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

    /// <summary>
    /// Добавление в заголовок окна информации о версии сборки.
    /// </summary>
    public static void ShowVersionInfoInTitle
        (
            this Window window
        )
    {
        Sure.NotNull (window);

        var assembly = Assembly.GetEntryAssembly();
        var location = assembly?.Location;
        if (string.IsNullOrEmpty (location))
        {
            // TODO: в single-exe-application .Location возвращает string.Empty
            // consider using the AppContext.BaseDirectory
            return;
        }

        // TODO: в single-exe-application .Location возвращает string.Empty
        // consider using the AppContext.BaseDirectory
        var fvi = FileVersionInfo.GetVersionInfo (location);
        var fi = new FileInfo (location);

        window.Title += $": version {fvi.FileVersion} from {fi.LastWriteTime.ToShortDateString()}";
    }

    /// <summary>
    /// Print system information in abstract output.
    /// </summary>
    public static void PrintSystemInformation
        (
            this AbstractOutput? output
        )
    {
        if (output is not null)
        {
            output.WriteLine
                (
                    "OS version: {0}",
                    Environment.OSVersion
                );
            output.WriteLine
                (
                    "Framework version: {0}",
                    Environment.Version
                );
            var assembly = Assembly.GetEntryAssembly();
            var vi = assembly?.GetName().Version;
            if (assembly?.Location is null)
            {
                // TODO: в single-exe-application .Location возвращает string.Empty
                // consider using the AppContext.BaseDirectory
                return;
            }

            // TODO: в single-exe-application .Location возвращает string.Empty
            // consider using the AppContext.BaseDirectory
            var fi = new FileInfo (assembly.Location);
            output.WriteLine
                (
                    "Application version: {0} ({1})",
                    vi.ToVisibleString(),
                    fi.LastWriteTime.ToShortDateString()
                );
            output.WriteLine
                (
                    "Memory: {0} Mb",
                    GC.GetTotalMemory (false) / 1024
                );
        }
    }

    /// <summary>
    /// Растягивание контрола по горизонтали и по вертикали.
    /// </summary>
    public static T Stretch<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);
        control.HorizontalAlignment = HorizontalAlignment.Stretch;
        control.VerticalAlignment = VerticalAlignment.Stretch;

        return control;
    }

    /// <summary>
    /// Растягивание контрола по горизонтали.
    /// </summary>
    public static T StretchHorizontally<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);
        control.HorizontalAlignment = HorizontalAlignment.Stretch;

        return control;
    }

    /// <summary>
    /// Растягивание контрола по вертикали.
    /// </summary>
    public static T StretchVertically<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);
        control.VerticalAlignment = VerticalAlignment.Stretch;

        return control;
    }

    /// <summary>
    /// Добавление дочерних контролов в панель
    /// </summary>
    public static T WithChildren<T>
        (
            this T panel,
            params Control[] children
        )
        where T: Panel
    {
        Sure.NotNull (panel);

        panel.Children.AddRange (children);

        return panel;
    }

    #endregion
}
