// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DiskCachedWebImageLoader.cs -- загрузчик, кеширующий картинки на диске.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Media.Imaging;

#endregion

#nullable enable

namespace AM.Avalonia.ImageLoading;

/// <summary>
/// Provides memory and disk cached way to asynchronously load images for <see cref="ImageLoader"/>
/// Can be used as base class if you want to create custom caching mechanism
/// </summary>
public class DiskCachedWebImageLoader
    : RamCachedWebImageLoader
{
    private readonly string _cacheFolder;

    public DiskCachedWebImageLoader (string cacheFolder = "Cache/Images/")
    {
        _cacheFolder = cacheFolder;
    }

    public DiskCachedWebImageLoader (HttpClient httpClient, bool disposeHttpClient,
        string cacheFolder = "Cache/Images/")
        : base (httpClient, disposeHttpClient)
    {
        _cacheFolder = cacheFolder;
    }

    /// <inheritdoc />
    protected override Task<Bitmap?> LoadFromGlobalCache (string url)
    {
        var path = Path.Combine (_cacheFolder, CreateMD5 (url));
        if (File.Exists (path))
        {
            return Task.FromResult (new Bitmap (path))!;
        }

        return Task.FromResult<Bitmap?> (null);
    }

    protected override Task SaveToGlobalCache (string url, byte[] imageBytes)
    {
        var path = Path.Combine (_cacheFolder, CreateMD5 (url));
        Directory.CreateDirectory (_cacheFolder);
        return File.WriteAllBytesAsync (path, imageBytes);
    }

    protected static string CreateMD5 (string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes (input);
            byte[] hashBytes = md5.ComputeHash (inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append (hashBytes[i].ToString ("X2"));
            }

            return sb.ToString();
        }
    }
}
