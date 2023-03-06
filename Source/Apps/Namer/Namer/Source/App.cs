// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* App.cs -- создание приложения Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Avalonia;
using AM.Avalonia.Dialogs;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;

#endregion

#nullable enable

namespace Namer;

internal class App
    : Application
{
    public override void Initialize()
    {
        Name = "Перенумерация";
        var nativeMenu = AboutDialog.BuildNativeMenuAboutApplication();
        SetValue (NativeMenu.MenuProperty, nativeMenu);

        Current!.Styles.Add (AvaloniaUtility.CreateFluentTheme());

        var gridUri = new Uri ("avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml");
        var includeGrid = new StyleInclude (gridUri)
        {
            Source = gridUri
        };
        Current!.Styles.Add (includeGrid);

        //Current!.Styles.Add (AvaloniaUtility.CreateMaterialTheme());
        // Current!.Styles.Add (AvaloniaUtility.CreateSimpleTheme());
        // Current!.Styles.Add (AvaloniaUtility.CreateCitrusTheme());
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
