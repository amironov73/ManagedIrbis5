// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RPen.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Adapters.Entities;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for platform specific pen objects - used to draw graphics (lines, rectangles and paths)
/// </summary>
public abstract class RPen
{
    #region Properties

    /// <summary>
    /// Gets or sets the width of this Pen, in units of the Graphics object used for drawing.
    /// </summary>
    public abstract double Width { get; set; }

    /// <summary>
    /// Gets or sets the style used for dashed lines drawn with this Pen.
    /// </summary>
    public abstract RDashStyle DashStyle { set; }

    #endregion
}
