// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* AboutWindow.cs -- диалог "О приложении"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace AM.Avalonia.Dialogs;

/// <summary>
/// Диалог "О приложении".
/// </summary>
public class AboutDialog
    : Window
{
    #region Properties

    /// <summary>
    /// Наименование продукта
    /// </summary>
    [Reactive]
    public string? ProductTitle { get; set; }

    /// <summary>
    /// Версия продукта.
    /// </summary>
    [Reactive]
    public string? ProductVersion { get; set; }

    /// <summary>
    /// Коммит Git.
    /// </summary>
    [Reactive]
    public string? GitCommit { get; set; }

    /// <summary>
    /// Дата коммита.
    /// </summary>
    [Reactive]
    public string? ProductDate { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public AboutDialog()
    {
        this.AttachDevTools();

        Title = "О продукте";
        Width = MinWidth = MaxWidth = 600;
        Height = MinHeight = MaxHeight = 300;
        ProductTitle = "Проект ARS MAGNA";
        ProductVersion = ThisAssembly.AssemblyFileVersion;
        GitCommit = ThisAssembly.GitCommitId;
        ProductDate = ThisAssembly.GitCommitDate.ToString (CultureInfo.CurrentUICulture);

            Content = new StackPanel
            {
                Spacing = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,

                Children =
                {
                    new Image
                    {
                        Width = 200,
                        Source = this.LoadBitmapFromAssets ("Assets/about.png").ThrowIfNull()
                    },

                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeight.Bold,
                        [!ContentProperty] = new Binding (nameof (ProductTitle))
                    },

                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeight.Bold,
                        [!ContentProperty] = new Binding (nameof (ProductVersion))
                    },

                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeight.Bold,
                        [!ContentProperty] = new Binding (nameof (ProductDate))
                    },

                    new Button
                    {
                        Content = "OK",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Width = 300,
                        Command = ReactiveCommand.Create (Close)
                    }
                }
            };
            DataContext = this;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение стандартного меню "О приложении" для OSX.
    /// </summary>
    public static NativeMenu BuildNativeMenuAboutApplication()
    {
        return new NativeMenu
        {
            new NativeMenuItem
            {
                Header = "About Peeper",
                Command = ReactiveCommand.Create (ShowAboutDialog)
            }
        };
    }

    /// <summary>
    /// Показ окна "О приложении".
    /// </summary>
    public static void ShowAboutDialog()
    {
        if (Application.Current?.ApplicationLifetime
            is ClassicDesktopStyleApplicationLifetime { MainWindow: {} mainWindow })
        {
            var aboutWindow = new AboutDialog();
            aboutWindow.ShowDialog (mainWindow);
        }
    }

    #endregion
}
