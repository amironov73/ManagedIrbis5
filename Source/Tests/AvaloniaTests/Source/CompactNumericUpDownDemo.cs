using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#nullable enable

namespace AvaloniaTests;

public class CompactNumericUpDownDemo
{
    private class Dummy: ReactiveObject
    {
        [Reactive]
        public decimal? Value1 { get; set; }

        [Reactive]
        public decimal? Value2 { get; set; }

        [Reactive]
        public string? Value3 { get; set; }

        [Reactive]
        public string? Value4 { get; set; }
    }

    public async void Show
        (
            Window owner
        )
    {
        var model = new Dummy
        {
            Value1 = 123,
            Value2 = 321,
            Value3 = "То",
            Value4 = "Сё"
        };

        var toolStrip = new WrapPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = Brushes.LightGray,

            Children =
            {
                new CompactNumericUpDown
                {
                    Caption = "Первое",
                    // Background = Brushes.Yellow,
                    Margin = new Thickness (5),
                    [!CompactNumericUpDown.ValueProperty]
                        = new Binding (nameof (model.Value1), BindingMode.TwoWay)
                },

                new CompactNumericUpDown
                {
                    Caption = "Второе",
                    // Background = Brushes.LimeGreen,
                    Margin = new Thickness (5),
                    [!CompactNumericUpDown.ValueProperty]
                        = new Binding (nameof (model.Value2), BindingMode.TwoWay)
                },

                new CompactTextBox
                {
                    Caption = "Третье",
                    // Background = Brushes.LightCoral,
                    Margin = new Thickness (5),
                    [!CompactTextBox.ValueProperty]
                        = new Binding (nameof (model.Value3), BindingMode.TwoWay)
                },

                new CompactTextBox
                {
                    Caption = "Четвертое",
                    // Background = Brushes.Aquamarine,
                    Margin = new Thickness (5),
                    [!CompactTextBox.ValueProperty]
                        = new Binding (nameof (model.Value4), BindingMode.TwoWay)
                }
            }
        };

        var window = new Window
        {
            Title = "CompactNumericUpDown demo",
            Width = 600,
            Height = 400,
            DataContext = model,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new StackPanel
            {
                Children =
                {
                    toolStrip,
                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        [!ContentControl.ContentProperty]
                            = new Binding (nameof (model.Value1), BindingMode.TwoWay)
                    },
                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        [!ContentControl.ContentProperty]
                            = new Binding (nameof (model.Value2), BindingMode.TwoWay)
                    },
                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        [!ContentControl.ContentProperty]
                            = new Binding (nameof (model.Value3), BindingMode.TwoWay)
                    },
                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        [!ContentControl.ContentProperty]
                            = new Binding (nameof (model.Value4), BindingMode.TwoWay)
                    }
                }
            }
        };

        window.DataContext = model;
        window.AttachDevTools();

        await window.ShowDialog (owner);
    }

}
