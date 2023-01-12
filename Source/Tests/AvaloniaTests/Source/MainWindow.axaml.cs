// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MainWindow.axaml.cs -- главное окно
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using AM;
using AM.Avalonia;
using AM.Avalonia.Controls;
using AM.Avalonia.Dialogs;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

using ManagedIrbis;
using ManagedIrbis.Avalonia;
using ManagedIrbis.Workspace;

using ReactiveUI;

#endregion

#nullable enable

namespace AvaloniaTests;

public partial class MainWindow
    : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void FieldEditorButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var lines = new List<FieldLine>();
        for (var i = 0; i < 100; i++)
        {
            var line = new FieldLine
            {
                Item = new WorksheetItem
                {
                    Title = $"Какое-то поле {i + 101}"
                },
                Instance = new Field (i + 1, $"Значение поля {i + 101}")
            };
            lines.Add (line);
        }

        var window = new FieldEditorWindow();
        window.SetLines (lines);
        window.SetHint ("Тут какая-то подсказка");
        window.ShowDialog (this);
    }

    private async void LoginWindowButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var window = new LoginWindow
        {
            Title = "Подключение к серверу 1.1.1.1"
        };
        var result = await window.ShowDialog<bool> (this);
        Debug.WriteLine ($"Dialog result is {result}");
    }

    private async void AboutWindowButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var window = new AboutDialog();
        await window.ShowDialog<bool> (this);
    }

    private async void LabeledTextBoxButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var labeledTextBox = new LabeledTextBox
        {
            Label = "Это метка",
            Text = "А это текст"
        };

        var labeledComboBox = new LabeledComboBox
        {
            Label = "Это другая метка",
            Items = new[]
            {
                "Первый элемент",
                "Второй элемент",
                "Третий элемент",
                "Четвертый элемент",
                "Пятый элемент"
            },
            SelectedIndex = 1
        };

        var resultLabel1 = new Label();
        var resultLabel2 = new Label();

        var button = new Button
        {
            Content = "Нажми меня",
            HorizontalAlignment = HorizontalAlignment.Center
        };

        button.Click += (_, _) =>
        {
            resultLabel1.Content = labeledTextBox.Text;
            resultLabel2.Content = labeledComboBox.SelectedItem;
        };

        var window = new Window
        {
            Title = "LabeledTextBox control demo",
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness (20),
                Spacing = 10,
                Children =
                {
                    new Label { Content = "Это сверху" },
                    labeledTextBox,
                    labeledComboBox,
                    new Label { Content = "Это снизу" },
                    button,
                    resultLabel1,
                    resultLabel2
                }
            }
        };

        await window.ShowDialog (this);
    }

    private async void ColorComboBoxButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var window = new Window
        {
            Title = "ColorComboBox control demo",
            Width = 300,
            Height = 100,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new ColorComboBox
            {
                Margin = new Thickness (10),
                HorizontalAlignment = HorizontalAlignment.Stretch
            }
        };

        await window.ShowDialog<bool> (this);
    }

    private async void BusyStripeButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var stripe = new BusyStripe
        {
            Text = "Приложение занято чем-то важным",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsVisible = false,
            Height = 17
        };

        var button = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = "Активность"
        };
        button.Click += (_, _) => stripe.Active = !stripe.Active;

        var window = new Window
        {
            Title = "BusyStripe control demo",
            Width = 300,
            Height = 150,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children =
                {
                    stripe,
                    new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness (10),
                        Spacing = 10,
                        Children =
                        {
                            new Label
                            {
                                Content = "У попа была собака, он ее любил",
                                HorizontalAlignment = HorizontalAlignment.Center
                            },
                            new Label
                            {
                                Content = "Она съела кусок мяса, он ее убил",
                                HorizontalAlignment = HorizontalAlignment.Center
                            },
                            button
                        }
                    }
                }
            }
        };

        await window.ShowDialog<bool> (this);
    }

    private async void DriveComboBoxButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var window = new Window
        {
            Title = "DriveComboBox control demo",
            Width = 300,
            Height = 150,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness (10),
                Children =
                {
                    new DriveComboBox
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    }
                }
            }
        };

        await window.ShowDialog<bool> (this);
    }

    private async void InputDialogButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var data = new InputData
        {
            Title = "Заголовок окна",
            Prompt = "Это просто тест",
            Value = "Начальное значение"
        };
        var result = await InputDialog.Query (this, data);
        Debug.WriteLine ("Result is: " + result);
        Debug.WriteLine ("Value is: " + data.Value);
    }

    private async void ExceptionDialogButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        try
        {
            throw new ArsMagnaException ("Учебная тревога");
        }
        catch (Exception exception)
        {
            await ExceptionDialog.Show (this, exception);
        }

    }

    private async void ButtonedTextBoxButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var counter = 0;
        var messageLabel = new Label();

        var textBox = new ButtonedTextBox
        {
            Text = "text in a box",
            Caption = "..."
        };
        textBox.ButtonClick += (o, args) =>
        {
            var box = (ButtonedTextBox) o.ThrowIfNull();
            messageLabel.Content = $"Нажато: {++counter}, текст: {box.Text}";
        };

        var window = new Window
        {
            Title = "ButtonedTextBox control demo",
            Width = 300,
            Height = 150,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness (10),
                Children =
                {
                    messageLabel,
                    textBox
                }
            }
        };

        await window.ShowDialog (this);
    }

    private async void AnalogClockButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var clock = new AnalogClock
        {
            Width = 200,
            Height = 200
        };

        var window = new Window
        {
            Title = "AnalogClock control demo",
            Width = 300,
            Height = 220,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness (10),
                Children =
                {
                    clock
                }
            }
        };

        await window.ShowDialog (this);
    }

    private async void BusyWindowButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var button = new Button
        {
            Content = "Жмите!"
        };

        var window = new BusyWindow
        {
            Header = "Ждите ответа!",
            Title = "BusyWindow demo",
            Width = 300,
            Height = 220,
            Content = new DockPanel
            {
                Margin = new Thickness (10),
                Children =
                {
                    new Label { Content = "До кнопки" }
                        .DockTop()
                        .CenterContent(),

                    new TextBox
                        {
                            Text = "У попа была собака",
                            Width = 200,
                        }
                        .DockTop()
                        .CenterHorizontally(),

                    new Label { Content = "После кнопки" }
                        .DockBottom()
                        .CenterContent(),

                    button.CenterHorizontally(),
                }
            }
        };

        button.Command = ReactiveCommand.CreateFromTask (async Task() =>
        {
            await window.Run (async Task() => await Task.Delay (1_500));
        });


        await window.ShowDialog (this);
    }

    private async void PlainTextEditorDialogButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        const string title = "Как тебе такое, Илон Маск?";
        const string text = "В целом, конечно, сложившаяся структура организации " +
            "обеспечивает широкому кругу (специалистов) участие в формировании " +
            "направлений прогрессивного развития. Также как, консультация с широким " +
            "активом выявляет срочную потребность новых предложений.\n" +
            "Задача организации, в особенности же постоянное информационно-пропагандистское " +
            "обеспечение нашей деятельности играет важную роль в формировании форм развития. " +
            "С другой стороны консультация с широким активом обеспечивает широкому кругу " +
            "(специалистов) участие в формировании соответствующий условий активизации. " +
            "Значимость этих проблем настолько очевидна, что реализация намеченных плановых " +
            "заданий в значительной степени обуславливает создание направлений " +
            "прогрессивного развития.\n" +
            "Разнообразный и богатый опыт постоянный количественный рост и сфера нашей активности " +
            "обеспечивает широкому кругу (специалистов) участие в формировании дальнейших " +
            "направлений развития. Товарищи! сложившаяся структура организации обеспечивает " +
            "широкому кругу (специалистов) участие в формировании модели развития. " +
            "Повседневная практика показывает, что постоянное информационно-пропагандистское " +
            "обеспечение нашей деятельности играет важную роль в формировании направлений " +
            "прогрессивного развития. Задача организации, в особенности же консультация " +
            "с широким активом в значительной степени обуславливает создание дальнейших " +
            "направлений развития. Значимость этих проблем настолько очевидна, что рамки " +
            "и место обучения кадров позволяет выполнять важные задания по разработке " +
            "позиций, занимаемых участниками в отношении поставленных задач. Значимость " +
            "этих проблем настолько очевидна, что сложившаяся структура организации " +
            "обеспечивает широкому кругу (специалистов) участие в формировании форм развития.";

        await PlainTextEditorDialog.Show (this, title, text);
    }

    private async void PlainTextViewerDialogButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        const string title = "Как тебе такое, Илон Маск?";
        const string text = "Увидел у экономиста Николая Кульбака в его ФБ " +
            "экономическую эффективность российских правителей XVIII века. " +
            " На графике приведена динамика подушевого ВВП в течение XVIII века. " +
            "Худшей правительницей с т.з. экономики оказывается Екатерина II, " +
            "которую у нас принято считать вершиной правления этого века. " +
            "При её власти подушевой ВВП россиян обрушился на 40% (даже во " +
            " время Великой Отечественной войны так не падало благосостояние людей).\n" +
            "Лучшим же временем оказался период «дворцовых переворотов» и " +
            "политической чехарды после смерти Петра I (хотя уже и в конце " +
            "правления Петра I пошёл существенный рост благосостояния). В это " +
            " время элиты были заняты грызнёй друг с другом и старались не вести " +
            "больших войн.\n" +
            "В исходнике этого исследования дан более длинный период динамики " +
            "подушевого ВВП, с 1690-х до 1880-, т.е. двести лет.\n" +
            "И мы видим, что к началу правления Александра III благосостояние " +
            "среднего россиянина оказалось таким же, как в 1690-е годы. " +
            "Никакого экономического роста на протяжении двух веков.\n" +
            "Перед нами классическая «мальтузианская ловушка», работавшая " +
            "до появления промышленного капитализма – даже если был общий " +
            "рост экономики, он «съедался» приростом населения. Даже " +
            "в 1880-е в России на промышленность приходилось только 10% ВВП страны.\n" +
            "Самое же интересное, на мой взгляд, этот «горб» в 1720-1750-е " +
            "годы, когда в России наблюдался существенный по меркам того времени " +
            "рост подушевого ВВП (на 40% за три десятилетия). Историческая аномалия, " +
            "мутация, требующая отдельного анализа (скорее всего в области " +
            "политики – когда высшая власть была слаба).";

        await PlainTextViewerDialog.Show (this, title, text);
    }

    private sealed class FieldPainterControl : Control
    {
        private readonly string _text;
        private readonly FieldPainter _painter;

        public FieldPainterControl (string text)
        {
            _text = text;
            _painter = new FieldPainter();
        }

        public override void Render
            (
                DrawingContext context
            )
        {
            context.FillRectangle
                (
                    Brushes.Aqua,
                    new Rect (new Point (), Bounds.Size)
                );
            _painter.Paint (context, Bounds, _text);
        }
    }

    private async void FieldPainterButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        var text = "У попа была^aсобака^bОн ее^cлюбил";
        var window = new Window
        {
            Title = "FieldPainter control demo",
            Width = 300,
            Height = 150,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness (10),
                Children =
                {
                    new FieldPainterControl (text)
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Width = 270,
                        Height = 50
                    }
                }
            }
        };

        await window.ShowDialog (this);
    }

    private void DataGridButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        new DataGridDemo().Show (this);
    }

    private void PropertyGridButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        new PropertyGridDemo().Show (this);
    }

    private void DocumentButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        new DocumentDemo().Show (this);
    }

    private void LogButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        new LogTextBoxDemo().Show (this);
    }

    private void GroupBoxButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        new GroupBoxDemo().Show (this);
    }

    private void ProgressRingButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        new ProgressRingDemo().Show (this);
    }

    private void CompactNumericUpDownButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        new CompactNumericUpDownDemo().Show (this);
    }
}
