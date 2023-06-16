// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* CivitDownloader.cs -- загрузчик картинок с CivitAI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.StableDiffusion.CivitAI;

#endregion

#nullable enable

namespace CivitGet;

/// <summary>
/// Умеет загружать файлы с CivitAI.
/// </summary>
public sealed class CivitDownloader
{
    #region Properties

    /// <summary>
    /// Директория, в которую надо складывать
    /// скачанные изображения.
    /// </summary>
    public string DestinationDirectory { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CivitDownloader()
    {
        DestinationDirectory = Path.Combine
            (
                Directory.GetCurrentDirectory(),
                "images"
            );
        Directory.CreateDirectory (DestinationDirectory);
        _client = new CivitClient();
        _progressReporter = new ();
    }

    #endregion

    #region Private members

    private readonly CivitClient _client;
    private readonly ConsoleProgressReporter _progressReporter;
    private readonly TimeSpan _delay = TimeSpan.FromMilliseconds (500);

    #endregion

    #region Public methods

    /// <summary>
    /// Загрузка по URLю
    /// </summary>
    public void DownloadFromUrl
        (
            string url
        )
    {
        Sure.NotNullNorEmpty (url);

        var uri = new Uri (url);
        var host = uri.Host;
        if (!host.SameString ("civitai.com"))
        {
            throw new Exception();
        }

        var segments = uri.Segments;
        if (segments.Length < 3)
        {
            throw new Exception();
        }

        // foreach (var segment in segments)
        // {
        //     Console.WriteLine (segment);
        // }

        switch (segments[1])
        {
            case "user/":
                DownloadImages (username: segments[2].TrimEnd ('/'));
                break;

            case "models/":
                DownloadImages (modelId: int.Parse (segments[2].TrimEnd ('/')));
                break;

            case "posts/":
                DownloadImages (postId: int.Parse (segments[2].TrimEnd ('/')));
                break;

            default:
                throw new Exception();
        }
    }

    /// <summary>
    /// Загрузка указанных изображений.
    /// </summary>
    public void DownloadImages
        (
            int postId = default,
            int modelId = default,
            string? notSafe = default,
            string? username = default
        )
    {
        var images = _client.EnumerateImages
            (
                postId,
                modelId,
                notSafe,
                username,
                _progressReporter
            );
        foreach (var image in images)
        {
            Console.WriteLine (image.Id);
            _client.SaveImage (image, DestinationDirectory, _progressReporter);
            Thread.Sleep (_delay);
        }

    }

    #endregion
}
