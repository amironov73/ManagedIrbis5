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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

#endregion

namespace EasyCaption;

/// <summary>
/// Главное окно приложения
/// </summary>
public sealed class MainWindow
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

    private void SelectionChangedHandler
        (
            object? sender,
            SelectionChangedEventArgs eventArgs
        )
    {
        if (Folder is not null)
        {
            Folder.Current = _imageList?.Selection.SelectedItem as Caption;
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

        Title = "Калькулятор Avalonia";
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
        };

        _imageList.SelectionChanged += SelectionChangedHandler;

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

        var content = new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse ("300,4,*"),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                _imageList,
                splitter,
                editor
            }
        };

        Content = content;

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
