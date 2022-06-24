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
    General = 0,
    CssParsing = 1,
    HtmlParsing = 2,
    Image = 3,
    Paint = 4,
    Layout = 5,
    KeyboardMouse = 6,
    Iframe = 7,
    ContextMenu = 8,
}
