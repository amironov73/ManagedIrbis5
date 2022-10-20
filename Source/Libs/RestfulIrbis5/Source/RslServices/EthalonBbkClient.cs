// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* EthalonBbkClient.cs -- клиент онлайн-эталона ББК от РГБ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using RestSharp;

#endregion

namespace RestfulIrbis.RslServices;

/// <summary>
/// Клиент онлайн-эталона ББК от РГБ.
/// </summary>
public sealed class EthalonBbkClient
{
    #region Public methods

    /// <summary>
    /// Поиск с выдачей сырого HTML
    /// </summary>
    public string GetRawHtml
        (
            string value,
            string field = "CAPTION",
            string compare = "LIKE",
            string baseUri = "http://bbk.rsl.ru/external/search"
        )
    {
        Sure.NotNullNorEmpty (value);
        Sure.NotNullNorEmpty (field);
        Sure.NotNullNorEmpty (compare);

        var uri = new Uri (baseUri);
        var origin = uri.Scheme + "://" + uri.Host;

        var restClient = new RestClient (baseUri)
            {
                Timeout = -1
            };
        var request = new RestRequest (Method.POST);
        request.AddHeader ("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        request.AddHeader ("Accept-Language", "ru,en-US;q=0.9,en;q=0.8,ja;q=0.7");
        request.AddHeader ("Cache-Control", "max-age=0");
        request.AddHeader ("Content-Type", "application/x-www-form-urlencoded");
        request.AddHeader ("Origin", origin);
        request.AddHeader ("Referer", baseUri);
        restClient.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
        request.AddParameter ("find", "Искать");
        request.AddParameter ("fields[0].searchField", field);
        request.AddParameter ("fields[0].compare", compare);
        request.AddParameter ("fields[0].value", value);
        request.AddParameter ("fields[0].caseSens", "false");
        request.AddParameter ("fields[0].logicLink", "AND");
        request.AddParameter ("fields[0].deactivate", "false");
        request.AddParameter ("checkForDelete", "");

        var response = restClient.Execute (request);

        return response.Content;
    }

    #endregion
}
