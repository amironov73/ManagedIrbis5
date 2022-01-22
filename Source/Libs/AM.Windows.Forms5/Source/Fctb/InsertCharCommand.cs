// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InsertCharCommand.cs -- вставка одиночного символа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Insert single char
/// </summary>
/// <remarks>This operation includes also insertion of new line and removing char by backspace</remarks>
public sealed class InsertCharCommand
    : UndoableCommand
{
    public char c;
    char deletedChar = '\x0';

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="ts">Underlaying textbox</param>
    /// <param name="c">Inserting char</param>
    public InsertCharCommand
        (
            TextSource ts,
            char c
        )
        : base (ts)
    {
        this.c = c;
    }

    #endregion

    #region Private members

    internal static void InsertChar
        (
            char c,
            ref char deletedChar,
            TextSource ts
        )
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

            case '\r':
                break;

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
                {
                    ts[tb.Selection.Start.Line].Insert (tb.Selection.Start.Column, new Character (' '));
                }

                tb.Selection.Start = new Place (tb.Selection.Start.Column + spaceCountNextTabStop,
                    tb.Selection.Start.Line);
                break;

            default:
                ts[tb.Selection.Start.Line].Insert (tb.Selection.Start.Column, new Character (c));
                tb.Selection.Start = new Place (tb.Selection.Start.Column + 1, tb.Selection.Start.Line);
                break;
        }
    }

    #endregion

    #region Command members

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

            case '\r':
                break;

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
                {
                    ts[sel.Start.Line].RemoveAt (sel.Start.Column);
                }

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

    /// <inheritdoc cref="UndoableCommand.Clone"/>
    public override UndoableCommand Clone()
    {
        return new InsertCharCommand (ts, c);
    }

    #endregion
}
