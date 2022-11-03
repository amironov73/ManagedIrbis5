// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* MagnaBackbone.cs -- хребет приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM.Interactivity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

#endregion

#nullable enable

namespace AM.AppServices;

/// <summary>
/// Хребет приложения.
/// </summary>
public class MagnaBackbone
    : IMagnaBackbone
{
    #region IApplicationBackbone members

    /// <summary>
    /// Аргументы командной строки.
    /// </summary>
    public string[] Args { get; }

    /// <inheritdoc cref="IMagnaBackbone.ApplicationHost"/>
    public IHost ApplicationHost { get; internal set; }

    /// <inheritdoc cref="IMagnaBackbone.Configuration"/>
    public IConfiguration Configuration { get; internal set; }

    /// <inheritdoc cref="IMagnaBackbone.Logger"/>
    public ILogger Logger { get; internal set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MagnaBackbone
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        ApplicationHost = null!;
        Configuration = null!;
        Logger = null!;

        Args = args;
        _builder = Host.CreateDefaultBuilder (args);
        EarlyInitialization();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MagnaBackbone
        (
            IHostBuilder builder,
            string[] args
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (args);

        ApplicationHost = null!;
        Configuration = null!;
        Logger = null!;

        _builder = builder;
        Args = args;
        EarlyInitialization();
    }

    #endregion

    #region Private members

    private readonly IHostBuilder _builder;
    private ServiceProvider? _preliminaryServices;
    private bool _alreadyInitialized;
    private bool _alreadyShutdown;

    /// <summary>
    /// Первоначальная инициализация.
    /// </summary>
    private void EarlyInitialization()
    {
        ApplicationHost = new HostBuilder().Build(); // это временный хост
        Magna.Host = ApplicationHost;

        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        // Это временный хост, чтобы сделать возможным логирование
        // до того, как всё проинициализируется окончательно
        _preliminaryServices = new ServiceCollection()
            .AddLogging (builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            })
            .BuildServiceProvider();

        // временный логгер
        Logger = _preliminaryServices.GetRequiredService<ILogger<MagnaApplication>>();
        Logger.LogInformation ("Preliminary initialization started");

        Configuration = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine (Args)
            .Build();

        _builder.ConfigureServices
            (
                services =>
                    services.AddSingleton<IInteractivityProvider, ConsoleInteractivityProvider>()
            );

        Logger.LogInformation ("Early initialization done");
    }

    #endregion

    #region IServiceProvider members

    /// <inheritdoc cref="IServiceProvider.GetService"/>
    public object? GetService (Type serviceType) =>
        ApplicationHost.Services.GetService (serviceType);

    #endregion

    #region Protected members

    /// <summary>
    /// Пометка экземпляра как проинициазированного.
    /// </summary>
    protected void MarkAsInitialized() => _alreadyInitialized = true;

    /// <summary>
    /// Пометка экземпляра как отработавшего.
    /// </summary>
    protected void MarkAsShutdown() => _alreadyShutdown = true;

    /// <summary>
    /// Проверяем, не поздно ли инициализироваться.
    /// </summary>
    protected void CheckForLateInitialization()
    {
        if (_alreadyInitialized)
        {
            throw new ArsMagnaException ("Already initialized");
        }
    }

    /// <summary>
    /// Проверяем, не забыли ли мы проинициализироваться.
    /// </summary>
    protected void CheckForgottenInitialization()
    {
        if (!_alreadyInitialized)
        {
            throw new ApplicationException ("Not initialized");
        }
    }

    /// <summary>
    /// Проверяем, не заглушили ли мы приложение.
    /// </summary>
    protected void CheckForShutdown()
    {
        if (_alreadyShutdown)
        {
            throw new ApplicationException ("Backbone is already shutdown");
        }
    }

    #endregion

    #region IApplicationBackbone members

    /// <inheritdoc cref="IMagnaBackbone.ConfigureLogging"/>
    public void ConfigureLogging
        (
            Action<HostBuilderContext, ILoggingBuilder> configureDelegate
        )
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureLogging (configureDelegate);
    }

    /// <inheritdoc cref="IMagnaBackbone.ConfigureServices(System.Action{HostBuilderContext,IServiceCollection})"/>
    public void ConfigureServices
        (
            Action<HostBuilderContext, IServiceCollection> configureDelegate
        )
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureServices (configureDelegate);
    }

    /// <inheritdoc cref="IMagnaBackbone.ConfigureServices(System.Action{IServiceCollection})"/>
    public void ConfigureServices
        (
            Action<IServiceCollection> configureDelegate
        )
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureServices (configureDelegate);
    }

    /// <inheritdoc cref="IMagnaBackbone.Initialize"/>
    public void Initialize()
    {
        if (_alreadyInitialized)
        {
            return;
        }

        Logger.LogInformation ("Final initialization started");

        // освобождаем предварительные сервисы
        _preliminaryServices?.Dispose();
        _preliminaryServices = null;

        _builder.ConfigureLogging (logging =>
        {
            logging.ClearProviders();
            logging.AddNLog (Configuration);
        });
        _builder.ConfigureServices (services =>
        {
            services.AddOptions();
            services.AddLocalization();
        });

        ApplicationHost = _builder.Build();
        Magna.Host = ApplicationHost;
        Logger = ApplicationHost.Services.GetRequiredService<ILogger<MagnaApplication>>();
        Magna.Logger = Logger;
        Configuration = ApplicationHost.Services.GetRequiredService<IConfiguration>();
        Magna.Configuration = Configuration;

        ApplicationHost.Start();

        Logger.LogInformation ("Final initialization done");

        MarkAsInitialized();
    }

    /// <inheritdoc cref="IMagnaBackbone.Shutdown"/>
    public void Shutdown()
    {
        if (_alreadyShutdown)
        {
            return;
        }

        var lifetime = this.GetRequiredService<IHostApplicationLifetime>();
        lifetime.StopApplication();

        MarkAsShutdown();
    }

    /// <summary>
    /// Ожидание завершения приложения.
    /// </summary>
    public void WaitForShutdown()
    {
        CheckForgottenInitialization();

        ApplicationHost.WaitForShutdown();
    }

    #endregion
}
