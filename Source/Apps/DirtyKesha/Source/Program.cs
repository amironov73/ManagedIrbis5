// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в демона
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

#endregion

#nullable enable

namespace DirtyKesha;

/// <summary>
/// Логика настройки и запуска демона.
/// </summary>
/// <remarks>Класс нельзя делать <c>static</c> из-за того,
/// что тогда компилятор запрещает использовать на нём дженерики.
/// </remarks>
internal sealed class Program
{
    #region Properties

    /// <summary>
    /// Конфигурация приложения.
    /// </summary>
    public static IConfiguration Configuration { get; private set; } = null!;

    /// <summary>
    /// Общий логгер.
    /// </summary>
    public static ILogger Logger { get; private set; } = null!;

    /// <summary>
    /// Хост приложения
    /// </summary>
    public static IHost ApplicationHost { get; private set; } = null!;

    #endregion

    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    internal static int Main
        (
            string[] args
        )
    {
        try
        {
            if (ServiceHelper.Setup (args))
            {
                return 0;
            }

            Initialize (args);

            ApplicationHost.Run();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception.ToString());

            return 1;
        }

        return 0;
    }

    #endregion

    #region Private members

    private static void Initialize
        (
            string[] args
        )
    {
        var builder = EarlyInitialization (args);
        Demonize (args, builder);
        FinalInitialization (builder);

        ApplicationHost = builder.Build();
        Logger = ApplicationHost.Services.GetRequiredService<ILogger<Program>>();
        Logger.LogInformation ("Initialization complete");
    }

    /// <summary>
    /// Общая инициализация.
    /// </summary>
    private static IHostBuilder EarlyInitialization
        (
            string[] args
        )
    {
        var hostBuilder = Host.CreateDefaultBuilder (args);
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine (args);

        ConfigureUserSecrets (configurationBuilder);

        Configuration = configurationBuilder
            .Build();

        hostBuilder.ConfigureLogging (logging =>
        {
            logging.ClearProviders();
            logging.AddNLog (Configuration);
        });
        hostBuilder.ConfigureServices (services =>
        {
            services.AddOptions();
            services.AddLocalization();
        });

        return hostBuilder;
    }

    /// <summary>
    /// Возня с полльзовательскими секретами.
    /// </summary>
    private static void ConfigureUserSecrets
        (
            IConfigurationBuilder configurationBuilder
        )
    {
        var assembly = Assembly.GetEntryAssembly()!;

        // не забудьте настроить для своих нужд!
        configurationBuilder
            .AddUserSecrets (assembly);
    }

    /// <summary>
    /// Настройка демона.
    /// </summary>
    private static void Demonize
        (
            string[] args,
            IHostBuilder hostBuilder
        )
    {
        // под отладчиком запускаемся как обычное консольное приложение
        var needDemonize = !Debugger.IsAttached;

        // при явном указании на запуск в консоли тоже запускаемся как обычно
        if (args.Length != 0)
        {
            var command = args[0].ToLowerInvariant();
            if (command == "console")
            {
                needDemonize = false;
            }
        }

        if (needDemonize)
        {
            if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux))
            {
                hostBuilder.UseSystemd();
            }
            else if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
            {
                hostBuilder.UseWindowsService();
            }
            else
            {
                ServiceHelper.OperatingSystemIsNotSupported();
            }
        }
    }

    /// <summary>
    /// Специфичная для приложения инициализация.
    /// </summary>
    private static void FinalInitialization
        (
            IHostBuilder hostBuilder
        )
    {
        hostBuilder.ConfigureServices (services =>
        {
            services.AddHostedService<Worker>();
        });

        // добавляйте свою инициализацию сюда
    }

    #endregion
}
