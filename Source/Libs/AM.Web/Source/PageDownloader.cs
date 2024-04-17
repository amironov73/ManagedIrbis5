// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* PageDownloader.cs -- умеет скачивать веб-страницу вместе с ее потрохами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using AM.IO;

using JetBrains.Annotations;

using HtmlAgilityPack;

#endregion

namespace AM.Web;

/// <summary>
/// Умеет скачивать веб-страницу вместе с ее потрохами.
/// </summary>
[PublicAPI]
public sealed class PageDownloader
{
    #region Properties

    /// <summary>
    /// Используемый HttpClient.
    /// </summary>
    public HttpClient Client { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PageDownloader()
        : this (new HttpClient())
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="factory">Фабрика, используемая для создания клиента.</param>
    public PageDownloader
        (
            IHttpClientFactory factory
        )
        : this (factory.CreateClient())
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="factory">Фабрика, используемая для создания клиента.</param>
    /// <param name="name">Имя клиента.</param>
    public PageDownloader
        (
            IHttpClientFactory factory,
            string name
        )
        : this (factory.CreateClient (name))
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="client">Клиент для использования при скачивании.</param>
    public PageDownloader
        (
            HttpClient client
        )
    {
        Sure.NotNull (client);

        Client = client;
    }

    #endregion

    #region Private members

    private string GetLocalFileName
        (
            Uri uri
        )
    {
        var fileName = Path.GetFileName (uri.AbsolutePath);
        fileName = string.IsNullOrEmpty (fileName)
            // либо сочиняем уникальное имя файла
            ? Guid.NewGuid().ToString ("N")
            // либо удаляем символы в имени файла, которые нам не нравятся
            : PathUtility.EnsureGoodName (fileName);

        return fileName;
    }

    /// <summary>
    /// Скачивание изображений.
    /// </summary>
    private async Task DownloadImagesAsync
        (
            HtmlNode rootNode,
            Uri baseUri
        )
    {
        const string imagePath = "images";

        var images = rootNode.Descendants ("img").ToArray();
        if (images.Length == 0)
        {
            return;
        }

        Directory.CreateDirectory (imagePath);
        foreach (var image in images)
        {
            var src = image.GetAttributeValue ("src", null);
            if (string.IsNullOrEmpty (src))
            {
                continue;
            }

            var uri = WebUtility.MergeUri (baseUri, src);
            var bytes = await Client.GetByteArrayAsync (uri);
            var fileName = GetLocalFileName (uri);
            var filePath = Path.Combine (imagePath, fileName);
            image.SetAttributeValue ("src", filePath);
            await File.WriteAllBytesAsync (filePath, bytes);
        }
    }

    /// <summary>
    /// Скачивание скриптов.
    /// </summary>
    private async Task DownloadScriptsAsync
        (
            HtmlNode rootNode,
            Uri baseUri
        )
    {
        const string scriptPath = "scripts";

        var scripts = rootNode.Descendants ("script")
            .Where (link => !string.IsNullOrEmpty (link.GetAttributeValue ("src", null)))
            .ToArray();
        if (scripts.Length == 0)
        {
            return;
        }

        Directory.CreateDirectory (scriptPath);
        foreach (var script in scripts)
        {
            var src = script.GetAttributeValue ("src", null);
            if (string.IsNullOrEmpty (src))
            {
                continue;
            }

            var uri = WebUtility.MergeUri (baseUri, src);
            var bytes = await Client.GetByteArrayAsync (uri);
            var fileName = GetLocalFileName (uri);
            var filePath = Path.Combine (scriptPath, fileName);
            script.SetAttributeValue ("src", filePath);
            await File.WriteAllBytesAsync (filePath, bytes);
        }
    }

    /// <summary>
    /// Скачивание стилей.
    /// </summary>
    private async Task DownloadStylesAsync
        (
            HtmlNode rootNode,
            Uri baseUri
        )
    {
        const string stylesPath = "styles";

        var styles = rootNode.Descendants ("link")
            .Where (link => link.GetAttributeValue ("rel", null) == "stylesheet")
            .ToArray();
        if (styles.Length == 0)
        {
            return;
        }

        Directory.CreateDirectory (stylesPath);
        foreach (var style in styles)
        {
            var href = style.GetAttributeValue ("href", null);
            if (string.IsNullOrEmpty (href))
            {
                continue;
            }

            var uri = WebUtility.MergeUri (baseUri, href);
            var bytes = await Client.GetByteArrayAsync (uri);
            var fileName = GetLocalFileName (uri);
            var filePath = Path.Combine (stylesPath, fileName);
            style.SetAttributeValue ("href", filePath);
            await File.WriteAllBytesAsync (filePath, bytes);
        }
    }

    private async Task SaveContentAsync
        (
            HttpResponseMessage response,
            string fileName
        )
    {
        var stream = await response.Content.ReadAsStreamAsync();
        await stream.CopyToAsync (File.OpenWrite (fileName));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Скачивание веб-страницы.
    /// Имя файла для сохранения выбирается автоматически.
    /// </summary>
    /// <param name="webUrl">URL страницы.</param>
    public Task DownloadAsync
        (
            Uri webUrl
        )
    {
        Sure.NotNull (webUrl);

        var fileName = Path.GetFileName (webUrl.LocalPath);
        return DownloadAsync (webUrl, fileName);
    }

    /// <summary>
    /// Скачивание веб-страницы.
    /// </summary>
    /// <param name="webUrl">URL страницы.</param>
    /// <param name="fileName">Имя файла для сохранения.</param>
    public async Task DownloadAsync
        (
            Uri webUrl,
            string fileName
        )
    {
        Sure.NotNull (webUrl);
        Sure.NotNullNorEmpty (fileName);

        var response = await Client.GetAsync (webUrl);
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        if (!response.Headers.TryGetValues ("Content-Type", out var contentType)
            || contentType.FirstOrDefault() != "text/html")
        {
            await SaveContentAsync (response, fileName);
            return;
        }

        var pageContent = await response.Content.ReadAsStringAsync();
        // await File.WriteAllTextAsync (fileName, pageContent);

        var document = new HtmlDocument();
        document.LoadHtml (pageContent);
        var rootNode = document.DocumentNode;

        // обрабатываем изображения
        await DownloadImagesAsync (rootNode, webUrl);

        // обрабатываем стили
        await DownloadStylesAsync (rootNode, webUrl);

        // обрабатываем скрипты
        await DownloadScriptsAsync (rootNode, webUrl);

        // сохраняем документ с учетом всех изменений
        document.Save (fileName);
    }

    #endregion
}
