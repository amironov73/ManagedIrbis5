// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FontVariant.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.RichTextKit;

/// <summary>
/// Describes variations to a base font for a run of text.
/// </summary>
public enum FontVariant
{
    /// <summary>
    /// Normal text.
    /// </summary>
    Normal,

    /// <summary>
    /// Super-script Text
    /// </summary>
    SuperScript,

    /// <summary>
    /// Sub-script Text
    /// </summary>
    SubScript
}
