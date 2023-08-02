// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SpaceWebClient.cs -- клиент SpaceWeb API
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

#nullable enable

namespace AM.SpaceWeb;

/// <summary>
/// Клиент SpaceWeb API.
/// </summary>
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
        _restClient = new RestClient (options);
        _restClient.UseNewtonsoftJson();
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

    #endregion

}
