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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AvaloniaApp;

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

        Content = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 5,
                Children =
                {
                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Content = "Метка № 1"
                    },

                    new Button
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Content = "Кнопка № 1"
                    },

                    new Button
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Content = "Кнопка № 2"
                    }
                }
            };
    }

    #endregion
}
