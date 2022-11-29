// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* LeisureImage.cs -- картинка, неспешно загружающаяся в фоновом режиме
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks.Dataflow;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Картинка, неспешно загружающаяся в фоновом режиме.
/// </summary>
public sealed class LeisurelyImage
    : Image, IStyleable
{
    #region Properties

    /// <summary>
    /// Описание свойства "Активность".
    /// </summary>
    public static readonly StyledProperty<string?> PathProperty
        = AvaloniaProperty.Register<LeisurelyImage, string?> (nameof (Path));

    /// <summary>
    /// Путь к картинке.
    /// </summary>
    public string? Path
    {
        get => _path;
        set
        {
            SetAndRaise (PathProperty, ref _path, value);
            // ThreadPool.QueueUserWorkItem (SlowLoading, this, true);
            _imageLoader ??= new ActionBlock<LeisurelyImage> (SlowLoading);
            _imageLoader.Post (this);
        }
    }

    #endregion

    #region Construction

    static LeisurelyImage()
    {
        AffectsRender<LeisurelyImage> (PathProperty);
    }

    #endregion

    #region Private members

    private static ActionBlock<LeisurelyImage>? _imageLoader;

    /// <summary>
    /// Путь к картинке
    /// </summary>
    private string? _path;


    private static void SlowLoading
        (
            LeisurelyImage image
        )
    {
        if (!string.IsNullOrEmpty (image._path))
        {
            try
            {
                var client = new HttpClient();
                var bytes = client.GetByteArrayAsync (image._path).Result;
                var stream = new MemoryStream (bytes);
                var bitmap = new Bitmap (stream);
                Dispatcher.UIThread.Post (() => { image.Source = bitmap; });
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
            }
        }
    }

    /// <inheritdoc cref="IStyleable.StyleKey"/>
    Type IStyleable.StyleKey => typeof (Image);

    #endregion
}
