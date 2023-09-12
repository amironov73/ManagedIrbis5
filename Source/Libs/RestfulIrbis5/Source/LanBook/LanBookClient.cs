// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* LanBookClient.cs -- клиент для LanBook.com
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Net;

using JetBrains.Annotations;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

namespace RestfulIrbis.LanBook;

//
// GiHub: https://github.com/spb-lan/ebs-sdk
// Песочница: https://developers.lanbook.com/swagger
//

/// <summary>
/// Клиент для LanBook.com.
/// </summary>
[PublicAPI]
public sealed class LanBookClient
{
    #region Constants

    /// <summary>
    /// URL для API по умолчанию.
    /// </summary>
    public const string DefaultUrl = "https://openapi.e.lanbook.com/1.0";

    /// <summary>
    /// Тестовый токен.
    /// </summary>
    public const string TestToken = "7c0c2193d27108a509abd8ea84a8750c82b3a520";

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LanBookClient
        (
            string baseUrl,
            string token
        )
    {
        Sure.NotNullNorEmpty (baseUrl);
        Sure.NotNullNorEmpty (token);

        _httpClient = new HttpClientWithProgress();
        _token = token;
        var options = new RestClientOptions
        {
            BaseUrl = new Uri (baseUrl)
        };
        _restClient = new RestClient
            (
                _httpClient,
                options,
                configureSerialization: s => s.UseNewtonsoftJson()
            );
    }

    #endregion

    #region Private members

    private readonly string _token;
    private readonly HttpClientWithProgress _httpClient;
    private readonly RestClient _restClient;

    private RestRequest CreateRequest
        (
            string resource,
            Method method = Method.Get
        )
    {
        var result = new RestRequest (resource, method);
        result.AddHeader ("X-Auth-Token", _token);
        result.AddHeader ("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
        result.AddHeader ("Accept", "application/json");

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение книг.
    /// </summary>
    public LanBookResponse<BookInfo[]>? GetBooks()
    {
        var request = CreateRequest ("book")
            .AddParameter ("limit", 5)
            .AddParameter ("offset", 0)
            .AddParameter ("fields", "name,authors,isbn,year,publisher,lang");


        var response = _restClient.Execute (request);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<LanBookResponse<BookInfo[]>> (content)
            : default;
    }

    /// <summary>
    /// Получение книги по идентификатору.
    /// </summary>
    public LanBookResponse<BookInfo>? GetBook
        (
            string bookId
        )
    {
        Sure.NotNullNorEmpty (bookId);

        var request = CreateRequest ("book/get/{bookId}")
            .AddUrlSegment ("bookId", bookId)
            .AddParameter ("limit", 5)
            .AddParameter ("offset", 0)
            .AddParameter ("fields", "name,authors,isbn,year,publisher,lang");

        var response = _restClient.Execute (request);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<LanBookResponse<BookInfo>> (content)
            : default;
    }

    #endregion
}
