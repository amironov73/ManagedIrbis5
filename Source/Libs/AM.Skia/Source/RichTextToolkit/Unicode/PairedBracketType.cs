// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PairedBracketType
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.RichTextKit;

/// <summary>
/// Unicode paired bracket types
/// </summary>
/// <remarks>
/// Note, these need to match those used by the JavaScript script that
/// generates the .trie resources
/// </remarks>
internal enum PairedBracketType : byte
{
    n,
    o,
    c
}
