// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

/* AboutWindow.cs -- окно "О приложении"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace AM.Avalonia.Dialogs;

/// <summary>
/// Окно "О приложении".
/// </summary>
public partial class AboutDialog
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
        AvaloniaXamlLoader.Load (this);

        ProductTitle = "Проект ARS MAGNA";
        ProductVersion = "0.0.0.0";
            // TODO восстановить
            // ThisAssembly.AssemblyFileVersion;
        GitCommit = "0.0.0.0";
            // TODO восстановить
            // ThisAssembly.GitCommitId;
        ProductDate ="0.0.0.0";
            // TODO восстановить
            // ThisAssembly.GitCommitDate.ToString (CultureInfo.CurrentUICulture);

        DataContext = this;

#if DEBUG
        this.AttachDevTools();
#endif
    }

    #endregion

    #region Private members

    private void OkButton_OnClick (object? sender, RoutedEventArgs e)
    {
        Close();
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
                Command = ReactiveCommand.Create (ShowAboutWindow)
            }
        };
    }

    /// <summary>
    /// Показ окна "О приложении".
    /// </summary>
    public static void ShowAboutWindow()
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
