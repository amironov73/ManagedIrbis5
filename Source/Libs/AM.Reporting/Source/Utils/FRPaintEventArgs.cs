// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// Provides a data for paint event.
    /// </summary>
    public class FRPaintEventArgs
    {
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

        /// <summary>
        /// Initializes a new instance of the <b>FRPaintEventArgs</b> class with specified settings.
        /// </summary>
        /// <param name="g"><b>IGraphicsRenderer</b> object to draw on.</param>
        /// <param name="scaleX">X scale factor.</param>
        /// <param name="scaleY">Y scale factor.</param>
        /// <param name="cache">Cache that contains graphics objects.</param>
        public FRPaintEventArgs (IGraphics g, float scaleX, float scaleY, GraphicCache cache)
        {
            Graphics = g;
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            this.Cache = cache;
        }

        /// <summary>
        /// Initializes a new instance of the <b>FRPaintEventArgs</b> class with specified settings.
        /// </summary>
        /// <param name="g"><b>Graphics</b> object to draw on.</param>
        /// <param name="scaleX">X scale factor.</param>
        /// <param name="scaleY">Y scale factor.</param>
        /// <param name="cache">Cache that contains graphics objects.</param>
        public FRPaintEventArgs (Graphics g, float scaleX, float scaleY, GraphicCache cache) :
            this (GdiGraphics.FromGraphics (g), scaleX, scaleY, cache)
        {
        }
    }
}
