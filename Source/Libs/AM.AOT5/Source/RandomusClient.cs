// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ParaPhraserClient.cs -- простейший клиент ParaPhraser.ru
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using JetBrains.Annotations;

using RestSharp;

#endregion

#nullable enable

namespace AM.AOT;

/// <summary>
/// Клиент сервиса https://randomus.ru.
/// Генерирует случайные имена, русские или английские,
/// в различных форматах. Для русских имен поддерживается
/// генерация как полных ФИО, так и их различных вариаций,
/// включая инициалы.
/// </summary>
[PublicAPI]
public sealed class RandomusClient
{
    #region Constants
    
    /// <summary>
    /// Базовый URL.
    /// </summary>
    private const string BaseUri = "https://randomus.ru";

    /// <summary>
    /// Полное русское ФИО, например:
    /// "Белоусов Тимофей Михайлович".
    /// </summary>
    public const string RussianFio = "0";

    /// <summary>
    /// Английские имя и фамилия, например:
    /// "Keith Smith".
    /// </summary>
    public const string EnglishFio = "101";

    /// <summary>
    /// Мужской пол.
    /// </summary>
    public const string Male = "0";

    /// <summary>
    /// Женский пол.
    /// </summary>
    public const string Female = "1";

    /// <summary>
    /// Любой пол.
    /// </summary>
    public const string Both = "10";
    
    #endregion
    
    #region Public methods
    
    /// <summary>
    /// Генератор фамилий, имен и отчеств
    /// с учетом реальной частоты их использования
    /// в современном мире.
    /// </summary>
    /// <example>
    /// <code>
    /// var client = new RandomusClient();
    /// var names = client.GenerateNames
    ///  (
    ///    RandomusClient.RussianFio,
    ///    RandomusClient.Male,
    ///    5
    ///  );
    /// if (names is not null)
    /// {
    ///    foreach (var name in names)
    ///    {
    ///       Console.WriteLine (name);
    ///    }
    /// }
    /// </code> 
    /// </example>
    public string[]? GenerateNames
        (
            string type,
            string gender,
            int count = 10
        )
    {
        var options = new RestClientOptions (BaseUri)
        {
            ThrowOnAnyError = true,
            MaxTimeout = 10_000,
        };
        var client = new RestClient (options);
        var request = new RestRequest ("name");
        request.AddParameter ("type", type);
        request.AddParameter ("sex", gender);
        request.AddParameter ("count", count);
        
        var response = client.Get (request);
        if (!response.IsSuccessful)
        {
            return null;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return null;
        }

        var document = new HtmlDocument();
        document.LoadHtml (content);

        var resultContainer = document.DocumentNode
            .Descendants ("div")
            .FirstOrDefault(x => x.HasClass ("result_container"));
        if (resultContainer is null)
        {
            return null;
        }

        var result = new List<string>();
        var elements = resultContainer
            .Descendants ("div")
            .Where (x => x.HasClass ("result_element"));
        foreach (var element in elements)
        {
            var name = element.Descendants ("span").First().InnerText;
            result.Add (name);
        }

        return result.ToArray();
    }

    #endregion
}
