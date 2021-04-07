﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PaletteColorAttribute.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing
{
    /// <summary>
    ///
    /// </summary>
    public class PaletteColorAttribute
        : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PaletteColorAttribute"/> class.
        /// </summary>
        public PaletteColorAttribute
            (
                string color
            )
        {
            Color = color;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>The color.</value>
        public string Color { get; }
    }
}
