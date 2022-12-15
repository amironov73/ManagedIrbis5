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

namespace AM.Skia.RichTextKit;

/// <summary>
/// Unicode grapheme cluster classes
/// </summary>
/// <remarks>
/// Note, these need to match those used by the JavaScript script that
/// generates the .trie resources
/// </remarks>
internal enum GraphemeClusterClass
{
    Any = 0,
    CR = 1,
    LF = 2,
    Control = 3,
    Extend = 4,
    Regional_Indicator = 5,
    Prepend = 6,
    SpacingMark = 7,
    L = 8,
    V = 9,
    T = 10,
    LV = 11,
    LVT = 12,
    ExtPict = 13,
    ZWJ = 14,

    // Pseudo classes, not generated from character data but used by pair table
    SOT = 15,
    EOT = 16,
    ExtPictZwg = 17,
}
