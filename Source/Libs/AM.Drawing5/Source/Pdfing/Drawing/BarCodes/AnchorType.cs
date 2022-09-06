// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* AnchorType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Specifies whether and how the text is displayed at the code area.
/// </summary>
public enum AnchorType
{
    /// <summary>
    /// The anchor is located top left.
    /// </summary>
    TopLeft,

    /// <summary>
    /// The anchor is located top center.
    /// </summary>
    TopCenter,

    /// <summary>
    /// The anchor is located top right.
    /// </summary>
    TopRight,

    /// <summary>
    /// The anchor is located middle left.
    /// </summary>
    MiddleLeft,

    /// <summary>
    /// The anchor is located middle center.
    /// </summary>
    MiddleCenter,

    /// <summary>
    /// The anchor is located middle right.
    /// </summary>
    MiddleRight,

    /// <summary>
    /// The anchor is located bottom left.
    /// </summary>
    BottomLeft,

    /// <summary>
    /// The anchor is located bottom center.
    /// </summary>
    BottomCenter,

    /// <summary>
    /// The anchor is located bottom right.
    /// </summary>
    BottomRight
}
