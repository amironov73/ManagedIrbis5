// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;
using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ReactiveUI;

#endregion

namespace TheNude;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : ReactiveWindow<GalleryInfo>
{
    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.AttachDevTools();

        Task.Run (() => ThumbnailLoader.Instance.LoadThumbnails())
            .NotUsed();

        Title = "Клиент TheNude.com";
        Width = MinWidth = 700;
        Height = MinHeight = 400;

        this.SetWindowIcon ("nude.ico");
        var busyStripe = new BusyStripe
            {
                IsVisible = false,
                Height = 20,
                Text = "Обращение к серверу",
            }
            .DockTop();
        var statusBar = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness (0, 1, 0, 0),
                Padding = new Thickness (5),
                Background = Brushes.AliceBlue,
                Child = new StackPanel
                {
                    Spacing = 5,
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new Label
                        {
                            Foreground = Brushes.Black,
                            [!ContentProperty] = new Binding (nameof (ViewModel.ModelCount))
                            {
                                StringFormat = "Всего найдено: {0}"
                            }
                        }
                    }
                }
            }
            .DockBottom();

        DataContext = new GalleryInfo { Busy = busyStripe };
        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness (0,0, 0, 1),

                        Child =
                            new StackPanel
                            {
                                Spacing = 5,
                                Margin = new Thickness (5),
                                Orientation = Orientation.Horizontal,
                                HorizontalAlignment = HorizontalAlignment.Stretch,

                                Children =
                                {
                                    new Label
                                    {
                                        Content = "Модель",
                                        VerticalContentAlignment = VerticalAlignment.Center
                                    },

                                    new TextBox
                                    {
                                        Width = 200,
                                        [!TextBox.TextProperty] = AvaloniaUtility.MakeBinding<string>
                                            (
                                                nameof (ViewModel.Name),
                                                it => ((GalleryInfo) it).Name,
                                                (it, value) => ((GalleryInfo) it).Name = (string?) value
                                            )
                                    },

                                    new CheckBox
                                    {
                                        Content = "точно",
                                        [!ToggleButton.IsCheckedProperty] = AvaloniaUtility.MakeBinding<bool>
                                            (
                                                nameof (ViewModel.Exact),
                                                it => ((GalleryInfo) it).Exact,
                                                (it, value) => ((GalleryInfo) it).Exact = (bool) value!
                                            )
                                    },

                                    new Button
                                    {
                                        Content = "Найти",
                                        IsDefault = true,
                                        Command = ReactiveCommand.Create (ViewModel!.Search)
                                    }
                                }
                            }
                    }
                    .DockTop(),

                busyStripe,
                statusBar,

                new ListBox
                {
                    Margin = new Thickness (5),
                    ItemsPanel = new FuncTemplate<Panel?>
                        (
                            () => new WrapPanel { Orientation = Orientation.Horizontal }
                        ),
                    ItemTemplate = new FuncDataTemplate<ModelInfo> ((_, _) => new ModelControl()),
                    [!ItemsControl.ItemsSourceProperty] = new Binding (nameof (ViewModel.Models))
                },
            }
        };
    }

    // /// <inheritdoc cref="Window.OnClosing"/>
    // protected override void OnClosing
    //     (
    //         CancelEventArgs eventArgs
    //     )
    // {
    //     base.OnClosing (eventArgs);
    //
    //     ThumbnailLoader.Instance.SaveThumbnails();
    // }

    #endregion

    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
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
