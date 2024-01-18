using AM.Avalonia;
using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace AvaloniaTests;

internal sealed class ConsoleDemo
{
    public async void Show
        (
            Window owner
        )
    {
        var console = new ConsoleControl
        {
            EchoEnabled = true,

            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        var button = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Content = "Напиши"
        }
        .DockBottom();
        button.Click += (_, _) => console.Write ("0123456789");

        var window = new Window
        {
            Title = "Console demo",
            Width = 800,
            Height = 600,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new DockPanel
            {
                Children =
                {
                    button,
                    console
                }
            }
        };

        window.AttachDevTools();

        await window.ShowDialog (owner);
    }
}
