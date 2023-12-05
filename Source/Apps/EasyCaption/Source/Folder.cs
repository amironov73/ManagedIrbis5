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

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Avalonia.Media.Imaging;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace EasyCaption;

/// <summary>
/// Папка с подписываемыми картинками.
/// </summary>
public sealed class Folder
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
            .Select (file => LoadCaption (file))
            .ToArray();

        Captions = captions;
    }

    #endregion
}
