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
using AM.Avalonia.Converters;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;

using ReactiveUI;

#endregion

#nullable enable

namespace Peeper;

/// <summary>
/// Главное окно приложения.
/// </summary>
internal sealed class MainWindow
    : ReactiveWindow<FolderModel>
{
    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public MainWindow()
    {
        this.AttachDevTools();

        Title = "Просмотр JPEG";
        Width = MinWidth = 800;
        Height = MinHeight = 450;

        this.SetWindowIcon ("Assets/view.ico");

        _model = new FolderModel();
        DataContext = _model;

        var toolBar = new StackPanel
            {
                Margin = new Thickness (5),
                Orientation = Orientation.Horizontal,
                Spacing = 5,
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
            .DockTop();

        var statusBlock = new TextBlock
            {
                Padding = new Thickness (5),
                Background = Brushes.LightGray,
                [!TextBlock.TextProperty] = new Binding (nameof (_model.SelectedFile))
                {
                    Converter = StorageItemToUriConverter.Instance
                }
            }
            .DockBottom();

        _fileList = new ListBox
            {
                Width = 250,
                [!ItemsControl.ItemsProperty] = new Binding (nameof (_model.Files)),
                [!SelectingItemsControl.SelectedItemProperty] = new Binding (nameof (_model.SelectedFile)),
                ItemTemplate = new FuncDataTemplate<IStorageItem> ((data, _) =>
                    new TextBlock{ Text = data.Name })
            }
            .DockLeft();
        _imageBox = new Image
        {
            IsTabStop = true,
            [!Image.SourceProperty] = new Binding (nameof (_model.SelectedFile))
            {
                Converter = StorageItemToImageConverter.Instance
            }
        };
        _scrollViewer = new ScrollViewer();
        _mainPanel = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                toolBar,
                statusBlock,
                _fileList,
                _imageBox
            }
        };
    }

    #endregion

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Content = _mainPanel;
        KeyDown += _Window_KeyDown;
        _fileList.KeyDown += _FileList_KeyDown;
    }

    #endregion

    #region Private members

    private FolderModel _model;

    private readonly ListBox _fileList;

    private readonly DockPanel _mainPanel;

    private bool _scrollEnabled;

    private readonly Image _imageBox;

    private readonly ScrollViewer _scrollViewer;

    private async void _ChangeDirectory()
    {
        var options = new FolderPickerOpenOptions
        {
            AllowMultiple = false
        };
        var result = await StorageProvider.OpenFolderPickerAsync (options);
        if (result.Count != 0)
        {
            var first = result[0];
            _model = FolderModel.LoadFolder (first);
            DataContext = _model;
            FocusManager.Instance?.Focus (_imageBox);
        }
    }

    private void _ChangeScroll()
    {
        if (_scrollEnabled)
        {
            _mainPanel.Children.Remove (_scrollViewer);
            _scrollViewer.Content = null;
            _mainPanel.Children.Add (_imageBox);
        }
        else
        {
            _mainPanel.Children.Remove (_imageBox);
            _scrollViewer.Content = _imageBox;
            _mainPanel.Children.Add (_scrollViewer);
        }

        _scrollEnabled = !_scrollEnabled;
    }

    private void _MoveToNextPicture()
    {
        var files = _model.Files;
        if (files is null)
        {
            return;
        }

        if (_fileList.SelectedIndex < files.Length)
        {
            _fileList.SelectedIndex++;
        }
    }

    private void _Window_KeyDown
        (
            object? sender,
            KeyEventArgs eventArgs
        )
    {
        switch (eventArgs.Key)
        {
            case Key.F2:
                eventArgs.Handled = true;
                _MoveToNextPicture();
                break;

            case Key.F3:
                eventArgs.Handled = true;
                _ChangeScroll();
                break;
        }
    }

    private void _FileList_KeyDown
        (
            object? sender,
            KeyEventArgs eventArgs
        )
    {
        switch (eventArgs.Key)
        {
            case Key.Space:
                eventArgs.Handled = true;
                _MoveToNextPicture();
                break;
        }
    }

    #endregion

}
