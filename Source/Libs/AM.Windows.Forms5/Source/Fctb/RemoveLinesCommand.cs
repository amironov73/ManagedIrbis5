// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RemoveLines.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Removes lines
/// </summary>
public sealed class RemoveLinesCommand
    : UndoableCommand
{
    #region Construction

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="textSource">Underlaying textbox</param>
    /// <param name="iLines">List of ranges for replace</param>
    public RemoveLinesCommand
        (
            TextSource textSource,
            List<int> iLines
        )
        : base (textSource)
    {
        //sort iLines
        iLines.Sort();

        //
        this._iLines = iLines;
        lastSel = sel = new RangeInfo (textSource.CurrentTextBox.Selection);
    }

    #endregion

    #region Private members

    private readonly List<int> _iLines;
    readonly List<string> _prevText = new ();

    #endregion

    #region Command members

    /// <inheritdoc cref="UndoableCommand.Undo"/>
    public override void Undo()
    {
        var tb = textSource.CurrentTextBox;

        textSource.OnTextChanging();

        tb.Selection.BeginUpdate();

        //tb.BeginUpdate();
        for (var i = 0; i < _iLines.Count; i++)
        {
            var iLine = _iLines[i];

            tb.Selection.Start = iLine < textSource.Count
                ? new Place (0, iLine)
                : new Place (textSource[^1].Count, textSource.Count - 1);

            InsertCharCommand.InsertLine (textSource);
            tb.Selection.Start = new Place (0, iLine);
            var text = _prevText[_prevText.Count - i - 1];
            InsertTextCommand.InsertText (text, textSource);
            textSource[iLine].IsChanged = true;
            if (iLine < textSource.Count - 1)
                textSource[iLine + 1].IsChanged = true;
            else
                textSource[iLine - 1].IsChanged = true;
            if (text.Trim() != string.Empty)
                textSource.OnTextChanged (iLine, iLine);
        }

        //tb.EndUpdate();
        tb.Selection.EndUpdate();

        textSource.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        var tb = textSource.CurrentTextBox;
        _prevText.Clear();

        textSource.OnTextChanging();

        tb.Selection.BeginUpdate();
        for (var i = _iLines.Count - 1; i >= 0; i--)
        {
            var iLine = _iLines[i];

            _prevText.Add (textSource[iLine].Text); //backward
            textSource.RemoveLine (iLine);

            //ts.OnTextChanged(ranges[i].Start.iLine, ranges[i].End.iLine);
        }

        tb.Selection.Start = new Place (0, 0);
        tb.Selection.EndUpdate();
        textSource.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));

        lastSel = new RangeInfo (tb.Selection);
    }

    /// <inheritdoc cref="UndoableCommand.Clone" />
    public override UndoableCommand Clone()
    {
        return new RemoveLinesCommand (textSource, new List<int> (_iLines));
    }

    #endregion
}
