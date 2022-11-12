// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlStylesheetLoadEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Entities;

/// <summary>
/// Invoked when a stylesheet is about to be loaded by file path or URL in 'link' element.<br/>
/// Allows to overwrite the loaded stylesheet by providing the stylesheet data manually, or different source (file or URL) to load from.<br/>
/// Example: The stylesheet 'href' can be non-valid URI string that is interpreted in the overwrite delegate by custom logic to pre-loaded stylesheet object<br/>
/// If no alternative data is provided the original source will be used.<br/>
/// </summary>
public sealed class HtmlStylesheetLoadEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// the source of the stylesheet as found in the HTML (file path or URL)
    /// </summary>
    public string Src { get; }

    /// <summary>
    /// collection of all the attributes that are defined on the link element
    /// </summary>
    public Dictionary<string, string> Attributes { get; }

    /// <summary>
    /// provide the new source (file path or URL) to load stylesheet from
    /// </summary>
    public string? SetSrc { get; set; }

    /// <summary>
    /// provide the stylesheet to load
    /// </summary>
    public string? SetStyleSheet { get; set; }

    /// <summary>
    /// provide the stylesheet data to load
    /// </summary>
    public CssData? SetStyleSheetData { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="src">the source of the image (file path or URL)</param>
    /// <param name="attributes">collection of all the attributes that are defined on the image element</param>
    internal HtmlStylesheetLoadEventArgs
        (
            string src,
            Dictionary<string, string> attributes
        )
    {
        Src = src;
        Attributes = attributes;
    }

    #endregion
}
