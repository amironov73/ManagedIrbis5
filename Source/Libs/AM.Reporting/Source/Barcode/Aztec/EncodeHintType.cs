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

#nullable enable

namespace AM.Reporting.Barcode.Aztec
{
    /// <summary>
    /// These are a set of hints that you may pass to Writers to specify their behavior.
    /// </summary>
    /// <author>dswitkin@google.com (Daniel Switkin)</author>
    internal enum EncodeHintType
    {
        /// <summary>
        /// Specifies the width of the barcode image
        /// type: <see cref="System.Int32" />
        /// </summary>
        WIDTH,

        /// <summary>
        /// Specifies the height of the barcode image
        /// type: <see cref="System.Int32" />
        /// </summary>
        HEIGHT,

        /// <summary>
        /// Don't put the content string into the output image.
        /// type: <see cref="System.Boolean" />
        /// </summary>
        PURE_BARCODE,


        ERROR_CORRECTION,

        /// <summary>
        /// Specifies what character encoding to use where applicable.
        /// type: <see cref="System.String" />
        /// </summary>
        CHARACTER_SET,

        /// <summary>
        /// Specifies margin, in pixels, to use when generating the barcode. The meaning can vary
        /// by format; for example it controls margin before and after the barcode horizontally for
        /// most 1D formats.
        /// type: <see cref="System.Int32" />
        /// </summary>
        MARGIN,

        /// <summary>
        /// Specifies whether to use compact mode for PDF417.
        /// type: <see cref="System.Boolean" />
        /// </summary>
        PDF417_COMPACT,


        PDF417_COMPACTION,


        PDF417_DIMENSIONS,

        /// <summary>
        /// Don't append ECI segment.
        /// That is against the specification of QR Code but some
        /// readers have problems if the charset is switched from
        /// ISO-8859-1 (default) to UTF-8 with the necessary ECI segment.
        /// If you set the property to true you can use UTF-8 encoding
        /// and the ECI segment is omitted.
        /// type: <see cref="System.Boolean" />
        /// </summary>
        DISABLE_ECI,


        DATA_MATRIX_SHAPE,


        MIN_SIZE,


        MAX_SIZE,

        /// <summary>
        /// if true, don't switch to codeset C for numbers
        /// </summary>
        CODE128_FORCE_CODESET_B,


        DATA_MATRIX_DEFAULT_ENCODATION,

        /// <summary>
        /// Specifies the required number of layers for an Aztec code:
        /// a negative number (-1, -2, -3, -4) specifies a compact Aztec code
        /// 0 indicates to use the minimum number of layers (the default)
        /// a positive number (1, 2, .. 32) specifies a normal (non-compact) Aztec code
        /// </summary>
        AZTEC_LAYERS,
    }
}
