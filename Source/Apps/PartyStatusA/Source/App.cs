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

using ActiproSoftware.UI.Avalonia.Themes;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;

#endregion

#nullable enable

namespace PartyStatusA;

/// <summary>
/// Класс приложения.
/// </summary>
public sealed class App
    : Application
{
    /// <inheritdoc cref="Application.Initialize"/>
    public override void Initialize()
    {
        // var theme = new FluentTheme();
        var theme = new ModernTheme();
        Styles.Add (theme);
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
