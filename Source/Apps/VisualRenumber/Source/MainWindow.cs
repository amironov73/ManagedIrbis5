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
using System.Reactive.Linq;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Styling;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

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
        Width = MinWidth = 800;
        Height = MinHeight = 550;
        _model = new FolderModel();
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
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness (5),
                        Spacing = 5,

                        Children =
                        {
                            new Button
                            {
                                Content = "Open",
                            },

                            new Button
                            {
                                Content = "Rename",
                            },
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
    }

    #endregion

    #region Program entry point

    private FolderModel _model;
    private ListBox _fileListBox;

    #endregion
}
