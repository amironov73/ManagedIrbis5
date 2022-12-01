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
using System.Text.Json;

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
        _memoryCache = new KeyedMemoryCache (options);

        _handler = new HttpClientHandler();
    }

    #endregion

    #region Private members

    private readonly KeyedMemoryCache _memoryCache;
    private readonly HttpClientHandler _handler;

    private static byte[]? BitmapToBytes
        (
            Bitmap bitmap
        )
    {
        try
        {
            var stream = new MemoryStream();
            bitmap.Save (stream);
            return stream.ToArray();
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        return null;
    }

    private static Bitmap? BytesToBitmap (byte[] bytes)
    {
        if (!bytes.IsNullOrEmpty())
        {
            try
            {
                var stream = new MemoryStream (bytes);
                return new Bitmap (stream);
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
            }
        }

        return null;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Очиска кеша.
    /// </summary>
    public void Clear()
    {
        _memoryCache.Clear();
    }

    /// <summary>
    /// Получение картинки по URL.
    /// </summary>
    public Bitmap? GetThumbnail
        (
            string url
        )
    {
        if (string.IsNullOrEmpty (url))
        {
            return null;
        }

        if (_memoryCache.TryGetValue (url, out Bitmap? result))
        {
            return result;
        }

        try
        {
            var client = new HttpClient (_handler);
            var bytes = client.GetByteArrayAsync (url).Result;
            result = BytesToBitmap (bytes);
            if (result is not null)
            {
                _memoryCache.Set (url, result);
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        return result;
    }

    /// <summary>
    /// Загрузка ранее сохраненных картинок.
    /// </summary>
    public void LoadThumbnails()
    {
        // C:\Users\amiro\AppData\Local
        var appData = Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData);
        var folder = Path.Combine (appData, Constants.ApplicationName);
        Directory.CreateDirectory (folder);

        var fileName = Path.Combine (folder, Constants.ThumbnailsFileName);
        if (!File.Exists (fileName))
        {
            return;
        }

        try
        {
            var text = File.ReadAllBytes (fileName);
            var reader = new Utf8JsonReader (text);
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var url = reader.GetString();
                    if (!reader.Read())
                    {
                        break;
                    }

                    var byteString = reader.GetString();
                    if (!string.IsNullOrEmpty (url)
                        && !string.IsNullOrEmpty (byteString))
                    {
                        var bytes = Convert.FromBase64String (byteString);
                        var bitmap = BytesToBitmap (bytes);
                        if (bitmap is not null)
                        {
                            _memoryCache.Set (url, bitmap);
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }
    }

    /// <summary>
    /// Сохренение картинок на будущее.
    /// </summary>
    public void SaveThumbnails()
    {
        // C:\Users\amiro\AppData\Local
        var appData = Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData);
        var folder = Path.Combine (appData, Constants.ApplicationName);
        Directory.CreateDirectory (folder);

        var fileName = Path.Combine (folder, Constants.ThumbnailsFileName);
        File.Delete (fileName);

        var options = new JsonWriterOptions { Indented = true };
        using var stream = File.Create (fileName);
        using var writer = new Utf8JsonWriter (stream, options);
        writer.WriteStartObject();

        var keys = _memoryCache.GetKeys();
        foreach (var key in keys)
        {
            var bitmap = _memoryCache.Get<Bitmap> (key);
            if (bitmap is not null)
            {
                var bytes = BitmapToBytes (bitmap);
                if (!bytes.IsNullOrEmpty())
                {
                    writer.WriteString
                        (
                            key,
                            Convert.ToBase64String (bytes)
                        );
                }
            }
        }
        writer.WriteEndObject();
    }

    #endregion
}
