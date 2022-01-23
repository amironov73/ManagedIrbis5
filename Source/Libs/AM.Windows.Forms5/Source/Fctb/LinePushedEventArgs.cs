// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LinePushedEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
public sealed class LinePushedEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string SourceLineText { get; }

    /// <summary>
    ///
    /// </summary>
    public int DisplayedLineIndex { get; }

    /// <summary>
    /// This property contains only changed text.
    /// If text of line is not changed, this property contains null.
    /// </summary>
    public string DisplayedLineText { get; }

    /// <summary>
    /// This text will be saved in the file
    /// </summary>
    public string SavedText { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public LinePushedEventArgs
        (
            string sourceLineText,
            int displayedLineIndex,
            string displayedLineText
        )
    {
        SourceLineText = sourceLineText;
        DisplayedLineIndex = displayedLineIndex;
        DisplayedLineText = displayedLineText;
        SavedText = displayedLineText;
    }

    #endregion
}
