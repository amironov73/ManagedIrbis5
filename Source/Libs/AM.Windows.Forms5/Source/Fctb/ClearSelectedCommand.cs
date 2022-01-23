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
    /// <param name="textSource">Underlaying textbox</param>
    public ClearSelectedCommand
        (
            TextSource textSource
        )
        : base (textSource)
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
        textSource.CurrentTextBox.Selection.Start = new Place (sel.FromX, Math.Min (sel.Start.Line, sel.End.Line));
        textSource.OnTextChanging();
        InsertTextCommand.InsertText (_deletedText!, textSource);
        textSource.OnTextChanged (sel.Start.Line, sel.End.Line);
        textSource.CurrentTextBox.Selection.Start = sel.Start;
        textSource.CurrentTextBox.Selection.End = sel.End;
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        var tb = textSource.CurrentTextBox;

        string temp = null;
        textSource.OnTextChanging (ref temp);
        if (temp == "")
            throw new ArgumentOutOfRangeException();

        _deletedText = tb.Selection.Text;
        ClearSelected (textSource);
        lastSel = new RangeInfo (tb.Selection);
        textSource.OnTextChanged (lastSel.Start.Line, lastSel.Start.Line);
    }

    public override UndoableCommand Clone()
    {
        return new ClearSelectedCommand (textSource);
    }

    #endregion
}
