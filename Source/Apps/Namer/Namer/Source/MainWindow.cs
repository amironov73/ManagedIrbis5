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
using System.Linq;

using AM;
using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
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
    }

    #endregion

    #region Window members

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var specificationBox = new TextBox
        {
            Width = 250,
            [!TextBox.TextProperty] = new Binding
            {
                Source = _processor,
                Path = nameof (NameProcessor.Specification)
            }
        };
        specificationBox.TextChanged += (sender, args) => _ApplySpecification();

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
                            specificationBox,
                            CreateButton ("refresh.png", _ApplySpecification)
                        )
                        .SetPanelMargin (5),
                        
                        CreateButton ("runner.png", _Run)
                    }
                }
            }
        }
        .DockTop();

        _fileListBox = new ListBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!ItemsControl.ItemsProperty] = new Binding (nameof (_folder.Files)),
            ItemTemplate = new FuncDataTemplate<NamePair> ((data, ns) => new PairControl()),
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
                _fileListBox
            }
        };

        if (Application.Current?.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime lifetime)
        {
            if (lifetime.Args is { Length: not 0 } args)
            {
                var folders = _processor.ParseCommandLine (_context, args);
                if (folders is { Count: not 0 })
                {
                    _ReadFolder (folders.First());
                }
            }
        }

    }

    #endregion

    #region Private members

    private string _currentPath = null!;
    private DirectoryInfo _directory = null!;
    private readonly NamingContext _context;
    private readonly NameProcessor _processor;
    private readonly Folder _folder;
    private ListBox _fileListBox = null!;

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
        var pairs = _processor.Render (_context, _directory);
        _folder.DirectoryName = _currentPath;
        _folder.Files = pairs.ToArray();
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

    private void _Run()
    {
        if (_folder.CheckNames())
        {
            if (_folder.Rename())
            {
                _folder.ClearChecked();
            }
        }
    }
    
    #endregion
}
