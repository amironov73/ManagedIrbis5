// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;

namespace AvaloniaTests;

internal sealed class GridUIDemo
{
    private sealed class MyData
    {
        public string? First { get; set; }

        public string? Second { get; set; }

        public string? Third { get; set; }
    }

    public async void Show
        (
            Window owner
        )
    {
        var data = new MyData
        {
            First = "First data",
            Second = "Second data",
            Third = "Third data"
        };

        var grid = new GridUI
            {
                Background = Brushes.Blue,
                Margin = new Thickness (5)
            }
            .AddColumn ()
            .AddTextBox (nameof (data.First), new Binding (nameof (data.First)))
            .AddTextBox (nameof (data.Second), new Binding (nameof (data.Second)))
            .AddVerticalSplitter()
            .AddColumn()
            .AddTextBox (nameof (data.Third), new Binding (nameof(data.Third)))
            .AddColumn (new ColumnDefinition (0.5, GridUnitType.Star))
            .AddButton ("Button", (_, _) => {})
            .Build();

        var window = new Window
        {
            Title = "GridUI demo",
            Width = 600,
            MinWidth = 600,
            Height = 400,
            MinHeight = 400,
            DataContext = data,
            Content = grid
        };
        window.AttachDevTools();

        await window.ShowDialog (owner);
    }
}
