// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StrikeThroughStyle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.RichTextKit;

/// <summary>
/// Describes the strike-through style for a run of text
/// </summary>
public enum StrikeThroughStyle
{
    /// <summary>
    /// No strike through.
    /// </summary>
    None,

    /// <summary>
    /// Standard solid line strike through.
    /// </summary>
    Solid,
}
