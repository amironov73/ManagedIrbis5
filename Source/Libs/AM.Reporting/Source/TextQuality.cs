// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* TextQuality.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting;

/// <summary>
/// Specifies the quality of text rendering.
/// </summary>
public enum TextQuality
{
    /// <summary>
    /// The default text quality, depends on system settings.
    /// </summary>
    Default,

    /// <summary>
    /// The regular quality.
    /// </summary>
    Regular,

    /// <summary>
    /// The "ClearType" quality.
    /// </summary>
    ClearType,

    /// <summary>
    /// The AntiAlias quality. This mode may be used to produce the WYSIWYG text.
    /// </summary>
    AntiAlias,

    /// <summary>
    /// The "SingleBitPerPixel" quality.
    /// </summary>
    SingleBPP,


    /// <summary>
    /// The "SingleBitPerPixelGridFit" quality.
    /// </summary>
    SingleBPPGridFit
}
