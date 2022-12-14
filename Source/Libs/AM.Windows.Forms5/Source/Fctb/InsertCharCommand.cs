// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

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
    /// <param name="textSource">Underlaying textbox</param>
    /// <param name="c">Inserting char</param>
    public InsertCharCommand
        (
            TextSource textSource,
            char c
        )
        : base (textSource)
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
        var tb = ts.CurrentTextBox;

        switch (c)
        {
            case '\n':
                if (!ts.CurrentTextBox.AllowInsertRemoveLines)
                {
                    throw new ArgumentOutOfRangeException (nameof (c), "Cant insert this char in ColumnRange mode");
                }

                if (ts.Count == 0)
                {
                    InsertLine (ts);
                }

                InsertLine (ts);
                break;

            case '\r':
                break;

            case '\b': //backspace
                if (tb.Selection.Start is { Column: 0, Line: 0 })
                {
                    return;
                }

                if (tb.Selection.Start.Column == 0)
                {
                    if (!ts.CurrentTextBox.AllowInsertRemoveLines)
                    {
                        throw new ArgumentOutOfRangeException (nameof (c), "Cant insert this char in ColumnRange mode");
                    }

                    if (tb.LineInfos[tb.Selection.Start.Line - 1].VisibleState != VisibleState.Visible)
                    {
                        tb.ExpandBlock (tb.Selection.Start.Line - 1);
                    }

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
                {
                    spaceCountNextTabStop = tb.TabLength;
                }

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
        textSource.OnTextChanging();
        switch (c)
        {
            case '\n':
                MergeLines (sel.Start.Line, textSource);
                break;

            case '\r':
                break;

            case '\b':
                textSource.CurrentTextBox.Selection.Start = lastSel!.Start;
                var cc = '\x0';
                if (deletedChar != '\x0')
                {
                    textSource.CurrentTextBox.ExpandBlock (textSource.CurrentTextBox.Selection.Start.Line);
                    InsertChar (deletedChar, ref cc, textSource);
                }

                break;

            case '\t':
                textSource.CurrentTextBox.ExpandBlock (sel.Start.Line);
                for (var i = sel.FromX; i < lastSel!.FromX; i++)
                {
                    textSource[sel.Start.Line].RemoveAt (sel.Start.Column);
                }

                textSource.CurrentTextBox.Selection.Start = sel.Start;
                break;

            default:
                textSource.CurrentTextBox.ExpandBlock (sel.Start.Line);
                textSource[sel.Start.Line].RemoveAt (sel.Start.Column);
                textSource.CurrentTextBox.Selection.Start = sel.Start;
                break;
        }

        textSource.NeedRecalc (new TextSource.TextChangedEventArgs (sel.Start.Line, sel.Start.Line));

        base.Undo();
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        textSource.CurrentTextBox.ExpandBlock (textSource.CurrentTextBox.Selection.Start.Line);
        var s = c.ToString();
        textSource.OnTextChanging (ref s);
        if (s!.Length == 1)
        {
            c = s[0];
        }

        if (String.IsNullOrEmpty (s))
        {
            throw new ArgumentOutOfRangeException();
        }


        if (textSource.Count == 0)
        {
            InsertLine (textSource);
        }

        InsertChar (c, ref deletedChar, textSource);

        textSource.NeedRecalc (new TextSource.TextChangedEventArgs (textSource.CurrentTextBox.Selection.Start.Line,
            textSource.CurrentTextBox.Selection.Start.Line));
        base.Execute();
    }

    internal static void InsertLine (TextSource textSource)
    {
        var tb = textSource.CurrentTextBox;

        if (tb is { Multiline: false, LinesCount: > 0 })
        {
            return;
        }

        if (textSource.Count == 0)
        {
            textSource.InsertLine (0, textSource.CreateLine());
        }
        else
        {
            BreakLines (tb.Selection.Start.Line, tb.Selection.Start.Column, textSource);
        }

        tb.Selection.Start = new Place (0, tb.Selection.Start.Line + 1);
        textSource.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
    }

    /// <summary>
    /// Merge lines i and i+1
    /// </summary>
    internal static void MergeLines (int i, TextSource ts)
    {
        var tb = ts.CurrentTextBox;

        if (i + 1 >= ts.Count)
        {
            return;
        }

        tb.ExpandBlock (i);
        tb.ExpandBlock (i + 1);
        var pos = ts[i].Count;

        //
        /*
        if(ts[i].Count == 0)
            ts.RemoveLine(i);
        else*/
        if (ts[i + 1].Count == 0)
        {
            ts.RemoveLine (i + 1);
        }
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
        return new InsertCharCommand (textSource, c);
    }

    #endregion
}
