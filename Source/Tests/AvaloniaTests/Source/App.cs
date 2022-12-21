// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* App.cs -- класс приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Themes.Fluent;

#endregion

#nullable enable

namespace AvaloniaTests;

/// <summary>
/// Класс приложения.
/// </summary>
public sealed class App
    : Application
{
    /// <inheritdoc cref="Application.Initialize"/>
    public override void Initialize()
    {
        Styles.Add (new FluentTheme (new Uri("avares://Avalonia.Themes.Fluent/FluentLight.xaml")));

        // стили для датагрида
        var uri = new Uri ("avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml");
        var include = new StyleInclude (uri) { Source = uri };
        Styles.Add (include);

        // стили из AM.Avalonia
        uri = new Uri ("avares://AM.Avalonia/Styles.axaml");
        include = new StyleInclude (uri) { Source = uri };
        Styles.Add (include);
    }

    /// <inheritdoc cref="Application.OnFrameworkInitializationCompleted"/>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
