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
using Avalonia.ThemeManager;

#endregion

#nullable enable

namespace AvaloniaWeb;

/// <summary>
/// Класс приложения.
/// </summary>
public sealed class App
    : Application
{
    public static IThemeManager? ThemeManager;
    
    /// <inheritdoc cref="Application.Initialize"/>
    public override void Initialize()
    {
        DataTemplates.Add (new ViewLocator());
        
        // стили для датагрида
        var uri = new Uri ("avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml");
        var include = new StyleInclude (uri)
        {
            Source = uri
        };
        Styles.Add (include);

        ThemeManager = new FluentThemeManager();
        ThemeManager.Initialize (this);
    }

    /// <inheritdoc cref="Application.OnFrameworkInitializationCompleted"/>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
