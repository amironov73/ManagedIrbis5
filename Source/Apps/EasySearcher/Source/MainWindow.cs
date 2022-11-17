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
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

#endregion

#nullable enable

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
                    }
                    .DockTop(),

                new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeight.Bold,
                        Foreground = Brushes.Red,
                        [!ContentProperty] = new Binding (nameof (SearcherModel.ErrorMessage))
                    }
                    .DockTop(),

                new ListBox
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    [!ItemsControl.ItemsProperty] = new Binding (nameof (SearcherModel.Found))
                }

            }
        };

        DataContext = model;
    }

    #endregion
}
