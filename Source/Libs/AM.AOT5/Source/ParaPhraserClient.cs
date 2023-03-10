// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ParaPhraserClient.cs -- простейший клиент ParaPhraser.ru
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using RestSharp;

#endregion

#nullable enable

namespace AM.AOT;

/// <summary>
/// Клиент сервиса http://paraphraser.ru
/// Пользоватеться API можно только
/// при условии регистрации на сайте.
/// </summary>
/// <example>
/// <code>
/// var client = new ParaPhraserClient
/// {
///    Username = "librarian",
///    Password = "secret"
/// };
///
/// if (client.Initialize())
/// {
///     var lines = client.Vector ("все будет хорошо");
///     if (lines is not null)
///     {
///        foreach (var line in lines)
///        {
///            Console.WriteLine (line);
///        }
///     }
/// }
/// </code>
/// </example>
[PublicAPI]
public sealed class ParaPhraserClient
{
    #region Properties

    /// <summary>
    /// Базовый UTL.
    /// </summary>
    public string BaseUri { get; init; } = "http://paraphraser.ru/";
    
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public required string Password { get; init; }

    #endregion

    #region Private members

    /// <summary>
    /// Токен, необходимый для доступа к сервису.
    /// Получение токена см. метод Initialize.
    /// </summary>
    private string? _token;

    /// <summary>
    /// REST-клиент.
    /// </summary>
    private RestClient? _restClient;

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнять запросы к API можно только
    /// имея учетную запись на ParaPhraser.ru.
    /// Этот метод должен быть вызван до всех остальных.
    /// </summary>
    public bool Initialize()
    {
        var options = new RestClientOptions (BaseUri)
        {
            ThrowOnAnyError = true,
            MaxTimeout = 10_000,
        };
        _restClient = new RestClient (options);

        var request = new RestRequest("token");
        request.AddParameter ("login", Username);
        request.AddParameter ("password", Password);

        var response = _restClient.Get (request);
        if (!response.IsSuccessful)
        {
            return false;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return false;
        }

        var document = JObject.Parse (content);
        _token = document.Root["token"]!.Value<string>();
        
        return true;
    }

    /// <summary>
    /// Возвращает синонимы к заданному слову (фразе)
    /// на основе Yet Another RussNet.
    /// </summary>
    public string?[]? Synonims
        (
            string text,
            int count = 4,
            string language = "ru"
        )
    {
        if (_restClient is null || string.IsNullOrEmpty (text))
        {
            return null;
        }

        var request = new RestRequest("api");
        request.AddParameter ("token", _token);
        request.AddParameter ("c", "syns");
        request.AddParameter ("query", text);
        request.AddParameter ("top", count);
        request.AddParameter ("lang", language);
        request.AddParameter ("scores", 0);
        request.AddParameter ("forms", 0);
        request.AddParameter ("format", "json");
        
        var response = _restClient.Get (request);
        if (!response.IsSuccessful)
        {
            return null;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return null;
        }

        var document = JObject.Parse (content);
        var message = document.Root["msg"]?.Value<string>();
        if (message is "OK")
        {
            var result = document.Root["response"]?["1"]?["syns"]?.Values<string>().ToArray();
            
            return result!;
        }

        return null;
    }

    /// <summary>
    /// Возвращает список слов или фраз, близких по смыслу
    /// исходному слову или фразе, на основе векторной
    /// семантической модели.
    /// </summary>
    public string?[]? Vector
        (
            string text,
            int count = 4,
            string language = "ru"
        )
    {
        if (_restClient is null || string.IsNullOrEmpty (text))
        {
            return null;
        }

        var request = new RestRequest("api");
        request.AddParameter ("token", _token);
        request.AddParameter ("c", "vector");
        request.AddParameter ("query", text);
        request.AddParameter ("top", count);
        request.AddParameter ("lang", language);
        request.AddParameter ("scores", 0);
        request.AddParameter ("forms", 0);
        request.AddParameter ("format", "json");

        var response = _restClient.Get (request);
        if (!response.IsSuccessful)
        {
            return null;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return null;
        }

        var document = JObject.Parse (content);
        var message = document.Root["msg"]?.Value<string>();
        if (message is "OK")
        {
            var result = document.Root["response"]?["1"]?["vector"]?.Values<string>().ToArray();
            
            return result!;
        }

        return null;
    }
    
    /// <summary>
    /// Возвращает тематику запроса исходя из классификации Википедии.
    /// </summary>
    public string?[]? WikiTopics
        (
            string text,
            int count = 4,
            string language = "ru"
        )
    {
        if (_restClient is null || string.IsNullOrEmpty (text))
        {
            return null;
        }

        var request = new RestRequest("api");
        request.AddParameter ("token", _token);
        request.AddParameter ("c", "wikitopic");
        request.AddParameter ("query", text);
        request.AddParameter ("top", count);
        request.AddParameter ("lang", language);
        request.AddParameter ("scores", 0);
        request.AddParameter ("forms", 0);
        request.AddParameter ("format", "json");

        var response = _restClient.Get (request);
        if (!response.IsSuccessful)
        {
            return null;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return null;
        }

        var document = JObject.Parse (content);
        var message = document.Root["msg"]?.Value<string>();
        if (message is "OK")
        {
            var result = document.Root["response"]?["topics"]?.Values<string>().ToArray();
            
            return result!;
        }

        return null;
    }
    
    /// <summary>
    /// Возвращает список ключевых слов и фраз для текста.
    /// </summary>
    public string?[]? Keywords
        (
            string text,
            int count = 4,
            string language = "ru"
        )
    {
        if (_restClient is null || string.IsNullOrEmpty (text))
        {
            return null;
        }

        var request = new RestRequest("api");
        request.AddParameter ("token", _token);
        request.AddParameter ("c", "keywords");
        request.AddParameter ("query", text);
        request.AddParameter ("top", count);
        request.AddParameter ("expand", 0);
        request.AddParameter ("mwe", 0);
        request.AddParameter ("lang", language);
        request.AddParameter ("scores", 0);
        request.AddParameter ("forms", 0);
        request.AddParameter ("clusters", 0);
        request.AddParameter ("stopwords", "");
        request.AddParameter ("format", "json");

        var response = _restClient.Post (request);
        if (!response.IsSuccessful)
        {
            return null;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return null;
        }

        var document = JObject.Parse (content);
        var message = document.Root["msg"]?.Value<string>();
        if (message is "OK")
        {
            var result = document.Root["response"]?["keywords"]?.Values<string>().ToArray();
            
            return result!;
        }

        return null;
    }

    #endregion
}
