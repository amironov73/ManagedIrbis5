// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Styling;

#endregion

#nullable enable

namespace Gatekeeper;

/// <summary>
/// Главное окно приложения.
/// </summary>
internal sealed class MainWindow
    : ReactiveWindow<GateModel>
{
    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public MainWindow()
    {
        this.AttachDevTools();

        Title = "Контроль входа-выхода";
        Width = MinWidth = 800;
        Height = MinHeight = 450;

        this.SetWindowIcon ("Assets/guard.ico");

        _model = GateModel.GetTestModel();
        DataContext = _model;

        var yellowBrush = new SolidColorBrush (0xFFFFFF00u);

        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,

            Children =
            {
                new Label
                {
                    // название библиотеки
                    Padding = new Thickness (5),
                    Background = yellowBrush,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = new TextBlock
                    {
                        TextAlignment = TextAlignment.Center,
                        Text = "Иркутская областная универсальная научная библиотека\nимени И. И. Молчанова-Сибирского"
                    },
                }
                .DockTop(),

                new Label
                {
                    // посещений за сегодня
                    Padding = new Thickness (5),
                    Background = yellowBrush,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeight.Bold,
                    [!ContentProperty] = new Binding (nameof (_model.VisitCount))
                    {
                        StringFormat = "Посещений за сегодня: {0}"
                    }
                }
                .DockTop(),

                new Label
                {
                    // читателей в библиотеке
                    Padding = new Thickness (5),
                    Background = yellowBrush,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeight.Bold,
                    [!ContentProperty] = new Binding (nameof (_model.InsiderCount))
                    {
                        StringFormat = "Читателей в библиотеке: {0}"
                    }
                }
                .DockTop(),

                new TextBox
                {
                    // штрих-код читателя
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    [!TextBox.TextProperty] = new Binding (nameof (_model.Barcode))
                }
                .DockTop(),

                new Label
                {
                    // обращение к охранникам
                    Padding = new Thickness (5),
                    Foreground = Brushes.Blue,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    [!ContentProperty] = new Binding (nameof (_model.Message))
                }
                .DockTop(),

                new TextBox
                {
                    // последний посетитель
                    IsReadOnly = true,
                    Height = 200,
                    Padding = new Thickness (5),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    [!TextBox.TextProperty] = new Binding (nameof (_model.Last)),
                }.DockTop(),

                new ListBox
                {
                    // список посетителей
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    [!ItemsControl.ItemsProperty] = new Binding (nameof (_model.Events)),
                    Styles =
                    {
                        new Style (x => x.OfType<ListBoxItem>())
                        {
                            Setters =
                            {
                                new Setter (MarginProperty, new Thickness (0)),
                                new Setter (PaddingProperty, new Thickness (10, 2))
                            }
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Private members

    private readonly GateModel _model;

    #endregion
}
