// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UnderlineStyle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.RichTextKit;

/// <summary>
/// Describes the underline style for a run of text
/// </summary>
public enum UnderlineStyle
{
    /// <summary>
    /// No underline.
    /// </summary>
    None,

    /// <summary>
    /// Underline with gaps over descenders.
    /// </summary>
    Gapped,

    /// <summary>
    /// Underline with no gaps over descenders.
    /// </summary>
    Solid,

    /// <summary>
    /// Underline style for IME input
    /// </summary>
    ImeInput,

    /// <summary>
    /// Underline style for converted IME input
    /// </summary>
    ImeConverted,

    /// <summary>
    /// Underline style for converted IME input (target clause)
    /// </summary>
    ImeTargetConverted,

    /// <summary>
    /// Underline style for unconverted IME input (target clause)
    /// </summary>
    ImeTargetNonConverted
}
