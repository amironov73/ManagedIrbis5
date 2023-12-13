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

using Microsoft.Extensions.Logging.Abstractions;

using NLog.Web;

using RestSharp;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

#endregion

namespace Gatekeeper2024;

internal sealed /* нельзя static */ class Program
{
    internal static void Main
        (
            string[] args
        )
    {
        // *******************************************************************
        // начальная настройка

        // временный логгер, будет заменен постоянным
        GlobalState.Logger = new NullLoggerFactory().CreateLogger ("null");

        // *******************************************************************

        var builder = WebApplication.CreateBuilder (args);

        // *******************************************************************
        // настройка конфигурации

        // используем json5, чтобы невозбранно использовать комментарии
        // хотя, говорят, можно было оставаться и на простом json
        var configuration = builder.Configuration;
        configuration.Sources.Clear();
        configuration.AddCommandLine (args);
        configuration.AddJsonFile ("appsettings.json5");

        // *******************************************************************
        // остановка приложения по требованию

        if (args is ["stop"] or ["/stop"])
        {
            RequestStop();
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
        services.AddHostedService<EventUploader>();
        services.AddSingleton<SigurHandler>();

        // *******************************************************************
        // предварительная настройка завершена, создаем объект приложения

        var app = builder.Build();

        GlobalState.Application = app;
        GlobalState.Logger = app.Services.GetRequiredService <ILogger<Program>>();

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
        // и при необходимости UseDirectoryBrowser.

        // *******************************************************************
        // создаем endpoint'ы

        var api = app.MapGroup("/api");
        api.MapGet ("/state", GetState);
        api.MapGet ("/stop", StopTheApplication);

        app.MapPost ("/auth", HandleAuth);

        GlobalState.SetMessageWithTimestamp ("Пока никаких событий не зафиксировано");
        GlobalState.Logger.LogInformation ("Application startup");

        app.Run();
    }

    private static IResult HandleAuth
        (
            HttpContext context
        )
    {
        var sigurHandler = GlobalState.Application.Services.GetRequiredService<SigurHandler>();

        return sigurHandler.HandleRequest (context);
    }

    private static IResult GetState()
    {
        return Results.Json (GlobalState.Instance);
    }

    private static void RequestStop()
    {
        Console.WriteLine ("Requesting stop the application");
        try
        {
            var client = new RestClient ();
            var request = new RestRequest ("http://127.0.0.1/api/stop");
            var response = client.Execute (request);
            Console.WriteLine ($"Response is {response.StatusCode}");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception.Message);
        }
    }

    private static void StopTheApplication()
    {
        GlobalState.Logger.LogInformation ("Stop the application requested");
        var lifetime = GlobalState.Application.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.StopApplication();
    }

}
