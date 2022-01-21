// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* WebUtility.cs -- полезные методы для работы с Web
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Net.Http;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// Полезные методы для работы с Webю
/// </summary>
public static class WebUtility
{
    #region Public methods

    /// <summary>
    /// Получение статус-кода для указанного адреса.
    /// </summary>
    static int GetHttpStatusCode
        (
            string address
        )
    {
        Sure.NotNullNorEmpty (address);

        var httpClient = new HttpClient();
        var request = new HttpRequestMessage
            (
                HttpMethod.Head,
                new Uri (address)
            );

        var response = httpClient.Send (request);

        return (int) response.StatusCode;
    }


    #endregion
}
