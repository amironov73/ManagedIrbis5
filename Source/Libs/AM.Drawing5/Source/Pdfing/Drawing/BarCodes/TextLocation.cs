// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TextLocation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Specifies whether and how the text is displayed at the code.
/// </summary>
public enum TextLocation
{
    /// <summary>
    /// No text is drawn.
    /// </summary>
    None,

    /// <summary>
    /// The text is located above the code.
    /// </summary>
    Above,

    /// <summary>
    /// The text is located below the code.
    /// </summary>
    Below,


    /// <summary>
    /// The text is located above within the code.
    /// </summary>
    AboveEmbedded,


    /// <summary>
    /// The text is located below within the code.
    /// </summary>
    BelowEmbedded
}
