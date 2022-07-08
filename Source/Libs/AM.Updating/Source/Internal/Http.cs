// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Http.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Net;
using System.Net.Http;

#endregion

#nullable enable

namespace AM.Updating.Internal;

internal static class Http
{
    private static readonly Lazy<HttpClient> ClientLazy = new (() =>
    {
        var handler = new HttpClientHandler();

        if (handler.SupportsAutomaticDecompression)
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        handler.UseCookies = false;

        var httpClient = new HttpClient (handler, true);
        httpClient.DefaultRequestHeaders.Add ("User-Agent", "AM.Updating (github.com/amironov73/ManagedIrbis5)");

        return httpClient;
    });

    public static HttpClient Client => ClientLazy.Value;
}
