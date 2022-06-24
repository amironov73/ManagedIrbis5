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
    /// the link href that was clicked
    /// </summary>
    private readonly string _link;

    /// <summary>
    /// collection of all the attributes that are defined on the link element
    /// </summary>
    private readonly Dictionary<string, string> _attributes;

    /// <summary>
    /// use to cancel the execution of the link
    /// </summary>
    private bool _handled;

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="link">the link href that was clicked</param>
    public HtmlLinkClickedEventArgs(string link, Dictionary<string, string> attributes)
    {
        _link = link;
        _attributes = attributes;
    }

    /// <summary>
    /// the link href that was clicked
    /// </summary>
    public string Link
    {
        get { return _link; }
    }

    /// <summary>
    /// collection of all the attributes that are defined on the link element
    /// </summary>
    public Dictionary<string, string> Attributes
    {
        get { return _attributes; }
    }

    /// <summary>
    /// use to cancel the execution of the link
    /// </summary>
    public bool Handled
    {
        get { return _handled; }
        set { _handled = value; }
    }

    public override string ToString()
    {
        return string.Format("Link: {0}, Handled: {1}", _link, _handled);
    }
}
