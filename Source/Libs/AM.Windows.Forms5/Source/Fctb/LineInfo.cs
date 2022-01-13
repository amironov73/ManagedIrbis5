// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* LineInfo.cs -- информация о строке в текстовом редакторе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Информация о строке в текстовом редакторе
/// </summary>
public struct LineInfo
{
    private List<int>? _cutOffPositions;

    //Y coordinate of line on screen
    internal int startY;

    internal int bottomPadding;

    //indent of secondary wordwrap strings (in chars)
    internal int wordWrapIndent;

    /// <summary>
    /// Visible state
    /// </summary>
    public VisibleState VisibleState;

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LineInfo
        (
            int startY
        )
    {
        _cutOffPositions = null;
        VisibleState = VisibleState.Visible;
        this.startY = startY;
        bottomPadding = 0;
        wordWrapIndent = 0;
    }

    #endregion

    /// <summary>
    /// Positions for wordwrap cutoffs
    /// </summary>
    public List<int> CutOffPositions => _cutOffPositions ??= new List<int>();

    /// <summary>
    /// Count of wordwrap string count for this line
    /// </summary>
    public int WordWrapStringsCount
    {
        get
        {
            switch (VisibleState)
            {
                case VisibleState.Visible:
                    return _cutOffPositions == null ? 1 : _cutOffPositions.Count + 1;

                case VisibleState.Hidden:
                    return 0;

                case VisibleState.StartOfHiddenBlock:
                    return 1;
            }

            return 0;
        }
    }

    internal int GetWordWrapStringStartPosition
        (
            int iWordWrapLine
        )
    {
        return iWordWrapLine == 0 ? 0 : CutOffPositions[iWordWrapLine - 1];
    }

    internal int GetWordWrapStringFinishPosition
        (
            int iWordWrapLine,
            Line line
        )
    {
        if (WordWrapStringsCount <= 0)
        {
            return 0;
        }

        return iWordWrapLine == WordWrapStringsCount - 1 ? line.Count - 1 : CutOffPositions[iWordWrapLine] - 1;
    }

    /// <summary>
    /// Gets index of wordwrap string for given char position
    /// </summary>
    public int GetWordWrapStringIndex
        (
            int iChar
        )
    {
        if (_cutOffPositions == null || _cutOffPositions.Count == 0)
        {
            return 0;
        }

        for (var i = 0; i < _cutOffPositions.Count; i++)
        {
            if (_cutOffPositions[i] > /*>=*/ iChar)
            {
                return i;
            }
        }

        return _cutOffPositions.Count;
    }
}
