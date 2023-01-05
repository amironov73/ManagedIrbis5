// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global

/* PaintEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Reporting.Utils;

/// <summary>
/// Provides a data for paint event.
/// </summary>
public class PaintEventArgs
{
    #region Properties

    /// <summary>
    /// Gets a <b>Graphics</b> object to draw on.
    /// </summary>
    public IGraphics Graphics { get; }

    /// <summary>
    /// Gets the X scale factor.
    /// </summary>
    public float ScaleX { get; }

    /// <summary>
    /// Gets the Y scale factor.
    /// </summary>
    public float ScaleY { get; }

    /// <summary>
    /// Gets the cache that contains graphics objects.
    /// </summary>
    public GraphicCache Cache { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <b>FRPaintEventArgs</b> class with specified settings.
    /// </summary>
    /// <param name="graphics"><b>IGraphicsRenderer</b> object to draw on.</param>
    /// <param name="scaleX">X scale factor.</param>
    /// <param name="scaleY">Y scale factor.</param>
    /// <param name="cache">Cache that contains graphics objects.</param>
    public PaintEventArgs
        (
            IGraphics graphics,
            float scaleX,
            float scaleY,
            GraphicCache cache
        )
    {
        Graphics = graphics;
        ScaleX = scaleX;
        ScaleY = scaleY;
        Cache = cache;
    }

    /// <summary>
    /// Initializes a new instance of the <b>FRPaintEventArgs</b> class with specified settings.
    /// </summary>
    /// <param name="graphics"><b>Graphics</b> object to draw on.</param>
    /// <param name="scaleX">X scale factor.</param>
    /// <param name="scaleY">Y scale factor.</param>
    /// <param name="cache">Cache that contains graphics objects.</param>
    public PaintEventArgs
        (
            Graphics graphics,
            float scaleX,
            float scaleY,
            GraphicCache cache
        )
        : this (GdiGraphics.FromGraphics (graphics), scaleX, scaleY, cache)
    {
        // пустое тело конструктора
    }

    #endregion
}
