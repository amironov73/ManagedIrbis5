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
using AM.Avalonia.AppServices;
using AM.Collections;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Styling;
using Avalonia.Threading;

#endregion

namespace EasyCaption;

/// <summary>
/// Главное окно приложения
/// </summary>
internal sealed class MainWindow
    : ReactiveWindow<Folder>
{
    #region Properties

    /// <summary>
    /// Просматриваемая папка.
    /// </summary>
    public Folder? Folder { get; set; }

    #endregion

    #region Private members

    private ListBox? _imageList;
    private ListBox? _statList;

    /// <summary>
    /// Вызывается для автоматической синхронизации с файловой системой.
    /// </summary>
    private void Synchronize()
    {
        if (Folder is not null)
        {
            Folder.Synchronize();
        }
    }

    private bool _guard;

    private void SelectionChangedHandler
        (
            object? sender,
            SelectionChangedEventArgs eventArgs
        )
    {
        if (_guard)
        {
            return;
        }

        _guard = true;
        try
        {
            if (Folder is not null)
            {
                Folder.Current = _imageList?.Selection.SelectedItem as Caption;

                // обновляем список токенов и восстанавливаем,
                // если это возможно, предыдущее положение в нем
                var currentToken = GetCurrentToken();
                Folder.Stat = Folder.GetTagStat();
                if (Folder.Stat is { } stat)
                {
                    foreach (var one in stat)
                    {
                        if (_statList is not null
                            && one.Tag == currentToken)
                        {
                            _statList.SelectedItem = one;
                        }
                    }
                }
            }
        }
        finally
        {
            _guard = false;
        }

        // автоматическая синхронизация
        Synchronize();
    }

    private void SelectFirstElement() => MoveToItem (0);

    private int GetCurrentIndex() => _imageList?.SelectedIndex ?? 0;

    private void MoveToItem
        (
            int index
        )
    {
        if (Folder?.Captions is { Length: not 0 and var length} and var captions
            && _imageList is not null)
        {
            while (index < 0)
            {
                index += length;
            }

            while (index >= length)
            {
                index -= length;
            }

            _imageList.SelectedItem = captions[index];
        }
    }

    private Style CreateListStyle() => new (static x => x.OfType<ListBoxItem>())
    {
        Setters =
        {
            new Setter (MarginProperty, new Thickness (0)),
            new Setter (PaddingProperty, new Thickness (10, 2))
        }
    };

    private string? GetCurrentToken() => _statList?.SelectedItem is not TokenStat stat
        ? null
        : stat.Tag;

    private void SearchForToken
        (
            string? token
        )
    {
        if (string.IsNullOrEmpty (token)
            || Folder?.Captions is not { } captions)
        {
            return;
        }

        var index = GetCurrentIndex() + 1;
        for (; index < captions.Length; index++)
        {
            var caption = captions[index];
            var text = caption.Text;
            if (!string.IsNullOrEmpty (text))
            {
                var dictionary = new DictionaryCounter<string, int>();
                var tokens = new TokenCounter (text, dictionary);
                tokens.CountLorasAndTokens();
                if (dictionary.ContainsKey (token))
                {
                    MoveToItem (index);
                    return;
                }
            }
        }
    }

    private void SearchFromBeginning()
    {
        MoveToItem (0);
        SearchForToken (GetCurrentToken());
    }

    private void StatListOnPointerPressed
        (
            object? sender,
            PointerPressedEventArgs eventArgs
        )
    {
        // TODO разобраться, почему не работает

        var point = eventArgs.GetCurrentPoint(sender as Control);
        if (point.Properties.IsLeftButtonPressed
            && eventArgs.ClickCount == 2)
        {
            SearchFromBeginning();
            eventArgs.Handled = true;
        }
    }


    #endregion

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.AttachDevTools();
        this.SetWindowIcon ("caption.ico");

        Title = "Редактор подсказок";
        Width = MinWidth = 760;
        Height = MinHeight = 560;

        Folder = new Folder();
        if (Magna.Args.Length != 0)
        {
            Folder.ScanForImages (Magna.Args[0]);
        }

        DataContext = Folder;

        _imageList = new ListBox
        {
            [Grid.ColumnProperty] = 0,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            ItemTemplate = new FuncDataTemplate<Caption> ((_, _) => new CaptionControl()),
            SelectionMode = SelectionMode.Single,
            [!ItemsControl.ItemsSourceProperty] = new Binding (nameof (Folder.Captions)),
            Styles = { CreateListStyle() }
        };

        _imageList.SelectionChanged += SelectionChangedHandler;

        _statList = new ListBox
        {
            [Grid.RowProperty] = 2,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            [!ItemsControl.ItemsSourceProperty] = new Binding (nameof (Folder.Stat)),
            Styles = { CreateListStyle() }
        };

        _statList.PointerPressed += StatListOnPointerPressed;

        var leftPane = new Grid
        {
            RowDefinitions = RowDefinitions.Parse ("*,4,300"),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                _imageList,
                new GridSplitter
                {
                    [Grid.RowProperty] = 1,
                    Background = Brushes.Black,
                    ResizeDirection = GridResizeDirection.Rows
                },
                _statList
            }
        };

        var splitter = new GridSplitter
        {
            [Grid.ColumnProperty] = 1,
            Background = Brushes.Black,
            ResizeDirection = GridResizeDirection.Columns
        };

        var editor = new CaptionEditor
        {
            [Grid.ColumnProperty] = 2,
            [!DataContextProperty] = new Binding (nameof (Folder.Current)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        var toolbelt = new Toolbelt
        {
            [Grid.RowProperty] = 1,
            [Grid.ColumnSpanProperty] = 3,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        var content = new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse ("300,4,*"),
            RowDefinitions = RowDefinitions.Parse ("*,Auto"),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                leftPane,
                splitter,
                editor,
                toolbelt
            }
        };

        Content = content;

        // KeyDownEvent.AddClassHandler<Window> (OnKeyDown);
        DispatcherTimer.RunOnce (SelectFirstElement, TimeSpan.FromSeconds (1));
    }

    protected override void OnClosed
        (
            EventArgs eventArgs
        )
    {
        // автоматическая синхронизация
        Synchronize();

        base.OnClosed (eventArgs);
    }

    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        base.OnKeyDown (eventArgs);

        switch (eventArgs.Key)
        {
            case Key.F2:
                // переход к следующей картинке
                MoveToItem (GetCurrentIndex() + 1);
                eventArgs.Handled = true;
                break;

            case Key.F3:
                // переход к предыдущей картинке
                MoveToItem (GetCurrentIndex() - 1);
                eventArgs.Handled = true;
                break;

            case Key.F4:
                // поиск других картинок с текущим токеном
                SearchForToken (GetCurrentToken());
                eventArgs.Handled = true;
                break;

            case Key.F5:
                // принудительная синхронизация
                Synchronize();
                eventArgs.Handled = true;
                break;

            case Key.F6:
                // замена текущего токена
                eventArgs.Handled = true;
                break;

            case Key.F7:
                // добавление токена во все картинки
                eventArgs.Handled = true;
                break;

            case Key.F8:
                // удаление текущего токена
                eventArgs.Handled = true;
                break;
        }
    }

    #endregion

    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        DesktopApplication
            .Run<MainWindow> (args);
    }

    #endregion
}
