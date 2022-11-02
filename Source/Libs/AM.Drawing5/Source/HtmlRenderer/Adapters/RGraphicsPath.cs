// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RGraphicsPath.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for platform specific graphics path object - used to render (draw/fill) path shape.
/// </summary>
public abstract class RGraphicsPath
    : IDisposable
{
    #region Public methods

    /// <summary>
    /// Start path at the given point.
    /// </summary>
    public abstract void Start (double x, double y);

    /// <summary>
    /// Add stright line to the given point from te last point.
    /// </summary>
    public abstract void LineTo (double x, double y);

    /// <summary>
    /// Add circular arc of the given size to the given point from the last point.
    /// </summary>
    public abstract void ArcTo (double x, double y, double size, Corner corner);

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public abstract void Dispose();

    #endregion

    #region Enums

    /// <summary>
    /// The 4 corners that are handled in arc rendering.
    /// </summary>
    public enum Corner
    {
        /// <summary>
        /// Верхний левый угол.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Верхний правый угол.
        /// </summary>
        TopRight,

        /// <summary>
        /// Нижний левый угол.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Нижний правый угол.
        /// </summary>
        BottomRight
    }

    #endregion
}
