// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* MagnaApplication.cs -- класс-приложение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

using AM.Interactivity;

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
    : IMagnaApplication
{
    #region Events

    /// <summary>
    /// Вызывается при возникновении исключения.
    /// </summary>
    public event EventHandler<UnhandledExceptionEventArgs>? ExceptionOccurs;

    #endregion

    #region Properties

    /// <inheritdoc cref="IMagnaApplication.IsInitialized"/>
    public bool IsInitialized { get; private set; }

    /// <inheritdoc cref="IMagnaApplication.IsShutdown"/>
    public bool IsShutdown { get; private set; }

    /// <inheritdoc cref="IMagnaApplication.Args"/>
    public string[] Args { get; }

    /// <summary>
    /// Результат разбора командной строки.
    /// </summary>
    public ParseResult? CommandLineParseResult { get; protected set; }

    /// <inheritdoc cref="IMagnaApplication.Configuration"/>
    [AllowNull]
    public IConfiguration Configuration { get; protected internal set; }

    /// <inheritdoc cref="IMagnaApplication.Logger"/>
    [AllowNull]
    public ILogger Logger { get; protected set; }

    /// <inheritdoc cref="IMagnaApplication.Stop"/>
    public bool Stop { get; set; }

    /// <inheritdoc cref="IMagnaApplication.ApplicationHost"/>
    [AllowNull]
    public IHost ApplicationHost { get; protected internal set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MagnaApplication
        (
            IHostBuilder builder,
            string[]? args = null,
            bool turnOffLogging = false
        )
    {
        Sure.NotNull (builder);

        _turnOffLogging = turnOffLogging;
        _builder = builder;
        Args = args ?? Array.Empty<string>();
        EarlyInitialization();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <param name="turnOffLogging">Отключение логирования.</param>
    public MagnaApplication
        (
            string[] args,
            bool turnOffLogging = false
        )
    {
        Sure.NotNull (args);

        _turnOffLogging = turnOffLogging;
        Args = args;
        _builder = Host.CreateDefaultBuilder (args);
        EarlyInitialization();
    }

    #endregion

    #region Private members

    private readonly IHostBuilder _builder;
    private ServiceProvider? _preliminaryServices;
    private bool _turnOffLogging;

    /// <summary>
    /// Пометка экземпляра как проинициазированного.
    /// </summary>
    protected void MarkAsInitialized()
    {
        IsInitialized = true;
    }

    /// <summary>
    /// Пометка экземпляра как отработавшего.
    /// </summary>
    protected void MarkAsShutdown()
    {
        IsShutdown = true;
    }

    /// <summary>
    /// Проверяем, не поздно ли инициализироваться.
    /// </summary>
    protected void CheckForLateInitialization()
    {
        if (IsInitialized)
        {
            throw new ApplicationException ("Too late");
        }
    }

    /// <summary>
    /// Проверяем, не забыли ли мы проинициализироваться.
    /// </summary>
    protected void CheckForgottenInitialization()
    {
        if (!IsInitialized)
        {
            throw new ApplicationException ("Not initialized");
        }
    }

    /// <summary>
    /// Проверяем, не заглушили ли мы приложение.
    /// </summary>
    protected void CheckForShutdown()
    {
        if (IsShutdown)
        {
            throw new ApplicationException ("Application is already completed");
        }
    }

    /// <summary>
    /// Первоначальная инициализация.
    /// </summary>
    protected virtual void EarlyInitialization()
    {
        Magna.Application = this;
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
        Logger = _turnOffLogging
            ? new NullLoggerFactory().CreateLogger<MagnaApplication> ()
            : _preliminaryServices.GetRequiredService<ILogger<MagnaApplication>>();
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

    /// <summary>
    /// Окончательная инициализация.
    /// </summary>
    protected virtual bool FinalInitialization()
    {
        if (IsInitialized)
        {
            return true;
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
        MarkAsInitialized();
        Logger = ApplicationHost.Services.GetRequiredService<ILogger<MagnaApplication>>();
        Magna.Logger = Logger;
        Configuration = ApplicationHost.Services.GetRequiredService<IConfiguration>();
        Magna.Configuration = Configuration;

        Logger.LogInformation ("Final initialization done");

        return true;
    }

    /// <summary>
    /// Вызывается в конце <see cref="Run(Func{IMagnaApplication,int})"/> и <see cref="RunAsync"/>.
    /// </summary>
    protected virtual void Cleanup()
    {
        // пустое тело метода
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Настройка конфигурации для построения хоста приложения.
    /// </summary>
    public void ConfigureAppConfiguration
        (
            Action<HostBuilderContext,IConfigurationBuilder> configureDelegate
        )
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureAppConfiguration (configureDelegate);
    }

    /// <summary>
    /// Настройка прекращения текущей операции по требованию пользователя.
    /// </summary>
    public virtual void ConfigureCancelKey()
    {
        Console.TreatControlCAsInput = false;
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            Stop = true;
            eventArgs.Cancel = true;
        };
    }

    /// <summary>
    /// Настройка контейнера сервисов для хоста приложения.
    /// </summary>
    public void ConfigureContainer<TContainerBuilder>
        (
            Action<HostBuilderContext, TContainerBuilder> configureDelegate
        )
        where TContainerBuilder: IContainer
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureContainer (configureDelegate);
    }

    /// <summary>
    /// Настройка конфигурации для самого построителя хоста.
    /// </summary>
    public virtual void ConfigureHostConfiguration
        (
            Action<IConfigurationBuilder> configureDelegate
        )
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureHostConfiguration (configureDelegate);
    }

    /// <summary>
    /// Настройка логирования.
    /// </summary>
    public virtual void ConfigureLogging
        (
            Action<HostBuilderContext, ILoggingBuilder> configureDelegate
        )
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureLogging (configureDelegate);
    }

    /// <summary>
    /// Добавление сервисов в контейнер. Метод может быть вызван несколько раз.
    /// </summary>
    public virtual void ConfigureServices
        (
            Action<HostBuilderContext, IServiceCollection> configureDelegate
        )
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureServices (configureDelegate);
    }

    /// <summary>
    /// Добавление сервисов в контейнер. Метод может быть вызван несколько раз.
    /// </summary>
    public virtual void ConfigureServices
        (
            Action<IServiceCollection> configureDelegate
        )
    {
        Sure.NotNull (configureDelegate);
        CheckForLateInitialization();

        _builder.ConfigureServices (configureDelegate);
    }

    /// <summary>
    /// Обработка исключения.
    /// </summary>
    public virtual void HandleException
        (
            Exception exception
        )
    {
        Sure.NotNull (exception);

        Logger.LogError
            (
                exception,
                nameof (MagnaApplication) + "::" + nameof (Run)
            );

        var handler = ExceptionOccurs;
        if (handler is not null)
        {
            var eventArgs = new UnhandledExceptionEventArgs (exception, true);
            handler (this, eventArgs);
        }
    }

    /// <summary>
    /// Разбор командной строки.
    /// </summary>
    public virtual void ParseCommandLine
        (
            Func<RootCommand> rootCommandDelegate
        )
    {
        Sure.NotNull (rootCommandDelegate);
        CheckForLateInitialization();

        var rootCommand = rootCommandDelegate();
        CommandLineParseResult = new CommandLineBuilder (rootCommand)
            .UseDefaults()
            .Build()
            .Parse (Args);
    }

    /// <summary>
    /// Запрос сервиса, который обязательно должен быть.
    /// </summary>
    public virtual TService RequireService<TService>()
        where TService: class
    {
        CheckForgottenInitialization();
        CheckForShutdown();

        return ApplicationHost.Services.GetRequiredService<TService>();
    }

    /// <inheritdoc cref="IMagnaApplication.Run"/>
    public virtual int Run
        (
            Func<IMagnaApplication, int> runDelegate
        )
    {
        Sure.NotNull (runDelegate);

        Run();
        var result = int.MaxValue;
        try
        {
            result = runDelegate (this);

            // ожидаем остановки хоста
            ApplicationHost.WaitForShutdown();
        }
        catch (Exception exception)
        {
            HandleException (exception);
        }

        Shutdown();

        return result;
    }

    /// <summary>
    /// Запуск приложения.
    /// </summary>
    public virtual async Task<int> RunAsync
        (
            Func<IMagnaApplication, Task<int>> runDelegate
        )
    {
        Sure.NotNull (runDelegate);

        if (!FinalInitialization())
        {
            return int.MaxValue;
        }

        var result = int.MaxValue;
        try
        {
            await ApplicationHost.StartAsync();

            result = await runDelegate (this);

            // ожидаем остановки хоста
            await ApplicationHost.WaitForShutdownAsync();
            MarkAsShutdown();
        }
        catch (Exception exception)
        {
            HandleException (exception);
        }

        Cleanup();

        // глушим хост
        ApplicationHost.Dispose();
        MarkAsShutdown(); // повторяем для надежности

        return result;
    }

    /// <summary>
    /// Фабрика, используемая для создания провайдера сервисов.
    /// </summary>
    public virtual void UseServiceProviderFactory<TContainerBuilder>
        (
            IServiceProviderFactory<TContainerBuilder> providerFactory
        )
        where TContainerBuilder: IContainer
    {
        Sure.NotNull (providerFactory);
        CheckForLateInitialization();

        _builder.UseServiceProviderFactory (providerFactory);
    }

    /// <summary>
    /// Ожидание завершения приложения.
    /// </summary>
    public virtual void WaitForShutdown()
    {
        CheckForgottenInitialization();

        ApplicationHost.WaitForShutdown();
    }

    /// <summary>
    /// Ожидание завершения приложения.
    /// </summary>
    public virtual Task WaitForShutdownAsync()
    {
        CheckForgottenInitialization();

        return ApplicationHost.WaitForShutdownAsync();
    }

    #endregion

    #region IMagnaApplication members

    /// <summary>
    /// Запуск приложения.
    /// </summary>
    public void Run()
    {
        if (!FinalInitialization())
        {
            throw new ArsMagnaException();
        }

        ApplicationHost.Start();
    }

    /// <summary>
    /// Отправка хосту команды "пора завершаться".
    /// </summary>
    public void Shutdown()
    {
        if (IsShutdown || !IsInitialized)
        {
            return;
        }

        Cleanup();

        var lifetime = ApplicationHost.Services
            .GetRequiredService<IHostApplicationLifetime>()
            .ThrowIfNull ();
        lifetime.StopApplication();
        MarkAsShutdown();
    }

    #endregion
}
