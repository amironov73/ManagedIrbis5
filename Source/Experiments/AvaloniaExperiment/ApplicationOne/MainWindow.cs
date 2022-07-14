// ReSharper disable StringLiteralTypo

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

#nullable enable

namespace ApplicationOne;

public class MainWindow
    : Window
{
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var padding = new Thickness (10);
        Title = "Это окно Avalonia, и это круто!";
        Width = 600;
        MinWidth = 600;
        Height = 300;
        MinHeight = 300;

        var labels = new WrapPanel
        {
            Margin = padding,
            Children =
            {
                new Label
                {
                    Content = "Первая метка",
                    Foreground = Brushes.Blue,
                },

                new Label
                {
                    Content = "Вторая метка",
                    Foreground = Brushes.Green,
                },

                new Label
                {
                    Content = "Третья метка",
                    Foreground = Brushes.Red,
                }
            }
        };

        var textBox = new StackPanel
        {
            Margin = padding,
            Children =
            {
                new Label
                {
                    Content = "Текстбокс с надписью"
                },

                new TextBox
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Text = "Тут какой-то текст"
                }
            }
        };

        var checkBox = new CheckBox
        {
            Margin = padding,
            Content = "Отметь меня"
        };

        var comboBox = new StackPanel
        {
            Margin = padding,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Children =
            {
                new Label
                {
                    Content = "Комбобокс с надписью"
                },

                new ComboBox
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Items = new object[]
                    {
                        "Первая строка",
                        "Вторая строка",
                        "Третья строка",
                        "Четвертая строка",
                        "Патая строка",
                        "Шестая строка",
                    },
                    SelectedIndex = 1
                }
            }
        };

        var leftPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                labels,
                textBox,
                checkBox,
                comboBox
            }
        };
        leftPanel.SetValue (Grid.ColumnProperty, 0);
        leftPanel.SetValue (Grid.RowProperty, 0);

        var okButton = new Button
        {
            Content = "OK",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Margin = padding,
            Padding = padding
        };

        var cancelButton = new Button
        {
            Content = "Cancel",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Margin = padding,
            Padding = padding
        };

        var rightPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                okButton,
                cancelButton
            }
        };
        rightPanel.SetValue (Grid.ColumnProperty, 1);
        rightPanel.SetValue (Grid.RowProperty, 0);

        var grid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            ColumnDefinitions =
            {
                new ColumnDefinition (1, GridUnitType.Star),
                new ColumnDefinition (150, GridUnitType.Pixel)
            },
            Children =
            {
                leftPanel,
                rightPanel
            }
        };

        var exitItem = new MenuItem
        {
            Header = "Выход"
        };
        exitItem.Click += (_, _) =>
        {
            Close();
        };

        var menu = new Menu
        {
            Items = new MenuItem[]
            {
                new ()
                {
                    Header = "Файл",
                    Items = new object[]
                    {
                        new MenuItem
                        {
                            Header = "Создать"
                        },
                        new Separator(),
                        exitItem
                    }
                },

                new ()
                {
                    Header = "Редактирование",
                    Items = new MenuItem[]
                    {
                        new ()
                        {
                            Header = "Вырезать"
                        },
                        new ()
                        {
                            Header = "Копировать"
                        },
                        new ()
                        {
                            Header = "Вставить"
                        }
                    }
                },

                new ()
                {
                    Header = "Просмотр",
                    Items = new MenuItem[]
                    {
                        new ()
                        {
                            Header = "Увеличить"
                        },
                        new ()
                        {
                            Header = "Уменьшить"
                        },
                        new ()
                        {
                            Header = "По умолчанию"
                        },
                    }
                }
            }
        };
        menu.SetValue (DockPanel.DockProperty, Dock.Top);

        var mainPanel = new DockPanel
        {
            Children =
            {
                menu,
                grid
            }
        };


        Content = mainPanel;
    }
}
