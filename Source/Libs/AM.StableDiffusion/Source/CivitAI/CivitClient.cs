// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* CivitClient.cs -- клиент для CivitAI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;

using RestSharp;

using SixLabors.ImageSharp;

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
public sealed class CivitClient
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
    public CivitClient()
        : this (BaseUrl, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CivitClient
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
    /// Получение информации о создателях.
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

        request.AddNonDefaultQueryParameter (limit)
            .AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (query);

        var response = _restClient.Execute (request);
        return response.Content is { } content
            ? JsonConvert.DeserializeObject<CreatorsResponse> (content)
            : null;
    }

    /// <summary>
    /// Получение информации об изображениях.
    /// </summary>
    public ImagesResponse? GetImages
        (
            int limit = default,
            int page = default,
            int postId = default,
            int modelId = default,
            string? notSafe = default,
            string? username = default
        )
    {
        var request = new RestRequest
            (
                "/images"
            );

        request.AddNonDefaultQueryParameter (limit)
            .AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (postId)
            .AddNonDefaultQueryParameter (modelId)
            .AddNonDefaultQueryParameter (notSafe)
            .AddNonDefaultQueryParameter (username);

        var response = _restClient.Execute (request);
        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ImagesResponse> (content)
            : null;
    }

    /// <summary>
    /// Перечисление всех изображений, соответствующих запросу.
    /// </summary>
    public IEnumerable<ImageInfo> EnumerateImages
        (
            int postId = default,
            int modelId = default,
            string? notSafe = default,
            string? username = default
        )
    {
        const int Limit = 100;

        var response = GetImages (Limit, 0, postId, modelId, notSafe, username);
        var items = response?.Items;
        var meta = response?.Metadata;
        if (items is not null && meta is not null)
        {
            foreach (var item in items)
            {
                yield return item;
            }

            for (var pageNumber = 2; pageNumber < meta.TotalPages; pageNumber++)
            {
                response = GetImages (Limit, pageNumber, postId, modelId, notSafe, username);
                items = response?.Items;
                if (items is not null)
                {
                    foreach (var item in items)
                    {
                        yield return item;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Получение модели по ее имени.
    /// </summary>
    public ModelInfo? GetModel
        (
            string modelName
        )
    {
        Sure.NotNullNorEmpty (modelName);

        var models = GetModels (query: modelName);
        var items = models?.Items;
        if (items is null || items.Length == 0)
        {
            return null;
        }

        var found = items
            .FirstOrDefault (it => it.Name.SameString (modelName));
        if (found is null)
        {
            return null;
        }

        var result = GetModel (found.Id);

        return result;
    }

    /// <summary>
    /// Получение модели по ее идентификатору.
    /// </summary>
    public ModelInfo? GetModel
        (
            int modelId
        )
    {
        Sure.Positive (modelId);

        var request = new RestRequest
            (
                "/models/{id}"
            );
        request.AddUrlSegment ("id", modelId);

        var response = _restClient.Execute (request);
        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ModelInfo> (content)
            : null;
    }

    /// <summary>
    /// Получение модели по ее идентификатору.
    /// </summary>
    public ModelVersion? GetModelVersion
        (
            int versionId
        )
    {
        Sure.Positive (versionId);

        var request = new RestRequest
            (
                $"/models-versions/:{versionId.ToInvariantString()}"
            );

        var response = _restClient.Execute (request);
        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ModelVersion> (content)
            : null;
    }

    /// <summary>
    /// Получение информации о моделях.
    /// </summary>
    public ModelsResponse? GetModels
        (
            int limit = default,
            int page = default,
            string? query = default,
            string? tag = default,
            string? username = default,
            string[]? types = default,
            int rating = default,
            string? notSafe = default
        )
    {
        var request = new RestRequest
            (
                "/models"
            );

        request.AddNonDefaultQueryParameter (limit)
            .AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (query)
            .AddNonDefaultQueryParameter (tag)
            .AddNonDefaultQueryParameter (username)
            .AddNonDefaultQueryParameter (rating)
            .AddNonDefaultQueryParameter (notSafe);

        if (types is not null)
        {
            foreach (var type in types)
            {
                request.AddQueryParameter ("types", type);
            }
        }

        var response = _restClient.Execute (request);
        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ModelsResponse> (content)
            : null;
    }

    /// <summary>
    /// Получение информации о метке.
    /// </summary>
    /// <returns></returns>
    public TagsResponse? GetTag
        (
            string query,
            int page = default,
            int limit = default
        )
    {
        Sure.NotNullNorEmpty (query);

        var request = new RestRequest
            (
                "/tags"
            );

        request.AddQueryParameter ("query", query);
        request.AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (limit);

        var response = _restClient.Execute (request);
        return response.Content is { } content
            ? JsonConvert.DeserializeObject<TagsResponse> (content)
            : null;
    }

    /// <summary>
    /// Синхронное скачивание указанного изображения.
    /// </summary>
    public Image? DownloadImage
        (
            ImageInfo imageInfo
        )
    {
        Sure.NotNull (imageInfo);

        var url = imageInfo.Url;
        if (string.IsNullOrEmpty (url))
        {
            return null;
        }

        var request = new RestRequest (url);
        var data = _restClient.DownloadData (request);
        if (data is null)
        {
            return null;
        }

        var result = Image.Load (data);

        return result;
    }

    /// <summary>
    /// Синхронное скачивание указанного изображения.
    /// </summary>
    public bool SaveImage
        (
            ImageInfo imageInfo,
            string? directoryToSave = default
        )
    {
        Sure.NotNull (imageInfo);

        directoryToSave ??= Directory.GetCurrentDirectory();
        Directory.CreateDirectory (directoryToSave);
        var url = imageInfo.Url;
        if (string.IsNullOrEmpty (url))
        {
            return false;
        }

        var uri = new Uri (url);
        var fileName = Path.GetFileName (uri.LocalPath);
        if (string.IsNullOrEmpty (fileName))
        {
            return false;
        }

        fileName = Path.Combine (directoryToSave, fileName);
        File.Delete (fileName);

        var request = new RestRequest (url);
        var data = _restClient.DownloadData (request);
        if (data is null)
        {
            return false;
        }

        File.WriteAllBytes (fileName, data);

        return true;
    }

    /// <summary>
    /// Синхронное скачивание файла модели.
    /// </summary>
    public bool SaveFile
        (
            FileInfo fileInfo,
            string? directoryToSave = default
        )
    {
        Sure.NotNull (fileInfo);

        directoryToSave ??= Directory.GetCurrentDirectory();
        Directory.CreateDirectory (directoryToSave);
        var url = fileInfo.DownloadUrl;
        if (string.IsNullOrEmpty (url))
        {
            return false;
        }

        var fileName = fileInfo.Name;
        if (string.IsNullOrEmpty (fileName))
        {
            return false;
        }

        fileName = Path.Combine (directoryToSave, fileName);
        File.Delete (fileName);

        var request = new RestRequest (url);
        var data = _restClient.DownloadData (request);
        if (data is null)
        {
            return false;
        }

        File.WriteAllBytes (fileName, data);

        return true;
    }

    /// <summary>
    /// Синхронное скачивание указанной модели.
    /// </summary>
    public bool SaveModel
        (
            ModelInfo modelInfo,
            string? versionName = default,
            string? directoryToSave = default,
            bool withImage = false
        )
    {
        Sure.NotNull (modelInfo);

        var version = modelInfo.GetVersion (versionName);
        var files = version?.Files;
        if (files is null || files.Length == 0)
        {
            return false;
        }

        foreach (var file in files)
        {
            if (!SaveFile (file, directoryToSave))
            {
                return false;
            }
        }

        if (withImage)
        {
            directoryToSave ??= Directory.GetCurrentDirectory();
            Directory.CreateDirectory (directoryToSave);
            var images = version?.Images;
            var primaryName =
                (
                    files.FirstOrDefault (it => it.Primary)
                    ?? files.FirstOrDefault()
                )
                ?.Name;
            var imageUrl = images?.FirstOrDefault()?.Url;
            if (!string.IsNullOrEmpty (primaryName)
                && !string.IsNullOrEmpty (imageUrl))
            {
                var fileName = Path.GetFileNameWithoutExtension (primaryName);
                var uri = new Uri (imageUrl);
                var extension = Path.GetExtension (uri.LocalPath);
                fileName = Path.Combine
                    (
                        directoryToSave,
                        fileName + extension
                    );
                File.Delete (fileName);

                var request = new RestRequest (imageUrl);
                var data = _restClient.DownloadData (request);
                if (data is null)
                {
                    return false;
                }

                File.WriteAllBytes (fileName, data);
            }
        }

        return true;
    }

    /// <summary>
    /// Массированная загрузка изображений по метке.
    /// </summary>
    public bool SaveImagesByTag
        (
            string tagName,
            string? directoryToSave = default
        )
    {
        Sure.NotNull (tagName);

        directoryToSave ??= Directory.GetCurrentDirectory();
        Directory.CreateDirectory (directoryToSave);

        var result = true;
        var tags = GetTag (tagName);
        var tagItems = tags?.Items;
        if (tagItems is null || tagItems.Length == 0)
        {
            return false;
        }

        foreach (var tagItem in tagItems)
        {
            var tagModels = GetModels (tag: tagItem.Name);
            if (tagModels?.Items is { } tagModelItems)
            {
                foreach (var tagModel in tagModelItems)
                {
                    Console.WriteLine (tagModel);
                }
            }
        }

        return result;
    }

    #endregion
}
