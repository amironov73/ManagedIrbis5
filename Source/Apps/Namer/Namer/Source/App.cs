// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* App.cs -- создание приложения Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Avalonia;
using AM.Avalonia.Dialogs;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

#endregion

#nullable enable

namespace Namer;

internal class App
    : Application
{
    public override void Initialize()
    {
        Name = "Переименование";
        var nativeMenu = AboutDialog.BuildNativeMenuAboutApplication();
        SetValue (NativeMenu.MenuProperty, nativeMenu);

        var currentApp = Current.ThrowIfNull();
        currentApp.Styles.Add (AvaloniaUtility.CreateFluentTheme());
        currentApp.Styles.Add (AvaloniaUtility.IncludeDataGridStyles());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
