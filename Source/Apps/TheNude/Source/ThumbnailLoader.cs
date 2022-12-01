// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* ThumbnailLoader.cs -- загрузчик картинок, совмещенный с кешем
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

using AM.Collections;

using Avalonia.Media.Imaging;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace TheNude;

/// <summary>
/// Загрузчик картинок, совмещенный с кешем.
/// </summary>
public class ThumbnailLoader
{
    #region Properties

    /// <summary>
    /// Общий экземпляр.
    /// </summary>
    public static readonly ThumbnailLoader Instance = new ();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ThumbnailLoader()
    {
        var options = new MemoryCacheOptions();
        _memoryCache = new MemoryCache (options);

        _handler = new HttpClientHandler();
    }

    #endregion

    #region Private members

    private readonly IMemoryCache _memoryCache;
    private readonly HttpClientHandler _handler;

    #endregion

    #region Public methods

    public Bitmap? GetThumbnail (string url)
    {
        if (_memoryCache.TryGetValue (url, out Bitmap? result))
        {
            return result;
        }

        try
        {
            var client = new HttpClient (_handler);
            var bytes = client.GetByteArrayAsync (url).Result;
            if (!bytes.IsNullOrEmpty())
            {
                var stream = new MemoryStream (bytes);
                result = new Bitmap (stream);
                _memoryCache.Set (url, result);
            }

        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        return result;
    }

    #endregion
}
