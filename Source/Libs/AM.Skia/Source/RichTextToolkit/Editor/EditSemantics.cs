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

using SkiaSharp;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor;

/// <summary>
/// Defines various semantics for TextDocument edit operations
/// </summary>
public enum EditSemantics
{
    /// <summary>
    /// No special behaviour
    /// </summary>
    None,

    /// <summary>
    /// Special behaviour for backspacing over one character
    /// </summary>
    Backspace,

    /// <summary>
    /// Special behaviour for forward deleting text one character
    /// </summary>
    ForwardDelete,

    /// <summary>
    /// Special behaviour typing text one character at time
    /// </summary>
    Typing,

    /// <summary>
    /// Special behaviour for overtyping existing text
    /// </summary>
    Overtype,

    /// <summary>
    /// Special behaviour for displaying the composition string of an IME
    /// </summary>
    ImeComposition,
}
