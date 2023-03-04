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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AM;
using AM.Avalonia;
using AM.Avalonia.Controls;
using AM.IO;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Styling;

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
    private const string IncludeFileName = "include.txt";
    private const string ExcludeFileName = "exclude.txt";

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
        _processor = new NameProcessor();
        _folder = new Folder();
        DataContext = _folder;
        _specifications = new ();
        _LoadIncludeExclude();
        _PreLoadSpecifications();
    }

    #endregion

    #region Window members

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var progressStripe = new ProgressStripe
        {
            Height = 10,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!ProgressStripe.ActiveProperty] = new Binding
            {
                Source = _folder,
                Path = nameof (Folder.Active)
            },
            [!ProgressStripe.PercentageProperty] = new Binding
            {
                Source = _folder,
                Path = nameof (Folder.Percentage)
            }
        }
        .DockTop();

        _currentSpecBox = new TextBox
        {
            Width = 250,
            [!TextBox.TextProperty] = new Binding
            {
                Source = _processor,
                Path = nameof (NameProcessor.Specification)
            }
        };
        _currentSpecBox.TextChanged += (_, _) => _ApplySpecification();

        _specListBox = new ComboBox
        {
            Width = 250,
            [!ItemsControl.ItemsProperty] = new Binding
            {
                Source = _specifications,
                Path = "."
            }
        };
        _specListBox.SelectionChanged += _SpecificationSelected;

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
        .DockTop();

        var statusBar = new StackPanel
        {
            Background = Brushes.LightGray,
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Children =
            {
                new TextBlock
                {
                    Foreground = Brushes.Green,
                    Padding = new Thickness (5),
                    [!TextBlock.TextProperty] = new Binding
                    {
                        Source = _folder,
                        Path = nameof (Folder.CheckedCount),
                        StringFormat = "Отмечено: {0}"
                    }
                },

                new TextBlock
                {
                    Foreground = Brushes.Blue,
                    Padding = new Thickness (5),
                    [!TextBlock.TextProperty] = new Binding
                    {
                        Source = _folder,
                        Path = nameof (Folder.ErrorCount),
                        StringFormat = "Ошибки: {0}"
                    }
                },

                new TextBlock
                {
                    Foreground = Brushes.Black,
                    Padding = new Thickness (5),
                    [!TextBlock.TextProperty] = new Binding
                    {
                        Source = _folder,
                        Path = nameof (Folder.DirectoryName)
                    }
                }
            }
        }
        .DockBottom();

        _fileListBox = new ListBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!ItemsControl.ItemsProperty] = new Binding (nameof (_folder.Files)),
            ItemTemplate = new FuncDataTemplate<NamePair> ((_, _) => new PairControl()),
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

        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                toolbar,
                progressStripe,
                statusBar,
                _fileListBox
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
                Close();
                break;
            
            case { Key: Key.F2, KeyModifiers: KeyModifiers.None }:
                await _RunAsync();
                break;
        }
    }

    #endregion

    #region Private members

    private readonly ObservableCollection<string> _specifications;
    private TextBox _currentSpecBox = null!;
    private ComboBox _specListBox = null!;
    private string _currentPath = null!;
    private DirectoryInfo _directory = null!;
    private readonly NamingContext _context;
    private readonly NameProcessor _processor;
    private readonly Folder _folder;
    private ListBox _fileListBox = null!;

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
            if (await _folder.RenameAsync())
            {
                _folder.ClearChecked();
            }
        }
    }

    private void _PreLoadSpecifications()
    {
        _specifications.Clear();
        if (File.Exists (SpecificationsFileName))
        {
            foreach (var line in File.ReadLines (SpecificationsFileName))
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

    private void _LoadIncludeExclude()
    {
        if (File.Exists (IncludeFileName))
        {
            foreach (var line in File.ReadLines (IncludeFileName))
            {
                if (!string.IsNullOrWhiteSpace (line))
                {
                    _context.Filters.Add (new IncludeFilter (line.Trim()));
                }
            }
        }
        
        if (File.Exists (ExcludeFileName))
        {
            foreach (var line in File.ReadLines (ExcludeFileName))
            {
                if (!string.IsNullOrWhiteSpace (line))
                {
                    _context.Filters.Add (new ExcludeFilter (line.Trim()));
                }
            }
        }
    }

    #endregion
}
