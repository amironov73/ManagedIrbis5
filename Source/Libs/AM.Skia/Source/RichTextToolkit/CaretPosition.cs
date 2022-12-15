// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CaretPosition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Skia.RichTextKit;

/// <summary>
/// Stores state information about a caret position
/// </summary>
/// <remarks>
/// The caret position is defined primarily by it's code point
/// index however there are other attributes that can affect
/// where it's displayed and how it moves.  This structure
/// encapsulates all the information about the caret required
/// to position and move it correctly.
/// </remarks>
public struct CaretPosition
{
    #region Fields

    /// <summary>
    /// The code point index of the caret insertion point
    /// </summary>
    public int CodePointIndex;

    /// <summary>
    /// True to display the caret at the end of the previous line
    /// rather than the start of the following line when the code
    /// point index is exactly on a line break.
    /// </summary>
    public bool AltPosition;

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a CaretPosition
    /// </summary>
    /// <param name="codePointIndex">The code point index of the caret</param>
    /// <param name="altPosition">Whether the caret should be displayed in its alternative position</param>
    public CaretPosition
        (
            int codePointIndex,
            bool altPosition = false
        )
    {
        CodePointIndex = codePointIndex;
        AltPosition = altPosition;
    }

    #endregion
}
