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

using System;

using AM;
using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Styling;
using Avalonia.Threading;

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

        _model = GateModel.FromConfiguration();
        DataContext = _model;

        Styles.Add
            (
                new Style (x => x.Class ("error"))
                {
                    Setters =
                    {
                        new Setter (ForegroundProperty, Brushes.Red)
                    }
                }
            );
        Styles.Add
            (
                new Style (x => x.Class ("info"))
                {
                    Setters =
                    {
                        new Setter (ForegroundProperty, Brushes.Blue)
                    }
                }
            );

        var background = new SolidColorBrush (0xFFCCCCCCu);
        _barcodeBox = new TextBox
            {
                // штрих-код читателя
                TextWrapping = TextWrapping.NoWrap,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                [!TextBox.TextProperty] = new Binding (nameof (_model.Barcode))
            }
            .DockTop();
        _barcodeBox.KeyDown += BarcodeBoxOnKeyDown;

        var lastBox = new TextBox
            {
                // последний посетитель
                TextWrapping = TextWrapping.Wrap,
                IsReadOnly = true,
                IsTabStop = false,
                Height = 200,
                Padding = new Thickness (10),
                FontSize = 22,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                [!TextBox.TextProperty] = new Binding (nameof (_model.Last)),
            }
            .DockTop();
        lastBox.BindClass ("error", new Binding (nameof (_model.IsError)), null!);
        lastBox.BindClass ("info", new Binding (nameof (_model.IsInfo)), null!);

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
                        Background = background,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Content = new TextBlock
                        {
                            TextAlignment = TextAlignment.Center,
                            [!TextBlock.TextProperty] = new Binding (nameof (_model.Title))
                        },
                    }
                    .DockTop(),

                new Label
                    {
                        // посещений за сегодня
                        Padding = new Thickness (5),
                        Background = background,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeight.Bold,
                        [!ContentProperty] = new Binding (nameof (_model.VisitCount))
                        {
                            StringFormat = _model.Today
                        }
                    }
                    .DockTop(),

                new Label
                    {
                        // читателей в библиотеке
                        Padding = new Thickness (5),
                        Background = background,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeight.Bold,
                        [!ContentProperty] = new Binding (nameof (_model.InsiderCount))
                        {
                            StringFormat = _model.Readers
                        }
                    }
                    .DockTop(),

                // штрих-код читателя
                _barcodeBox,

                new Label
                    {
                        // обращение к охранникам
                        Padding = new Thickness (5),
                        Background = background,
                        Foreground = Brushes.Blue,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        [!ContentProperty] = new Binding (nameof (_model.Message))
                    }
                    .DockTop(),

                // последний посетитель
                lastBox,

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

        DispatcherTimer.Run
            (() =>
                {
                    _model.AutoUpdate().Forget();
                    return true;
                },
                TimeSpan.FromMinutes (1)
            );
    }

    #endregion

    #region Window members

    protected override void OnInitialized()
    {
        base.OnInitialized();

        DispatcherTimer.RunOnce (() =>
        {
            _barcodeBox.Focus();
            _model.AutoUpdate().Forget();
        }, TimeSpan.FromMilliseconds (100));
    }

    #endregion

    #region Private members

    private readonly GateModel _model;
    private readonly TextBox _barcodeBox;

    private async void BarcodeBoxOnKeyDown
        (
            object? sender,
            KeyEventArgs eventArgs
        )
    {
        if (eventArgs.Key == Key.Enter)
        {
            var barcode = _model.Barcode;
            _model.Barcode = null;
            await _model.HandleReader (barcode);
            _barcodeBox.Focus();
        }
    }

    #endregion
}
