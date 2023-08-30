// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SpaceWebClient.cs -- клиент SpaceWeb API
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

namespace AM.SpaceWeb;

/// <summary>
/// Клиент SpaceWeb API.
/// </summary>
[PublicAPI]
public sealed class SpaceWebClient
{
    #region Constants

    /// <summary>
    /// URL для API по умолчанию.
    /// </summary>
    public const string DefaultUrl = "https://api.sweb.ru/";

    #endregion

    #region Properties

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string? Password { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SpaceWebClient()
        : this (DefaultUrl)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SpaceWebClient
        (
            string baseUrl
        )
    {
        Sure.NotNullNorEmpty (baseUrl);

        var options = new RestClientOptions
        {
            BaseUrl = new Uri (baseUrl)
        };
        _restClient = new RestClient
            (
                options,
                configureSerialization: s => s.UseNewtonsoftJson()
            );
    }

    #endregion

    #region Private members

    private readonly RestClient _restClient;
    private string? _apiToken;

    private RestRequest CreateRequest
        (
            string resource,
            Method method = Method.Post
        )
    {
        Sure.NotNullNorEmpty (resource);

        var result = new RestRequest (resource, method);
        if (!string.IsNullOrEmpty (_apiToken))
        {
            result.AddHeader ("Authorization", $"Bearer {_apiToken}");
        }

        return result;
    }

    private RestRequest CreateRequest
        (
            string resource,
            string method,
            object payload
        )
    {
        Sure.NotNullNorEmpty (resource);
        Sure.NotNullNorEmpty (method);
        Sure.NotNull (payload);

        var result = CreateRequest (resource)
            .AddJsonBody (new
            {
                jsonrpc = "2.0",
                method,
                @params = payload
            });

        return result;
    }

    private static TResult? GetResult<TResult>
        (
            RestResponse? response
        )
        where TResult: class
    {
        if (response is null)
        {
            return default;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return default;
        }

        var data = JsonConvert.DeserializeObject<SpaceWebResponse<TResult>> (content);

        return data?.Result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение токена для взаимодействия с API.
    /// </summary>
    public bool GetToken
        (
            string login,
            string password
        )
    {
        var request = CreateRequest
            (
                "notAuthorized",
                "getToken",
                new { login, password }
            );
        var response = _restClient.Execute (request);
        var token = GetResult<string> (response);
        if (string.IsNullOrEmpty (token))
        {
            return false;
        }

        _apiToken = token;

        return true;
    }

    /// <summary>
    /// Требование получения токена для доступа.
    /// </summary>
    public void RequireToken()
    {
        if (string.IsNullOrEmpty (_apiToken))
        {
            var login = Login.ThrowIfNullOrEmpty();
            var password = Password.ThrowIfNullOrEmpty();

            if (!GetToken (login, password))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }

    /// <summary>
    /// Получение списка с краткой информацией о каждом сайте.
    /// </summary>
    public BriefSiteInfo[]? ListSites
        (
            int pageNumber = 1,
            int perPage = 20,
            string? filter = default
        )
    {
        RequireToken();

        var request = CreateRequest
            (
                "sites",
                "index",
                new { page = pageNumber, perPage, filter }
            );
        var response = _restClient.Execute (request);
        var result = GetResult<BriefSiteInfo[]> (response);

        return result;
    }

    /// <summary>
    /// Получение полной информации о сайте
    /// с указанной корневой директорией.
    /// </summary>
    public FullSiteInfo? GetSiteInfo
        (
            string docRoot
        )
    {
        Sure.NotNullNorEmpty (docRoot);

        RequireToken();

        var request = CreateRequest
            (
                "sites",
                "getSiteInfo",
                new { docRoot }
            );

        var response = _restClient.Execute (request);
        var result = GetResult<FullSiteInfo> (response);

        return result;
    }

    /// <summary>
    /// Добавление нового сайта.
    /// </summary>
    public bool AddSite
        (
            NewSite newSite
        )
    {
        Sure.NotNull (newSite);

        RequireToken();

        var request = CreateRequest
            (
                "sites",
                "add",
                newSite
            );

        var response = _restClient.Execute (request);
        var code = GetResult<string> (response).SafeToInt32();

        return code != 0;
    }

    /// <summary>
    /// Удаление сайте, расположенного в указанной директории.
    /// </summary>
    public bool DeleteSite
        (
            string docRoot
        )
    {
        Sure.NotNullNorEmpty (docRoot);

        RequireToken();

        var request = CreateRequest
            (
                "sites",
                "del",
                new { docRoot }
            );

        var response = _restClient.Execute (request);
        var code = GetResult<string> (response).SafeToInt32();

        return code != 0;
    }

    /// <summary>
    /// Включение/выключение SSH.
    /// </summary>
    /// <param name="on">Включение/выключение.</param>
    /// <param name="period">На какой период (в часах),
    /// имеет смысл только для включения.</param>
    public bool ToggleSsh
        (
            bool on,
            int period = 3
        )
    {
        RequireToken();

        var request = CreateRequest
            (
                "vh/utils",
                on ? "sshOn" : "sshOff",
                new { period }
            );

        var response = _restClient.Execute (request);
        var code = GetResult<string> (response).SafeToInt32();

        return code != 0;

    }

    #endregion
}
