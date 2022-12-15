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

namespace AM.Skia.RichTextKit.Editor
{
    /// <summary>
    /// Defines a kind of keyboard navigation
    /// </summary>
    public enum NavigationKind
    {
        /// <summary>
        /// No movement
        /// </summary>
        None,

        /// <summary>
        /// Move one character to the left
        /// </summary>
        CharacterLeft,

        /// <summary>
        /// Move one character to the right
        /// </summary>
        CharacterRight,

        /// <summary>
        /// Move up one line
        /// </summary>
        LineUp,

        /// <summary>
        /// Move down one line
        /// </summary>
        LineDown,

        /// <summary>
        /// Move left one word
        /// </summary>
        WordLeft,

        /// <summary>
        /// Move right one word
        /// </summary>
        WordRight,

        /// <summary>
        /// Move up one page
        /// </summary>
        PageUp,

        /// <summary>
        /// Move down one page
        /// </summary>
        PageDown,

        /// <summary>
        /// Move to the start of the line
        /// </summary>
        LineHome,

        /// <summary>
        /// Move to the end of the line
        /// </summary>
        LineEnd,

        /// <summary>
        /// Move to the top of the document
        /// </summary>
        DocumentHome,

        /// <summary>
        /// Move to the end of the document
        /// </summary>
        DocumentEnd,
    }
}
