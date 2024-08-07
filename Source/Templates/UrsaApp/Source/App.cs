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
using Avalonia.Styling;

using Ursa.Themes.Semi;

#endregion

namespace UrsaApp;

/// <summary>
/// Класс приложения.
/// </summary>
public sealed class App
    : Application
{

    /// <inheritdoc cref="Application.Initialize"/>
    public override void Initialize()
    {
        RequestedThemeVariant = ThemeVariant.Light;

        var semiTheme = new SemiTheme();
        var uri = new Uri ("avares://Semi.Avalonia/Themes/Index.axaml");
        var ursa = new StyleInclude (uri)
        {
            Source = uri
        };

        var styles = Current!.Styles;
        styles.Add (ursa);
        styles.Add (semiTheme);
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
