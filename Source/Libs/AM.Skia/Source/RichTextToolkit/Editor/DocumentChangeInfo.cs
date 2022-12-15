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

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor;

/// <summary>
/// Provides information about the range of changes to a document
/// </summary>
public struct DocumentChangeInfo
{
    /// <summary>
    /// The index of the code point index at which the change was made
    /// </summary>
    public int CodePointIndex;

    /// <summary>
    /// Length of the text that was replaced
    /// </summary>
    public int OldLength;

    /// <summary>
    /// Length of the replacement text
    /// </summary>
    public int NewLength;

    /// <summary>
    /// True if the current edit operation is the result of an
    /// undo operation.
    /// </summary>
    public bool IsUndoing;

    /// <summary>
    /// Semantics of the edit operation
    /// </summary>
    public EditSemantics Semantics;

    /// <summary>
    /// Offset of the IME caret from the code point index
    /// </summary>
    public int ImeCaretOffset;
}
