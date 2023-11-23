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

using System;
using System.Threading.Tasks;

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
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
                        [!ContentProperty] = AvaloniaUtility.MakeBinding<string>
                            (
                                nameof (SearcherModel.ErrorMessage),
                                static it => ((SearcherModel) it).ErrorMessage,
                                static (it, value) => ((SearcherModel) it).ErrorMessage = (string?) value
                            )
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
            [!ItemsControl.ItemsSourceProperty] = AvaloniaUtility.MakeBinding<FoundItem[]>
                (
                    nameof (SearcherModel.Found),
                    static it => ((SearcherModel) it).Found,
                    static (it, value) => ((SearcherModel) it).Found = (FoundItem[]?) value
                ),
            ItemTemplate = new FuncDataTemplate<FoundItem> (static (_, _) =>
            {
                var firstBlock = new TextBlock
                    {
                        MinWidth = 90,
                        FontWeight = FontWeight.Bold,
                        Margin = new Thickness (0, 0, 10, 0),
                        [!TextBlock.TextProperty] = AvaloniaUtility.MakeBinding<int>
                            (
                                nameof (FoundItem.Mfn),
                                static it => ((FoundItem) it).Mfn,
                                static (it, value) => ((FoundItem) it).Mfn = (int) value!
                            )
                    }
                    .DockLeft();

                var secondBlock = new TextBlock
                {
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    [!TextBlock.TextProperty] = AvaloniaUtility.MakeBinding<string>
                        (
                            nameof (FoundItem.Text),
                            static it => ((FoundItem) it).Text,
                            static (it, value) => ((FoundItem) it).Text = (string?) value
                        )
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
                        [!Button.CommandProperty] = AvaloniaUtility.MakeBinding<Func<Task>>
                            (
                                nameof (SearcherModel.PerformSearch),
                                static it => (Func<Task>)(((SearcherModel) it).PerformSearch),
                                static (_, _) => { /* do nothing */ }
                            )
                    }
                    .DockRight(),

                new TextBox
                {
                    Margin = new Thickness (10, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    [!TextBox.TextProperty] = AvaloniaUtility.MakeBinding<string>
                        (
                            nameof (SearcherModel.LookingFor),
                            static it => ((SearcherModel) it).LookingFor,
                            static (it, value) => ((SearcherModel) it).LookingFor = (string?) value
                        )
                }
            }
        };
    }

    #endregion
}
