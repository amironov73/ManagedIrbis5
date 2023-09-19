// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* IrbisCorpClient.cs -- клиент для ИРБИС-корпорации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AM;

using HtmlAgilityPack;

using JetBrains.Annotations;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

namespace RestfulIrbis.IrbisCorp;

/// <summary>
/// Клиент для ИРБИС-корпорации.
/// </summary>
[PublicAPI]
public sealed class IrbisCorpClient
{
    #region Constants

    /// <summary>
    /// URL для API по умолчанию.
    /// </summary>
    public const string DefaultUrl = "http://icorp2.elnit.org/icorp";

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisCorpClient
        (
            string baseUrl,
            string cookie
        )
    {
        Sure.NotNullNorEmpty (baseUrl);
        Sure.NotNullNorEmpty (cookie);

        _cookie = cookie;
        var options = new RestClientOptions
        {
            BaseUrl = new Uri (baseUrl)
        };
        _restClient = new RestClient
            (
                options,
                configureSerialization: s => s.UseNewtonsoftJson()
            );
    }

    #endregion

    #region Private members

    private readonly string _cookie;
    private readonly RestClient _restClient;

    private RestRequest CreateRequest
        (
            string resource,
            Method method = Method.Get
        )
    {
        var result = new RestRequest (resource, method);
        result.AddHeader ("Cookie", $"ICORPS=_cookie");

        return result;
    }

    private static string? CleanRecord
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return default;
        }

        var lines = text.SplitLines();
        var result = new List<string>();
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty (line) && line[0] is '#' or '*')
            {
                result.Add (line);
            }
        }

        if (result.Count is 0)
        {
            return default;
        }

        return string.Join (Environment.NewLine, result.ToArray());
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение книг.
    /// </summary>
    public string? SearchBroadcast
        (
            IrbisCorpQuery query
        )
    {
        Sure.NotNull (query);

        var request = CreateRequest ("index.php")
            .AddParameter ("task", "search_broadcast")
            .AddParameter ("isbn", query.Isbn)
            .AddParameter ("author", query.Author)
            .AddParameter ("title", query.Title)
            .AddParameter ("year", query.Year);

        var response = _restClient.Execute (request);

        return response.Content;
    }

    /// <summary>
    /// Извлечение идентификатора запроса из возвращенного ответа.
    /// </summary>
    public string? ExtractRequestId
        (
            string? content
        )
    {
        if (string.IsNullOrEmpty (content))
        {
            return default;
        }

        var regex = new Regex ("renew_results\\(\"(-?\\d+)\"\\)");
        var match = regex.Match (content);
        if (!match.Success)
        {
            regex = new Regex ("set_stat_field\\('req_id',\\s*'(-?\\d+)'\\)");
            match = regex.Match (content);
        }

        return match.Success
            ? match.Groups[1].Value
            : default;
    }

    /// <summary>
    /// Получение результата для указанного запроса.
    /// </summary>
    public string? GetResult
        (
            string? underscore,
            string? requestId
        )
    {
        if (string.IsNullOrEmpty (underscore)
            || string.IsNullOrEmpty (requestId))
        {
            return default;
        }

        var request = CreateRequest ("index.php")
            .AddParameter ("_", underscore)
            .AddParameter ("task", "show_results")
            .AddParameter ("req_id", requestId);

        var response = _restClient.Execute (request);

        return response.Content;
    }

    /// <summary>
    /// Нахождение записей в результате поиска.
    /// </summary>
    public IrbisCorpRecord[] FindRecords
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return Array.Empty<IrbisCorpRecord>();
        }

        var result = new List<IrbisCorpRecord>();
        var html = new HtmlDocument();
        html.LoadHtml (text);
        var nodes = html.DocumentNode.Descendants()
            .Where (x => x.HasClass ("record"));
        var regex = new Regex ("show_record\\('(\\d+)'\\)");
        foreach (var node in nodes)
        {
            var nodeText = node.InnerHtml;
            var match = regex.Match (nodeText);
            if (match.Success)
            {
                var record = new IrbisCorpRecord
                {
                    Id = match.Groups[1].Value
                };

                var recordText = GetRecordText (record.Id);
                if (!string.IsNullOrEmpty (recordText))
                {
                    var recordHtml = new HtmlDocument();
                    recordHtml.LoadHtml (recordText);
                    if (record.Decode (recordHtml.DocumentNode))
                    {
                        result.Add (record);
                    }
                }

            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Получение текста записи по ее идентификатору.
    /// </summary>
    public string? GetRecordText
        (
            string? recordId
        )
    {
        if (string.IsNullOrEmpty (recordId))
        {
            return default;
        }

        var request = CreateRequest ("index.php")
            .AddParameter ("task", "show_record")
            .AddParameter ("rec_id", recordId);

        var response = _restClient.Execute (request);

        return response.Content;
    }

    /// <summary>
    /// Получение записи из ИРБИС-корпорации.
    /// </summary>
    public string? DownloadRecord
        (
            string? recordId
        )
    {
        if (string.IsNullOrEmpty (recordId))
        {
            return default;
        }

        var absoluteMfn = recordId;
        if (absoluteMfn.StartsWith ('-'))
        {
            absoluteMfn = absoluteMfn[1..];
        }
        var request = CreateRequest ("index.php")
            .AddParameter ("task", "download_record")
            .AddParameter ("EXP21FMT", "TEXT")
            .AddParameter ("rec_id", recordId)
            .AddParameter ("FICTION_C21COM", "4")
            .AddParameter ("EXP21MFN", absoluteMfn);

        var response = _restClient.Execute (request);
        var result = CleanRecord (response.Content);

        return result;
    }

    #endregion

}
