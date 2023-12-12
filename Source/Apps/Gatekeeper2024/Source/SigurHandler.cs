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
    public async Task HandleRequest
        (
            HttpContext context
        )
    {
        var request = await JsonSerializer.DeserializeAsync<SigurRequest> (context.Request.Body);
        if (request is null)
        {
            GlobalState.Logger.LogError ("Can't parse the request");
            return;
        }

        request.Arrived = TimeProvider.System.GetLocalNow();

        var response = ProcessRequest (request);

        GlobalState.Instance.HasError = !response.Allow;
        LogRequestAndResponse (request, response);

        await context.Response.WriteAsJsonAsync (response);
    }

    #endregion

    #region Private members

    /// <summary>
    /// Сохранение запроса в файловой системе для последующей отправки
    /// на сервер ИРБИС64.
    /// </summary>
    private void SaveEventForFurtherSending
        (
            PassEvent pass
        )
    {
        var queue = Utility.GetQueueDirectory();
        Directory.CreateDirectory (queue);
        var moment = pass.Moment.ToString ("yy-MM-dd-hh-mm-ss-ff");
        var fileName = $"{moment}.{pass.Type}.json";
        var path = Path.Combine ("Pool", fileName);
        var json = JsonSerializer.Serialize (pass);
        File.WriteAllText (path, json);
    }

    /// <summary>
    /// Обработка запроса.
    /// </summary>
    private SigurResponse ProcessRequest
        (
            SigurRequest request
        )
    {
        var arrival = Utility.GetArrival();
        var departure = Utility.GetDeparture();
        var isDepature = request.AccessPoint == departure;
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
            var message = Utility.GetIrbisFailure (readerId);
            return LetMyPeopleGo
                (
                    isDepature,
                    GlobalState.Instance.Message = message,
                    allow: letPeopleGo
                );
        }

        if (readers.Length == 0)
        {
            var message = string.Format (Utility.GetReaderFailure (readerId));
            return LetMyPeopleGo
                (
                    isDepature,
                    GlobalState.Instance.Message = message,
                    allow: letPeopleGo
                );
        }

        if (readers.Length != 1)
        {
            var message = string.Format (Utility.GetManyReaders (readerId));
            return LetMyPeopleGo
                (
                    isDepature,
                    GlobalState.Instance.Message = message,
                    allow: letPeopleGo
                );
        }

        var reader = readers[0];
        if (request.AccessPoint == arrival)
        {
            return HandleArrival (request, reader);
        }

        if (request.AccessPoint == departure)
        {
            return HandleDeparture (request, reader);
        }

        GlobalState.Logger.LogError ("Unknown access point {Request}", request);
        return FallbackResolution
            (
                $"Неизвестная точка доступа: {request.AccessPoint}",
                allow: letPeopleGo
            );
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
        var data = new PassEvent
        {
            Type = request.AccessPoint,
            Moment = request.Arrived,
            Id = reader.Ticket,
            Data = "Сформировать" // TODO
        };

        SaveEventForFurtherSending (data);

        var message = $"Вход читателя {reader.FullName}: {reader.Ticket}";

        return FallbackResolution
            (
                message: GlobalState.Instance.Message = message,
                allow: true
            );
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
        var data = new PassEvent
        {
            Type = request.AccessPoint,
            Moment = request.Arrived,
            Id = reader.Ticket,
            Data = "Сформировать", // TODO
            Auxiliary = "Сформировать" // TODO
        };

        SaveEventForFurtherSending (data);

        // выходящие все выходят беспрепятственно
        return FallbackResolution (message: null, allow: true);
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

        return FallbackResolution (message, allow);
    }

    /// <summary>
    /// Стандартное разрешение проходить, применяемое при любых проблемах.
    /// </summary>
    private SigurResponse FallbackResolution
        (
            string? message = null,
            bool allow = true
        )
        => new()
    {
        Allow = allow,
        Message = message ?? (allow ? "Проход разрешен" : "Проход запрещен")
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
