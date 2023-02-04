// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;

using AM;
using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Styling;

using ReactiveUI;

#endregion

#nullable enable

namespace VisualRenumber;

/// <summary>
/// Главное окно приложения
/// </summary>
public sealed class MainWindow
    : Window
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainWindow()
    {
        base.OnInitialized();
        this.AttachDevTools();

        this.SetWindowIcon ("Assets/number.ico");

        Title = "Перенумерация файлов";
        Width = MinWidth = 960;
        Height = MinHeight = 550;
        _model = new FolderModel
        {
            DryRun = true
        };
        _fileListBox = new ListBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!ItemsControl.ItemsProperty] = new Binding (nameof (_model.Files)),
            ItemTemplate = new FuncDataTemplate<FileItem> ((data, ns) => new FileControl()),
            Styles =
            {
                new Style (x => x.OfType<ListBoxItem>())
                {
                    Setters =
                    {
                        new Setter (MarginProperty, new Thickness (0)),
                        new Setter (PaddingProperty, new Thickness (10, 0))
                    }
                }
            }
        };
        DataContext = _model;

        if (Application.Current?.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime lifetime)
        {
            if (lifetime.Args is { Length: not 0 } args)
            {
                _model.ReadDirectory (args[0]);
            }
        }
    }

    #endregion

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        var toolbar = new Panel
            {
                Background = Brushes.LightGray,
                Children =
                {
                    new WrapPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness (5),

                        Children =
                        {
                            CreateButton ("open.png", _OpenFolder),
                            CreateButton ("check-all.png", () => _model.CheckAll()),
                            CreateButton ("check-none.png", () => _model.CheckNone()),
                            CreateButton ("check-reverse.png", () => _model.CheckReverse()),
                            AvaloniaUtility.HorizontalGroup
                                (
                                    new Label
                                    {
                                        Content = "Группа",
                                        VerticalAlignment = VerticalAlignment.Center
                                    },
                                    new NumericUpDown
                                    {
                                        Width = 120,
                                        Minimum = 1,
                                        Maximum = 9,
                                        [!NumericUpDown.ValueProperty] = new Binding (nameof (_model.GroupNumber))
                                    }
                                )
                                .SetPanelMargin (5),
                            AvaloniaUtility.HorizontalGroup
                                (
                                    new Label
                                    {
                                        Content = "Ширина",
                                        VerticalAlignment = VerticalAlignment.Center
                                    },
                                    new NumericUpDown
                                    {
                                        Width = 120,
                                        Minimum = 0,
                                        Maximum = 9,
                                        [!NumericUpDown.ValueProperty] = new Binding (nameof (_model.GroupWidth))
                                    }
                                )
                                .SetPanelMargin (5),
                            AvaloniaUtility.HorizontalGroup
                                (
                                    new Label
                                    {
                                        Content = "Префикс",
                                        VerticalAlignment = VerticalAlignment.Center
                                    },
                                    new TextBox
                                    {
                                        Width = 150,
                                        [!TextBox.TextProperty] = new Binding (nameof (_model.Prefix))
                                    }
                                )
                                .SetPanelMargin (5),
                            AvaloniaUtility.HorizontalGroup
                                (
                                    new Label
                                    {
                                        Content = "Суффикс",
                                        VerticalAlignment = VerticalAlignment.Center
                                    },
                                    new TextBox
                                    {
                                        Width = 50,
                                        [!TextBox.TextProperty] = new Binding (nameof (_model.Suffix))
                                    }
                                )
                                .SetPanelMargin (5),
                            AvaloniaUtility.HorizontalGroup
                                (
                                    new Label
                                    {
                                        Content = "Начальный",
                                        VerticalAlignment = VerticalAlignment.Center
                                    },
                                    new TextBox
                                    {
                                        Width = 50,
                                        [!TextBox.TextProperty] = new Binding (nameof  (_model.StartNumber))
                                    }
                                )
                                .SetPanelMargin (5),
                            new CheckBox
                            {
                                Content = "Сухой",
                                [!ToggleButton.IsCheckedProperty] = new Binding (nameof (_model.DryRun))
                            },
                            CreateButton ("runner.png", _Run)
                        }
                    }
                }
            }
            .DockTop();
        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                toolbar,
                _fileListBox
            }
        };

        _model.PropertyChanged += _Refresh;
    }

    #endregion

    #region Program entry point

    private readonly FolderModel _model;
    private readonly ListBox _fileListBox;

    private Button CreateButton
        (
            string assetName,
            Action action
        )
    {
        Sure.NotNullNorEmpty (assetName);
        Sure.NotNull (action);

        assetName = Path.Combine ("Assets", assetName);
        return new Button
        {
            Content = new Image
            {
                Width = 24,
                Height = 24,
                Source = this.LoadBitmapFromAssets (assetName).ThrowIfNull()
            },
            Background = Brushes.Transparent,
            Command = ReactiveCommand.Create (action)
        };
    }

    private async void _OpenFolder()
    {
        if (!StorageProvider.CanPickFolder)
        {
            return;
        }

        var options = new FolderPickerOpenOptions();
        var result = await StorageProvider.OpenFolderPickerAsync (options);
        if (result.Count != 0)
        {
            // if (result[0].TryGetUri (out var uri))
            // {
            //     _model.ReadDirectory (uri.LocalPath);
            // }
        }
    }

    private void _Refresh (object? sender, PropertyChangedEventArgs eventArgs)
    {
        _model.Refresh();
    }

    private void _Run()
    {
        if (_model.DryRun)
        {
            var message = _model.HasError()
                ? "Нельзя"
                : "Можно";
            MessageBoxManager
                .GetMessageBoxStandardWindow ("Renumberer", message)
                .ShowDialog (this);
        }
        else
        {
            _model.RenameChecked();
            _model.ClearChecked();
        }
    }

    #endregion
}
