// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IAsyncImageLoader.cs -- интерфейс асинхронного загрузчика картинок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using Avalonia.Media.Imaging;

#endregion

#nullable enable

namespace AM.Avalonia.ImageLoading;

/// <summary>
/// Интерфейс асинхронного загрузчика картинов.
/// </summary>
public interface IAsyncImageLoader
    : IDisposable
{
    /// <summary>
    /// Loads image
    /// </summary>
    /// <param name="url">Target url</param>
    /// <returns>Bitmap</returns>
    public Task<IBitmap?> ProvideImageAsync (string url);
}
