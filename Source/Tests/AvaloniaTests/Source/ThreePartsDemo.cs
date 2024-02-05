// ReSharper disable CheckNamespace

using AM.Avalonia.Controls;

using Avalonia.Controls;
using Avalonia.Layout;

namespace AvaloniaTests;

internal sealed class ThreePartsDemo
{
    public async void Show
        (
            Window owner
        )
    {
        var toolbar = new Toolbar()
            .AddLabel ("Это панель инструментов")
            .AddButton ("Кнопка", (_, _) => {});

        var statusBar = new StatusBar()
            .AddLabel ("Это панель статуса")
            .AddButton ("Кнопка", (_, _) => {});

        var mainArea = new Label
        {
            Content = "Основной контент окна",
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };

        var window = new Window
        {
            Title = "ThreePartLayout demo",
            Width = 600,
            MinWidth = 600,
            Height = 400,
            MinHeight = 400,
            Content = new ThreePartLayout
                (
                    toolbar,
                    mainArea,
                    statusBar
                )
        };

        await window.ShowDialog (owner);
    }
}
