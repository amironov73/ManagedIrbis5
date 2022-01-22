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
    /// <param name="ts">Underlaying textbox</param>
    /// <param name="iLines">List of ranges for replace</param>
    public RemoveLinesCommand
        (
            TextSource ts,
            List<int> iLines
        )
        : base (ts)
    {
        //sort iLines
        iLines.Sort();

        //
        this._iLines = iLines;
        lastSel = sel = new RangeInfo (ts.CurrentTextBox.Selection);
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
        var tb = ts.CurrentTextBox;

        ts.OnTextChanging();

        tb.Selection.BeginUpdate();

        //tb.BeginUpdate();
        for (var i = 0; i < _iLines.Count; i++)
        {
            var iLine = _iLines[i];

            tb.Selection.Start = iLine < ts.Count
                ? new Place (0, iLine)
                : new Place (ts[^1].Count, ts.Count - 1);

            InsertCharCommand.InsertLine (ts);
            tb.Selection.Start = new Place (0, iLine);
            var text = _prevText[_prevText.Count - i - 1];
            InsertTextCommand.InsertText (text, ts);
            ts[iLine].IsChanged = true;
            if (iLine < ts.Count - 1)
                ts[iLine + 1].IsChanged = true;
            else
                ts[iLine - 1].IsChanged = true;
            if (text.Trim() != string.Empty)
                ts.OnTextChanged (iLine, iLine);
        }

        //tb.EndUpdate();
        tb.Selection.EndUpdate();

        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        var tb = ts.CurrentTextBox;
        _prevText.Clear();

        ts.OnTextChanging();

        tb.Selection.BeginUpdate();
        for (var i = _iLines.Count - 1; i >= 0; i--)
        {
            var iLine = _iLines[i];

            _prevText.Add (ts[iLine].Text); //backward
            ts.RemoveLine (iLine);

            //ts.OnTextChanged(ranges[i].Start.iLine, ranges[i].End.iLine);
        }

        tb.Selection.Start = new Place (0, 0);
        tb.Selection.EndUpdate();
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));

        lastSel = new RangeInfo (tb.Selection);
    }

    /// <inheritdoc cref="UndoableCommand.Clone" />
    public override UndoableCommand Clone()
    {
        return new RemoveLinesCommand (ts, new List<int> (_iLines));
    }

    #endregion
}
