// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RImage.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for platform specific image object - used to render images.
/// </summary>
public abstract class RImage 
    : IDisposable
{
    /// <summary>
    /// Get the width, in pixels, of the image.
    /// </summary>
    public abstract double Width { get; }

    /// <summary>
    /// Get the height, in pixels, of the image.
    /// </summary>
    public abstract double Height { get; }

    public abstract void Dispose();
}
