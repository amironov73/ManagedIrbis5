// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LineNeededEventArgs.cs --
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
public sealed class LineNeededEventArgs
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
    /// This text will be displayed in textbox
    /// </summary>
    public string DisplayedLineText { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LineNeededEventArgs
        (
            string sourceLineText,
            int displayedLineIndex
        )
    {
        SourceLineText = sourceLineText;
        DisplayedLineIndex = displayedLineIndex;
        DisplayedLineText = sourceLineText;
    }

    #endregion
}
