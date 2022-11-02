// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RBrush.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for platform specific brush objects - used to fill graphics (rectangles, polygons and paths).<br/>
/// The brush can be solid color, gradient or image.
/// </summary>
public abstract class RBrush
    : IDisposable
{
    #region Public methods

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public abstract void Dispose();

    #endregion
}
