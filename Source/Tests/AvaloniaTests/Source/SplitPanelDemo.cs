// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace AvaloniaTests;

internal sealed class SplitPanelDemo
{
    public async void Show
        (
            Window owner
        )
    {
        var panel = new SplitPanel();
        panel.First = new Label
        {
            Content = "First",
            Background = Brushes.Blue,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        panel.Second = new Label
        {
            Content = "Second",
            Background = Brushes.Green,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Center
        };

        var window = new Window
        {
            Title = "SplitPanel demo",
            Width = 600,
            MinWidth = 600,
            Height = 400,
            MinHeight = 400,
            Content = panel
        };
        window.AttachDevTools();

        await window.ShowDialog (owner);
    }
}
