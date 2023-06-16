// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* HttpClientWithProgress.cs -- клиент с отслеживанием прогресса скачивания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Net;

/// <summary>
/// Клиент с отслеживанием прогресса скачивания.
/// </summary>
[PublicAPI]
public class HttpClientWithProgress
    : HttpClient
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public HttpClientWithProgress()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Констуктор.
    /// </summary>
    public HttpClientWithProgress
        (
            HttpMessageHandler handler
        )
        : base (handler)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HttpClientWithProgress
        (
            HttpMessageHandler handler,
            bool disposeHandler
        )
        : base (handler, disposeHandler)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Скачивание ресурса с отслеживанием прогресса.
    /// </summary>
    public HttpHeaders? Download
        (
            Uri requestUri,
            Stream destination,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default,
            int bufferSize = 4 * 1024
        )
    {
        try
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            using var response = GetAsync
                    (
                        requestUri,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken
                    )
                .GetAwaiter().GetResult();
            var stat = new DownloadStat
            {
                RequestUri = requestUri,
                ContentLength = response.Content.Headers.ContentLength
            };
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            progress?.OnDownloadBegin (stat);
            using var download = response.Content
                .ReadAsStream (cancellationToken);
            var buffer = new byte[bufferSize];
            try
            {
                int bytesRead;
                while ((bytesRead = download.Read (buffer)) != 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    stat.Downloaded += bytesRead;
                    destination.Write (buffer, 0, bytesRead);
                    progress?.Report (stat);
                }
            }
            catch (Exception exception)
            {
                progress?.OnDownloadError (stat, exception);
            }

            progress?.OnDownloadEnd (stat);

            return response.Headers;
        }
        catch (Exception exception)
        {
            progress?.OnDownloadError
                (
                    new DownloadStat
                    {
                        RequestUri = requestUri
                    },
                    exception
                );
        }

        return null;
    }

    /// <summary>
    /// Скачивание ресурса с отслеживанием прогресса.
    /// </summary>
    public async Task<HttpHeaders?> DownloadAsync
        (
            Uri requestUri,
            Stream destination,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default,
            int bufferSize = 4 * 1024
        )
    {
        try
        {
            using var response = await GetAsync
                (
                    requestUri,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken
                );
            var stat = new DownloadStat
            {
                RequestUri = requestUri,
                ContentLength = response.Content.Headers.ContentLength
            };
            progress?.OnDownloadBegin (stat);
            await using var download = await response.Content
                .ReadAsStreamAsync (cancellationToken);
            var buffer = new byte[bufferSize];
            try
            {
                int bytesRead;
                while ((bytesRead = await download.ReadAsync (buffer, cancellationToken)) != 0)
                {
                    stat.Downloaded += bytesRead;
                    await destination.WriteAsync
                        (
                            buffer.AsMemory (start: 0, length: bytesRead),
                            cancellationToken
                        );
                    progress?.Report (stat);
                }
            }
            catch (Exception exception)
            {
                progress?.OnDownloadError (stat, exception);
            }

            progress?.OnDownloadEnd (stat);

            return response.Headers;
        }
        catch (Exception exception)
        {
            progress?.OnDownloadError
                (
                    new DownloadStat
                    {
                        RequestUri = requestUri
                    },
                    exception
                );
        }

        return null;
    }

    /// <summary>
    /// Асинхронная загрузка файла.
    /// </summary>
    public HttpHeaders? DownloadFile
        (
            Uri requestUri,
            string fileName,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default,
            int bufferSize = 64 * 1024
        )
    {
        Sure.NotNull (requestUri);
        Sure.NotNullNorEmpty (fileName);

        using var destination = File.Create (fileName);
        return Download
            (
                requestUri,
                destination,
                progress,
                cancellationToken,
                bufferSize
            );
    }

    /// <summary>
    /// Асинхронная загрузка файла.
    /// </summary>
    public async Task DownloadFileAsync
        (
            Uri requestUri,
            string fileName,
            IDownloadProgress? progress = default,
            CancellationToken cancellationToken = default,
            int bufferSize = 64 * 1024
        )
    {
        Sure.NotNull (requestUri);
        Sure.NotNullNorEmpty (fileName);

        await using var destination = File.Create (fileName);
        await DownloadAsync
            (
                requestUri,
                destination,
                progress,
                cancellationToken,
                bufferSize
            );
    }

    #endregion
}
