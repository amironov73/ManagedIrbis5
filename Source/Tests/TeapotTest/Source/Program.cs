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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using AM;
using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Searching;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

#endregion

#nullable enable

internal class Program
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
            Initialize (args);


            var found = SearchForBooks ("Файерабенд").Result;
            Console.WriteLine ($"Found: {found.Length}");
            foreach (var one in found)
            {
                Console.WriteLine (one);
            }

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

    #endregion

    private static async Task<string[]> SearchForBooks
        (
            string query
        )
    {
        await using var connection = ConnectionFactory.Shared.CreateAsyncConnection();
        var connectionString = Configuration["connection"].ThrowIfNullOrEmpty();
        connection.ParseConnectionString (connectionString);
        await connection.ConnectAsync();
        if (!connection.IsConnected)
        {
            return Array.Empty<string>();
        }

        var serviceProvider = Magna.Host.Services;
        var teapot = new AsyncTeapotSearcher (serviceProvider);
        var found = await teapot.SearchFormatAsync (connection, query);
        if (found.IsNullOrEmpty())
        {
            return Array.Empty<string>();
        }

        return found
            .Where (line => !string.IsNullOrEmpty (line))
            .Select (line => line.ThrowIfNull())
            .ToArray();
    }

}
