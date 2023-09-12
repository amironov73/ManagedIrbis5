// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* OpenLibraryClient.cs -- клиент для OpenLibrary.org
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Net;

using JetBrains.Annotations;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

namespace RestfulIrbis.OpenLibrary;

//
// Документация: https://openlibrary.org/developers/api
// Песочница: https://openlibrary.org/swagger/docs
//


/// <summary>
/// Клиент для OpenLibrary.org.
/// </summary>
[PublicAPI]
public sealed class OpenLibraryClient
{
    #region Constants

    /// <summary>
    /// URL для API по умолчанию.
    /// </summary>
    public const string DefaultUrl = "https://openlibrary.org";

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public OpenLibraryClient()
        : this (DefaultUrl)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OpenLibraryClient
        (
            string baseUrl
        )
    {
        _httpClient = new HttpClientWithProgress();

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

    private readonly HttpClientWithProgress _httpClient;
    private readonly RestClient _restClient;

    private RestRequest CreateRequest
        (
            string resource,
            Method method = Method.Get
        )
    {
        var result = new RestRequest (resource, method);

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение информации по ISBN.
    /// </summary>
    public IsbnResponse? GetIsbn
        (
            string? isbn
        )
    {
        if (string.IsNullOrEmpty (isbn))
        {
            return default;
        }

        var request = CreateRequest ("isbn/{number}")
            .AddUrlSegment ("number", isbn);
        var response = _restClient.Execute (request);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<IsbnResponse> (content)
            : default;
    }

    /// <summary>
    /// Получение обложки.
    /// </summary>
    /// <param name="key">Атрибут для поиска: ISBN, OCLC, LCCN, OLID, ID.</param>
    /// <param name="value">Значение искомого атрибута.</param>
    /// <param name="size">Размер: S, M или L.</param>
    public byte[]? GetCover
        (
            string key,
            string? value,
            string size
        )
    {
        if (string.IsNullOrEmpty (key)
            || string.IsNullOrEmpty (value)
            || string.IsNullOrEmpty (size))
        {
            return default;
        }

        var client = new RestClient ("https://covers.openlibrary.org");
        var request = new RestRequest ("b/{key}/{value}-{size}")
            .AddUrlSegment ("key", key)
            .AddUrlSegment ("value", value)
            .AddUrlSegment ("size", size);

        return client.DownloadData (request);
    }

    /// <summary>
    /// Получение маленькой обложки по укзанному ISBN.
    /// </summary>
    public byte[]? SmallCoverForIsbn (string? isbn)
        => GetCover ("isbn", isbn, "S");

    /// <summary>
    /// Получение большой обложки по укзанному ISBN.
    /// </summary>
    public byte[]? LargeCoverForIsbn (string? isbn)
        => GetCover ("isbn", isbn, "L");

    #endregion
}
