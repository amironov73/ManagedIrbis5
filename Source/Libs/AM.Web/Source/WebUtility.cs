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

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// Полезные методы для работы с Webю
/// </summary>
[PublicAPI]
public static class WebUtility
{
    #region Public methods

    /// <summary>
    /// Получение статус-кода для указанного адреса.
    /// </summary>
    public static int GetHttpStatusCode
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

    /// <summary>
    /// Соединение пути к файлу относительно заданного URI.
    /// </summary>
    /// <param name="baseUri">Базовый URI.</param>
    /// <param name="relativePath">Относительный путь к файлу.</param>
    /// <returns>Путь к файлу.</returns>
    public static Uri MergeUri
        (
            Uri baseUri,
            string relativePath
        )
    {
        Sure.NotNull (baseUri);
        Sure.NotNullNorEmpty (relativePath);

        // если относительный путь начинается с "http://" или "https://"
        // значит, это не относительный, а полный путь
        if (relativePath.StartsWith ("http://", StringComparison.OrdinalIgnoreCase)
            || relativePath.StartsWith ("https://", StringComparison.OrdinalIgnoreCase))
        {
            // просто возвращаем полный путь
            return new Uri (relativePath);
        }

        var uri = new Uri (baseUri, relativePath);

        return uri;
    }

    /*

    /// <summary>
    /// Forces your browser to download any kind of file instead
    /// of trying to open it inside the browser (e.g. pictures, pdf, mp3).
    /// Works in Chrome, Opera, Firefox and IE 7 &amp; 8!
    /// </summary>
    public static void ForceDownload
        (
            this HttpResponse response,
            string fullPathToFile,
            string outputFileName
        )
    {
        response.Clear();
        response.AddHeader ("content-disposition", "attachment; filename=" + outputFileName);
        response.WriteFile (fullPathToFile);
        response.ContentType = "";
        response.End();
    }

    */

    #endregion
}
