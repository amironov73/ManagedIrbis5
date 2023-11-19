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

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

using ManagedIrbis;

#endregion

namespace EasySearcher;

/// <summary>
/// Главное окно для вывода результатов поиска по эталону ББК.
/// </summary>
public sealed class MainWindow
    : Window
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MainWindow()
    {
        this.AttachDevTools();

        Width = MinWidth = 600;
        Height = MinHeight = 400;
        Title = "Поиск по электронному каталогу";

        var model = new SearcherModel { window = this };
        Content = new DockPanel
        {
            Margin = new Thickness (10),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                CreateTopPanel()
                    .DockTop(),

                new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeight.Bold,
                        Foreground = Brushes.Red,
                        [!ContentProperty] = new Binding (nameof (SearcherModel.ErrorMessage))
                    }
                    .DockTop(),

                CreateListBox()
            }
        };

        DataContext = model;
    }

    #endregion

    #region Private members

    private static ListBox CreateListBox()
    {
        return new ListBox
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!ItemsControl.ItemsSourceProperty] = new Binding (nameof (SearcherModel.Found)),
            ItemTemplate = new FuncDataTemplate<FoundItem> (static (_, _) =>
            {
                var firstBlock = new TextBlock
                    {
                        MinWidth = 90,
                        FontWeight = FontWeight.Bold,
                        Margin = new Thickness (0, 0, 10, 0),
                        [!TextBlock.TextProperty] = new Binding (nameof (FoundItem.Mfn))
                    }
                    .DockLeft();

                var secondBlock = new TextBlock
                {
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    [!TextBlock.TextProperty] = new Binding (nameof (FoundItem.Text))
                };

                var result = new DockPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Children =
                    {
                        firstBlock,
                        secondBlock
                    }
                };

                return result;
            })
        };
    }

    private static DockPanel CreateTopPanel()
    {
        return new DockPanel
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
                        [!Button.CommandProperty] = new Binding (nameof (SearcherModel.PerformSearch))
                    }
                    .DockRight(),

                new TextBox
                {
                    Margin = new Thickness (10, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    [!TextBox.TextProperty] = new Binding (nameof (SearcherModel.LookingFor))
                }
            }
        };
    }

    #endregion
}
