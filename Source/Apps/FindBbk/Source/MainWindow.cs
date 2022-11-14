// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable StringLiteralTypo

/* BbkModel.cs -- модель для окна поиска в эталоне ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

using RestfulIrbis.RslServices;

#endregion

#nullable enable

namespace FindBbk;

public partial class MainWindow
    : Window
{
    public MainWindow()
    {
        this.AttachDevTools();

        Width = 600;
        MinWidth = 600;
        Height = 400;
        MinHeight = 400;
        Title = "Поиск по эталону ББК";

        var model = new BbkModel { window = this };

        var bbkList = new ListBox
        {
            Height = 320, // TODO разобраться, почему вылазит за границы по вертикали
            // VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!ItemsControl.ItemsProperty] = new Binding (nameof (BbkModel.Found)),
            ItemTemplate = new FuncDataTemplate<BbkEntry> ((_, _) =>
            {
                var firstBlock = new TextBlock
                {
                    FontWeight = FontWeight.Bold,
                    Margin = new Thickness (0, 0, 10, 0),
                    [!TextBlock.TextProperty] = new Binding (nameof (BbkEntry.Index))
                };
                firstBlock.PointerPressed += List_HandleItemClick;

                var secondBlock = new TextBlock
                {
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    [!TextBlock.TextProperty] = new Binding (nameof (BbkEntry.Description))
                };
                secondBlock.SetValue (Grid.ColumnProperty, 1);
                secondBlock.PointerPressed += List_HandleItemClick;

                var result = new Grid
                {
                    RowDefinitions = new RowDefinitions ("*"),
                    ColumnDefinitions = new ColumnDefinitions ("Auto,*"),
                    HorizontalAlignment = HorizontalAlignment.Stretch,

                    Children =
                    {
                        firstBlock,
                        secondBlock
                    }
                };

                result.PointerPressed += List_HandleItemClick;

                return result;
            })
        };

        var textBox = new TextBox
        {
            Margin = new Thickness (10, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!TextBox.TextProperty] = new Binding (nameof (BbkModel.LookingFor))
        };
        textBox.SetValue (Grid.ColumnProperty, 1);

        var button = new Button
        {
            IsDefault = true,
            Content = "Найти",
            [!Button.CommandProperty] = new Binding (nameof (BbkModel.PerformSearch))
        };
        button.SetValue (Grid.ColumnProperty, 2);

        Content = new StackPanel
        {
            Margin = new Thickness (10),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Orientation = Orientation.Vertical,
            Spacing = 10,
            Children =
            {
                new Grid
                {
                    RowDefinitions = new RowDefinitions ("*"),
                    ColumnDefinitions = new ColumnDefinitions ("Auto,*,Auto"),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch,

                    Children =
                    {
                        new Label
                        {
                            VerticalAlignment = VerticalAlignment.Center,
                            Content = "Искомое:"
                        },

                        textBox,
                        button
                    }
                },

                new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeight.Bold,
                    Foreground = Brushes.Red,
                    [!ContentProperty] = new Binding (nameof (BbkModel.ErrorMessage))
                },

                bbkList
            }
        };

        DataContext = model;
    }

    private void List_HandleItemClick
        (
            object? sender,
            PointerPressedEventArgs eventArgs
        )
    {
        eventArgs.NotUsed();

        if (sender is StyledElement { DataContext: BbkEntry entry })
        {
            var text = entry.Index;
            if (!string.IsNullOrEmpty (text))
            {
                Application.Current?.Clipboard?.SetTextAsync (text);
            }
        }
    }
}
