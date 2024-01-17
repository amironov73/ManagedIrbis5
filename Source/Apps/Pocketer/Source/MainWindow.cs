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

namespace Pocketer;

/// <summary>
/// Главное окно приложения.
/// </summary>
internal sealed class MainWindow
    : ReactiveWindow<PocketerModel>
{
    #region Window members

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.AttachDevTools();
        Title = "Клиент ИРБИС";
        Width = MinWidth = 700;
        Height = MinHeight = 400;

        this.SetWindowIcon ("barsik.ico");

        DataContext = new PocketerModel();
        var model = ViewModel!;

        var busyStripe = new BusyStripe
            {
                IsVisible = false,
                Height = 20,
                Text = "Обращение к серверу",
            }
            .DockTop();
        model.Busy = busyStripe;

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
                            Content = "Тут будет контент"
                        }
                    }
                }
            }
            .DockBottom();

        var barsikPanel = new DockPanel
            {
                Children =
                {
                    new Button
                    {
                        Content = "Выполнить"
                    }
                    .DockRight(),

                    new TextBox
                    {
                        Text = "println (\"Hello\")",
                        [!TextBox.TextProperty] = model.SearchExpressionBinding()
                    }
                }
            }
            .DockBottom();

        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                busyStripe,
                statusBar,
                barsikPanel,

                new Grid
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    RowDefinitions = RowDefinitions.Parse ("Auto, *"),
                    ColumnDefinitions = ColumnDefinitions.Parse ("*"),

                    Children =
                    {
                        new StackPanel
                        {
                            Spacing = 5,
                            Margin = new Thickness (5),
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Stretch,

                            Children =
                            {
                                new TextBox
                                {
                                    Width = 200,
                                    [!TextBox.TextProperty] = model.SearchExpressionBinding()
                                },
                                new Button
                                {
                                    Content = "Найти",
                                    IsDefault = true,
                                    Command = ReactiveCommand.Create (ViewModel!.Search)
                                }

                            }
                        },

                        new ListBox
                        {
                            [Grid.RowProperty] = 1,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch
                        }
                    }
                }
            }
        };
    }

    #endregion
}
