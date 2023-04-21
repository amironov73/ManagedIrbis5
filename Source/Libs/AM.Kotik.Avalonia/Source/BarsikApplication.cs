// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* BarsikApplication.cs -- Avalonia-приложение для Barsik
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Avalonia.ThemeManager;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Kotik.Avalonia;

/// <summary>
/// Avalonia-приложение для Barsik.
/// </summary>
[PublicAPI]
public class BarsikApplication
    : Application
{
    #region Properties

    /// <summary>
    /// Конфигурация.
    /// </summary>
    public static IConfiguration Configuration { get; private set; } = null!;

    /// <summary>
    /// Логгер.
    /// </summary>
    public static ILogger Logger { get; private set; } = null!;

    /// <summary>
    /// Хост.
    /// </summary>
    public static IHost ApplicationHost { get; private set; } = null!;

    /// <summary>
    /// Менеджер тем.
    /// </summary>
    public static IThemeManager? ThemeManager;

    /// <summary>
    /// Аргументы командной строки.
    /// </summary>
    public static string[] Arguments = Array.Empty<string>();

    #endregion

    #region Private members

    private static Func <Window>? _mainWindowCreator;

    #endregion

    #region Public methods

    /// <summary>
    /// Запуск десктопного приложения с указанным главным окном.
    /// </summary>
    public static void RunDesktopApplication
        (
            Func<Window> mainWindowCreator
        )
    {
        Sure.NotNull (mainWindowCreator);

        _mainWindowCreator = mainWindowCreator;

        var builder = Host.CreateDefaultBuilder (Arguments);
        Configuration = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine (Arguments)
            .Build();

        builder.ConfigureLogging (logging =>
        {
            logging.ClearProviders();
            logging.AddNLog (Configuration);
        });
        builder.ConfigureServices (services =>
        {
            services.AddOptions();
            services.AddLocalization();
        });

        ApplicationHost = builder.Build();
        Logger = ApplicationHost.Services.GetRequiredService<ILogger<BarsikApplication>>();

        var app = AppBuilder.Configure<BarsikApplication>()
            .UsePlatformDetect()
            .UseReactiveUI()
            .LogToTrace();

        app.StartWithClassicDesktopLifetime (Arguments);
    }

    #endregion

    #region Application members

    /// <inheritdoc cref="Application.Initialize"/>
    public override void Initialize()
    {
        var currentApp = Current.ThrowIfNull();
        var fluentTheme = AvaloniaUtility.CreateFluentTheme().ThrowIfNull();
        currentApp.Styles.Add (fluentTheme);
        // currentApp.Styles.Add (AvaloniaUtility.IncludeDataGridStyles());
        // ThemeManager = new FluentThemeManager();
        // ThemeManager.Initialize (this);
    }

    /// <inheritdoc cref="Application.OnFrameworkInitializationCompleted"/>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = _mainWindowCreator.ThrowIfNull()();
        }

        base.OnFrameworkInitializationCompleted();
    }

    #endregion
}
