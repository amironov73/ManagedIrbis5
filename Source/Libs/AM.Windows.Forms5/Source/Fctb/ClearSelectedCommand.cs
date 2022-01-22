// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ClearSelectedCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Clear selected text.
/// </summary>
public sealed class ClearSelectedCommand
    : UndoableCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="tb">Underlaying textbox</param>
    public ClearSelectedCommand
        (
            TextSource ts
        )
        : base (ts)
    {
    }

    #endregion

    #region Private members

    private string? _deletedText;

    internal static void ClearSelected
        (
            TextSource ts
        )
    {
        var tb = ts.CurrentTextBox;

        var start = tb.Selection.Start;
        var end = tb.Selection.End;
        var fromLine = Math.Min (end.Line, start.Line);
        var toLine = Math.Max (end.Line, start.Line);
        var fromChar = tb.Selection.FromX;
        var toChar = tb.Selection.ToX;
        if (fromLine < 0) return;

        //
        if (fromLine == toLine)
            ts[fromLine].RemoveRange (fromChar, toChar - fromChar);
        else
        {
            ts[fromLine].RemoveRange (fromChar, ts[fromLine].Count - fromChar);
            ts[toLine].RemoveRange (0, toChar);
            ts.RemoveLine (fromLine + 1, toLine - fromLine - 1);
            InsertCharCommand.MergeLines (fromLine, ts);
        }

        //
        tb.Selection.Start = new Place (fromChar, fromLine);

        //
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (fromLine, toLine));
    }

    #endregion

    #region Command methods

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        ts.CurrentTextBox.Selection.Start = new Place (sel.FromX, Math.Min (sel.Start.Line, sel.End.Line));
        ts.OnTextChanging();
        InsertTextCommand.InsertText (_deletedText, ts);
        ts.OnTextChanged (sel.Start.Line, sel.End.Line);
        ts.CurrentTextBox.Selection.Start = sel.Start;
        ts.CurrentTextBox.Selection.End = sel.End;
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        var tb = ts.CurrentTextBox;

        string temp = null;
        ts.OnTextChanging (ref temp);
        if (temp == "")
            throw new ArgumentOutOfRangeException();

        _deletedText = tb.Selection.Text;
        ClearSelected (ts);
        lastSel = new RangeInfo (tb.Selection);
        ts.OnTextChanged (lastSel.Start.Line, lastSel.Start.Line);
    }

    public override UndoableCommand Clone()
    {
        return new ClearSelectedCommand (ts);
    }

    #endregion
}
