// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* AutomaticApi.cs -- обертка над API, предоставлемым Automatic1111
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Обертка над API, предоставляемым Automatic1111.
/// </summary>
[PublicAPI]
public sealed class AutomaticApi
{
    #region Properties

    /// <summary>
    /// Адрес сервера, по умолчанию 127.0.0.1.
    /// </summary>
    public string ServerAddress { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AutomaticApi()
        : this ("127.0.0.1")
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AutomaticApi
        (
            string serverAddress
        )
    {
        Sure.NotNullNorEmpty (serverAddress);

        ServerAddress = serverAddress;
    }

    #endregion

    #region Private members

    private static StringContent EmptyJsonContent() =>
        new (string.Empty, Encoding.UTF8, "application/json");

    private static StringContent JsonContent<T> (T value) =>
        new (JsonConvert.SerializeObject (value), Encoding.UTF8, "application/json");

    #endregion

    #region Public methods

    /// <summary>
    /// Создание текстовой инверсии.
    /// </summary>
    public async Task<CreateResponse?> CreateEmbeddingAsync
        (
            JObject payload
        )
    {
        var client = new HttpClient();
        var content = JsonContent (payload);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/create/embedding"),
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateResponse> (json);
            return result;
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
    public async Task<string?> GetCurrentModelAsync()
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
        var client = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/memory"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MemoryResponse> (json);
            return result;
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
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/options"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse (json);
            return result;
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

        var client = new HttpClient();
        var content = JsonContent (payload);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/png-info"),
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PngInfoResponse> (json);
            return result;
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
        var client = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/progress"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProgressResponse> (json);
            return result;
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
        var client = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/scipt-info"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ScriptInfo[]> (json);
            return result;
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

        var client = new HttpClient();
        var content = JsonContent (payload);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/interrogate"),
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<InterrogateResponse> (json);
            return result;
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
    public async Task InterruptAsync()
    {

        var client = new HttpClient();
        var content = EmptyJsonContent();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/interrupt"),
                Method = HttpMethod.Post,
                Content = content
            };
            await client.SendAsync (request);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during Interrupt");
        }
    }

    /// <summary>
    /// Получение списка моделей для ControlNet.
    /// </summary>
    public async Task<string[]?> ListControlNetModelsAsync()
    {
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/controlnet/model_list"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Dictionary<string, List<string>>> (json);
            if (responseObject is not null)
            {
                var result = responseObject["model_list"];

                return result.ToArray();
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListControlNetModels");
        }

        return null;
    }

    /// <summary>
    /// Получение списка моделей для ControlNet.
    /// </summary>
    public async Task<EmbeddingResponse?> ListEmbeddingsAsync()
    {
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/controlnet/embeddings"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmbeddingResponse> (json);
            return result;
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
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/controlnet/hypernetworks"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HypernetworkInfo[]> (json);
            return result;
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
    public async Task<JArray?> ListModelsAsync()
    {
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/sd-models"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var array = JsonConvert.DeserializeObject<JArray> (json);
            return array;
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
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/samplers"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SamplerInfo[]> (json);
            return result;
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
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/scripts"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ScriptsResponse> (json);
            return result;
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
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/prompt-styles"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StyleInfo[]>(json);
            return result;
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
        var client = new HttpClient();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/upscalers"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync (request);
            var jsonRaw = await response.Content.ReadAsStringAsync();
            var array = JsonConvert.DeserializeObject<UpscalerInfo[]>(jsonRaw);
            return array;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during ListUpscalers");
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

        var client = new HttpClient();
        var content = JsonContent (payload);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/preprocess"),
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProgressResponse> (json);
            return result;
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
    public async Task RefreshModelsAsync ()
    {
        var client = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/refresh-checkpoints"),
                Method = HttpMethod.Get
            };
             await client.SendAsync(request);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during RefreshModels");
        }
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
    public async Task SetModelAsync
        (
            string modelName
        )
    {
        Sure.NotNullNorEmpty (modelName);

        var payload = new JObject
        {
            ["sd_model_checkpoint"] = modelName
        };
        var client = new HttpClient();
        var content = JsonContent (payload);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/options"),
                Method = HttpMethod.Post,
                Content = content
            };
            await client.SendAsync (request);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during SetModel");
        }
    }

    /// <summary>
    /// Отмена текущего задания на обработку.
    /// </summary>
    public async Task SkipAsync()
    {
        var client = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/skip"),
                Method = HttpMethod.Get
            };
            await client.SendAsync (request);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during Skip");
        }
    }

    /// <summary>
    /// Тренировка указанной текстовой инверсии.
    /// </summary>
    public async Task<TrainResponse?> TrainEmbeddingAsync
        (
            dynamic payload
        )
    {
        var client = new HttpClient();
        var content = JsonContent (payload);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/train/embedding"),
                Method = HttpMethod.Post,
                Content = content
            };
            var response = await client.SendAsync (request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TrainResponse> (json);
            return result;
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
    public async Task TextToImageAsync
        (
            TextToImageRequest payload
        )
    {
        Sure.NotNull (payload);

        var client = new HttpClient();
        var content = JsonContent (payload);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri ($"http://{ServerAddress}/sdapi/v1/txt2img"),
                Method = HttpMethod.Post,
                Content = content
            };
            await client.SendAsync (request);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError (exception, "error during SetModel");
        }
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
