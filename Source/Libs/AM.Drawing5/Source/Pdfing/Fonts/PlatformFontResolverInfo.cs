// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PlatformFontResolverInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Fonts;

/// <summary>
/// Represents a font resolver info created by the platform font resolver.
/// </summary>
internal sealed class PlatformFontResolverInfo
    : FontResolverInfo
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="faceName"></param>
    /// <param name="mustSimulateBold"></param>
    /// <param name="mustSimulateItalic"></param>
    public PlatformFontResolverInfo
        (
            string faceName,
            bool mustSimulateBold,
            bool mustSimulateItalic
        )
        : base (faceName, mustSimulateBold, mustSimulateItalic)
    {
        //_gdiFont = gdiFont;
    }

    #endregion
}
