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

using AM.Avalonia;
using AM.Avalonia.AppServices;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using ReactiveUI;

#endregion

#nullable enable

namespace StableErection;

/// <summary>
/// Главное окно приложения
/// </summary>
public sealed class MainWindow
    : Window
{
    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        this.AttachDevTools();

        Title = "Книжные богини Avalonia";
        Width = MinWidth = 400;
        Height = MinHeight = 600;

        var goddess1 = this.LoadBitmapFromAssets ("Assets/goddess1.jpg");
        var goddess2 = this.LoadBitmapFromAssets ("Assets/goddess2.jpg");
        var goddess3 = this.LoadBitmapFromAssets ("Assets/goddess3.jpg");
        var goddess4 = this.LoadBitmapFromAssets ("Assets/goddess4.jpg");

        var carousel = new Carousel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            // PageTransition = new CompositePageTransition
            // {
            //     PageTransitions =
            //     {
            //         new PageSlide
            //         {
            //             Duration = TimeSpan.FromMilliseconds (1500),
            //             Orientation = PageSlide.SlideAxis.Horizontal
            //         }
            //     }
            // },
            Items =
            {
                new Image { Source = goddess1 },
                new Image { Source = goddess2 },
                new Image { Source = goddess3 },
                new Image { Source = goddess4 }
            }
        };

        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition (GridLength.Star),
                        new ColumnDefinition (GridLength.Star)
                    },
                    [DockPanel.DockProperty] = Dock.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Children =
                    {
                        CreateButton (0, "<<", () => carousel.Previous()),
                        CreateButton (1, ">>", () => carousel.Next()),
                    }
                },
                carousel
            }
        };

        Button CreateButton (int column, string content, Action action) => new()
        {
            [Grid.ColumnProperty] = column,
            [Grid.RowProperty] = 0,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            Command = ReactiveCommand.Create (action),
            Content = content
        };
    }

    #endregion

    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        DesktopApplication
            .Run<MainWindow> (args);
    }

    #endregion
}
