using System.Threading.Tasks;

using AM.Avalonia;
using AM.Avalonia.Controls;
using AM.Logging;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

using NLog;

using ReactiveUI;

namespace AvaloniaTests;

public class ProgressRingDemo
{
    public async void Show
        (
            Window owner
        )
    {
        var ring = new ProgressRing
        {
            Width = 100,
            Height = 100,
            IsActive = true
        };

        var button = new Button
        {
            Content = "Переключение",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        button.Click += (_, _) =>
        {
            ring.IsActive = !ring.IsActive;
        };

        var window = new Window
        {
            Title = "ProgressRing demo",
            Width = 600,
            Height = 400,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new StackPanel
            {
                Children =
                {
                    ring,
                    button
                }
            }
        };

        await window.ShowDialog (owner);
    }
}
