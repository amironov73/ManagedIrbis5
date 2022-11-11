// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* Program.cs -- точка входа в приложение
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

namespace DaemonStandalone;

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
            if (Setup (args))
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
        Demonize (builder);
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
    /// Регистрация демона в системе.
    /// </summary>
    private static bool Setup
        (
            string[] args
        )
    {
        if (args.Length == 0)
        {
            return false;
        }

        var command = args[0].ToLowerInvariant();
        switch (command)
        {
            case "create":
            case "register":
                ServiceControl.RegisterService();
                return true;

            case "delete":
            case "unregister":
                ServiceControl.UnregisterService();
                return true;

            case "start":
                ServiceControl.StartService();
                return true;

            case "stop":
                ServiceControl.StopService();
                return true;

            case "query":
                ServiceControl.QueryServiceStatus();
                return true;
        }

        return false;
    }

    /// <summary>
    /// Настройка демона.
    /// </summary>
    private static void Demonize
        (
            IHostBuilder hostBuilder
        )
    {
        // под отладчиком запускаемся как обычное консольное приложение
        if (!Debugger.IsAttached)
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
                throw new ApplicationException ("Unsupported operating system");
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
