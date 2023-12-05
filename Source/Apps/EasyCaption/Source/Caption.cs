// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Caption.cs -- пара "картинка плюс подпись"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;

using Avalonia.Media.Imaging;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace EasyCaption;

/// <summary>
/// Пара "картинка плюс подпись".
/// </summary>
public sealed class Caption
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Полный путь к файлу с картинкой.
    /// </summary>
    public string? ImageFile { get; set; }

    /// <summary>
    /// Имя файла с картинкой.
    /// </summary>
    public string? ShortName => string.IsNullOrEmpty (ImageFile)
        ? null
        : Path.GetFileNameWithoutExtension (ImageFile);

    public string? CaptionFile { get; set; }

    /// <summary>
    /// Подпись к картинке.
    /// </summary>
    [Reactive]
    public string? Text { get; set; }

    /// <summary>
    /// Позиция курсора в текстовом редакторе.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Уменьшенная картинка.
    /// </summary>
    [Reactive]
    public Bitmap? Thumbnail { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => ShortName.IfNull ("?");

    #endregion
}
