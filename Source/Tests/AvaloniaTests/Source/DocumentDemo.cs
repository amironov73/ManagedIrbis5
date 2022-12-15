using Avalonia.Controls;
using Avalonia.Layout;

using AM.Avalonia.Documents;

using Avalonia;
using Avalonia.Controls.Documents;
using Avalonia.Media;

namespace AvaloniaTests;

public sealed class DocumentDemo
{
    public async void Show
        (
            Window owner
        )
    {
        var document = new TextBlock
        {
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness (10),
            Inlines = new InlineCollection
            {
                "У попа была (вы мне не поверите, но это чистейшая правда) ",
                "собака".Italic(),
                new LineBreak(),
                new Span().FontSize (16).With
                    (
                        "Он ее любил".Foreground (Brushes.Red).Bold(),
                        " (как это ни странно). ",
                        "Она съела кусок мяса".Background (Brushes.Yellow),
                        " (согласитесь, это вполне ожидаемо) ",
                        new LineBreak(),
                        "Он ее убил".Underline(),
                        " в землю закопал и надпись написал"
                    ),
                new Button
                {
                    Margin = new Thickness (5),
                    Content = "Кнопочка"
                },
                "А это текст после кнопочки",
                new LineBreak(),
                "Текст с новой строки"
            }
        };

        var window = new Window
        {
            Title = "Avalonia Document demo",
            Width = 400,
            Height = 150,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = document
        };

        await window.ShowDialog (owner);
    }
}
