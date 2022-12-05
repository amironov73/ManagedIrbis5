using AM.Avalonia.Controls;

using Avalonia.Controls;
using Avalonia.Layout;

namespace AvaloniaTests;

public sealed class PropertyGridDemo
{
    public sealed class Product
    {
        public string? Title { get; set; }

        public string? Category { get; set; }

        public string? Manufacturer { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }

    public async void Show
        (
            Window owner
        )
    {
        var product = new Product
        {
            Title = "Time machine",
            Category = "God's vehicles",
            Manufacturer = "Made in Heaven Corp",
            Price = 123.45m,
            Quantity = 1,
        };
        var propertyGrid = new PropertyGrid
        {
            SelectedObject = product
        };

        var window = new Window
        {
            Title = "Avalonia PropertyGrid demo",
            Width = 500,
            Height = 200,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = propertyGrid
        };

        await window.ShowDialog (owner);
    }

}
