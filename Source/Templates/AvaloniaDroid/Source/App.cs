// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* App.cs -- приложение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM;
using AM.Avalonia.AppServices;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Hosting;

#endregion

#nullable enable

namespace AvaloniaDroid;

/// <summary>
/// Приложение.
/// </summary>
internal sealed class App
    : Application
{
    /// <inheritdoc cref="AvaloniaApplication.Initialize"/>
    public override void Initialize()
    {
        base.Initialize();

        Magna.Initialize (Array.Empty<string>());

        var uri = new Uri ("avares://Avalonia.Themes.Fluent/FluentLight.xaml");
        var theme = new FluentTheme (uri)
        {
            Mode = FluentThemeMode.Light
        };
        Styles.Add (theme);

        // добавить сюда свою инициализацию
    }

    /// <inheritdoc cref="AvaloniaApplication.OnFrameworkInitializationCompleted"/>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
