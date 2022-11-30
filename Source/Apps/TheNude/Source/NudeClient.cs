// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* NudeClient.cs -- умеет получать список моделей с сайта thenude.com
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using AM;

using HtmlAgilityPack;

using RestSharp;

#endregion

#nullable enable

namespace TheNude;

/// <summary>
/// Умеет получать список моделей с сайта thenude.com.
/// </summary>
public sealed class NudeClient
{
    /// <summary>
    /// Получение HTML-документа.
    /// </summary>
    /// <param name="modelName"></param>
    /// <param name="exact"></param>
    /// <returns></returns>
    public HtmlDocument? FindModel
        (
            string modelName,
            bool exact = true
        )
    {
        WebProxy? proxy = null;
        var proxyAddress = Magna.Configuration["proxy"];
        if (!string.IsNullOrEmpty (proxyAddress))
        {
            proxy = new WebProxy
            {
                Address = new Uri (proxyAddress)
            };
        }

        const string baseUri = "https://www.thenude.com/index.php";
        var client = new RestClient (baseUri)
        {
            Timeout = -1,
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36"
        };
        if (proxy is not null)
        {
            client.Proxy = proxy;
        }

        var request = new RestRequest (Method.GET);
        request.AddHeader ("authority", "www.thenude.com");
        request.AddHeader ("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        request.AddHeader ("accept-language", "en-US;q=0.9,en;q=0.8,ja;q=0.7");
        request.AddHeader ("referer", "https://www.thenude.com/index.php");

        request.AddParameter ("page", "search");
        request.AddParameter ("action", "searchModels");
        request.AddParameter ("__form_name", "model_search");

        // request.AddParameter ("__form_name", "filtering_form");
        // request.AddParameter ("filter_per_page", "all");
        // request.AddParameter ("filter_sort", "newest");
        request.AddParameter ("m_name", modelName);
        request.AddParameter ("m_has_any_tattoos", "1");
        request.AddParameter ("m_aka", "on");
        request.AddParameter ("m_exact", exact ? "on" : "off");


        var response = client.Execute (request);
        if (!response.IsSuccessful)
        {
            return null;
        }

        var document = new HtmlDocument();
        document.LoadHtml (response.Content);

        return document;
    }

    /// <summary>
    /// Разбор полученного ответа сайта.
    /// </summary>
    public ModelInfo[]? ParseModels
        (
            HtmlDocument? document
        )
    {
        if (document is null)
        {
            return new ModelInfo[]
            {
                new () { Name = "Nothing Found" }
            };
        }

        var list = new List<ModelInfo>();

        var models = document.DocumentNode
            .Descendants ("section")
            .FirstOrDefault (x => x.HasClass ("missed-models"));
        if (models is null)
        {
            return null;
        }

        var figures = models
            .Descendants ("figure");
        foreach (var figure in figures)
        {
            var model = new ModelInfo();
            if (model.Parse (figure))
            {
                list.Add (model);
            }
        }

        return list.ToArray();
    }
}
