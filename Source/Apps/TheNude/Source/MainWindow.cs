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

using AM.Avalonia;
using AM.Avalonia.AppServices;

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

#nullable enable

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

        Title = "Клиент TheNude.com";
        Width = MinWidth = 600;
        Height = MinHeight = 400;

        this.SetWindowIcon ("nude.ico");
        DataContext = new GalleryInfo();
        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                new StackPanel
                    {
                        Spacing = 5,
                        Margin = new Thickness (5),
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Center,

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
                                [!TextBox.TextProperty] = new Binding (nameof (ViewModel.Name))
                            },

                            new CheckBox
                            {
                                Content = "точно",
                                [!ToggleButton.IsCheckedProperty] = new Binding (nameof (ViewModel.Exact))
                            },

                            new Button
                            {
                                Content = "Найти",
                                Command = ReactiveCommand.Create (ViewModel!.Search)
                            }
                        }
                    }
                    .DockTop(),

                new ListBox
                {
                    Margin = new Thickness (5),

                    ItemsPanel = new FuncTemplate<IPanel>
                    (
                        () => new WrapPanel { Orientation = Orientation.Horizontal }
                    ),

                    ItemTemplate = new FuncDataTemplate<ModelInfo> ((_, _) =>
                    {
                        var border = new Border
                        {
                            BorderThickness = new Thickness (1),
                            BorderBrush = Brushes.Blue,
                            Margin = new Thickness (5),
                            Child = new ModelControl()
                        };

                        return border;
                    }),

                    [!ItemsRepeater.ItemsProperty] = new Binding (nameof (ViewModel.Models))
                },
            }
        };
    }

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
