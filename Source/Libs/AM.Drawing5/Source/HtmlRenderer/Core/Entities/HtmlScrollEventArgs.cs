// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlScrollEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Drawing.HtmlRenderer.Adapters.Entities;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Entities;

/// <summary>
/// Raised when Html Renderer request scroll to specific location.<br/>
/// This can occur on document anchor click.
/// </summary>
public sealed class HtmlScrollEventArgs 
    : EventArgs
{
    /// <summary>
    /// the location to scroll to
    /// </summary>
    private readonly RPoint _location;

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="location">the location to scroll to</param>
    public HtmlScrollEventArgs(RPoint location)
    {
        _location = location;
    }

    /// <summary>
    /// the x location to scroll to
    /// </summary>
    public double X
    {
        get { return _location.X; }
    }

    /// <summary>
    /// the x location to scroll to
    /// </summary>
    public double Y
    {
        get { return _location.Y; }
    }

    public override string ToString()
    {
        return string.Format("Location: {0}", _location);
    }
}
