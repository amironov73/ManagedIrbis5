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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit
{
    /// <summary>
    /// Unicode directionality classes
    /// </summary>
    /// <remarks>
    /// Note, these need to match those used by the JavaScript script that
    /// generates the .trie resources
    /// </remarks>
    enum Directionality : byte
    {
        // Strong types
        L = 0,
        R = 1,
        AL = 2,

        // Weak Types
        EN = 3,
        ES = 4,
        ET = 5,
        AN = 6,
        CS = 7,
        NSM = 8,
        BN = 9,

        // Neutral Types
        B = 10,
        S = 11,
        WS = 12,
        ON = 13,

        // Explicit Formatting Types - Embed
        LRE = 14,
        LRO = 15,
        RLE = 16,
        RLO = 17,
        PDF = 18,

        // Explicit Formatting Types - Isolate
        LRI = 19,
        RLI = 20,
        FSI = 21,
        PDI = 22,

        /** Minimum bidi type value. */
        TYPE_MIN = 0,

        /** Maximum bidi type value. */
        TYPE_MAX = 22,

        /* Unknown */
        Unknown = 0xFF,
    }

}
