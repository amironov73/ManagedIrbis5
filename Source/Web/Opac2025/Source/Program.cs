// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.Extensions.Logging.Abstractions;

using NLog.Web;

using RestSharp;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

#endregion

namespace Opac2025;

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

    public static void Main
        (
            string[] args
        )
    {
        // *******************************************************************
        // начальная настройка

        // временный логгер, будет заменен постоянным
        Logger = new NullLoggerFactory().CreateLogger ("null");

        // *******************************************************************

        var builder = WebApplication.CreateBuilder(args);

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
        // services.AddHostedService<EventUploader>();
        // services.AddSingleton<StatProvider>();

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
        // app.UseMiddleware<NoCacheMiddleware>();

        // *******************************************************************
        // создаем endpoint'ы

        var api = app.MapGroup ("/opac");
        api.MapGet ("test", HandleTestRequest);
        // api.MapGet ("state", HandleStateRequest);
        // api.MapGet ("history", HandleHistoryRequest);
        // api.MapGet ("stat", HandleStatRequest);
        // api.MapGet ("stop", HandleStopRequest);
        // api.MapGet ("ok", HandleOkRequest);
        // api.MapGet ("version", HandleVersionRequest);
        // api.MapGet ("staff", HandleStaffRequest);
        // api.MapGet ("passage", HandlePassageRequest);
        //
        // app.MapPost ("/auth", HandleAuthRequest);
        //
        // GlobalState.SetMessageWithTimestamp ("Пока никаких событий не зафиксировано");
        Logger.LogInformation ("Application startup");

        app.Run();
    }

    #endregion

    #region HTTP request handlers

    private static IResult HandleTestRequest()
    {
        try
        {
            return Results.Ok (OpacUtility.TestIrbisConnection());
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while testing IRBIS connection");
            return Results.Problem (exception.Message);
        }
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
