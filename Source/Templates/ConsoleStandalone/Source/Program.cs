// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* Program.cs -- класс приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

#endregion

#nullable enable

namespace ConsoleApp;

/// <summary>
/// Класс приложения.
/// </summary>
/// <remarks>Делать класс static нельзя, слетят дженерики!</remarks>
internal sealed class Program
{
    /// <summary>
    /// Аргументы командной строки.
    /// </summary>
    public static string[] Args { get; internal set; } = null!;

    /// <summary>
    /// Хост приложения.
    /// </summary>
    public static IHost ApplicationHost { get; internal set; } = null!;

    /// <summary>
    /// Конфигурация приложения.
    /// </summary>
    public static IConfiguration Configuration { get; internal set; } = null!;

    /// <summary>
    /// Логгер.
    /// </summary>
    public static ILogger Logger { get; internal set; } = null!;

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    public static void Main
        (
            string[] args
        )
    {
        // для кодировок вроде CP1251
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        Args = args;
        Configuration = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine (Args)
            .Build();

        var hostBuilder = Host.CreateDefaultBuilder (args);
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

        // сюда добавляем свою инициализацию

        ApplicationHost = hostBuilder.Build();
        Logger = ApplicationHost.Services.GetRequiredService<ILogger<Program>>();

        ApplicationHost.Start();

        // сюда добавляем полезную нагрузку

        ApplicationHost.WaitForShutdown();
    }
}
