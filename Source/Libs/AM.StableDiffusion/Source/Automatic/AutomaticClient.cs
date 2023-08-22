// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* AutomaticClient.cs -- обертка над API, предоставлемым Automatic1111
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

namespace AM.StableDiffusion.Automatic;

//
// Документация: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/API
//

/// <summary>
/// Обертка над API, предоставляемым Automatic1111.
/// </summary>
[PublicAPI]
public sealed class AutomaticClient
{
    #region Constants

    /// <summary>
    /// URL для API по умолчанию.
    /// </summary>
    public const string DefaultUrl = "http://127.0.0.1:7860/sdapi/v1/";

    #endregion

    #region Properties

    /// <summary>
    /// Путь, по которому сохраняются картинки.
    /// </summary>
    public string OutputPath { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AutomaticClient()
        : this (DefaultUrl)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AutomaticClient
        (
            string baseUrl
        )
    {
        Sure.NotNullNorEmpty (baseUrl);

        OutputPath = "output";
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

    private static StringContent EmptyJsonContent() =>
        new (string.Empty, Encoding.UTF8, "application/json");

    private static StringContent JsonContent<T> (T value) =>
        new (JsonConvert.SerializeObject (value), Encoding.UTF8, "application/json");

    private RestRequest CreateRequest
        (
            string resource,
            Method method = Method.Get
        )
    {
        Sure.NotNullNorEmpty (resource);

        return new RestRequest (resource, method);
    }

    private void DumpRequest
        (
            RestRequest request
        )
    {
        var uri = _restClient.BuildUri (request);
        Console.WriteLine ($"Request URI: {uri}");
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Преобразование строки в байты.
    /// </summary>
    public static byte[] TextToBytes
        (
            string text
        )
    {
        if (string.IsNullOrWhiteSpace (text))
        {
            return Array.Empty<byte>();
        }

        return Convert.FromBase64String (text);
    }

    /// <summary>
    /// Сохранение полученных картинок.
    /// </summary>
    public void SaveImages
        (
            IEnumerable<string>? lines
        )
    {
        if (lines is null)
        {
            return;
        }

        Directory.CreateDirectory (OutputPath);
        foreach (var line in lines)
        {
            var extension = ".png"; // TODO определять расширение по содержимому
            var bytes = TextToBytes (line);
            var fileName = Path.Combine
                (
                    OutputPath,
                    DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-ffff")
                )
                + extension;

            File.WriteAllBytes (fileName, bytes);
        }
    }

    /// <summary>
    /// Создание текстовой инверсии.
    /// </summary>
    public async Task<CreateResponse?> CreateEmbeddingAsync
        (
            CreateEmbeddingRequest payload
        )
    {
        Sure.NotNull (payload);

        var request = CreateRequest ("create/embedding", Method.Post)
            .AddJsonBody (payload);

        try
        {
            var result = await _restClient
                .ExecuteAsync<CreateResponse> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during CreateEmbedding");
        }

        return null;
    }

    /// <summary>
    /// Получение текущей модели.
    /// </summary>
    public async Task<string?> GetCurrentCheckpointAsync()
    {
        var options = await GetOptionsAsync();
        return options is not null ?
            (string?) options["sd_model_checkpoint"]
            : null;
    }

    /// <summary>
    /// Получение информации о памяти.
    /// </summary>
    public async Task<MemoryResponse?> GetMemoryStatAsync()
    {
        var request = CreateRequest ("memory");
        try
        {
            var result = await _restClient
                .ExecuteAsync<MemoryResponse> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during GetMemoryStat");
        }

        return null;
    }

    /// <summary>
    /// Получение действующих опций.
    /// </summary>
    public async Task<JObject?> GetOptionsAsync()
    {
        var request = CreateRequest ("options");
        try
        {
            var result = await _restClient
                .ExecuteAsync<JObject> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during GetOptions");
        }

        return null;
    }


    /// <summary>
    /// Получение действующих опций.
    /// </summary>
    public async Task<PngInfoResponse?> GetPngInfoAsync
        (
            PngInfoRequest payload
        )
    {
        Sure.NotNull (payload);

        var request = CreateRequest ("png-info", Method.Post)
            .AddJsonBody (payload);

        try
        {
            var result = await _restClient
                .ExecuteAsync<PngInfoResponse> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during GetPngInfo");
        }

        return null;
    }

    /// <summary>
    /// Получение информации о прогрессе текущей операции.
    /// </summary>
    public async Task<ProgressResponse?> GetProgressAsync()
    {
        var request = CreateRequest ("progress");
        try
        {
            var result = await _restClient
                .ExecuteAsync<ProgressResponse> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during GetProgress");
        }

        return null;
    }

    /// <summary>
    /// Получение информации о скриптах.
    /// </summary>
    public async Task<ScriptInfo[]?> GetScriptInfoAsync()
    {
        var request = CreateRequest ("script-info");
        try
        {
            var result = await _restClient
                .ExecuteAsync<ScriptInfo[]> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during GetScriptInfo");
        }

        return null;
    }

    /// <summary>
    /// Определение содержимого изображения.
    /// </summary>
    public async Task<InterrogateResponse?> InterrogateAsync
        (
            InterrogateRequest payload
        )
    {
        Sure.NotNull (payload);

        var request = CreateRequest ("interrogate", Method.Post)
            .AddJsonBody (payload);
        try
        {
            var result = await _restClient
                .ExecuteAsync<InterrogateResponse> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during Interrogate");
        }

        return null;
    }

    /// <summary>
    /// Прерывание текущего действия.
    /// </summary>
    public async Task<bool> InterruptAsync()
    {
        var request = CreateRequest ("interrupt");
        try
        {
            await _restClient.ExecuteAsync (request);
            return true;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during Interrupt");
        }

        return false;
    }

    /// <summary>
    /// Получение списка моделей для ControlNet.
    /// </summary>
    public async Task<string[]?> ListControlNetModelsAsync()
    {
        var request = CreateRequest ("controlnet/model_list");
        try
        {
            var response = await _restClient
                .ExecuteAsync<Dictionary<string, List<string>>> (request);
            var obj = response.Data;
            if (obj is not null)
            {
                var result = obj["model_list"];
                return result.ToArray();
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListControlNetModel");
        }

        return null;
    }

    /// <summary>
    /// Получение списка моделей для ControlNet.
    /// </summary>
    public async Task<EmbeddingResponse?> ListEmbeddingsAsync()
    {
        var request = CreateRequest ("embeddings");
        try
        {
            var result = await _restClient
                .ExecuteAsync<EmbeddingResponse> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListEmbeddings");
        }

        return null;
    }

    /// <summary>
    /// Получение списка моделей для ControlNet.
    /// </summary>
    public async Task<HypernetworkInfo[]?> ListHypernetworksAsync()
    {
        var request = CreateRequest ("hypernetworks");
        try
        {
            var result = await _restClient
                .ExecuteAsync<HypernetworkInfo[]> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListHypernetworks");
        }

        return null;
    }

    /// <summary>
    /// Получение списка моделей.
    /// </summary>
    public async Task<JArray?> ListCheckpointsAsync()
    {
        var request = CreateRequest ("sd-models");
        try
        {
            var result = await _restClient
                .ExecuteAsync<JArray> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListModels");
        }

        return null;
    }

    /// <summary>
    /// Получение списка семплеров.
    /// </summary>
    public async Task<SamplerInfo[]?> ListSamplersAsync()
    {
        var request = CreateRequest ("samplers");
        try
        {
            var result = await _restClient
                .ExecuteAsync<SamplerInfo[]> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListSamplers");
        }

        return null;
    }

    /// <summary>
    /// Получение списка доступных скриптов.
    /// </summary>
    public async Task<ScriptsResponse?> ListScriptsAsync()
    {
        var request = CreateRequest ("scripts");
        try
        {
            var result = await _restClient
                .ExecuteAsync<ScriptsResponse> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListScripts");
        }

        return null;
    }

    /// <summary>
    /// Получение списка пользовательских стилей.
    /// </summary>
    public async Task<StyleInfo[]?> ListStylesAsync()
    {
        var request = CreateRequest ("prompt-styles");
        try
        {
            var result = await _restClient
                .ExecuteAsync<StyleInfo[]> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListStyles");
        }

        return null;
    }

    /// <summary>
    /// Получение списка апскейлеров.
    /// </summary>
    public async Task<UpscalerInfo[]?> ListUpscalersAsync()
    {
        var request = CreateRequest ("upscalers");
        try
        {
            var result = await _restClient
                .ExecuteAsync<UpscalerInfo[]> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListUpscalers");
        }

        return null;
    }

    /// <summary>
    /// Получение списка VAE.
    /// </summary>
    public async Task<VaeInfo[]?> ListVaesAsync()
    {
        var request = CreateRequest ("sd-vae");
        try
        {
            var result = await _restClient
                .ExecuteAsync<VaeInfo[]> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListVaes");
        }

        return null;
    }

    /// <summary>
    /// Подготовка датасета для обучения.
    /// </summary>
    public async Task<ProgressResponse?> PreprocessImagesAsync
        (
            JObject payload
        )
    {
        Sure.NotNull (payload);

        var request = CreateRequest ("preprocess", Method.Post)
            .AddJsonBody (payload);
        try
        {
            var result = await _restClient
                .ExecuteAsync<ProgressResponse> (request);
            return result.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during PreprocessImages");
        }

        return null;
    }

    /// <summary>
    /// Обновление списка моделей на сервере.
    /// </summary>
    public async Task<bool> RefreshModelsAsync ()
    {
        var request = CreateRequest ("refresh-checkpoints");
        try
        {
            await _restClient
                .ExecuteAsync (request);
            return true;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during RefreshModels");
        }

        return false;
    }

    /// <summary>
    /// Перезагрузка указанной модели.
    /// </summary>
    public async Task ReloadModelAsync()
    {
        await Task.Yield();
    }

    /// <summary>
    /// Установка в качестве текущей модели с указанным именем.
    /// </summary>
    public async Task<bool> SetModelAsync
        (
            string modelName
        )
    {
        Sure.NotNullNorEmpty (modelName);

        var payload = new JObject
        {
            ["sd_model_checkpoint"] = modelName
        };
        var request = CreateRequest ("options", Method.Post)
            .AddJsonBody (payload);
        try
        {
            await _restClient
                .ExecuteAsync<ProgressResponse> (request);
            return true;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during SetModel");
        }

        return false;
    }

    /// <summary>
    /// Отмена текущего задания на обработку.
    /// </summary>
    public async Task<bool> SkipAsync()
    {
        var request = CreateRequest ("skip");
        try
        {
            await _restClient.ExecuteAsync (request);
            return true;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during Skip");
        }

        return false;
    }

    /// <summary>
    /// Тренировка указанной текстовой инверсии.
    /// </summary>
    public async Task<TrainResponse?> TrainEmbeddingAsync
        (
            TrainEmbeddingRequest payload
        )
    {
        var request = CreateRequest ("train/embedding", Method.Post)
            .AddJsonBody (payload);
        try
        {
            var response = await _restClient
                .ExecuteAsync<TrainResponse> (request);
            return response.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during TrainEmbedding");
        }

        return null;
    }

    /// <summary>
    /// Генерация изображения по тексту.
    /// </summary>
    public async Task<TextToImageResponse?> TextToImageAsync
        (
            TextToImageRequest payload
        )
    {
        Sure.NotNull (payload);

        var request = CreateRequest ("txt2img", Method.Post)
            .AddJsonBody ((object) payload);

        // DumpRequest (request);

        try
        {
            var response = await _restClient
                .ExecuteAsync<TextToImageResponse> (request);
            return response.Data;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during TextToImage");
        }

        return null;
    }

    /// <summary>
    /// Выгрузка указанной модели.
    /// </summary>
    public async Task UnloadModelAsync()
    {
        await Task.Yield();
    }

    #endregion
}
