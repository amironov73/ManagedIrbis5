// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* ViewModel.cs -- модель данных главного окна
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AM.Skia;

using Avalonia.Controls;
using Avalonia.Platform.Storage;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SkiaSharp;

#endregion

namespace SimilarImageFinder;

/// <summary>
/// Модель данных главного окна.
/// </summary>
public sealed class ViewModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Окно, чтобы было на что ссылаться.
    /// </summary>
    public Window? Window { get; set; }

    /// <summary>
    /// Пороговое значение сходства.
    /// </summary>
    [Reactive]
    public double Threshold { get; set; } = 0.5;

    /// <summary>
    /// Директория для сканирования.
    /// </summary>
    [Reactive]
    public Uri? Directory { get; set; }

    /// <summary>
    /// Все изображения в папке.
    /// </summary>
    [Reactive]
    public ReducedImage[]? AllImages { get; set; }

    /// <summary>
    /// Найденные схожие изображения.
    /// </summary>
    [Reactive]
    public SimilarImages[]? FoundSimilarities { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Открытие папки с файлами.
    /// </summary>
    public async Task OpenDirectory()
    {
        var options = new FolderPickerOpenOptions
        {
            Title = "Выбор папки с изображениями",
            AllowMultiple = false
        };
        var folder = (await Window!.StorageProvider.OpenFolderPickerAsync (options))
            .FirstOrDefault();
        if (folder is null)
        {
            return;
        }

        Directory = folder.Path;
        var images = new List<ReducedImage>();
        await foreach (var item in folder.GetItemsAsync())
        {
            if (item is not IStorageFile file)
            {
                continue;
            }

            var name = file.Name;
            var ext = Path.GetExtension (name).ToLowerInvariant();
            if (ext is not ".jpg")
            {
                continue;
            }

            await using var stream = await file.OpenReadAsync();
            using var bitmap = SKBitmap.Decode (stream);
            using var image = SKImage.FromBitmap (bitmap);
            var reduced = new ReducedImage
            {
                FullPath = new Uri (file.Path, file.Name),
                Reduced = ImageUtility.ReduceImage (image)
            };
            images.Add (reduced);

            // reduced.DumpTo ("reduced.txt");
        }

        AllImages = images.ToArray();

    }

    #endregion
}
