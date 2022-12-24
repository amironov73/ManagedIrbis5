// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
/// Represents the type of fragment in a mixed code document.
/// </summary>
public enum MixedCodeDocumentFragmentType
{
    /// <summary>
    /// The fragment contains code.
    /// </summary>
    Code,

    /// <summary>
    /// The fragment contains text.
    /// </summary>
    Text,
}
