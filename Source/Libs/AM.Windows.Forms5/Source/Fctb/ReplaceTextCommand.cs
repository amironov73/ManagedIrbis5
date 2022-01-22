// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReplaceTextCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Insert text into given ranges
/// </summary>
public sealed class ReplaceTextCommand
    : UndoableCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="ts">Underlaying textbox</param>
    /// <param name="ranges">List of ranges for replace</param>
    /// <param name="insertedText">Text for inserting</param>
    public ReplaceTextCommand
        (
            TextSource ts,
            List<TextRange> ranges,
            string insertedText
        )
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
        this._ranges = ranges;
        this._insertedText = insertedText;
        lastSel = sel = new RangeInfo (ts.CurrentTB.Selection);
    }

    #endregion

    #region Private members

    private string _insertedText;
    private readonly List<TextRange> _ranges;
    private readonly List<string> _prevText = new ();

    internal static void ClearSelected
        (
            TextSource ts
        )
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

    #endregion

    #region Command members

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        var tb = ts.CurrentTB;

        ts.OnTextChanging();
        tb.BeginUpdate();

        tb.Selection.BeginUpdate();
        for (var i = 0; i < _ranges.Count; i++)
        {
            tb.Selection.Start = _ranges[i].Start;
            for (var j = 0; j < _insertedText.Length; j++)
                tb.Selection.GoRight (true);
            ClearSelected (ts);
            InsertTextCommand.InsertText (_prevText[_prevText.Count - i - 1], ts);
        }

        tb.Selection.EndUpdate();
        tb.EndUpdate();

        if (_ranges.Count > 0)
        {
            ts.OnTextChanged (_ranges[0].Start.Line, _ranges[^1].End.Line);
        }

        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));
    }

    /// <summary>
    /// Execute operation
    /// </summary>
    public override void Execute()
    {
        var tb = ts.CurrentTB;
        _prevText.Clear();

        ts.OnTextChanging (ref _insertedText);

        tb.Selection.BeginUpdate();
        tb.BeginUpdate();
        for (var i = _ranges.Count - 1; i >= 0; i--)
        {
            tb.Selection.Start = _ranges[i].Start;
            tb.Selection.End = _ranges[i].End;
            _prevText.Add (tb.Selection.Text);
            ClearSelected (ts);
            if (_insertedText != string.Empty)
            {
                InsertTextCommand.InsertText (_insertedText, ts);
            }
        }

        if (_ranges.Count > 0)
        {
            ts.OnTextChanged (_ranges[0].Start.Line, _ranges[^1].End.Line);
        }

        tb.EndUpdate();
        tb.Selection.EndUpdate();
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));

        lastSel = new RangeInfo (tb.Selection);
    }

    /// <inheritdoc cref="UndoableCommand.Clone"/>
    public override UndoableCommand Clone()
    {
        return new ReplaceTextCommand (ts, new List<TextRange> (_ranges), _insertedText);
    }

    #endregion
}
