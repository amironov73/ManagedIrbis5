// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* WebFile.cs -- скачивание файлов при необходимости
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

namespace AM.Web;

/// <summary>
/// Скачивание файлов с веб при необходимости.
/// </summary>
[PublicAPI]
public sealed class WebFile
{
    #region Properties

    /// <summary>
    /// Клиент.
    /// </summary>
    public HttpClient Client { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public WebFile()
        : this (new HttpClient())
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WebFile
        (
            HttpClient client
        )
    {
        Sure.NotNull (client);

        Client = client;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Скачивание файла, если его еще нет локально.
    /// </summary>
    /// <param name="url">Веб-адрес, с которого предполагается скачать файл.
    /// </param>
    /// <param name="localPath">Локальный путь к файлу.</param>
    public async Task DownloadAsync
        (
            string url,
            string localPath
        )
    {
        Sure.NotNullNorEmpty (url);
        Sure.NotNullNorEmpty (localPath);

        if (File.Exists (localPath))
        {
            return;
        }

        var temporaryFile = localPath + ".download";
        File.Delete (temporaryFile);
        {
            await using var webStream = await Client.GetStreamAsync (url);
            await using var fileStream = File.Create (temporaryFile);
            await webStream.CopyToAsync (fileStream);
        }

        File.Move (temporaryFile, localPath);
    }

    /// <summary>
    /// Открытие файла для чтения.
    /// </summary>
    public async Task <Stream> OpenReadAsync
        (
            string url,
            string localPath
        )
    {
        Sure.NotNullNorEmpty (url);
        Sure.NotNullNorEmpty (localPath);

        await DownloadAsync (url, localPath);

        return File.OpenRead (localPath);
    }

    /// <summary>
    /// Открытие файла для чтения в текстовом режиме.
    /// </summary>
    public async Task <TextReader> OpenTextAsync
        (
            string url,
            string localPath
        )
    {
        Sure.NotNullNorEmpty (url);
        Sure.NotNullNorEmpty (localPath);

        await DownloadAsync (url, localPath);

        return File.OpenText (localPath);
    }

    #endregion
}
