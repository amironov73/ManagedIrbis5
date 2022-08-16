// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MainWindow.axaml.cs -- главное окно
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;

using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

using ManagedIrbis;
using ManagedIrbis.Avalonia;
using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace AvaloniaTests;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void FieldEditorButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var lines = new List<FieldLine>();
        for (var i = 0; i < 100; i++)
        {
            var line = new FieldLine
            {
                Item = new WorksheetItem
                {
                    Title = $"Какое-то поле {i + 101}"
                },
                Instance = new Field (i + 1, $"Значение поля {i + 101}")
            };
            lines.Add (line);
        }

        var window = new FieldEditorWindow();
        window.SetLines (lines);
        window.SetHint ("Тут какая-то подсказка");
        window.ShowDialog (this);
    }

    private async void LoginWindowButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var window = new LoginWindow();
        var result = await window.ShowDialog<bool> (this);
        Debug.WriteLine ($"Dialog result is {result}");
    }

    private async void AboutWindowButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var window = new AboutWindow();
        await window.ShowDialog<bool> (this);
    }

    private async void LabeledTextBoxButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var labeledTextBox = new LabeledTextBox
        {
            Label = "Это метка",
            Text = "А это текст"
        };

        var labeledComboBox = new LabeledComboBox
        {
            Label = "Это другая метка",
            Items = new[]
            {
                "Первый элемент",
                "Второй элемент",
                "Третий элемент",
                "Четвертый элемент",
                "Пятый элемент"
            },
            SelectedIndex = 1
        };

        var resultLabel1 = new Label();
        var resultLabel2 = new Label();

        var button = new Button
        {
            Content = "Нажми меня",
            HorizontalAlignment = HorizontalAlignment.Center
        };

        button.Click += (_, _) =>
        {
            resultLabel1.Content = labeledTextBox.Text;
            resultLabel2.Content = labeledComboBox.SelectedItem;
        };

        var window = new Window
        {
            Title = "LabeledTextBox control demo",
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness (20),
                Spacing = 10,
                Children =
                {
                    new Label { Content = "Это сверху" },
                    labeledTextBox,
                    labeledComboBox,
                    new Label { Content = "Это снизу" },
                    button,
                    resultLabel1,
                    resultLabel2
                }
            }
        };

        await window.ShowDialog (this);
    }

    private async void ColorComboBoxButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var window = new Window
        {
            Title = "ColorComboBox control demo",
            Width = 300,
            Height = 100,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new ColorComboBox
            {
                Margin = new Thickness (10),
                HorizontalAlignment = HorizontalAlignment.Stretch
            }
        };

        await window.ShowDialog<bool> (this);
    }

    private async void BusyStripeButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var stripe = new BusyStripe
        {
            Text = "Приложение занято чем-то важным",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsVisible = false,
            Height = 17
        };

        var button = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = "Активность"
        };
        button.Click += (_, _) => stripe.Active = !stripe.Active;

        var window = new Window
        {
            Title = "BusyStripe control demo",
            Width = 300,
            Height = 150,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children =
                {
                    stripe,
                    new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness (10),
                        Spacing = 10,
                        Children =
                        {
                            new Label
                            {
                                Content = "У попа была собака, он ее любил",
                                HorizontalAlignment = HorizontalAlignment.Center
                            },
                            new Label
                            {
                                Content = "Она съела кусок мяса, он ее убил",
                                HorizontalAlignment = HorizontalAlignment.Center
                            },
                            button
                        }
                    }
                }
            }
        };

        await window.ShowDialog<bool> (this);
    }
}
