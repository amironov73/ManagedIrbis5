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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ActiproSoftware.UI.Avalonia.Controls;

using AM;
using AM.Avalonia;
using AM.Avalonia.Controls;
using AM.Collections;
using AM.IO;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

using ReactiveUI;

using NamerCommon;

#endregion

#nullable enable

namespace Namer;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : Window
{
    #region Constants

    private const string SpecificationsFileName = "specifications.txt";

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainWindow()
    {
        this.AttachDevTools();
        this.SetWindowIcon ("Assets/number.ico");

        Title = "Пакетное переименование файлов";
        Width = MinWidth = 960;
        Height = MinHeight = 550;

        _context = new NamingContext();
        _context.LoadDefaultIncludeExclude();
        _processor = new NameProcessor();
        _folder = new Folder();
        DataContext = _folder;
        _folder.Files.ItemPropertyChanged += (_, eventArgs) =>
        {
            if (eventArgs.PropertyName == nameof (NamePair.IsChecked))
            {
                _folder.CheckNames();
            }
        };
        _specifications = new ();
        _PreLoadSpecifications();
    }

    #endregion

    #region Window members

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        // var theme = AvaloniaUtility.GetThemeResources (this);
        // var foreground = theme.ThemeForegroundBrush;
        // var background = theme.ThemeBackgroundBrush;

        _progressStripe = new ProgressStripe { Height = 5, Active = false }
        .StretchHorizontally()
        .DockTop();

        _currentSpecBox = new TextBox
        {
            Width = 250,
            [!TextBox.TextProperty] = AvaloniaUtility.MakeBinding<string>
                (
                    _processor,
                    null,
                    nameof (NameProcessor.Specification),
                    static it => ((NameProcessor) it).Specification,
                    static (it, value) => ((NameProcessor) it).Specification = (string?) value
                )
        };
        _currentSpecBox.TextChanged += (_, _) => _ApplySpecification();

        _specListBox = new ComboBox
        {
            Width = 250,
            [!ItemsControl.ItemsSourceProperty] = AvaloniaUtility.MakeBinding<ObservableCollection<string>>
                (
                    _specifications,
                    null,
                    ".",
                    static it => (ObservableCollection<string>) it,
                    static (_, _) => { /* пустое тело метода */ }
                )
        };
        _specListBox.SelectionChanged += _SpecificationSelected;

        var toolbar = new Border
        {
            // BorderBrush = foreground,
            BorderThickness = new Thickness (0, 0, 0, 1),
            Child = new Panel
            {
                // Background = background,
                Children =
                {
                    new WrapPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness (5),

                        Children =
                        {
                            CreateButton ("open.png", _OpenFolder),
                            CreateButton ("check-all.png", () => _folder.CheckAll()),
                            CreateButton ("check-none.png", () => _folder.CheckNone()),
                            CreateButton ("check-reverse.png", () => _folder.CheckReverse()),

                            AvaloniaUtility.HorizontalGroup
                                (
                                    _specListBox,
                                    CreateButton ("save.png", _SaveCurrentSpecification)
                                )
                                .SetPanelMargin (5),

                            AvaloniaUtility.HorizontalGroup
                                (
                                    _currentSpecBox,
                                    CreateButton ("refresh.png", _ApplySpecification)
                                )
                                .SetPanelMargin (5),

                            CreateButton ("runner.png", _RunAsync)
                        }
                    }
                }
            }
        }
        .DockTop();

        var statusBar = new Border
        {
            // BorderBrush = foreground,
            BorderThickness = new Thickness (0, 1, 0, 0),
            Child = new StackPanel
            {
                // Background = background,
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Children =
                {
                    new TextBlock
                    {
                        Foreground = Brushes.Green,
                        Padding = new Thickness (5),
                        [!TextBlock.TextProperty] = AvaloniaUtility.MakeBinding<int>
                            (
                                _folder,
                                "Отмечено: {0}",
                                nameof (Folder.CheckedCount),
                                static it => ((Folder) it).CheckedCount,
                                static (it, value) => ((Folder) it).CheckedCount = (int) value!
                            )
                    },

                    new TextBlock
                    {
                        Foreground = Brushes.Red,
                        Padding = new Thickness (5),
                        [!TextBlock.TextProperty] = AvaloniaUtility.MakeBinding<int>
                            (
                                _folder,
                                "Ошибки: {0}",
                                nameof (Folder.ErrorCount),
                                static it => ((Folder) it).ErrorCount,
                                static (it, value) => ((Folder) it).ErrorCount = (int) value!
                            )
                    },

                    new TextBlock
                    {
                        // Foreground = foreground,
                        Padding = new Thickness (5),
                        [!TextBlock.TextProperty] = AvaloniaUtility.MakeBinding<string>
                            (
                                _folder,
                                null,
                                nameof (Folder.DirectoryName),
                                static it => ((Folder) it).DirectoryName,
                                static (it, value) => ((Folder) it).DirectoryName = (string?) value
                            )
                    }
                }
            }
        }
        .DockBottom();

        _dataGrid = new DataGrid
        {
            AutoGenerateColumns = false,
            CanUserResizeColumns = true,
            CanUserSortColumns = false,
            SelectionMode = DataGridSelectionMode.Single,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalGridLinesBrush = Brushes.Gray,
            VerticalGridLinesBrush = Brushes.Gray,
            GridLinesVisibility = DataGridGridLinesVisibility.All,
            [!DataGrid.ItemsSourceProperty] = AvaloniaUtility.MakeBinding<ItemPropertyTrackingCollection<NamePair>>
                (
                    nameof(_folder.Files),
                    static it => ((Folder) it).Files,
                    static (_, _) => { /* пустое тело метода */ }
                ),
            Columns =
            {
                new DataGridCheckBoxColumn
                {
                    MinWidth = 40,
                    Width = new DataGridLength (40, DataGridLengthUnitType.Pixel),
                    Binding = AvaloniaUtility.MakeBinding<bool>
                        (
                            nameof (NamePair.IsChecked),
                            static it => ((NamePair) it).IsChecked,
                            static (it, value) => ((NamePair) it).IsChecked = (bool) value!
                        )
                },

                new DataGridTextColumn
                {
                    IsReadOnly = true,
                    Header = "Старое имя",
                    Width = new DataGridLength (1, DataGridLengthUnitType.Star),
                    Binding = AvaloniaUtility.MakeBinding<string>
                        (
                            nameof (NamePair.Old),
                            static it => ((NamePair) it).Old,
                            static (_, _) => { /* пустое тело метода */}
                        )
                },

                new DataGridTextColumn
                {
                    Header = "Новое имя",
                    Width = new DataGridLength (1, DataGridLengthUnitType.Star),
                    Binding = AvaloniaUtility.MakeBinding<string>
                        (
                            nameof (NamePair.New),
                            static it => ((NamePair) it).New,
                            static (_, _) => { /* пустое тело метода */}
                        )
                },

                new DataGridTextColumn
                {
                    IsReadOnly = true,
                    Foreground = Brushes.Red,
                    Header = "Сообщение об ошибке",
                    Width = new DataGridLength (1, DataGridLengthUnitType.Star),
                    Binding = AvaloniaUtility.MakeBinding<string>
                        (
                            nameof (NamePair.ErrorMessage),
                            static it => ((NamePair) it).ErrorMessage,
                            static (it, value) => ((NamePair) it).ErrorMessage = (string?) value
                        )
                }
            }
        };
        _dataGrid.KeyDown += (_, eventArgs) =>
        {
            switch (eventArgs)
            {
                case { Key: Key.Space, KeyModifiers: KeyModifiers.None }:
                    eventArgs.Handled = true;
                    _ToggleMark();
                    break;
            }
        };
        _dataGrid.CellPointerPressed += (_, eventArgs) =>
        {
            if (eventArgs.PointerPressedEventArgs is { ClickCount: >= 2, KeyModifiers: KeyModifiers.None })
            {
                _OpenCurrentFile();
            }
        };

        // возня вокруг заголовков столбцов, не желающих самостоятельно подхватывать цвета темной темы
        // _dataGrid.ColumnHeaderTheme = new ControlTheme (typeof (DataGridColumnHeader))
        // {
        //     BasedOn = AvaloniaUtility.GetControlTheme (typeof (DataGridColumnHeader)),
        //     Setters =
        //     {
        //         new Setter (BackgroundProperty, background),
        //         new Setter (ForegroundProperty, foreground)
        //     }
        // };

        var titleBar = new ChromedTitleBar
        {
            LeftContent = new Image
            {
                Height = 32,
                Width = 32,
                Source = GetIcon ("Assets/number.ico")
            },
            RightContent = new ToggleThemeButton(),
            Content = Title
        }
        .DockTop();

        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                titleBar,
                toolbar,
                _progressStripe,
                statusBar,
                _dataGrid
            }
        };

        if (Application.Current?.ApplicationLifetime
            is ClassicDesktopStyleApplicationLifetime { Args: { Length: not 0 } args })
        {
            var folders = _processor.ParseCommandLine (_context, args);
            if (folders is { Count: not 0 })
            {
                _ReadFolder (folders.First());
            }
        }
    }

    /// <inheritdoc cref="InputElement.OnKeyDown"/>
    protected override async void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        base.OnKeyDown (eventArgs);

        switch (eventArgs)
        {
            case { Key: Key.Escape, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                Close();
                break;

            case { Key: Key.F2, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                await _RunAsync();
                break;

            case { Key: Key.F3, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                _currentSpecBox.Focus();
                break;

            case { Key: Key.F4, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                _OpenFolder();
                break;

            case { Key: Key.F5, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                _Refresh();
                break;

            case { Key: Key.F10, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                _OpenCurrentFile();
                break;
        }
    }

    #endregion

    #region Private members

    private ProgressStripe _progressStripe = null!;
    private readonly ObservableCollection<string> _specifications;
    private TextBox _currentSpecBox = null!;
    private ComboBox _specListBox = null!;
    private string _currentPath = null!;
    private DirectoryInfo _directory = null!;
    private readonly NamingContext _context;
    private readonly NameProcessor _processor;
    private readonly Folder _folder;
    private DataGrid _dataGrid = null!;

    private Bitmap? GetIcon
        (
            string iconName
        )
    {
        using var stream = AvaloniaUtility.OpenAssetStream (GetType(), iconName);
        if (stream is not null)
        {
            return new Bitmap (stream);
        }

        return null;
    }

    private async Task<bool> RenameImplAsync
        (
            Folder folder,
            NamePair pair,
            double percentage
        )
    {
        var oldName = Path.Combine (folder.DirectoryName!, pair.Old);
        var newName = Path.Combine (folder.DirectoryName!, pair.New);

        try
        {
            File.Move (oldName, newName);
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception);
            return false;
        }

        await Dispatcher.UIThread.InvokeAsync (() =>
        {
            _progressStripe.Percentage = percentage;
            _progressStripe.InvalidateVisual();
        });
        await Task.Delay (20);

        return true;
    }

    private Button CreateButton
        (
            string assetName,
            Func<Task> action
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
            Command = ReactiveCommand.CreateFromTask (action)
        };
    }

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

    private void _Refresh()
    {
        _processor.Reset();
        var pairs = _processor.Render (_context, _directory);
        _folder.DirectoryName = _currentPath;
        _folder.Assign (pairs.ToArray());
        _folder.CheckNames();
    }

    private void _ApplySpecification()
    {
        if (_processor.Specification is { } specification)
        {
            try
            {
                _processor.ParseSpecification (specification);
                _Refresh();
            }
            catch
            {
                // Do nothing
            }
        }
    }

    private void _ReadFolder
        (
            string path
        )
    {
        _currentPath = path;
        _directory = new DirectoryInfo (path);
        _Refresh();
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
            _ReadFolder (result[0].Path.LocalPath);
        }
    }

    private async Task _RunAsync()
    {
        if (_folder.CheckNames())
        {
            await Dispatcher.UIThread.InvokeAsync (() => _progressStripe.Active = true);
            if (await _folder.RenameAsync (RenameImplAsync))
            {
                _folder.ClearChecked();
            }
            await Dispatcher.UIThread.InvokeAsync(() => _progressStripe.Active = false);
        }
    }

    private void _PreLoadSpecifications()
    {
        _specifications.Clear();
        var fileName = Path.Combine (AppContext.BaseDirectory, SpecificationsFileName);
        if (File.Exists (fileName))
        {
            foreach (var line in File.ReadLines (fileName))
            {
                if (!string.IsNullOrWhiteSpace (line))
                {
                    _specifications.Add (line.Trim());
                }
            }
        }
    }

    private void _SaveCurrentSpecification()
    {
        if (_currentSpecBox.Text is { Length: not 0 } spec)
        {
            FileUtility.Touch (SpecificationsFileName);
            var nl = Environment.NewLine;
            File.AppendAllText (SpecificationsFileName, nl + spec + nl);
            _specifications.Add (spec);
        }
    }

    private void _SpecificationSelected
        (
            object? sender,
            SelectionChangedEventArgs eventArgs
        )
    {
        if (_specListBox.SelectedItem is string spec)
        {
            _processor.Specification = spec;
            _ApplySpecification();
        }
    }

    private void _OpenCurrentFile()
    {
        if (_dataGrid.SelectedItem is NamePair { Old: { Length: not 0 } currentFile})
        {
            var fileName = Path.Combine (_folder.DirectoryName!, currentFile);
            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                UseShellExecute = true
            };
            Process.Start (processStartInfo)?.Dispose();
        }
    }

    private void _ToggleMark()
    {
        if (_dataGrid.SelectedItem is NamePair pair)
        {
            pair.IsChecked = !pair.IsChecked;
        }
    }

    #endregion
}
