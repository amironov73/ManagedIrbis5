// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlRefreshEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Entities;

/// <summary>
/// Raised when html renderer requires refresh of the control hosting (invalidation and re-layout).<br/>
/// It can happen if some async event has occurred that requires re-paint and re-layout of the html.<br/>
/// Example: async download of image is complete.
/// </summary>
public sealed class HtmlRefreshEventArgs 
    : EventArgs
{
    /// <summary>
    /// is re-layout is required for the refresh
    /// </summary>
    private readonly bool _layout;

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="layout">is re-layout is required for the refresh</param>
    public HtmlRefreshEventArgs(bool layout)
    {
        _layout = layout;
    }

    /// <summary>
    /// is re-layout is required for the refresh
    /// </summary>
    public bool Layout
    {
        get { return _layout; }
    }

    public override string ToString()
    {
        return string.Format("Layout: {0}", _layout);
    }
}
