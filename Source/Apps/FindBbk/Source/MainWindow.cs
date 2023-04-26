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

/* MainWindow.cs -- главное окно для вывода результатов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Avalonia;

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

/// <summary>
/// Главное окно для вывода результатов поиска по эталону ББК.
/// </summary>
public sealed class MainWindow
    : Window
{
    public MainWindow()
    {
        this.AttachDevTools();

        Width = MinWidth = 600;
        Height = MinHeight = 400;
        Title = "Поиск по эталону ББК";

        var model = new BbkModel { window = this };

        var bbkList = new ListBox
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!ItemsControl.ItemsSourceProperty] = new Binding (nameof (BbkModel.Found)),
            ItemTemplate = new FuncDataTemplate<BbkEntry> ((_, _) =>
            {
                var firstBlock = new TextBlock
                {
                    MinWidth = 100,
                    FontWeight = FontWeight.Bold,
                    Margin = new Thickness (0, 0, 10, 0),
                    [!TextBlock.TextProperty] = new Binding (nameof (BbkEntry.Index))
                };
                firstBlock.SetValue (DockPanel.DockProperty, Dock.Left);
                firstBlock.PointerPressed += List_HandleItemClick;

                var secondBlock = new TextBlock
                {
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    [!TextBlock.TextProperty] = new Binding (nameof (BbkEntry.Description))
                };
                secondBlock.PointerPressed += List_HandleItemClick;

                var result = new DockPanel
                {
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

        Content = new DockPanel
        {
            Margin = new Thickness (10),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                new DockPanel
                    {
                        Children =
                        {
                            new Label
                                {
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Content = "Искомое: "
                                }
                                .DockLeft(),

                            new Button
                                {
                                    IsDefault = true,
                                    Content = "Найти",
                                    [!Button.CommandProperty] = new Binding (nameof (BbkModel.PerformSearch))
                                }
                                .DockRight(),

                            new TextBox
                            {
                                Margin = new Thickness (10, 0),
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                [!TextBox.TextProperty] = new Binding (nameof (BbkModel.LookingFor))
                            }
                        }
                    }
                    .DockTop(),

                new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeight.Bold,
                        Foreground = Brushes.Red,
                        [!ContentProperty] = new Binding (nameof (BbkModel.ErrorMessage))
                    }
                    .DockTop(),

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
                Clipboard?.SetTextAsync (text);
            }
        }
    }
}
