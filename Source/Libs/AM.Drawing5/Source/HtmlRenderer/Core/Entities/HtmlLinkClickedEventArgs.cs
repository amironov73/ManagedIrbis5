// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlLinkClickedEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Entities;

/// <summary>
/// Raised when the user clicks on a link in the html.
/// </summary>
public sealed class HtmlLinkClickedEventArgs
    : EventArgs
{
    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="link">the link href that was clicked</param>
    /// <param name="attributes"></param>
    public HtmlLinkClickedEventArgs
        (
            string link,
            Dictionary<string, string> attributes
        )
    {
        Link = link;
        Attributes = attributes;
    }

    /// <summary>
    /// the link href that was clicked
    /// </summary>
    public string Link { get; }

    /// <summary>
    /// collection of all the attributes that are defined on the link element
    /// </summary>
    public Dictionary<string, string> Attributes { get; }

    /// <summary>
    /// use to cancel the execution of the link
    /// </summary>
    public bool Handled { get; set; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"Link: {Link}, Handled: {Handled}";
    }
}
