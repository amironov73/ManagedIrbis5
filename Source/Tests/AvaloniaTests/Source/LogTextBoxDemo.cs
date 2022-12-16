using System.Threading.Tasks;

using AM.Avalonia;
using AM.Avalonia.Controls;

using Avalonia.Controls;
using Avalonia.Layout;

using AM.Logging;

using Avalonia;
using Avalonia.Media;

using NLog;

using ReactiveUI;

namespace AvaloniaTests;

public sealed class LogTextBoxDemo
{
    public async void Show
        (
            Window owner
        )
    {
        MagnaTarget.AddToNlogConfiguration();


        var window = new Window
        {
            Title = "Avalonia Logging demo",
            Width = 400,
            Height = 200,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new DockPanel
            {
                //HorizontalAlignment = HorizontalAlignment.Stretch,
                //VerticalAlignment = VerticalAlignment.Stretch,
                Children =
                {
                    new Label
                    {
                        Content = "Нажми на кнопку",
                        HorizontalAlignment = HorizontalAlignment.Center
                    }
                    .DockTop(),

                    new Button
                    {
                        Content = "Получишь результат",
                        Margin = new Thickness (10),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Command = ReactiveCommand.CreateFromTask (DoSomething)
                    }
                    .DockTop(),

                    new LogTextBox
                    {
                        FontFamily = new FontFamily ("Courier")
                    }
                }
            }
        };

        await window.ShowDialog (owner);

        async Task DoSomething()
        {
            var logger = LogManager.GetCurrentClassLogger();

            for (var i = 0; i < 10; i++)
            {
                logger.Log (LogLevel.Info, "Попытка № {Attempt}", i + 1);

                await Task.Delay (100);
            }

            logger.Log (LogLevel.Info, "Успешно завершено!");
        }

    }
}
