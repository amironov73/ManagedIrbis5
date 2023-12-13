// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* SigurHandler.cs -- обработчик запросов от Sigur
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json;

using AM;

using ManagedIrbis.Readers;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Обработчик запросов от Sigur.
/// </summary>
internal class SigurHandler
{
    #region Public methods

    /// <summary>
    /// Обработчик POST-запроса.
    /// </summary>
    public IResult HandleRequest
        (
            HttpContext context
        )
    {
        var body = context.Request.Body;

        // синхронно нельзя - лается и бросается исключениями
        var request = JsonSerializer.DeserializeAsync<SigurRequest> (body)
            .GetAwaiter().GetResult();
        if (request is null)
        {
            // нам прислали непонятную штуку, отказываемся обрабатывать
            GlobalState.Logger.LogError ("Can't parse the request body: {Body}", body);
            var letPeopleGo = Utility.GetPeopleGo();
            return Results.Json (Resolution ("Запрос в неверном формате", letPeopleGo));
        }

        request.Arrived = Utility.GetNow();

        var response = ProcessRequest (request);
        LogRequestAndResponse (request, response);

        return Results.Json (response);
    }

    #endregion

    #region Private members

    /// <summary>
    /// Сохранение запроса в файловой системе для последующей отправки
    /// на сервер ИРБИС64.
    /// </summary>
    private void SaveEventForFurtherSending
        (
            PassEvent passEvent
        )
    {
        var queueDirectory = Utility.GetQueueDirectory();
        try
        {
            Directory.CreateDirectory (queueDirectory);
        }
        catch (Exception exception)
        {
            GlobalState.Logger.LogError
                (
                    exception,
                    "Can't create queue directory {Directory}",
                    queueDirectory
                );
            return;
        }

        var moment = passEvent.Moment.ToString ("yyyy-MM-dd-hh-mm-ss-ff");
        var fileName = $"{moment}.{passEvent.Type}.json";
        var path = Path.Combine (queueDirectory, fileName);
        var json = JsonSerializer.Serialize (passEvent);

        try
        {
            File.WriteAllText (path, json);
        }
        catch (Exception exception)
        {
            GlobalState.Logger.LogError
                (
                    exception,
                    "Can't save pass event file {Path}",
                    path
                );
        }
    }

    /// <summary>
    /// Обработка запроса.
    /// </summary>
    private SigurResponse ProcessRequest
        (
            SigurRequest request
        )
    {
        var arrivalPoint = Utility.GetArrivalPoint();
        var departurePoint = Utility.GetDeparturePoint();
        var isDepature = request.AccessPoint == departurePoint;
        var letPeopleGo = Utility.GetPeopleGo();

        var readerId = request.KeyHex;
        if (string.IsNullOrEmpty (readerId))
        {
            return LetMyPeopleGo
                (
                    isDepature,
                    GlobalState.Instance.Message = "Нет идентификатора читателя",
                    allow: letPeopleGo
                );
        }

        var readers = Utility.SearchForReader (readerId);
        if (readers is null)
        {
            // сохраняем событие для дальнейшей отправки на сервер ИРБИС64
            SaveEventForFurtherSending (new PassEvent
            {
                Type = request.AccessPoint,
                Moment = request.Arrived,
                Id = readerId
            });

            var message = Utility.GetIrbisFailure (readerId);
            var result = LetMyPeopleGo
                (
                    isDepature,
                    GlobalState.Instance.Message = message,
                    allow: letPeopleGo
                );
            GlobalState.Instance.HasError = true;

            return result;
        }

        if (readers.Length == 0)
        {
            // не сохраняем событие для дальнейшей отправки на сервер ИРБИС64!

            var message = string.Format (Utility.GetReaderFailure (readerId));
            var result = LetMyPeopleGo
                (
                    isDepature,
                    GlobalState.Instance.Message = message,
                    allow: letPeopleGo
                );
            GlobalState.Instance.HasError = true;

            return result;
        }

        if (readers.Length != 1)
        {
            // не сохраняем событие для дальнейшей отправки на сервер ИРБИС64!

            var message = string.Format (Utility.GetManyReaders (readerId));
            var result = LetMyPeopleGo
                (
                    isDepature,
                    GlobalState.Instance.Message = message,
                    allow: letPeopleGo
                );
            GlobalState.Instance.HasError = true;

            return result;
        }

        var reader = readers[0];
        if (request.AccessPoint == arrivalPoint)
        {
            return HandleArrival (request, reader);
        }

        if (request.AccessPoint == departurePoint)
        {
            return HandleDeparture (request, reader);
        }

        GlobalState.Logger.LogError ("Unknown access point {Request}", request);
        var finalResult = Resolution
            (
                $"Неизвестная точка доступа: {request.AccessPoint}",
                allow: letPeopleGo
            );
        GlobalState.Instance.HasError = true;

        return finalResult;
    }

    /// <summary>
    /// Обработка входа читателя.
    /// </summary>
    private SigurResponse HandleArrival
        (
            SigurRequest request,
            ReaderInfo reader
        )
    {
        var message = Utility.GetArrivalMessage (request.KeyHex!);
        message += $" ({reader.FullName})";

        var result = Resolution
            (
                message: GlobalState.Instance.Message = message,
                allow: true
            );
        GlobalState.Instance.HasError = false;

        // сохраняем событие для дальнейшей отправки на сервер ИРБИС64
        SaveEventForFurtherSending (new PassEvent
        {
            Type = request.AccessPoint,
            Moment = request.Arrived,
            Id = request.KeyHex
        });

        return result;
    }

    /// <summary>
    /// Обработка выхода читателя.
    /// </summary>
    private SigurResponse HandleDeparture
        (
            SigurRequest request,
            ReaderInfo reader
        )
    {
        request.NotUsed();
        reader.NotUsed();

        // выходящие всегда выходят беспрепятственно
        var result = Resolution (message: null, allow: true);
        GlobalState.Instance.HasError = false;

        // сохраняем событие для дальнейшей отправки на сервер ИРБИС64
        SaveEventForFurtherSending (new PassEvent
        {
            Type = request.AccessPoint,
            Moment = request.Arrived,
            Id = request.KeyHex
        });

        return result;
    }

    /// <summary>
    /// Разруливаем вход/выход читателей,
    /// настраивая соответствующим образом сообщения.
    /// </summary>
    private SigurResponse LetMyPeopleGo
        (
            bool isDeparture,
            string? message = null,
            bool allow = true
        )
    {
        if (isDeparture || allow)
        {
            message = null;
        }

        return Resolution (message, allow);
    }

    /// <summary>
    /// Стандартное разрешение проходить, применяемое при любых проблемах.
    /// </summary>
    private SigurResponse Resolution
        (
            string? message = null,
            bool allow = true
        )
        => new()
    {
        Allow = allow,
        Message = message
    };

    /// <summary>
    /// Логирование запроса и ответа на него.
    /// </summary>
    private void LogRequestAndResponse
        (
            SigurRequest request,
            SigurResponse response
        )
    {
        GlobalState.Logger.LogInformation
            (
                "Got auth request: {Request}, send {Response}",
                request,
                response
            );
    }

    #endregion
}
