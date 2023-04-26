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

using System.IO;
using System.Linq;

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;

using ReactiveUI;

#endregion

#nullable enable

namespace Onna;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : ReactiveWindow<Folder>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainWindow()
    {
        this.AttachDevTools();
        this.SetWindowIcon ("Assets/onna.ico");

        Title = "Onna";
        Width = MinWidth = 960;
        Height = MinHeight = 550;
        WindowState = WindowState.Maximized;

        _folder = new Folder();
        DataContext = _folder;
    }

    #endregion

    #region Window members

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var theme = AvaloniaUtility.GetThemeResources (this);
        var foreground = theme.ThemeForegroundBrush;
        // var background = theme.ThemeBackgroundBrush;

        var toolbar = new Border
        {
            BorderBrush = foreground,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            BorderThickness = new Thickness (0, 0, 0, 1),
            Child = new WrapPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness (5),
                Children =
                {
                    new Button
                    {
                        Content = "Дир",
                        Command = ReactiveCommand.Create (_ChangeDirectory)
                    },

                    new Button
                    {
                        Content = "Скролл",
                        Command = ReactiveCommand.Create (_ChangeScroll)
                    }
                }
            }
        }
        .DockTop();

        var statusBar = new Border
        {
            BorderBrush = foreground,
            BorderThickness = new Thickness (0, 1, 0, 0),
            Child = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness (5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Children =
                {
                    new TextBlock
                    {
                        Margin = new Thickness (5),
                        [!TextBlock.TextProperty] = new Binding (nameof (Folder.SelectedFile))
                    }
                }
            }
        }
        .DockBottom();

        _fileList = new ListBox
            {
                Width = 300,
                Focusable = true,
                IsTabStop = true,
                [!ItemsControl.ItemsSourceProperty] = new Binding (nameof (Folder.Files)),
                [!SelectingItemsControl.SelectedItemProperty] = new Binding (nameof (Folder.SelectedFile)),
                ItemTemplate = new FuncDataTemplate<string> ((data, _) =>
                    new TextBlock { Text = Path.GetFileName (data) })
            }
            .DockLeft();
        _fileList.KeyDown += _KeyDown;

        _imageBox = new Image
        {
            IsTabStop = true,
            [!Image.SourceProperty] = new Binding (nameof (Folder.SelectedFile))
            {
                Converter = new PathToBitmapConverter()
            }
        };
        _imageBox.KeyDown += _KeyDown;

        _scrollViewer = new ScrollViewer();
        _scrollViewer.KeyDown += _KeyDown;
        Content = _dockPanel = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                toolbar,
                statusBar,
                _fileList,
                _imageBox
            }
        };
        _dockPanel.KeyDown += _KeyDown;

        if (Application.Current?.ApplicationLifetime
            is ClassicDesktopStyleApplicationLifetime { Args: { Length: not 0 } args })
        {
            _ReadFolder (args[0]);
        }
    }

    /// <inheritdoc cref="InputElement.OnKeyDown"/>
    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        base.OnKeyDown (eventArgs);
        _HandleKeys (eventArgs);
    }

    #endregion

    #region Private members

    private Folder _folder;
    private ListBox _fileList = null!;
    private DockPanel _dockPanel = null!;
    private bool _scrollEnabled;
    private Image _imageBox = null!;
    private ScrollViewer _scrollViewer = null!;

    private void _ReadFolder
        (
            string dirName
        )
    {
        dirName = Path.GetFullPath (dirName);
        _folder = Folder.LoadFolder (dirName);
        DataContext = _folder;

        if (string.IsNullOrEmpty (_folder.SelectedFile))
        {
            _folder.SelectedFile = _folder.Files?.FirstOrDefault();
        }

        _fileList.Focus();
    }

    private async void _ChangeDirectory()
    {
        var options = new FolderPickerOpenOptions
        {
            AllowMultiple = false
        };
        var result = await StorageProvider.OpenFolderPickerAsync (options);
        if (result.Count != 0)
        {
            _ReadFolder (result[0].Path.LocalPath);
        }
    }

    private void _ChangeScroll()
    {
        if (_scrollEnabled)
        {
            _dockPanel.Children.Remove (_scrollViewer);
            _scrollViewer.Content = null;
            _dockPanel.Children.Add (_imageBox);
        }
        else
        {
            _dockPanel.Children.Remove (_imageBox);
            _scrollViewer.Content = _imageBox;
            _dockPanel.Children.Add (_scrollViewer);
        }

        _scrollEnabled = !_scrollEnabled;
    }

    private void _MoveToPreviousPicture()
    {
        var files = _folder.Files;
        if (files is null)
        {
            return;
        }

        if (_fileList.SelectedIndex > 0)
        {
            _fileList.SelectedIndex--;
        }
        else
        {
            _fileList.SelectedIndex = files.Length - 1;
        }
    }

    private void _MoveToNextPicture()
    {
        var files = _folder.Files;
        if (files is null)
        {
            return;
        }

        var newIndex = _fileList.SelectedIndex + 1;
        _fileList.SelectedIndex = newIndex < files.Length ? newIndex : 0;
    }

    private void _ChangeBang()
    {
        var selected = _fileList.SelectedIndex;
        var directoryName = _folder.DirectoryName;
        var files = _folder.Files;
        var oldName = _folder.SelectedFile;
        if (directoryName is null
            || files is null
            || oldName is null)
        {
            return;
        }

        var nameOnly = Path.GetFileNameWithoutExtension (oldName);
        var extension = Path.GetExtension (oldName);
        if (string.IsNullOrEmpty (nameOnly))
        {
            return;
        }

        nameOnly = nameOnly.EndsWith ('!') ? nameOnly[..^1] : nameOnly + "!";
        var newName = Path.Combine (directoryName, nameOnly) + extension;

        File.Move (oldName, newName);
        _ReadFolder (directoryName);
        _fileList.SelectedIndex = selected;
        _MoveToNextPicture();
    }

    private void _HandleKeys
        (
            KeyEventArgs eventArgs
        )
    {
        if (eventArgs.Handled)
        {
            return;
        }

        switch (eventArgs)
        {
            case { Key: Key.Escape, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                Close();
                break;

            case { Key: Key.Back, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                _MoveToPreviousPicture();
                break;

            case { Key: Key.Enter, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                _ChangeBang();
                break;

            case { Key: Key.Space, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                _MoveToNextPicture();
                break;

            case { Key: Key.F2, KeyModifiers: KeyModifiers.None }:
                eventArgs.Handled = true;
                _ChangeScroll();
                break;
        }
    }

    private void _KeyDown
        (
            object? sender,
            KeyEventArgs eventArgs
        )
    {
        _HandleKeys (eventArgs);
    }

    #endregion
}
