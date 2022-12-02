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
using Avalonia.Themes.Fluent;

#endregion

#nullable enable

namespace NativeAvaloniaApp;

/// <summary>
/// Класс приложения.
/// </summary>
public sealed class App
    : Application
{
    /// <inheritdoc cref="Application.Initialize"/>
    public override void Initialize()
    {
        Current!.Styles.Add (new FluentTheme (new Uri ("avares://Avalonia.Themes.Fluent/FluentLight.xaml")));
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
