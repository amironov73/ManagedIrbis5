// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlRenderErrorType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Drawing.HtmlRenderer.Core.Entities;

/// <summary>
/// Enum of possible error types that can be reported.
/// </summary>
public enum HtmlRenderErrorType
{
    /// <summary>
    ///
    /// </summary>
    General = 0,

    /// <summary>
    ///
    /// </summary>
    CssParsing = 1,

    /// <summary>
    ///
    /// </summary>
    HtmlParsing = 2,

    /// <summary>
    ///
    /// </summary>
    Image = 3,

    /// <summary>
    ///
    /// </summary>
    Paint = 4,

    /// <summary>
    ///
    /// </summary>
    Layout = 5,

    /// <summary>
    ///
    /// </summary>
    KeyboardMouse = 6,

    /// <summary>
    ///
    /// </summary>
    Iframe = 7,

    /// <summary>
    ///
    /// </summary>
    ContextMenu = 8,
}
