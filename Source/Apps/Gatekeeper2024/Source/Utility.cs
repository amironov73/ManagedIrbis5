// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Utility.cs -- утилитные методы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Утилитные методы.
/// </summary>
internal static class Utility
{
    public static string? GetString
        (
            string keyName,
            string? defaultValue = null
        )
    {
        var configuration = Program.Application.Configuration;
        var result = configuration[keyName];
        if (string.IsNullOrEmpty (result))
        {
            result = defaultValue;
        }

        return result;
    }

    public static string GetRequiredString
        (
            string keyName
        )
    {
        var result = GetString (keyName);
        if (string.IsNullOrEmpty (result))
        {
            throw new ApplicationException ($"{keyName} not configured");
        }

        return result;
    }

    public static bool GetBoolean
        (
            string keyName,
            bool defaultValue = false
        )
    {
        var result = GetString (keyName);
        return string.IsNullOrEmpty (result)
            ? defaultValue
            : bool.Parse (result);
    }

    public static int GetInt32
        (
            string keyName,
            int defaultValue = -1
        )
    {
        var result = GetString (keyName);
        return string.IsNullOrEmpty (result)
            ? defaultValue
            : int.Parse (result, NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    public static int GetRequiredInt32
        (
            string keyName,
            int defaultValue = -1
        )
    {
        var result = GetInt32 (keyName);
        if (result == defaultValue)
        {
            throw new ApplicationException ($"{keyName} not configured");
        }

        return result;
    }

    /// <summary>
    /// Подключение к серверу ИРБИС64.
    /// </summary>
    public static ISyncProvider? ConnectToIrbis
        (
            bool gracefully = false
        )
    {
        var connectionString = GetRequiredString ("irbis-connection");
        if (string.IsNullOrWhiteSpace (connectionString))
        {
            Program.Logger.LogError ("IRBIS connection string is empty");
            return null;
        }

        var timeout = GetInt32 ("timeout", 100);
        SyncConnection result;
        if (gracefully)
        {
            var socket = new GracefulSocket
            {
                Timeout = timeout
            };
            var serviceProvider = Program.Application.Services;
            result = new SyncConnection (socket, serviceProvider);
        }
        else
        {
            result = new SyncConnection();
        }

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
    public static string GetSearchExpression()
        => GetRequiredString ("search-expression");

    /// <summary>
    /// Получение номера входного турникета.
    /// </summary>
    public static int GetArrivalPoint()
        => GetRequiredInt32 ("arrival");

    /// <summary>
    /// Получение сообщения о входе читателя в библиотеку.
    /// </summary>
    public static string GetArrivalMessage
        (
            string keyHex
        )
    {
        const string defaultValue = "Зафиксирован вход читателя с пропуском {0}";
        var result = GetString ("arrival-message", defaultValue);
        return string.Format (result!, keyHex);
    }

    /// <summary>
    /// Получение сообщения о выходе читателя из библиотеки.
    /// </summary>
    public static string GetDepartureMessage
        (
            string keyHex
        )
    {
        const string defaultValue = "Зафиксирован выход читателя с пропуском {0}";
        var result = GetString ("departure-message", defaultValue);
        return string.Format (result!, keyHex);
    }

    /// <summary>
    /// Текущий момент времени.
    /// </summary>
    public static DateTimeOffset GetNow
        (
            TimeProvider? timeProvider = null
        )
        => (timeProvider ?? TimeProvider.System).GetLocalNow();

    /// <summary>
    /// Получение поля 40, формируемого при входе читателя в библиотеку.
    /// </summary>
    public static string GetArrivalField
        (
            DateTimeOffset moment
        )
    {
        const string defaultValue = "^d{date}^1{time}^c{event}^i{person}^v{department}";
        var result = GetString ("arrival-field", defaultValue);
        return FormatDateTime (result!, moment);
    }

    /// <summary>
    /// Форматирование даты и времени для поля 40.
    /// </summary>
    public static string FormatDateTime
        (
            string format,
            DateTimeOffset moment
        )
    {
        var result = format.Replace ("{date}", moment.ToString ("yyyyMMdd"));

        // обрати внимание на "HH" -- это означает время в 24-часовом формате
        result = result.Replace ("{time}", moment.ToString ("HHmmss"));
        result = result.Replace ("{department}", GetDepartment());
        result = result.Replace ("{person}", GetPerson());
        result = result.Replace ("{event}", GetEvent());
        return result;
    }

    /// <summary>
    /// Разбор момента времени в текстовом представлении.
    /// </summary>
    public static TimeOnly ParseTime (string text)
        => TimeOnly.ParseExact (text, "HH:mm:ss");

    /// <summary>
    /// Получение номера выходного турникета.
    /// </summary>
    public static int GetDeparturePoint()
        => GetRequiredInt32 ("departure");

    /// <summary>
    /// Получение поля 40, формируемого при выходе читателя из библиотеки.
    /// Поскольку читатель не отметился на выходе, проставляем время выхода
    /// равным времени входа.
    /// </summary>
    public static string GetDepartureField
        (
            VisitInfo visit
        )
    {
        var result = $"^f{visit.DateGivenString}^2{visit.TimeIn}";
        return result;
    }

    /// <summary>
    /// Получение поля 40, формируемого при выходе читателя из библиотеки.
    /// </summary>
    public static string GetDepartureField
        (
            DateTimeOffset moment
        )
    {
        const string defaultValue = "^f{date}^2{time}";
        var result = GetString ("departure-field", defaultValue);
        return FormatDateTime (result!, moment);
    }

    /// <summary>
    /// Получение сообщения об ошибке связи с ИРБИС64.
    /// </summary>
    public static string GetIrbisFailure
        (
            string readerId
        )
    {
        const string defaultMessage = "ВНИМАНИЕ! Сервер ИРБИС64 недоступен. Примите решение о пропуске посетителя с картой {0} в ручном режиме";
        var result = GetString ("irbis-failure", defaultMessage)!;
        return string.Format (result, readerId);
    }

    /// <summary>
    /// Получение сообщения о том, что читатель не найден.
    /// </summary>
    public static string GetReaderFailure
        (
            string readerId
        )
    {
        const string defaultMessage = "ВНИМАНИЕ! Посетитель с картой {0} не найден в базе данных читателей, либо лишен права обслуживания. Попросите посетителя пройти на ресепшн";
        var result = GetString ("reader-failure", defaultMessage)!;
        return string.Format (result, readerId);
    }

    /// <summary>
    /// Получение сообщения о том, что много читателей с указанным идентификатором.
    /// </summary>
    public static string GetManyReaders
        (
            string readerId
        )
    {
        const string defaultMessage = "ВНИМАНИЕ! В базе данных читателей обнаружена множественность идентификатора {0}. Попросите посетителя подойти на ресепшн";
        var result = GetString ("many-readers", defaultMessage)!;
        return string.Format (result, readerId);
    }

    /// <summary>
    /// Получение имени подпапки, в которой хранятся
    /// запросы для отправки на сервер ИРБИС64.
    /// </summary>
    public static string GetQueueDirectory()
        => GetRequiredString ("queue-directory");

    /// <summary>
    /// Пропускать или нет при любых недоразумениях.
    /// </summary>
    public static bool GetPeopleGo()
        => GetBoolean ("let-my-people-go");

    /// <summary>
    /// Поиск читателя с указанным идентификатором.
    /// Может найтись несколько, если в базе есть дублеты.
    /// </summary>
    public static ReaderInfo[]? SearchForReader
        (
            ISyncProvider connection,
            string id
        )
    {
        var expression = GetSearchExpression();
        if (string.IsNullOrEmpty (expression))
        {
            Program.Logger.LogError ("Search expression not specified");
            return null;
        }

        // подставляем номер читательского в поисковое выражение
        expression = string.Format (expression, id);

        var records = connection.SearchReadRecords (expression);
        if (records.IsNullOrEmpty())
        {
            return Array.Empty<ReaderInfo>();
        }

        var result = records.Select (ReaderInfo.Parse)
            .ToArray();

        return result;
    }

    /// <summary>
    /// Поиск читателя с указанным идентификатором.
    /// Может найтись несколько, если в базе есть дублеты.
    /// </summary>
    public static ReaderInfo[]? SearchForReader
        (
            string id,
            bool gracefully = false
        )
    {
        using var connection = ConnectToIrbis (gracefully);
        if (connection is null)
        {
            Program.Logger.LogError ("Can't connect to the IRBIS");
            return null;
        }

        return SearchForReader (connection, id);
    }

    /// <summary>
    /// Проверка подключения к серверу ИРБИС64.
    /// </summary>
    public static void TestIrbisConnection()
    {
        try
        {
            var connection = Utility.ConnectToIrbis();

            if (connection is null)
            {
                GlobalState.SetMessageWithTimestamp ("Тестовое подключение к серверу ИРБИС64: ОШИБКА");
                GlobalState.Instance.HasError = true;
            }
            else
            {
                GlobalState.SetMessageWithTimestamp ("Тестовое подключение к серверу ИРБИС64 выполнено успешно");
                GlobalState.Instance.HasError = false;
                connection.Dispose();
            }
        }
        catch (Exception exception)
        {
            Program.Logger.LogError (exception, $"Error during {nameof (TestIrbisConnection)}");
        }
    }

    /// <summary>
    /// Получение кода кафедры обслуживания.
    /// </summary>
    public static string GetDepartment() => GetString ("department", "*")!;

    /// <summary>
    /// Получение ответственного лица.
    /// </summary>
    public static string GetPerson() => GetString ("person", "sigur")!;

    /// <summary>
    /// Описание события.
    /// </summary>
    public static string GetEvent() => GetString ("event", "(Посещение)")!;

    /// <summary>
    /// Формат для имени файла, формируемого из текущей даты-времени.
    /// </summary>
    public static string GetDateTimeFormatForFileName() => "yyyy-MM-dd-HH-mm-ss-ff";

    /// <summary>
    /// Минимальный промежуток времени между последовательными
    /// проходами читателя, минуты.
    /// </summary>
    public static int GetTimeSpan() => GetInt32 ("minimum-time-span", 60);

    /// <summary>
    /// Метка поля "Место работы".
    /// </summary>
    public static int GetPrivilegeTag() => GetInt32 ("privilege-tag", 15);

    /// <summary>
    /// Место работы, означающее что читатель свой.
    /// </summary>
    public static string GetPrivilegeText() => GetString ("privilege-text", "ИОГУНБ")!;
}
