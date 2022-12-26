// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* App.cs -- создание приложения Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia;
using AM.Avalonia.Dialogs;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

#endregion

#nullable enable

namespace VisualRenumber;

internal class App
    : Application
{
    public override void Initialize()
    {
        Name = "Перенумерация";
        var nativeMenu = AboutDialog.BuildNativeMenuAboutApplication();
        SetValue (NativeMenu.MenuProperty, nativeMenu);

        Current!.Styles.Add (AvaloniaUtility.CreateFluentTheme ());
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
