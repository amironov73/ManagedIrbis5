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
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* AvaloniaApplication.cs -- приложение на основе Avalonia UI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

using AM.AppServices;
using AM.Avalonia.Dialogs;
using AM.Interactivity;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Avalonia.AppServices;

/// <summary>
/// Приложение на основе Avalonia UI.
/// </summary>
public class AvaloniaApplication
    : Application,
    IMagnaApplication
{
    #region Properties

    /// <summary>
    /// Главное окно.
    /// </summary>
    public Window MainWindow { get; internal set; } = null!;

    /// <summary>
    /// Список активных окон.
    /// </summary>
    public List<Window> Windows { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AvaloniaApplication()
    {
        _hostBuilder = Host.CreateDefaultBuilder();
        Windows = new List<Window>();
        Args = Array.Empty<string>();
        EarlyInitialization();
    }

    #endregion

    #region Private members

    internal IAvaloniaApplicationBuilder _applicationBuilder = null!;
    private readonly IHostBuilder _hostBuilder;
    private ServiceProvider? _preliminaryServices;

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
    /// Реальная работа приложения.
    /// </summary>
    /// <returns>Код завершения.</returns>
    protected virtual int DoTheWork()
    {
        return 0;
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
        Logger = _preliminaryServices.GetRequiredService<ILogger<MagnaApplication>>();
        Logger.LogInformation ("Preliminary initialization started");

        Configuration = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine (Args)
            .Build();

        _hostBuilder.ConfigureServices
            (
                // TODO сделать соответствующий провайдер интерактивности
                services => services.AddSingleton<IInteractivityProvider, ConsoleInteractivityProvider>()
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

        _hostBuilder.ConfigureLogging (logging =>
        {
            logging.ClearProviders();
            logging.AddNLog (Configuration);
        });
        _hostBuilder.ConfigureServices (services =>
        {
            services.AddOptions();
            services.AddLocalization();
        });

        _hostBuilder.ConfigureServices (services =>
        {
            services.AddSingleton<IWindowFactory, StandardWindowFactory>();
        });

        // запускаем ранее заданные действия по настройке сервисов
        var actions = (_applicationBuilder as DesktopApplication)
            ?._configurationActions;
        if (actions is not null)
        {
            foreach (var action in actions)
            {
                _hostBuilder.ConfigureServices (action);
            }
        }

        ApplicationHost = _hostBuilder.Build();
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

    #region Application members

    /// <inheritdoc cref="Application.Initialize"/>
    public override void Initialize()
    {
        Current!.Styles.Add (new FluentTheme (new Uri("avares://Avalonia.Themes.Fluent/FluentLight.xaml")));
    }

    /// <inheritdoc cref="Application.OnFrameworkInitializationCompleted"/>
    public override void OnFrameworkInitializationCompleted()
    {
        if (!FinalInitialization())
        {
            throw new ApplicationException ("Initialization failed");
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindow = DesktopApplication._instance.CreateMainWindow (this);
            MainWindow.Closed += (_, _) =>
            {
                var lifetime =  RequireService<IHostApplicationLifetime>();
                lifetime.StopApplication();
            };

            desktop.MainWindow = MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание окна указанного типа.
    /// </summary>
    public virtual TWindow CreateWindow<TWindow>()
        where TWindow: Window, new()
    {
        CheckForgottenInitialization();
        CheckForShutdown();

        var factory = ApplicationHost.Services.GetService<IWindowFactory>();

        return factory is not null
            ? factory.CreateWindow<TWindow>()
            : new TWindow();
    }

    /// <summary>
    /// Уничтожение указанного окна.
    /// Метод не занимается закрытием окна, его дело -- реагировать на это закрытие.
    /// </summary>
    public virtual void DestroyWindow
        (
            Window window
        )
    {
        Sure.NotNull (window);

        CheckForgottenInitialization();
        CheckForShutdown();

        var factory = ApplicationHost.Services.GetService<IWindowFactory>();
        if (factory is not null)
        {
            factory.DestroyWindow (window);
        }
    }

    /// <inheritdoc cref="MagnaApplication.HandleException"/>
    public virtual void HandleException
        (
            Exception exception
        )
    {
        ShowException (exception);
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

    /// <summary>
    /// Показ исключения.
    /// </summary>
    public void ShowException
        (
            Exception exception
        )
    {
        Task.Factory.StartNew
            (
                async () =>
                await ExceptionDialog.Show (MainWindow, exception)
            )
            .Forget();

    }

    /// <summary>
    /// Визуальная инициализация.
    /// </summary>
    protected virtual void VisualInitialization()
    {
        // перекрыть в потомке
    }

    /// <summary>
    /// Визуальная де-инициализация.
    /// </summary>
    protected virtual void VisualShutdown()
    {
        // перекрыть в потомке
    }

    #endregion

    #region IMagnaApplication members

    /// <inheritdoc cref="IMagnaApplication.IsInitialized"/>
    public bool IsInitialized { get; protected set; }

    /// <inheritdoc cref="IMagnaApplication.IsShutdown"/>
    public bool IsShutdown { get; protected set; }

    /// <inheritdoc cref="IMagnaApplication.Args"/>
    public string[] Args { get; protected internal set; }

    /// <inheritdoc cref="IMagnaApplication.Configuration"/>
    [AllowNull]
    public IConfiguration Configuration { get; protected set; }

    /// <inheritdoc cref="IMagnaApplication.Logger"/>
    [AllowNull]
    public ILogger Logger { get; protected set; }

    /// <inheritdoc cref="IMagnaApplication.Stop"/>
    public bool Stop { get; set; }

    /// <inheritdoc cref="IMagnaApplication.ApplicationHost"/>
    [AllowNull]
    public IHost ApplicationHost { get; protected set; }

    /// <inheritdoc cref="IMagnaApplication.Run"/>
    public int Run
        (
            Func<IMagnaApplication, int> runDelegate
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
            ApplicationHost.Start();

            VisualInitialization();

            result = runDelegate (this);

            // TODO разобраться, когда вызывать VisualShutdown
            VisualShutdown();

            ApplicationHost.WaitForShutdown();
            MarkAsShutdown();
        }
        catch (Exception exception)
        {
            HandleException (exception);
        }

        Cleanup();

        ApplicationHost.Dispose();
        MarkAsShutdown();

        return result;
    }

    /// <inheritdoc cref="RunAsync"/>
    public Task<int> RunAsync
        (
            Func<IMagnaApplication, Task<int>> runDelegate
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Использовать указанные аргументы командной строки.
    /// </summary>
    public void UseArgs
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        Args = args;
    }

    #endregion
}
