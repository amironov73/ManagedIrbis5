// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Folder.cs -- папка с подписываемыми картинками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Collections;

using Avalonia.Media.Imaging;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace EasyCaption;

/// <summary>
/// Папка с подписываемыми картинками.
/// </summary>
internal sealed class Folder
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Подлежащие подписи картинки.
    /// </summary>
    [Reactive]
    public Caption[]? Captions { get; set; }

    /// <summary>
    /// Текущая картинка.
    /// </summary>
    [Reactive]
    public Caption? Current { get; set; }

    /// <summary>
    /// Статистика использования меток.
    /// </summary>
    [Reactive]
    public TokenStat[]? Stat { get; set; }

    #endregion

    #region Private members

    private string[] EnumerateFiles
        (
            string path,
            string pattern
        )
    {
        var options = new EnumerationOptions
        {
            AttributesToSkip = FileAttributes.Directory
                | FileAttributes.Offline
                | FileAttributes.System,
            IgnoreInaccessible = true,
            MatchCasing = MatchCasing.CaseInsensitive,
            MatchType = MatchType.Win32,
            RecurseSubdirectories = true,
            ReturnSpecialDirectories = false
        };
        return Directory.GetFiles (path, pattern, options);
    }

    private Caption LoadCaption
        (
            string imageFile
        )
    {
        var result = new Caption
        {
            ImageFile = imageFile,
            Thumbnail = new Bitmap (imageFile),
            CaptionFile = Path.ChangeExtension (imageFile, ".txt")
        };

        if (File.Exists (result.CaptionFile))
        {
            result.Text = File.ReadAllText (result.CaptionFile);
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Подсчет статистики использования меток.
    /// </summary>
    /// <returns></returns>
    public TokenStat[] GetTagStat()
    {
        var captions = Captions;
        if (captions is null)
        {
            return Array.Empty<TokenStat>();
        }

        var dictionary = new DictionaryCounter<string, int>();
        foreach (var caption in captions)
        {
            var text = caption.Text;
            if (!string.IsNullOrEmpty (text))
            {
                var counter = new TokenCounter (text, dictionary);
                counter.CountLorasAndTokens();
            }
        }

        var result = new List<TokenStat>();
        foreach (var pair in dictionary)
        {
            var stat = new TokenStat
            {
                Tag = pair.Key,
                Count = pair.Value
            };
            result.Add (stat);
        }

        // сортировка по убыванию
        result.Sort ((left, right) => right.Count - left.Count);

        return result.ToArray();
    }

    /// <summary>
    /// Поиск картинок по указанному пути.
    /// </summary>
    public void ScanForImages
        (
            string path
        )
    {
        Current = null;

        var files = new List<string>();
        files.AddRange (EnumerateFiles (path, "*.jpg"));
        files.AddRange (EnumerateFiles (path, "*.jpeg"));
        files.AddRange (EnumerateFiles (path, "*.jfif"));
        files.AddRange (EnumerateFiles (path, "*.png"));

        var captions = files
            .Select (LoadCaption)
            .ToArray();

        Captions = captions;
    }

    /// <summary>
    /// Синхронизация текстов с файловой системой.
    /// </summary>
    public void Synchronize()
    {
        if (Captions is not null)
        {
            foreach (var caption in Captions)
            {
                caption.Synchcronize();
            }
        }
    }

    #endregion
}
