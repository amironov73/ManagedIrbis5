// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReplaceMultipleTextCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Replaces text
/// </summary>
public sealed class ReplaceMultipleTextCommand
    : UndoableCommand
{
    #region Properties

    public class ReplaceRange
    {
        public TextRange ReplacedRange { get; set; }
        public String ReplaceText { get; set; }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ts">Underlaying textsource</param>
    /// <param name="ranges">List of ranges for replace</param>
    public ReplaceMultipleTextCommand
        (
            TextSource ts,
            List<ReplaceRange> ranges
        )
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
        this._ranges = ranges;
        lastSel = sel = new RangeInfo (ts.CurrentTB.Selection);
    }

    #endregion

    #region Private members

    private readonly List<ReplaceRange> _ranges;
    private readonly List<string> _prevText = new ();

    #endregion

    #region Command members

    /// <summary>
    /// Undo operation
    /// </summary>
    public override void Undo()
    {
        var tb = ts.CurrentTB;

        ts.OnTextChanging();

        tb.Selection.BeginUpdate();
        for (var i = 0; i < _ranges.Count; i++)
        {
            tb.Selection.Start = _ranges[i].ReplacedRange.Start;
            for (var j = 0; j < _ranges[i].ReplaceText.Length; j++)
                tb.Selection.GoRight (true);
            ClearSelectedCommand.ClearSelected (ts);
            var prevTextIndex = _ranges.Count - 1 - i;
            InsertTextCommand.InsertText (_prevText[prevTextIndex], ts);
            ts.OnTextChanged (_ranges[i].ReplacedRange.Start.Line, _ranges[i].ReplacedRange.Start.Line);
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
        _prevText.Clear();

        ts.OnTextChanging();

        tb.Selection.BeginUpdate();
        for (var i = _ranges.Count - 1; i >= 0; i--)
        {
            tb.Selection.Start = _ranges[i].ReplacedRange.Start;
            tb.Selection.End = _ranges[i].ReplacedRange.End;
            _prevText.Add (tb.Selection.Text);
            ClearSelectedCommand.ClearSelected (ts);
            InsertTextCommand.InsertText (_ranges[i].ReplaceText, ts);
            ts.OnTextChanged (_ranges[i].ReplacedRange.Start.Line, _ranges[i].ReplacedRange.End.Line);
        }

        tb.Selection.EndUpdate();
        ts.NeedRecalc (new TextSource.TextChangedEventArgs (0, 1));

        lastSel = new RangeInfo (tb.Selection);
    }

    /// <inheritdoc cref="UndoableCommand.Clone"/>
    public override UndoableCommand Clone()
    {
        return new ReplaceMultipleTextCommand (ts, new List<ReplaceRange> (_ranges));
    }

    #endregion
}
