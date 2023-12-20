using AM.Avalonia.Controls;

using Avalonia.Controls;
using Avalonia.Layout;

using NLog;

namespace AvaloniaTests;

internal sealed class ComfortableWindowDemo
{
    public async void Show
        (
            Window owner
        )
    {
        var content = new Label
        {
            Content = "Основной контент окна",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };

        var window = new ComfortableWindow
        {
            Title = "ComfortableWindow demo",
            Width = 600,
            MinWidth = 600,
            Height = 400,
            MinHeight = 400,
            MainContent = content
        };
        window.StatusBar.Children.Add (new StatusLabel ("Привет"));
        window.StatusBar.Children.Add (new StatusButton ("Нажми меня"));

        window.WriteLog (LogLevel.Info, "Как все запущено");

        await window.ShowDialog (owner);
    }
}
