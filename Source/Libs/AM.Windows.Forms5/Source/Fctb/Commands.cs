// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Commands.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Insert single char
/// </summary>
/// <remarks>This operation includes also insertion of new line and removing char by backspace</remarks>
public class InsertCharCommand
    : UndoableCommand
{
    public char c;
    char deletedChar = '\x0';

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tb">Underlaying textbox</param>
    /// <param name="c">Inserting char</param>
    public InsertCharCommand (TextSource ts, char c) : base (ts)
    {
        this.c = c;
    }

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        ts.OnTextChanging();
        switch (c)
        {
            case '\n':
                MergeLines (sel.Start.Line, ts);
                break;
            case '\r': break;
            case '\b':
                ts.CurrentTB.Selection.Start = lastSel.Start;
                var cc = '\x0';
                if (deletedChar != '\x0')
                {
                    ts.CurrentTB.ExpandBlock (ts.CurrentTB.Selection.Start.Line);
                    InsertChar (deletedChar, ref cc, ts);
                }

                break;
            case '\t':
                ts.CurrentTB.ExpandBlock (sel.Start.Line);
                for (var i = sel.FromX; i < lastSel.FromX; i++)
                    ts[sel.Start.Line].RemoveAt (sel.Start.Column);
                ts.CurrentTB.Selection.Start = sel.Start;
                break;
            default:
                ts.CurrentTB.ExpandBlock (sel.Start.Line);
                ts[sel.Start.Line].RemoveAt (sel.Start.Column);
                ts.CurrentTB.Selection.Start = sel.Start;
                break;
        }

        ts.NeedRecalc (new TextSource.TextChangedEventArgs (sel.Start.Line, sel.Start.Line));

        base.Undo();
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        ts.CurrentTB.ExpandBlock (ts.CurrentTB.Selection.Start.Line);
        var s = c.ToString();
        ts.OnTextChanging (ref s);
        if (s.Length == 1)
            c = s[0];

        if (String.IsNullOrEmpty (s))
            throw new ArgumentOutOfRangeException();


        if (ts.Count == 0)
            InsertLine (ts);
        InsertChar (c, ref deletedChar, ts);

        ts.NeedRecalc (new TextSource.TextChangedEventArgs (ts.CurrentTB.Selection.Start.Line,
            ts.CurrentTB.Selection.Start.Line));
        base.Execute();
    }

    internal static void InsertChar (char c, ref char deletedChar, TextSource ts)
    {
        var tb = ts.CurrentTB;

        switch (c)
        {
            case '\n':
                if (!ts.CurrentTB.AllowInsertRemoveLines)
                    throw new ArgumentOutOfRangeException ("Cant insert this char in ColumnRange mode");
                if (ts.Count == 0)
                    InsertLine (ts);
                InsertLine (ts);
                break;
            case '\r': break;
            case '\b': //backspace
                if (tb.Selection.Start.Column == 0 && tb.Selection.Start.Line == 0)
                    return;
                if (tb.Selection.Start.Column == 0)
                {
                    if (!ts.CurrentTB.AllowInsertRemoveLines)
                        throw new ArgumentOutOfRangeException ("Cant insert this char in ColumnRange mode");
                    if (tb.LineInfos[tb.Selection.Start.Line - 1].VisibleState != VisibleState.Visible)
                        tb.ExpandBlock (tb.Selection.Start.Line - 1);
                    deletedChar = '\n';
                    MergeLines (tb.Selection.Start.Line - 1, ts);
                }
                else
                {
                    deletedChar = ts[tb.Selection.Start.Line][tb.Selection.Start.Column - 1].c;
                    ts[tb.Selection.Start.Line].RemoveAt (tb.Selection.Start.Column - 1);
                    tb.Selection.Start = new Place (tb.Selection.Start.Column - 1, tb.Selection.Start.Line);
                }

                break;
            case '\t':
                var spaceCountNextTabStop = tb.TabLength - (tb.Selection.Start.Column % tb.TabLength);
                if (spaceCountNextTabStop == 0)
                    spaceCountNextTabStop = tb.TabLength;

                for (var i = 0; i < spaceCountNextTabStop; i++)
                    ts[tb.Selection.Start.Line].Insert (tb.Selection.Start.Column, new Character (' '));

                tb.Selection.Start = new Place (tb.Selection.Start.Column + spaceCountNextTabStop,
                    tb.Selection.Start.Line);
                break;
            default:
                ts[tb.Selection.Start.Line].Insert (tb.Selection.Start.Column, new Character (c));
                tb.Selection.Start = new Place (tb.Selection.Start.Column + 1, tb.Selection.Start.Line);
                break;
        }
    }

    internal static void InsertLine (TextSource ts)
    {
        var tb = ts.CurrentTB;

        if (!tb.Multiline && tb.LinesCount > 0)
            return;

        if (ts.Count == 0)
            ts.InsertLine (0, ts.CreateLine());
        else
            BreakLines (tb.Selection.Start.Line, tb.Selection.Start.Column, ts);

        tb.Selection.Start = new Place (0, tb.Selection.Start.Line + 1);
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
    }

    /// <summary>
    /// Merge lines i and i+1
    /// </summary>
    internal static void MergeLines (int i, TextSource ts)
    {
        var tb = ts.CurrentTB;

        if (i + 1 >= ts.Count)
            return;
        tb.ExpandBlock (i);
        tb.ExpandBlock (i + 1);
        var pos = ts[i].Count;

        //
        /*
        if(ts[i].Count == 0)
            ts.RemoveLine(i);
        else*/
        if (ts[i + 1].Count == 0)
            ts.RemoveLine (i + 1);
        else
        {
            ts[i].AddRange (ts[i + 1]);
            ts.RemoveLine (i + 1);
        }

        tb.Selection.Start = new Place (pos, i);
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
    }

    internal static void BreakLines (int iLine, int pos, TextSource ts)
    {
        var newLine = ts.CreateLine();
        for (var i = pos; i < ts[iLine].Count; i++)
            newLine.Add (ts[iLine][i]);
        ts[iLine].RemoveRange (pos, ts[iLine].Count - pos);

        //
        ts.InsertLine (iLine + 1, newLine);
    }

    public override UndoableCommand Clone()
    {
        return new InsertCharCommand (ts, c);
    }
}

/// <summary>
/// Insert text
/// </summary>
public class InsertTextCommand
    : UndoableCommand
{
    public string InsertedText;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tb">Underlaying textbox</param>
    /// <param name="insertedText">Text for inserting</param>
    public InsertTextCommand (TextSource ts, string insertedText) : base (ts)
    {
        this.InsertedText = insertedText;
    }

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        ts.CurrentTB.Selection.Start = sel.Start;
        ts.CurrentTB.Selection.End = lastSel.Start;
        ts.OnTextChanging();
        ClearSelectedCommand.ClearSelected (ts);
        base.Undo();
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        ts.OnTextChanging (ref InsertedText);
        InsertText (InsertedText, ts);
        base.Execute();
    }

    internal static void InsertText (string insertedText, TextSource ts)
    {
        var tb = ts.CurrentTB;
        try
        {
            tb.Selection.BeginUpdate();
            var cc = '\x0';

            if (ts.Count == 0)
            {
                InsertCharCommand.InsertLine (ts);
                tb.Selection.Start = Place.Empty;
            }

            tb.ExpandBlock (tb.Selection.Start.Line);
            var len = insertedText.Length;
            for (var i = 0; i < len; i++)
            {
                var c = insertedText[i];
                if (c == '\r' && (i >= len - 1 || insertedText[i + 1] != '\n'))
                    InsertCharCommand.InsertChar ('\n', ref cc, ts);
                else
                    InsertCharCommand.InsertChar (c, ref cc, ts);
            }

            ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
        }
        finally
        {
            tb.Selection.EndUpdate();
        }
    }

    public override UndoableCommand Clone()
    {
        return new InsertTextCommand (ts, InsertedText);
    }
}

/// <summary>
/// Insert text into given ranges
/// </summary>
public class ReplaceTextCommand
    : UndoableCommand
{
    string insertedText;
    List<TextRange> ranges;
    List<string> prevText = new List<string>();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tb">Underlaying textbox</param>
    /// <param name="ranges">List of ranges for replace</param>
    /// <param name="insertedText">Text for inserting</param>
    public ReplaceTextCommand (TextSource ts, List<TextRange> ranges, string insertedText)
        : base (ts)
    {
        //sort ranges by place
        ranges.Sort ((r1, r2) =>
        {
            if (r1.Start.Line == r2.Start.Line)
                return r1.Start.Column.CompareTo (r2.Start.Column);
            return r1.Start.Line.CompareTo (r2.Start.Line);
        });

        //
        this.ranges = ranges;
        this.insertedText = insertedText;
        lastSel = sel = new RangeInfo (ts.CurrentTB.Selection);
    }

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        var tb = ts.CurrentTB;

        ts.OnTextChanging();
        tb.BeginUpdate();

        tb.Selection.BeginUpdate();
        for (var i = 0; i < ranges.Count; i++)
        {
            tb.Selection.Start = ranges[i].Start;
            for (var j = 0; j < insertedText.Length; j++)
                tb.Selection.GoRight (true);
            ClearSelected (ts);
            InsertTextCommand.InsertText (prevText[prevText.Count - i - 1], ts);
        }

        tb.Selection.EndUpdate();
        tb.EndUpdate();

        if (ranges.Count > 0)
            ts.OnTextChanged (ranges[0].Start.Line, ranges[ranges.Count - 1].End.Line);

        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        var tb = ts.CurrentTB;
        prevText.Clear();

        ts.OnTextChanging (ref insertedText);

        tb.Selection.BeginUpdate();
        tb.BeginUpdate();
        for (var i = ranges.Count - 1; i >= 0; i--)
        {
            tb.Selection.Start = ranges[i].Start;
            tb.Selection.End = ranges[i].End;
            prevText.Add (tb.Selection.Text);
            ClearSelected (ts);
            if (insertedText != "")
                InsertTextCommand.InsertText (insertedText, ts);
        }

        if (ranges.Count > 0)
            ts.OnTextChanged (ranges[0].Start.Line, ranges[ranges.Count - 1].End.Line);
        tb.EndUpdate();
        tb.Selection.EndUpdate();
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));

        lastSel = new RangeInfo (tb.Selection);
    }

    public override UndoableCommand Clone()
    {
        return new ReplaceTextCommand (ts, new List<TextRange> (ranges), insertedText);
    }

    internal static void ClearSelected (TextSource ts)
    {
        var tb = ts.CurrentTB;

        tb.Selection.Normalize();

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
    }
}

/// <summary>
/// Replaces text
/// </summary>
public class ReplaceMultipleTextCommand
    : UndoableCommand
{
    List<ReplaceRange> ranges;
    List<string> prevText = new List<string>();

    public class ReplaceRange
    {
        public TextRange ReplacedRange { get; set; }
        public String ReplaceText { get; set; }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ts">Underlaying textsource</param>
    /// <param name="ranges">List of ranges for replace</param>
    public ReplaceMultipleTextCommand (TextSource ts, List<ReplaceRange> ranges)
        : base (ts)
    {
        //sort ranges by place
        ranges.Sort ((r1, r2) =>
        {
            if (r1.ReplacedRange.Start.Line == r2.ReplacedRange.Start.Line)
                return r1.ReplacedRange.Start.Column.CompareTo (r2.ReplacedRange.Start.Column);
            return r1.ReplacedRange.Start.Line.CompareTo (r2.ReplacedRange.Start.Line);
        });

        //
        this.ranges = ranges;
        lastSel = sel = new RangeInfo (ts.CurrentTB.Selection);
    }

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        var tb = ts.CurrentTB;

        ts.OnTextChanging();

        tb.Selection.BeginUpdate();
        for (var i = 0; i < ranges.Count; i++)
        {
            tb.Selection.Start = ranges[i].ReplacedRange.Start;
            for (var j = 0; j < ranges[i].ReplaceText.Length; j++)
                tb.Selection.GoRight (true);
            ClearSelectedCommand.ClearSelected (ts);
            var prevTextIndex = ranges.Count - 1 - i;
            InsertTextCommand.InsertText (prevText[prevTextIndex], ts);
            ts.OnTextChanged (ranges[i].ReplacedRange.Start.Line, ranges[i].ReplacedRange.Start.Line);
        }

        tb.Selection.EndUpdate();

        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        var tb = ts.CurrentTB;
        prevText.Clear();

        ts.OnTextChanging();

        tb.Selection.BeginUpdate();
        for (var i = ranges.Count - 1; i >= 0; i--)
        {
            tb.Selection.Start = ranges[i].ReplacedRange.Start;
            tb.Selection.End = ranges[i].ReplacedRange.End;
            prevText.Add (tb.Selection.Text);
            ClearSelectedCommand.ClearSelected (ts);
            InsertTextCommand.InsertText (ranges[i].ReplaceText, ts);
            ts.OnTextChanged (ranges[i].ReplacedRange.Start.Line, ranges[i].ReplacedRange.End.Line);
        }

        tb.Selection.EndUpdate();
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));

        lastSel = new RangeInfo (tb.Selection);
    }

    public override UndoableCommand Clone()
    {
        return new ReplaceMultipleTextCommand (ts, new List<ReplaceRange> (ranges));
    }
}

/// <summary>
/// Removes lines
/// </summary>
public class RemoveLinesCommand
    : UndoableCommand
{
    List<int> iLines;
    List<string> prevText = new List<string>();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tb">Underlaying textbox</param>
    /// <param name="ranges">List of ranges for replace</param>
    /// <param name="insertedText">Text for inserting</param>
    public RemoveLinesCommand (TextSource ts, List<int> iLines)
        : base (ts)
    {
        //sort iLines
        iLines.Sort();

        //
        this.iLines = iLines;
        lastSel = sel = new RangeInfo (ts.CurrentTB.Selection);
    }

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        var tb = ts.CurrentTB;

        ts.OnTextChanging();

        tb.Selection.BeginUpdate();

        //tb.BeginUpdate();
        for (var i = 0; i < iLines.Count; i++)
        {
            var iLine = iLines[i];

            if (iLine < ts.Count)
                tb.Selection.Start = new Place (0, iLine);
            else
                tb.Selection.Start = new Place (ts[ts.Count - 1].Count, ts.Count - 1);

            InsertCharCommand.InsertLine (ts);
            tb.Selection.Start = new Place (0, iLine);
            var text = prevText[prevText.Count - i - 1];
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
        var tb = ts.CurrentTB;
        prevText.Clear();

        ts.OnTextChanging();

        tb.Selection.BeginUpdate();
        for (var i = iLines.Count - 1; i >= 0; i--)
        {
            var iLine = iLines[i];

            prevText.Add (ts[iLine].Text); //backward
            ts.RemoveLine (iLine);

            //ts.OnTextChanged(ranges[i].Start.iLine, ranges[i].End.iLine);
        }

        tb.Selection.Start = new Place (0, 0);
        tb.Selection.EndUpdate();
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));

        lastSel = new RangeInfo (tb.Selection);
    }

    public override UndoableCommand Clone()
    {
        return new RemoveLinesCommand (ts, new List<int> (iLines));
    }
}

/// <summary>
/// Wrapper for multirange commands
/// </summary>
public class MultiRangeCommand
    : UndoableCommand
{
    private UndoableCommand cmd;
    private TextRange range;
    private List<UndoableCommand> commandsByRanges = new List<UndoableCommand>();

    public MultiRangeCommand (UndoableCommand command) : base (command.ts)
    {
        this.cmd = command;
        range = ts.CurrentTB.Selection.Clone();
    }

    public override void Execute()
    {
        commandsByRanges.Clear();
        var prevSelection = range.Clone();
        var iChar = -1;
        var iStartLine = prevSelection.Start.Line;
        var iEndLine = prevSelection.End.Line;
        ts.CurrentTB.Selection.ColumnSelectionMode = false;
        ts.CurrentTB.Selection.BeginUpdate();
        ts.CurrentTB.BeginUpdate();
        ts.CurrentTB.AllowInsertRemoveLines = false;
        try
        {
            if (cmd is InsertTextCommand)
                ExecuteInsertTextCommand (ref iChar, (cmd as InsertTextCommand).InsertedText);
            else if (cmd is InsertCharCommand && (cmd as InsertCharCommand).c != '\x0' &&
                     (cmd as InsertCharCommand).c != '\b') //if not DEL or BACKSPACE
                ExecuteInsertTextCommand (ref iChar, (cmd as InsertCharCommand).c.ToString());
            else
                ExecuteCommand (ref iChar);
        }
        catch (ArgumentOutOfRangeException)
        {
        }
        finally
        {
            ts.CurrentTB.AllowInsertRemoveLines = true;
            ts.CurrentTB.EndUpdate();

            ts.CurrentTB.Selection = range;
            if (iChar >= 0)
            {
                ts.CurrentTB.Selection.Start = new Place (iChar, iStartLine);
                ts.CurrentTB.Selection.End = new Place (iChar, iEndLine);
            }

            ts.CurrentTB.Selection.ColumnSelectionMode = true;
            ts.CurrentTB.Selection.EndUpdate();
        }
    }

    private void ExecuteInsertTextCommand (ref int iChar, string text)
    {
        var lines = text.Split ('\n');
        var iLine = 0;
        foreach (var r in range.GetSubRanges (true))
        {
            var line = ts.CurrentTB[r.Start.Line];
            var lineIsEmpty = r.End < r.Start && line.StartSpacesCount == line.Count;
            if (!lineIsEmpty)
            {
                var insertedText = lines[iLine % lines.Length];
                if (r.End < r.Start && insertedText != "")
                {
                    //add forwarding spaces
                    insertedText = new string (' ', r.Start.Column - r.End.Column) + insertedText;
                    r.Start = r.End;
                }

                ts.CurrentTB.Selection = r;
                var c = new InsertTextCommand (ts, insertedText);
                c.Execute();
                if (ts.CurrentTB.Selection.End.Column > iChar)
                    iChar = ts.CurrentTB.Selection.End.Column;
                commandsByRanges.Add (c);
            }

            iLine++;
        }
    }

    private void ExecuteCommand (ref int iChar)
    {
        foreach (var r in range.GetSubRanges (false))
        {
            ts.CurrentTB.Selection = r;
            var c = cmd.Clone();
            c.Execute();
            if (ts.CurrentTB.Selection.End.Column > iChar)
                iChar = ts.CurrentTB.Selection.End.Column;
            commandsByRanges.Add (c);
        }
    }

    public override void Undo()
    {
        ts.CurrentTB.BeginUpdate();
        ts.CurrentTB.Selection.BeginUpdate();
        try
        {
            for (var i = commandsByRanges.Count - 1; i >= 0; i--)
                commandsByRanges[i].Undo();
        }
        finally
        {
            ts.CurrentTB.Selection.EndUpdate();
            ts.CurrentTB.EndUpdate();
        }

        ts.CurrentTB.Selection = range.Clone();
        ts.CurrentTB.OnTextChanged (range);
        ts.CurrentTB.OnSelectionChanged();
        ts.CurrentTB.Selection.ColumnSelectionMode = true;
    }

    public override UndoableCommand Clone()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Remembers current selection and restore it after Undo
/// </summary>
public class SelectCommand
    : UndoableCommand
{
    public SelectCommand (TextSource ts) : base (ts)
    {
    }

    public override void Execute()
    {
        //remember selection
        lastSel = new RangeInfo (ts.CurrentTB.Selection);
    }

    protected override void OnTextChanged (bool invert)
    {
    }

    public override void Undo()
    {
        //restore selection
        ts.CurrentTB.Selection = new TextRange (ts.CurrentTB, lastSel.Start, lastSel.End);
    }

    public override UndoableCommand Clone()
    {
        var result = new SelectCommand (ts);
        if (lastSel != null)
            result.lastSel = new RangeInfo (new TextRange (ts.CurrentTB, lastSel.Start, lastSel.End));
        return result;
    }
}
