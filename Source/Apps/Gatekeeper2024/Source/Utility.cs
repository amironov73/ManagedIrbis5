// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Utility.cs -- утилитные методы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

namespace Gatekeeper2024;

/// <summary>
/// Утилитные методы.
/// </summary>
internal static class Utility
{
    /// <summary>
    /// Подключение к серверу ИРБИС64.
    /// </summary>
    public static ISyncProvider? ConnectToIrbis
        (
            WebApplication application
        )
    {
        var configuration = application.Configuration;
        var connectionString = configuration["irbis-connection"];
        if (string.IsNullOrEmpty (connectionString))
        {
            return null;
        }

        var result = ConnectionFactory.Shared.CreateSyncConnection();
        result.ParseConnectionString (connectionString);
        if (!result.Connect() || !result.IsConnected)
        {
            result.Dispose();
            return null;
        }

        return result;
    }

    /// <summary>
    /// Получение поискового запроса для поиска читателей в базе RDR.
    /// </summary>
    public static string? GetSearchExpression
        (
            WebApplication application
        )
    {
        var configuration = application.Configuration;
        var result = configuration["search-expression"];

        return string.IsNullOrEmpty (result) ? null : result;
    }

    /// <summary>
    /// Получение номера входного турникета.
    /// </summary>
    public static int GetArrival
        (
            WebApplication application
        )
    {
        var configuration = application.Configuration;
        var result = configuration["arrival"];

        return string.IsNullOrEmpty (result) ? -1 : int.Parse (result);
    }

    /// <summary>
    /// Получение номера выходного турникета.
    /// </summary>
    public static int GetDeparture
        (
            WebApplication application
        )
    {
        var configuration = application.Configuration;
        var result = configuration["departure"];

        return string.IsNullOrEmpty (result) ? -1 : int.Parse (result);
    }

    /// <summary>
    /// Получение сообщения об ошибке связи с ИРБИС64.
    /// </summary>
    public static string GetIrbisFailure
        (
            WebApplication application,
            string readerId
        )
    {
        var configuration = application.Configuration;
        var result = configuration["irbis-failure"];
        result =  string.IsNullOrEmpty (result)
            ? "ВНИМАНИЕ! Сервер ИРБИС64 недоступен. Примите решение о пропуске посетителя с картой {0} в ручном режиме"
            : result;
        result = string.Format (result, readerId);

        return result;
    }

    /// <summary>
    /// Получение сообщения о том, что читатель не найден.
    /// </summary>
    public static string GetReaderFailure
        (
            WebApplication application,
            string readerId
        )
    {
        var configuration = application.Configuration;
        var result = configuration["reader-failure"];
        result = string.IsNullOrEmpty (result)
            ? "ВНИМАНИЕ! Посетитель с картой {0} не найден в базе данных читателей, либо лишен права обслуживания. Попросите посетителя пройти на ресепшн"
            : result;
        result = string.Format (result, readerId);

        return result;
    }

    /// <summary>
    /// Получение сообщения о том, что много читателей с указанным идентификатором.
    /// </summary>
    public static string GetManyReaders
        (
            WebApplication application,
            string readerId
        )
    {
        var configuration = application.Configuration;
        var result = configuration["many-readers"];
        result = string.IsNullOrEmpty (result)
            ? "ВНИМАНИЕ! В базе данных читателей обнаружена множественность идентификатора {0}. Попросите посетителя подойти на ресепшн"
            : result;
        result = string.Format (result, readerId);

        return result;
    }

    /// <summary>
    /// Получение имени подпапки, в которой хранятся
    /// запросы для отправки на сервер ИРБИС64.
    /// </summary>
    public static string GetQueueDirectory
        (
            WebApplication application
        )
    {
        var configuration = application.Configuration;
        var result = configuration["queue-directory"];
        result = string.IsNullOrEmpty (result)
            ? "Queue"
            : result;

        return result;
    }

    /// <summary>
    /// Пропускать или нет при любых недоразумениях.
    /// </summary>
    public static bool GetPeopleGo
        (
            WebApplication application
        )
    {
        var configuration = application.Configuration;
        var value = configuration["let-my-people-go"] ?? "true";
        var result = bool.Parse (value);

        return result;
    }

    /// <summary>
    /// Поиск читателя с указанным идентификатором.
    /// </summary>
    public static ReaderInfo[]? SearchForReader
        (
            WebApplication application,
            string id
        )
    {
        var expression = GetSearchExpression (application);
        if (string.IsNullOrEmpty (expression))
        {
            application.Logger.LogError ("Search expression not specified");
            return null;
        }

        // подставляем номер читательского в поисковое выражение
        expression = string.Format (expression, id);

        using var connection = ConnectToIrbis (application);
        if (connection is null)
        {
            application.Logger.LogError ("Can't connect to the IRBIS");
            return null;
        }

        var records = connection.SearchReadRecords (expression);
        if (records.IsNullOrEmpty())
        {
            return Array.Empty<ReaderInfo>();
        }

        var result = records.Select (ReaderInfo.Parse)
            .ToArray();

        return result;
    }
}
