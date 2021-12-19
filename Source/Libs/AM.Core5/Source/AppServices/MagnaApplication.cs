// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* MagnaApplication.cs -- класс-приложение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using NLog.Extensions.Logging;

#endregion

#nullable enable

namespace AM.AppServices;

/// <summary>
/// Класс-приложение.
/// </summary>
public class MagnaApplication
{
    #region Properties

    /// <summary>
    /// Аргументы командной строки.
    /// </summary>
    public string[] Args { get; }

    /// <summary>
    /// Результат разбора командной строки.
    /// </summary>
    public ParseResult? ParseResult { get; protected set; }

    /// <summary>
    /// Конфигурация.
    /// </summary>
    [AllowNull]
    public IConfiguration Configuration { get; protected set; }

    /// <summary>
    /// Логгер.
    /// </summary>
    [AllowNull]
    public ILogger Logger { get; protected set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    public MagnaApplication
        (
            string[] args
        )
    {
        Args = args;
    } // constructor

    #endregion

    #region Private members

    private bool _prerun;

    #endregion

    #region Public methods

    /// <summary>
    /// Построение конфигурации.
    /// </summary>
    protected virtual IConfigurationBuilder BuildConfiguration()
    {
        var result = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine (Args);

        return result;
    } // method BuildConfiguration

    /// <summary>
    /// Построение хоста.
    /// </summary>
    protected virtual IHostBuilder BuildHost() => Host.CreateDefaultBuilder (Args);

    /// <summary>
    /// Корневая команда для разбора командной строки.
    /// </summary>
    protected virtual RootCommand? BuildRootCommand() => null;

    /// <summary>
    /// Конфигурирование сервисов.
    /// </summary>
    /// <param name="context">Контекст.</param>
    /// <param name="services">Коллекция сервисов.</param>
    protected virtual void ConfigureServices
        (
            HostBuilderContext context,
            IServiceCollection services
        )
    {
        services.AddOptions();
    } // method ConfigureServices

    /// <summary>
    /// Конфигурирование логирования.
    /// </summary>
    /// <param name="logging">Билдер.</param>
    protected virtual void ConfigureLogging
        (
            ILoggingBuilder logging
        )
    {
        logging.ClearProviders();
        logging.AddNLog (Configuration);
    } // method ConfigureLogging

    /// <summary>
    /// Разбор командной строки.
    /// </summary>
    protected virtual ParseResult? ParseCommandLine()
    {
        var rootCommand = BuildRootCommand();
        if (rootCommand is null)
        {
            return null;
        }

        var result = new CommandLineBuilder (rootCommand)
            .UseDefaults()
            .Build()
            .Parse (Args);

        return result;
    } // method ParseCommandLine

    /// <summary>
    /// Конфигурирование перед запуском.
    /// </summary>
    protected virtual MagnaApplication PreRun()
    {
        if (_prerun)
        {
            return this;
        }

        // Это временный хост, чтобы сделать возможным логирование
        // до того, как всё проинициализируется окончательно
        var preliminaryServices = new ServiceCollection()
            .AddLogging (builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            })
            .BuildServiceProvider();

        Logger = preliminaryServices.GetRequiredService<ILogger<MagnaApplication>>();
        Logger.LogInformation ("Preliminary logging enabled");

        Magna.Application = this;
        Configuration = BuildConfiguration().Build();
        ParseResult = ParseCommandLine();

        var hostBuilder = BuildHost();
        hostBuilder.ConfigureServices (ConfigureServices);
        hostBuilder.ConfigureServices
            (
                serviceCollection => serviceCollection.AddLogging (ConfigureLogging)
            );

        var host = hostBuilder.Build();
        Magna.Host = host;

        Logger.LogInformation ("Switching to main logging");
        preliminaryServices.Dispose();
        Logger = host.Services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger<MagnaApplication>();

        _prerun = true;

        Logger.LogInformation ("Pre-run configuration done");

        return this;
    }

    /// <summary>
    /// Собственно работа приложения.
    /// Метод должен быть переопределен в классе-потомке.
    /// </summary>
    /// <returns>Код, возвращаемый операционной системе.</returns>
    protected virtual int ActualRun()
    {
        return 0;
    }

    /// <summary>
    /// Собственно работа приложения.
    /// </summary>
    /// <returns>Код, возвращаемый операционной системе.
    /// </returns>
    public virtual int Run()
    {
        try
        {
            Logger = new NullLogger<MagnaApplication>();

            PreRun();

            using var host = Magna.Host;

            Magna.Host.Start();

            return ActualRun();
        }
        catch (Exception exception)
        {
            Logger.LogError
                (
                    exception,
                    nameof (MagnaApplication) + "::" + nameof (Run)
                );
        }

        return 1;
    }

    #endregion
}
