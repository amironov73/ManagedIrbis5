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
    /// Aztec 2D code representation
    /// </summary>
    /// <author>Rustam Abdullaev</author>
    internal sealed class AztecCode
    {
        /// <summary>
        /// Compact or full symbol indicator
        /// </summary>
        public bool isCompact { get; set; }

        /// <summary>
        /// Size in pixels (width and height)
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Number of levels
        /// </summary>
        public int Layers { get; set; }

        /// <summary>
        /// Number of data codewords
        /// </summary>
        public int CodeWords { get; set; }

        /// <summary>
        /// The symbol image
        /// </summary>
        public BitMatrix Matrix { get; set; }
    }
}
