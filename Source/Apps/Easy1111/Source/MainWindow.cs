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
using System.Reactive.Linq;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace Easy1111;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : ReactiveWindow<ViewModel>
{
    #region Private members

    private Control WrapControl
        (
            string title,
            int column,
            int row,
            Control control
        )
    {
        var wrapper = new StackPanel
        {
            [Grid.ColumnProperty] = column,
            [Grid.RowProperty] = row,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness (5),

            Children =
            {
                new Label
                {
                    FontSize = 10.0,
                    Foreground = Brushes.Coral,
                    Content = title,
                },

                control
            }
        };


        return wrapper;
    }

    #endregion

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = "Легко и просто генерируем";
        Width = MinWidth = 800;
        Height = MinHeight = 600;

        DataContext = new ViewModel { Window = this };

        Content = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            RowDefinitions = RowDefinitions.Parse ("Auto,Auto,Auto,Auto,*"),
            ColumnDefinitions = ColumnDefinitions.Parse ("*,*"),

            Children =
            {
                WrapControl
                    (
                        "Позитивная подсказка", 0, 0,
                        new TextBox
                        {
                            AcceptsReturn = true,
                            Height = 50,
                            [!TextBox.TextProperty] = new Binding (nameof (ViewModel.Positive))
                        }
                    ),

                WrapControl
                    (
                        "Негативная подсказка", 0, 1,
                        new TextBox
                        {
                            AcceptsReturn = true,
                            Height = 50,
                            [!TextBox.TextProperty] = new Binding (nameof (ViewModel.Negative))
                        }
                    ),

                new StackPanel
                {
                    [Grid.ColumnProperty] = 0,
                    [Grid.RowProperty] = 2,
                    Spacing = 5,
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Stretch,

                    Children =
                    {
                        WrapControl
                            (
                                "Ширина", 0, 0,
                                new NumericUpDown
                                {
                                    Minimum = 64,
                                    Maximum = 2048,
                                    Increment = 64,
                                    FormatString = "0",
                                    [!NumericUpDown.ValueProperty] = new Binding (nameof (ViewModel.Width))
                                }
                            ),

                        WrapControl
                            (
                                "Высота", 0, 0,
                                new NumericUpDown
                                {
                                    Minimum = 64,
                                    Maximum = 2048,
                                    Increment = 64,
                                    FormatString = "0",
                                    [!NumericUpDown.ValueProperty] = new Binding (nameof (ViewModel.Height))
                                }
                            ),
                    }
                },

                WrapControl
                    (
                        "CFG", 0, 3,
                        new Grid
                        {
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            ColumnDefinitions = ColumnDefinitions.Parse ("*,Auto"),
                            RowDefinitions = RowDefinitions.Parse ("*"),
                            Children =
                            {
                                new Slider
                                {
                                    [Grid.RowProperty] = 0,
                                    [Grid.ColumnProperty] = 0,
                                    Minimum = 1,
                                    Maximum = 32,
                                    Margin = new Thickness (5),
                                    [!RangeBase.ValueProperty] = new Binding (nameof (ViewModel.Cfg))
                                },

                                new Label
                                {
                                    [Grid.RowProperty] = 0,
                                    [Grid.ColumnProperty] = 1,
                                    [!ContentProperty] = new Binding (nameof (ViewModel.Cfg))
                                    {
                                        StringFormat = "0.0"
                                    }
                                }
                            }
                        }
                    )
            }
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
