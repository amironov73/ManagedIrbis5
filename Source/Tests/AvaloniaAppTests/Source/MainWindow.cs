// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Themes.Fluent;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AvaloniaAppTests;

/// <summary>
/// Главное окно приложения
/// </summary>
public sealed class MainWindow
    : Window
{
    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public MainWindow()
    {
        Title = "Тестовое приложение Avalonia";
        Width = 400;
        Height = 250;

        Content = new StackPanel()
            .StretchHorizontally()
            .CenterVertically()
            .Also (panel => panel.Spacing = 5)
            .WithChildren
                (
                    new Label { Content = "Метка № 1" }
                        .CenterControl(),

                    new Button { Content = "Кнопка № 1" }
                        .CenterControl()
                        .OnClick ((_, _) =>
                        {
                            Magna.Application.Logger.LogInformation ("Кнопка нажата {Moment}", DateTime.Now);
                            Magna.Logger.LogInformation ("Нажата кнопка {Moment}", DateTime.Now);

                        }),

                    new Button { Content = "Светлая тема" }
                        .CenterControl()
                        .OnClick ((_, _) =>
                        {
                            // var lightTheme = new FluentTheme (new Uri ("avares://Avalonia.Themes.Fluent/FluentLight.xaml"));
                            // var styles = Application.Current!.Styles;
                            // styles.Clear();
                            // styles.Add (lightTheme);
                            // InvalidateStyles();
                            // InvalidateVisual();
                        }),

                    new Button { Content = "Темная тема" }
                        .CenterControl()
                        .OnClick ((_, _) =>
                        {
                            // var darkTheme = new FluentTheme (new Uri ("avares://Avalonia.Themes.Fluent/FluentDark.xaml"));
                            // var styles = Application.Current!.Styles;
                            // styles.Clear();
                            // styles.Add (darkTheme);
                            // InvalidateStyles();
                            // InvalidateVisual();
                        })
                );
    }

    #endregion
}
