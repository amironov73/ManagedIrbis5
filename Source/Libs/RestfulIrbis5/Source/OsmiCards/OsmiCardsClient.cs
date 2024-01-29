// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* OsmiCardsClient.cs -- клиент DiCARDS
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using AM;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using RestSharp;
using RestSharp.Authenticators.Digest;

#endregion

namespace RestfulIrbis.OsmiCards;

//
// Онлайн-документация на API: https://apidocs.osmicards.com/
// Настоящий код соответствует неактуальной версии
//

/// <summary>
/// Клиент DiCARDS.
/// </summary>
[PublicAPI]
public class OsmiCardsClient
{
    #region Properties

    /// <summary>
    /// Используемый RestClient.
    /// </summary>
    public RestClient Connection { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OsmiCardsClient
        (
            string baseUrl,
            string apiId,
            string apiKey
        )
    {
        Sure.NotNullNorEmpty (baseUrl);
        Sure.NotNullNorEmpty (apiId);
        Sure.NotNullNorEmpty (apiKey);

        var options = new RestClientOptions (baseUrl)
        {
            Authenticator = new DigestAuthenticator (apiId, apiKey)
        };
        Connection = new RestClient (options);
    }

    #endregion

    #region Public methods

    // =========================================================

    /// <summary>
    /// Проверить ранее выданный PIN-код.
    /// </summary>
    public void CheckPinCode()
    {
        var request = new RestRequest
            (
                "/activation/checkpin"
            );

        Connection.Post (request);
    }

    // =========================================================

    /// <summary>
    /// Создать новую карту.
    /// </summary>
    public void CreateCard
        (
            string cardNumber,
            string template
        )
    {
        Sure.NotNullNorEmpty (cardNumber);
        Sure.NotNullNorEmpty (template);

        var request = new RestRequest
            (
                "/passes/{number}/{template}"
            )
            {
                RequestFormat = DataFormat.Json
            };
        request.AddUrlSegment ("number", cardNumber);
        request.AddUrlSegment ("template", template);

        Connection.Post (request);
    }

    // =========================================================

    /// <summary>
    /// Создать новую карту.
    /// </summary>
    public void CreateCard
        (
            string cardNumber,
            string template,
            string jsonText
        )
    {
        Sure.NotNullNorEmpty (cardNumber);
        Sure.NotNullNorEmpty (template);
        Sure.NotNullNorEmpty (jsonText);

        var request = new RestRequest
            (
                "/passes/{number}/{template}"
            );
        request.AddUrlSegment ("number", cardNumber);
        request.AddUrlSegment ("template", template);
        request.AddQueryParameter ("withValues", "true");
        request.AddJsonBody (jsonText);

        Connection.Post (request);
    }

    // =========================================================

    /// <summary>
    /// Создать новый шаблон.
    /// </summary>
    public void CreateTemplate
        (
            string templateName,
            string jsonText
        )
    {
        Sure.NotNullNorEmpty (templateName);
        Sure.NotNullNorEmpty (jsonText);

        var request = new RestRequest
            (
                "/templates/{template}"
            );
        request.AddUrlSegment ("template", templateName);
        request.AddJsonBody (jsonText);

        Connection.Post (request);
    }

    // =========================================================

    /// <summary>
    /// Удалить карту.
    /// </summary>
    public void DeleteCard
        (
            string cardNumber,
            bool push
        )
    {
        Sure.NotNullNorEmpty (cardNumber);

        var url = "/passes/{number}";
        if (push)
        {
            url += "/push";
        }

        var request = new RestRequest (url);
        request.AddUrlSegment ("number", cardNumber);

        Connection.Delete (request);
    }

    // =========================================================

    /// <summary>
    /// Запросить информацию по карте.
    /// </summary>
    public OsmiCard? GetCardInfo
        (
            string cardNumber
        )
    {
        Sure.NotNullNorEmpty (cardNumber);

        var request = new RestRequest ("/passes/{number}");
        request.AddUrlSegment ("number", cardNumber);
        var response = Connection.Get (request);
        var jObject = JObject.Parse (response.Content!);
        var result = OsmiCard.FromJObject (jObject);

        return result;
    }

    // =========================================================

    /// <summary>
    /// Запросить "сырую" информацию по карте.
    /// </summary>
    public JObject? GetRawCard
        (
            string cardNumber
        )
    {
        Sure.NotNullNorEmpty (cardNumber);

        var request = new RestRequest
            (
                "/passes/{number}"
            );
        request.AddUrlSegment ("number", cardNumber);
        var response = Connection.Get (request);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return null;
        }
        var result = JObject.Parse (content);

        return result;
    }

    // =========================================================

    /// <summary>
    /// Запросить ссылку на загрузку карты.
    /// </summary>
    public string? GetCardLink
        (
            string cardNumber
        )
    {
        Sure.NotNullNorEmpty (cardNumber);

        var request = new RestRequest
            (
                "/passes/{number}/link"
            );
        request.AddUrlSegment ("number", cardNumber);
        var response = Connection.Get (request);
        var result = JObject.Parse (response.Content!);

        return result["link"]?.Value<string>();
    }

    // =========================================================

    /// <summary>
    /// Запросить список карт.
    /// </summary>
    public string?[]? GetCardList()
    {
        var request = new RestRequest ("/passes");
        var response = Connection.Get (request);
        var result = JObject.Parse (response.Content!);

        return result["cards"]?.Values<string>().ToArray();
    }

    // =========================================================

    /// <summary>
    /// Запросить общие параметры сервиса.
    /// </summary>
    public JObject GetDefaults()
    {
        var request = new RestRequest ("/defaults/all");
        var response = Connection.Get (request);
        var result = JObject.Parse (response.Content!);

        return result;
    }

    // =========================================================

    /// <summary>
    /// Запросить список доступных графических файлов.
    /// </summary>
    public OsmiImage[]? GetImages()
    {
        var request = new RestRequest ("/images");
        var response = Connection.Get (request);
        var result = JObject.Parse (response.Content!);

        return result["images"]?.ToObject<OsmiImage[]>();
    }

    // =========================================================

    /// <summary>
    /// Запросить общую статистику.
    /// </summary>
    public JObject GetStat()
    {
        var request = new RestRequest ("/stats/general");
        var response = Connection.Get (request);
        var result = JObject.Parse (response.Content!);

        return result;
    }

    // =========================================================

    /// <summary>
    /// Запросить информацию о шаблоне.
    /// </summary>
    public JObject GetTemplateInfo
        (
            string templateName
        )
    {
        Sure.NotNullNorEmpty (templateName);

        var request = new RestRequest ("/templates/{name}");
        request.AddUrlSegment ("name", templateName);

        JObject result;
        try
        {
            var response = Connection.Get (request);
            var content = response.Content ?? string.Empty;
            result = JObject.Parse (content);
        }
        catch (Exception inner)
        {
            Magna.Logger.LogError (inner, "Error during GetTemplateInfo for {TemplateName}", templateName);
            throw;

            // Encoding encoding = Encoding.UTF8;
            // ArsMagnaException outer = new ArsMagnaException ("Error Get template info", inner);
            // outer.Attach (new BinaryAttachment ("content", encoding.GetBytes (content)));
            // outer.Attach (new BinaryAttachment ("statusCode", encoding.GetBytes (statusCode.ToString())));
            // throw outer;
        }

        return result;
    }

    // =========================================================

    /// <summary>
    /// Запросить список доступных шаблонов.
    /// </summary>
    public string?[]? GetTemplateList()
    {
        var request = new RestRequest ("/templates");
        var response = Connection.Get (request);
        var result = JObject.Parse (response.Content!);

        return result["templates"]?.Values<string>().ToArray();
    }

    // =========================================================

    /// <summary>
    /// Проверить подключение к сервису.
    /// </summary>
    public JObject Ping()
    {
        var request = new RestRequest ("ping");
        var response = Connection.Get (request);
        var result = JObject.Parse (response.Content!);

        return result;
    }

    // =========================================================

    /// <summary>
    /// Текстовый поиск по содержимому полей карт.
    /// </summary>
    public string[] SearchCards
        (
            string text
        )
    {
        var request = new RestRequest
            (
                "/search/passes"
            );

        var requestJObject = new JObject
        {
            { "text", text }
        };
        request.AddJsonBody (requestJObject.ToString());

        var response = Connection.Post (request);
        var responseArray = JArray.Parse (response.Content!);

        List<string> result = new List<string>();
        foreach (var element in responseArray.Children<JObject>())
        {
            var item = element["serial"]?.Value<string>();
            if (!string.IsNullOrEmpty (item))
            {
                result.Add (item);
            }
        }

        return result.ToArray();
    }

    // =========================================================

    /// <summary>
    /// Отправить ссылку на загрузку карты по email.
    /// </summary>
    public void SendCardMail
        (
            string cardNumber,
            string email
        )
    {
        Sure.NotNullNorEmpty (cardNumber);
        Sure.NotNullNorEmpty (email);

        var request = new RestRequest
            (
                "/passes/{number}/email/{email}"
            );
        request.AddUrlSegment ("number", cardNumber);
        request.AddUrlSegment ("email", email);

        Connection.Get (request);
    }

    // =========================================================

    /// <summary>
    /// Отправить ссылку на загрузку карты по СМС.
    /// </summary>
    public void SendCardSms
        (
            string cardNumber,
            string phoneNumber
        )
    {
        Sure.NotNullNorEmpty (cardNumber);
        Sure.NotNullNorEmpty (phoneNumber);

        var request = new RestRequest
            (
                "/passes/{number}/sms/{phone}"
            );
        request.AddUrlSegment ("number", cardNumber);
        request.AddUrlSegment ("phone", phoneNumber);

        Connection.Get (request);
    }

    // =========================================================

    /// <summary>
    /// Отправить PIN-код по СМС.
    /// </summary>
    public void SendPinCode
        (
            string phoneNumber
        )
    {
        Sure.NotNullNorEmpty (phoneNumber);

        var request = new RestRequest ("/activation/sendpin/{phone}");

        Connection.Post (request);
    }

    // =========================================================

    /// <summary>
    /// Отправить push-сообщение на указанные карты.
    /// </summary>
    public void SendPushMessage
        (
            string[] cardNumbers,
            string messageText
        )
    {
        Sure.NotNull (cardNumbers);
        Sure.NotNullNorEmpty (messageText);

        var request = new RestRequest ("/marketing/pushmessage");
        var obj = new JObject();
        object[] serials = cardNumbers
            .Cast<object>()
            .ToArray();
        obj.Add ("serials", new JArray (serials));
        obj.Add ("message", messageText);
        request.AddJsonBody (obj.ToString());

        Connection.Post (request);
    }

    // =========================================================

    /// <summary>
    /// Переместить карту на другой шаблон.
    /// </summary>
    public void SetCardTemplate
        (
            string cardNumber,
            string template,
            bool push
        )
    {
        Sure.NotNullNorEmpty (cardNumber);
        Sure.NotNullNorEmpty (template);

        var url = "/passes/move/{number}/{template}";
        if (push)
        {
            url += "/push";
        }

        var request = new RestRequest (url);
        request.AddUrlSegment ("number", cardNumber);
        request.AddUrlSegment ("template", template);

        Connection.Put (request);
    }

    // =========================================================

    /// <summary>
    /// Изменить общие параметры сервиса.
    /// </summary>
    public void SetDefaults
        (
            string newSettings
        )
    {
        Sure.NotNullNorEmpty (newSettings);

        var request = new RestRequest ("/defaults");
        request.AddJsonBody (newSettings);

        /* IRestResponse response = */
        Connection.Put (request);
    }

    // =========================================================

    /// <summary>
    /// Обновить значения карты.
    /// </summary>
    public void UpdateCard
        (
            string cardNumber,
            string jsonText,
            bool push
        )
    {
        Sure.NotNullNorEmpty (cardNumber);
        Sure.NotNullNorEmpty (jsonText);

        var url = "/passes/{number}";
        if (push)
        {
            url += "/push";
        }

        var request = new RestRequest (url);
        request.AddUrlSegment ("number", cardNumber);
        request.AddJsonBody (jsonText);

        Connection.Put (request);
    }

    // =========================================================

    /// <summary>
    /// Обновить значения шаблона.
    /// </summary>
    public void UpdateTemplate
        (
            string templateName,
            string jsonText,
            bool push
        )
    {
        Sure.NotNullNorEmpty (templateName);
        Sure.NotNullNorEmpty (jsonText);

        var url = "/templates/{template}";
        if (push)
        {
            url += "/push";
        }

        var request = new RestRequest (url);
        request.AddUrlSegment ("template", templateName);
        request.AddJsonBody (jsonText);

        Connection.Put (request);
    }

    // =========================================================

    /// <summary>
    /// Получение регистрационных данных для карт.
    ///
    /// Эта команда позволяет получить ранее сохраненные
    /// регистрационные данные для карт, которые использовали
    /// параметры полей из заданной группы. Данные возвращаются
    /// только для карт со статусом <code>–registered–</code>.
    /// </summary>
    /// <param name="groupName">Имя группы</param>
    /// <returns>Массив регистрационных данных.</returns>
    public OsmiRegistrationInfo[] GetRegistrations
        (
            string groupName
        )
    {
        Sure.NotNullNorEmpty (groupName);

        var request = new RestRequest ("/registration/data/{group}");
        request.AddUrlSegment ("group", groupName);
        var response = Connection.Execute (request);
        var content = JObject.Parse (response.Content!);
        var registrations = (JArray?) content["registrations"];
        if (ReferenceEquals (registrations, null))
        {
            return Array.Empty<OsmiRegistrationInfo>();
        }

        var result = new List<OsmiRegistrationInfo>();
        foreach (var registration in registrations)
        {
            var info = OsmiRegistrationInfo.FromJson ((JObject) registration);
            result.Add (info);
        }

        return result.ToArray();
    }

    // =========================================================

    /// <summary>
    /// Удаление регистрационных данных карт.
    ///
    /// Эта команда удаляет ранее сохраненные регистрационные
    /// данные карт.
    /// </summary>
    /// <param name="numbers">Список серийных номеров карт.</param>
    public void DeleteRegistrations
        (
            string[] numbers
        )
    {
        Sure.NotNull (numbers);

        var request = new RestRequest ("/registration/deletedata");
        var json = new JObject();
        var registrations = new JArray();
        foreach (var number in numbers)
        {
            registrations.Add (number);
        }

        json.Add (new JProperty ("registrations", registrations));
        var jsonText = json.ToString();
        request.AddJsonBody (jsonText);
        Connection.Post (request);
    }

    // =========================================================

    #endregion
}
