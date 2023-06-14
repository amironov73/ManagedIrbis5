// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* CivitAIClient.cs -- клиент для CivitAI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

using RestSharp;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

//
// Документация: https://github.com/civitai/civitai/wiki/REST-API-Reference
//

/// <summary>
/// Клиент CivitAI.
/// </summary>
[PublicAPI]
public sealed class CivitAIClient
{
    #region Constants

    /// <summary>
    /// Базовый URL для API.
    /// </summary>
    public const string BaseUrl = "https://civitai.com/api/v1/";

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public CivitAIClient()
        : this (BaseUrl, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CivitAIClient
        (
            string baseUrl,
            string? apiKey
        )
    {
        Sure.NotNullNorEmpty (baseUrl);

        _apiKey = apiKey;
        _restClient = new RestClient (baseUrl);
    }

    #endregion

    #region Private members

    private readonly string? _apiKey;
    private readonly RestClient _restClient;

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка создателей.
    /// </summary>
    public CreatorsResponse? GetCreators
        (
            int limit = default,
            int page = default,
            string? query = default
        )
    {
        var request = new RestRequest
            (
                "/creators"
            );

        request.AddNonDefaultUrlSegment (limit);
        request.AddNonDefaultUrlSegment (page);
        request.AddNonDefaultUrlSegment (query);

        var response = _restClient.Execute (request);
        return response.Content is { } content
            ? JsonConvert.DeserializeObject<CreatorsResponse> (content)
            : null;
    }

    #endregion
}
