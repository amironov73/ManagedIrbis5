using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using ReactiveUI;

#nullable enable

namespace AvaloniaTests;

public class ProgressStripeDemo
{
    public ProgressStripeDemo()
    {
        _stripe = new ProgressStripe
        {
            IsVisible = false,
            Height = 4,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
    }

    public async void Show
        (
            Window owner
        )
    {
        var button = new Button
        {
            Content = "Запуск действий",
            Command = ReactiveCommand.Create (StartProgress)
        };

        var window = new Window
        {
            Title = "ProgressStripe demo",
            Width = 600,
            Height = 400,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new StackPanel
            {
                Children =
                {
                    _stripe,
                    new StackPanel
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,

                        Children =
                        {
                            new Label { Content = "Нажми на кнопку" },
                            button
                        }
                    }
                }
            }
        };

        window.AttachDevTools();

        await window.ShowDialog (owner);
    }

    private readonly ProgressStripe _stripe;

    private void SomeActivity()
    {
        _stripe.SetProgress (0);
        _stripe.SetActive (true);

        for (double progress = 0; progress < 100; progress++)
        {
            _stripe.SetProgress (progress);
            Thread.Sleep (10);
        }

        _stripe.SetActive (false);
    }

    private void StartProgress()
    {
        Task.Run (SomeActivity).FireAndForget();
    }
}
