// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* Program.cs -- настройка и запуск Web-приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using NLog.Web;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

#endregion

namespace Gatekeeper2024;

internal sealed /* нельзя static */ class Program
{
    private static int _currentFish;

    private static readonly string[] fishLines =
    {
        "Задача организации, в особенности же постоянное информационно-пропагандистское обеспечение нашей деятельности.",
        "Повседневная практика показывает, что рамки и место обучения кадров влечет за собой процесс внедрения.",
        "Значимость этих проблем настолько очевидна, что сложившаяся структура организации представляет собой интересный эксперимент.",
        "Повседневная практика показывает, что начало повседневной работы по формированию позиции играет важную роль.",
        "Разнообразный и богатый опыт дальнейшее развитие различных форм деятельности представляет собой интересный эксперимент.",
        "Разнообразный и богатый опыт консультация с широким активом в значительной степени обуславливает создание.",
        "Идейные соображения высшего порядка, а также постоянное информационно-пропагандистское обеспечение нашей деятельности.",
        "Значимость этих проблем настолько очевидна, что дальнейшее развитие различных форм деятельности позволяет выполнять.",
        "Товарищи! новая модель организационной деятельности в значительной степени обуславливает создание новых предложений.",
        "С другой стороны постоянный количественный рост и сфера нашей активности позволяет выполнять важные задания по разработке."
    };


    internal static void Main (string[] args)
    {
        var builder = WebApplication.CreateBuilder (args);

        // *******************************************************************
        // настройка логирования

        var logging = builder.Logging;
        logging.ClearProviders();
        logging.SetMinimumLevel (LogLevel.Information);
        builder.Host.UseNLog();

        // *******************************************************************

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
        app.UseDefaultFiles();

        app.UseStaticFiles();

        // UseFileServer объединяет функции UseStaticFiles, UseDefaultFiles
        // и при необходимости UseDirectoryBrowser.

        // *******************************************************************

        // создаем папку, в которую будут складываться задания на отправку
        var queue = Utility.GetQueueDirectory();
        Directory.CreateDirectory (queue);

        app.MapGet ("/api/fish", GetNextFish);
        app.MapGet ("/api/state", GetState);

        var handler = new SigurHandler();
        app.MapPost ("/auth", context => handler.HandleRequest (context));

        var sender = new IrbisSender();
        sender.CheckIrbisConnection();
        sender.StartWorkingLoop();

        GlobalState.SetMessageWithTimestamp ("Пока никаких событий не зафиксировано");
        GlobalState.Logger.LogInformation ("Application startup");

        app.Run();
    }

    private static string GetNextFish()
    {
        var result = fishLines[_currentFish++];
        if (_currentFish >= fishLines.Length)
        {
            _currentFish = 0;
        }

        return result;
    }

    private static IResult GetState()
    {
        return Results.Json (GlobalState.Instance);
    }
}
