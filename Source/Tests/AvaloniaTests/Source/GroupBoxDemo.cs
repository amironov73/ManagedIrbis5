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

public sealed class GroupBoxDemo
{
    public async void Show
        (
            Window owner
        )
    {
        MagnaTarget.AddToNlogConfiguration();

        var window = new Window
        {
            Title = "GroupBox demo",
            Width = 600,
            Height = 400,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new StackPanel()
            {
                Children =
                {
                    new Label()
                    {
                        Background = Brushes.Beige,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Content = "Чисто для контроля"
                    },

                    new GroupBox
                    {
                        Header = "First group",
                        Width = 600,
                        Height = 200,
                        Background = Brushes.Aqua,

                        Content = new StackPanel
                        {
                            Background = Brushes.Lavender,
                            Children =
                            {
                                new TextBlock { Text = "Первая строчка" },
                                new TextBlock { Text = "Вторая строчка" },
                                new TextBlock { Text = "Третья строчка" },
                            }
                        }
                    }
                }
            }
        };

        await window.ShowDialog (owner);
    }
}
