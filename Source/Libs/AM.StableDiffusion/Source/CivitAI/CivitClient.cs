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
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using AM.Net;

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
        _httpClient = new HttpClientWithProgress();

        var options = new RestClientOptions
        {
            BaseUrl = new Uri (baseUrl)
        };
        _restClient = new RestClient (_httpClient, options);
    }

    #endregion

    #region Private members

    private readonly string? _apiKey;
    private readonly HttpClientWithProgress _httpClient;
    private readonly RestClient _restClient;

    #endregion

    #region Public methods

    /// <summary>
    /// Синхронное получение информации о создателях.
    /// </summary>
    public CreatorsResponse? GetCreators
        (
            int limit = default,
            int page = default,
            string? query = default
        )
    {
        var request = new RestRequest ("/creators")
            .AddNonDefaultQueryParameter (limit)
            .AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (query);

        var response = _restClient.Execute (request);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<CreatorsResponse> (content)
            : null;
    }

    /// <summary>
    /// Асинхронное получение информации о создателях.
    /// </summary>
    public async Task<CreatorsResponse?> GetCreatorsAsync
        (
            int limit = default,
            int page = default,
            string? query = default,
            CancellationToken cancellationToken = default
        )
    {
        var request = new RestRequest ("/creators")
            .AddNonDefaultQueryParameter (limit)
            .AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (query);

        var response = await _restClient.ExecuteAsync (request, cancellationToken);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<CreatorsResponse> (content)
            : null;
    }

    /// <summary>
    /// Синхронное получение информации об изображениях.
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
        var request = new RestRequest ("/images")
            .AddNonDefaultQueryParameter (limit)
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
    /// Асинхронное получение информации об изображениях.
    /// </summary>
    public async Task<ImagesResponse?> GetImagesAsync
        (
            int limit = default,
            int page = default,
            int postId = default,
            int modelId = default,
            string? notSafe = default,
            string? username = default,
            CancellationToken cancellationToken = default
        )
    {
        var request = new RestRequest ("/images")
            .AddNonDefaultQueryParameter (limit)
            .AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (postId)
            .AddNonDefaultQueryParameter (modelId)
            .AddNonDefaultQueryParameter (notSafe)
            .AddNonDefaultQueryParameter (username);

        var response = await _restClient.ExecuteAsync (request, cancellationToken);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ImagesResponse> (content)
            : null;
    }

    /// <summary>
    /// Синхронное перечисление всех изображений, соответствующих запросу.
    /// </summary>
    public IEnumerable<ImageInfo> EnumerateImages
        (
            int postId = default,
            int modelId = default,
            string? notSafe = default,
            string? username = default,
            IProgress<ProgressInfo<int>>? progress = default
        )
    {
        const int Limit = 100;

        var response = GetImages (Limit, 0, postId, modelId, notSafe, username);
        var items = response?.Items;
        var meta = response?.Metadata;
        if (items is not null && meta is not null)
        {
            var count = 0;
            var progressInfo = new ProgressInfo<int>
            {
                StartedAt = DateTime.Now,
                Total = meta.TotalItems
            };

            foreach (var item in items)
            {
                yield return item;
                progressInfo.Done = ++count;
                progressInfo.ExtraInfo = item;
                progress?.Report (progressInfo);
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
                        progressInfo.Done = ++count;
                        progressInfo.ExtraInfo = item;
                        progress?.Report (progressInfo);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Асинхронное перечисление всех изображений, соответствующих запросу.
    /// </summary>
    public async IAsyncEnumerable<ImageInfo> EnumerateImagesAsync
        (
            int postId = default,
            int modelId = default,
            string? notSafe = default,
            string? username = default,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
    {
        const int Limit = 100;

        var response = await GetImagesAsync
            (
                Limit,
                page: 0,
                postId,
                modelId,
                notSafe,
                username,
                cancellationToken
            );
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
                response = await GetImagesAsync
                    (
                        Limit,
                        pageNumber,
                        postId,
                        modelId,
                        notSafe,
                        username,
                        cancellationToken
                    );
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
    /// Синхронное получение модели по ее имени.
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
    /// Асинхронное получение модели по ее имени.
    /// </summary>
    public async Task<ModelInfo?> GetModelAsync
        (
            string modelName,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNullNorEmpty (modelName);

        var models = await GetModelsAsync
            (
                query: modelName,
                cancellationToken: cancellationToken
            );
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

        var result = await GetModelAsync (found.Id, cancellationToken);

        return result;
    }

    /// <summary>
    /// Синхронное получение модели по ее идентификатору.
    /// </summary>
    public ModelInfo? GetModel
        (
            int modelId
        )
    {
        Sure.Positive (modelId);

        var request = new RestRequest ("/models/{id}")
            .AddUrlSegment ("id", modelId);

        var response = _restClient.Execute (request);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ModelInfo> (content)
            : null;
    }

    /// <summary>
    /// Асинхронное получение модели по ее идентификатору.
    /// </summary>
    public async Task<ModelInfo?> GetModelAsync
        (
            int modelId,
            CancellationToken cancellationToken = default
        )
    {
        Sure.Positive (modelId);

        var request = new RestRequest ("/models/{id}")
            .AddUrlSegment ("id", modelId);

        var response = await _restClient.ExecuteAsync (request, cancellationToken);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ModelInfo> (content)
            : null;
    }

    /// <summary>
    /// Синхронное получение модели по ее идентификатору.
    /// </summary>
    public ModelVersion? GetModelVersion
        (
            int versionId
        )
    {
        Sure.Positive (versionId);

        var request = new RestRequest ($"/models-versions/{versionId}")
            .AddUrlSegment ("versionId", versionId);

        var response = _restClient.Execute (request);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ModelVersion> (content)
            : null;
    }

    /// <summary>
    /// Асинхронное получение модели по ее идентификатору.
    /// </summary>
    public async Task<ModelVersion?> GetModelVersionAsync
        (
            int versionId,
            CancellationToken cancellationToken = default
        )
    {
        Sure.Positive (versionId);

        var request = new RestRequest ($"/models-versions/{versionId}")
            .AddUrlSegment ("versionId", versionId);

        var response = await _restClient.ExecuteAsync (request, cancellationToken);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ModelVersion> (content)
            : null;
    }

    /// <summary>
    /// Синхронное получение информации о моделях.
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
        var request = new RestRequest ("/models")
            .AddNonDefaultQueryParameter (limit)
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
    /// Асинхронное получение информации о моделях.
    /// </summary>
    public async Task<ModelsResponse?> GetModelsAsync
        (
            int limit = default,
            int page = default,
            string? query = default,
            string? tag = default,
            string? username = default,
            string[]? types = default,
            int rating = default,
            string? notSafe = default,
            CancellationToken cancellationToken = default
        )
    {
        var request = new RestRequest ("/models")
            .AddNonDefaultQueryParameter (limit)
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

        var response = await _restClient.ExecuteAsync (request, cancellationToken);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<ModelsResponse> (content)
            : null;
    }

    /// <summary>
    /// Синхронное получение информации о метке.
    /// </summary>
    public TagsResponse? GetTag
        (
            string query,
            int page = default,
            int limit = default
        )
    {
        Sure.NotNullNorEmpty (query);

        var request = new RestRequest ("/tags")
            .AddQueryParameter ("query", query)
            .AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (limit);

        var response = _restClient.Execute (request);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<TagsResponse> (content)
            : null;
    }

    /// <summary>
    /// Асинхронное получение информации о метке.
    /// </summary>
    public async Task<TagsResponse?> GetTagAsync
        (
            string query,
            int page = default,
            int limit = default,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNullNorEmpty (query);

        var request = new RestRequest ("/tags")
            .AddQueryParameter ("query", query)
            .AddNonDefaultQueryParameter (page)
            .AddNonDefaultQueryParameter (limit);

        var response = await _restClient.ExecuteAsync (request, cancellationToken);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<TagsResponse> (content)
            : null;
    }

    /// <summary>
    /// Синхронное скачивание указанного изображения.
    /// </summary>
    public Image? DownloadImage
        (
            ImageInfo imageInfo,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (imageInfo);

        var url = imageInfo.Url;
        if (string.IsNullOrEmpty (url))
        {
            return null;
        }

        var memory = new MemoryStream();
        var headers = _httpClient.Download
            (
                new Uri (url),
                memory,
                progress,
                cancellationToken
            );
        var result = headers is null
            ? null
            : Image.Load (memory);

        return result;
    }

    /// <summary>
    /// Асинхронное скачивание указанного изображения.
    /// </summary>
    public async Task<Image?> DownloadImageAsync
        (
            ImageInfo imageInfo,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (imageInfo);

        var url = imageInfo.Url;
        if (string.IsNullOrEmpty (url))
        {
            return null;
        }

        var memory = new MemoryStream();
        var headers = await _httpClient.DownloadAsync
            (
                new Uri (url),
                memory,
                progress,
                cancellationToken
            );
        var result = headers is null
            ? null
            : await Image.LoadAsync (memory, cancellationToken);

        return result;
    }

    /// <summary>
    /// Синхронное скачивание указанного изображения.
    /// </summary>
    public HttpHeaders? SaveImage
        (
            ImageInfo imageInfo,
            string? directoryToSave = default,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (imageInfo);

        directoryToSave ??= Directory.GetCurrentDirectory();
        Directory.CreateDirectory (directoryToSave);
        var url = imageInfo.Url;
        if (string.IsNullOrEmpty (url))
        {
            return null;
        }

        var uri = new Uri (url);
        var fileName = Path.GetFileName (uri.LocalPath);
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        fileName = Path.Combine (directoryToSave, fileName);
        File.Delete (fileName);

        return SaveFile (uri, fileName, progress, cancellationToken);
    }

    /// <summary>
    /// Асинхронное скачивание указанного изображения.
    /// </summary>
    public async Task<bool> SaveImageAsync
        (
            ImageInfo imageInfo,
            string? directoryToSave = default,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
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
        await SaveFileAsync (uri, fileName, progress, cancellationToken);

        return true;
    }

    /// <summary>
    /// Синхронное скачивание файла модели.
    /// </summary>
    public HttpHeaders? SaveFile
        (
            FileInfo fileInfo,
            string? directoryToSave = default,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (fileInfo);

        directoryToSave ??= Directory.GetCurrentDirectory();
        Directory.CreateDirectory (directoryToSave);
        var url = fileInfo.DownloadUrl;
        if (string.IsNullOrEmpty (url))
        {
            return null;
        }

        var fileName = fileInfo.Name;
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        fileName = Path.Combine (directoryToSave, fileName);
        File.Delete (fileName);

        return SaveFile (new Uri (url), fileName, progress, cancellationToken);
    }

    /// <summary>
    /// Асинхронное скачивание файла.
    /// </summary>
    public HttpHeaders? SaveFile
        (
            Uri requestUri,
            string fileName,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (requestUri);
        Sure.NotNullNorEmpty (fileName);

        return _httpClient.DownloadFile
            (
                requestUri,
                fileName,
                progress,
                cancellationToken
            );
    }

    /// <summary>
    /// Асинхронное скачивание файла.
    /// </summary>
    public async Task SaveFileAsync
        (
            Uri requestUri,
            string fileName,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (requestUri);
        Sure.NotNullNorEmpty (fileName);

        await _httpClient.DownloadFileAsync
            (
                requestUri,
                fileName,
                progress,
                cancellationToken
            );
    }

    /// <summary>
    /// Асинхронное скачивание файла модели.
    /// </summary>
    public async Task<bool> SaveFileAsync
        (
            FileInfo fileInfo,
            string? directoryToSave = default,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
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

        await SaveFileAsync (new Uri (url), fileName, progress, cancellationToken);

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
            IDownloadProgress? progress = default,
            bool withImage = false,
            CancellationToken cancellationToken = default
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
            if (SaveFile (file, directoryToSave, progress, cancellationToken) is null)
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
                SaveFile (uri, fileName, progress, cancellationToken);
            }
        }

        return true;
    }

    /// <summary>
    /// Асинхронное скачивание указанной модели.
    /// </summary>
    public async Task<bool> SaveModelAsync
        (
            ModelInfo modelInfo,
            string? versionName = default,
            string? directoryToSave = default,
            bool withImage = false,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default
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
            if (!await SaveFileAsync (file, directoryToSave, progress, cancellationToken))
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
                await SaveFileAsync
                    (
                        new Uri (imageUrl),
                        fileName,
                        progress,
                        cancellationToken
                    );
            }
        }

        return true;
    }

    #endregion
}
