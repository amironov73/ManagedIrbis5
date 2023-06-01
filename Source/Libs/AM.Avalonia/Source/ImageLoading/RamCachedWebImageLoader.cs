// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* RamCachedWebImageLoader.cs -- загрузчик, кеширующий картинки в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;

using Avalonia.Media.Imaging;

#endregion

#nullable enable

namespace AM.Avalonia.ImageLoading;

/// <summary>
/// Provides memory cached way to asynchronously load images for <see cref="ImageLoader"/>
/// Can be used as base class if you want to create custom in memory caching
/// </summary>
public class RamCachedWebImageLoader : BaseWebImageLoader
{
    private readonly ConcurrentDictionary<string, Task<Bitmap?>> _memoryCache = new ();

    /// <inheritdoc />
    public RamCachedWebImageLoader()
    {
    }

    /// <inheritdoc />
    public RamCachedWebImageLoader (HttpClient httpClient, bool disposeHttpClient) : base (httpClient,
        disposeHttpClient)
    {
    }

    /// <inheritdoc />
    public override async Task<Bitmap?> ProvideImageAsync (string url)
    {
        var bitmap = await _memoryCache.GetOrAdd (url, LoadAsync);

        // If load failed - remove from cache and return
        // Next load attempt will try to load image again
        if (bitmap == null) _memoryCache.TryRemove (url, out _);
        return bitmap;
    }
}
