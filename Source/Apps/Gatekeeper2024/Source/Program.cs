// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- настройка и запуск Web-приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using Microsoft.Extensions.Logging.Abstractions;

using NLog.Web;

using RestSharp;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

#endregion

namespace Gatekeeper2024;

internal sealed /* нельзя static */ class Program
{
    #region Properties

    /// <summary>
    /// Ссылка на веб-приложение.
    /// </summary>
    public static WebApplication Application { get; private set; } = null!;

    /// <summary>
    /// Общий логгер для наших классов.
    /// </summary>
    public static ILogger Logger { get; private set; } = null!;

    #endregion

    #region Main

    internal static void Main
        (
            string[] args
        )
    {
        // *******************************************************************
        // начальная настройка

        // временный логгер, будет заменен постоянным
        Logger = new NullLoggerFactory().CreateLogger ("null");

        // *******************************************************************

        var builder = WebApplication.CreateBuilder (args);

        // *******************************************************************
        // настройка конфигурации

        BuildConfiguration (builder, args);

        // *******************************************************************
        // остановка приложения по требованию

        if (args is ["stop"] or ["/stop"])
        {
            RequestStop (builder, args);
            return;
        }

        // *******************************************************************
        // настройка логирования

        var logging = builder.Logging;
        logging.ClearProviders();
        logging.SetMinimumLevel (LogLevel.Information);
        builder.Host.UseNLog();

        // *******************************************************************
        // регистрация сервисов

        var services = builder.Services;
        services.AddMemoryCache (options =>
        {
            options.SizeLimit = 1024;
            options.ExpirationScanFrequency = TimeSpan.FromMinutes (1);
        });
        services.AddHostedService<EventUploader>();
        services.AddSingleton<SigurHandler>();
        services.AddSingleton<HistoryProvider>();
        services.AddSingleton<StatProvider>();

        // *******************************************************************
        // предварительная настройка завершена, создаем объект приложения

        var app = builder.Build();

        Application = app;
        Logger = app.Services.GetRequiredService <ILogger<Program>>();

        // *******************************************************************
        // отдача статических файлов

        // При использовании UseDefaultFiles запросы к папке в wwwroot будут искать следующие файлы:
        // default.htm
        // default.html
        // index.htm
        // index.html
        // app.UseDefaultFiles();

        app.UseStaticFiles();
        app.MapGet ("/", () => Results.Redirect ("/index.html"));

        // UseFileServer объединяет функции UseStaticFiles, UseDefaultFiles
        // и при необходимости UseDirectoryBrowser. Но мы ее не используем

        // *******************************************************************
        // добавляем Middleware

        // включаем заголовок "Cache-Control: no-cache" для всех файлов,
        // чтобы не заморачиваться с закешировавшимися скриптами
        app.UseMiddleware<NoCacheMiddleware>();

        // *******************************************************************
        // создаем endpoint'ы

        var api = app.MapGroup("/api");
        api.MapGet ("state", HandleStateRequest);
        api.MapGet ("history", HandleHistoryRequest);
        api.MapGet ("stat", HandleStatRequest);
        api.MapGet ("stop", HandleStopRequest);
        api.MapGet ("test", HandleTestRequest);
        api.MapGet ("ok", HandleOkRequest);
        api.MapGet ("version", HandleVersionRequest);
        api.MapGet ("blat", HandlePrivateRequest);

        app.MapPost ("/auth", HandleAuthRequest);

        GlobalState.SetMessageWithTimestamp ("Пока никаких событий не зафиксировано");
        Logger.LogInformation ("Application startup");

        app.Run();
    }

    #endregion

    #region HTTP request handlers

    private static IResult HandleTestRequest()
    {
        try
        {
            Utility.TestIrbisConnection();
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while testing IRBIS connection");
        }

        return Results.Ok();
    }

    private static IResult HandleAuthRequest
        (
            HttpContext context,
            SigurHandler handler
        )
    {
        try
        {
            return handler.HandleRequest (context);
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while handling Sigur request");
        }

        return Results.Problem();
    }

    private static IResult HandleHistoryRequest
        (
            HttpContext context,
            HistoryProvider provider
        )
    {
        try
        {
            return provider.HandleRequest (context);
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while gathering history");
        }

        return Results.Problem();
    }

    private static IResult HandleStatRequest
        (
            HttpContext context,
            StatProvider provider
        )
    {
        try
        {
            return provider.HandleRequest (context);
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while handling stat request");
        }

        return Results.Problem();
    }

    private static IResult HandleStateRequest()
    {
        try
        {
            return Results.Json (GlobalState.Instance);
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while handling state request");
        }

        return Results.Problem();
    }

    private static void HandleStopRequest()
    {
        try
        {
            Logger.LogInformation ("Stop the application requested");
            var lifetime = Application.Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.StopApplication();
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while stopping the application");
        }
    }

    /// <summary>
    /// Сигнал от пользователя о сбросе текущей ошибки.
    /// </summary>
    private static void HandleOkRequest()
    {
        try
        {
            Logger.LogInformation ("It's OK signal");
            GlobalState.Instance.HasError = false;
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while handling It's OK signal");
        }
    }

    private static IResult HandleVersionRequest()
    {
        try
        {
            var location = typeof (Program).Assembly.Location;
            var version = FileVersionInfo.GetVersionInfo (location)?.FileVersion;
            return Results.Text (version ?? "unknown");
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while handling Version request");
        }

        return Results.Problem();
    }

    private static IResult HandlePrivateRequest
        (
            bool on
        )
    {
        var now = DateTime.Now.ToShortTimeString();
        var message = on
            ? "Вход только для библиотекарей"
            : "Библиотека открыта для посещения";
        GlobalState.Instance.Message = $"{now}: {message}";
        GlobalState.Instance.IsBlatOnly = on;
        Logger.LogInformation ("Private = " + GlobalState.Instance.IsBlatOnly);

        return Results.Text (message);
    }

    #endregion

    #region Private members

    private static IConfiguration BuildConfiguration
        (
            WebApplicationBuilder builder,
            string[] args
        )
    {
        // используем json5, чтобы невозбранно использовать комментарии
        // хотя, говорят, можно было оставаться и на простом json
        var configuration = builder.Configuration;
        configuration.Sources.Clear();
        configuration.AddCommandLine (args);
        configuration.AddJsonFile ("appsettings.json5");

        return configuration;
    }

    private static void RequestStop
        (
            WebApplicationBuilder builder,
            string[] args
        )
    {
        const StringSplitOptions options = StringSplitOptions.TrimEntries
            | StringSplitOptions.RemoveEmptyEntries;
        Console.WriteLine ("Requesting stop the application");
        try
        {
            var configuration = BuildConfiguration (builder, args);
            var baseUrl = configuration["Urls"]!
                .Split (';', options)
                .First();

            var client = new RestClient (baseUrl);
            var request = new RestRequest ("/api/stop");
            var response = client.Execute (request);
            Console.WriteLine ($"Response is {response.StatusCode}");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception.Message);
        }
    }

    #endregion
}
