﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Reporting.Barcode.Aztec
{
    /// <summary>
    /// Defines an container for encoder options
    /// </summary>
    [Serializable]
    internal class EncodingOptions
    {
        /// <summary>
        /// Gets the data container for all options
        /// </summary>
        [Browsable (false)]
        public IDictionary<EncodeHintType, object> Hints { get; }

        /// <summary>
        /// Specifies the height of the barcode image
        /// </summary>
        public int Height
        {
            get
            {
                if (Hints.ContainsKey (EncodeHintType.HEIGHT))
                {
                    return (int)Hints[EncodeHintType.HEIGHT];
                }

                return 0;
            }
            set => Hints[EncodeHintType.HEIGHT] = value;
        }

        /// <summary>
        /// Specifies the width of the barcode image
        /// </summary>
        public int Width
        {
            get
            {
                if (Hints.ContainsKey (EncodeHintType.WIDTH))
                {
                    return (int)Hints[EncodeHintType.WIDTH];
                }

                return 0;
            }
            set => Hints[EncodeHintType.WIDTH] = value;
        }

        /// <summary>
        /// Don't put the content string into the output image.
        /// </summary>
        public bool PureBarcode
        {
            get
            {
                if (Hints.ContainsKey (EncodeHintType.PURE_BARCODE))
                {
                    return (bool)Hints[EncodeHintType.PURE_BARCODE];
                }

                return false;
            }
            set => Hints[EncodeHintType.PURE_BARCODE] = value;
        }

        /// <summary>
        /// Specifies margin, in pixels, to use when generating the barcode. The meaning can vary
        /// by format; for example it controls margin before and after the barcode horizontally for
        /// most 1D formats.
        /// </summary>
        public int Margin
        {
            get
            {
                if (Hints.ContainsKey (EncodeHintType.MARGIN))
                {
                    return (int)Hints[EncodeHintType.MARGIN];
                }

                return 0;
            }
            set => Hints[EncodeHintType.MARGIN] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingOptions"/> class.
        /// </summary>
        public EncodingOptions()
        {
            Hints = new Dictionary<EncodeHintType, object>();
        }
    }
}
