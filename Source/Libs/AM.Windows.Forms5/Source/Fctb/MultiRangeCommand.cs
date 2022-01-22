// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MultiRangeCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Wrapper for multirange commands
/// </summary>
public class MultiRangeCommand
    : UndoableCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="command"></param>
    public MultiRangeCommand
        (
            UndoableCommand command
        )
        : base (command.ts)
    {
        _cmd = command;
        _range = ts.CurrentTextBox.Selection.Clone();
    }

    #endregion

    #region Private members

    private readonly UndoableCommand _cmd;
    private readonly TextRange _range;
    private readonly List<UndoableCommand> _commandsByRanges = new ();

    private void ExecuteInsertTextCommand
        (
            ref int iChar,
            string text
        )
    {
        var lines = text.Split ('\n');
        var iLine = 0;
        foreach (var r in _range.GetSubRanges (true))
        {
            var line = ts.CurrentTextBox[r.Start.Line];
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

                ts.CurrentTextBox.Selection = r;
                var c = new InsertTextCommand (ts, insertedText);
                c.Execute();
                if (ts.CurrentTextBox.Selection.End.Column > iChar)
                {
                    iChar = ts.CurrentTextBox.Selection.End.Column;
                }

                _commandsByRanges.Add (c);
            }

            iLine++;
        }
    }

    private void ExecuteCommand
        (
            ref int iChar
        )
    {
        foreach (var r in _range.GetSubRanges (false))
        {
            ts.CurrentTextBox.Selection = r;
            var c = _cmd.Clone();
            c.Execute();
            if (ts.CurrentTextBox.Selection.End.Column > iChar)
                iChar = ts.CurrentTextBox.Selection.End.Column;
            _commandsByRanges.Add (c);
        }
    }

    #endregion

    #region Command members

    /// <inheritdoc cref="UndoableCommand.Execute"/>
    public override void Execute()
    {
        _commandsByRanges.Clear();
        var prevSelection = _range.Clone();
        var iChar = -1;
        var iStartLine = prevSelection.Start.Line;
        var iEndLine = prevSelection.End.Line;
        ts.CurrentTextBox.Selection.ColumnSelectionMode = false;
        ts.CurrentTextBox.Selection.BeginUpdate();
        ts.CurrentTextBox.BeginUpdate();
        ts.CurrentTextBox.AllowInsertRemoveLines = false;
        try
        {
            if (_cmd is InsertTextCommand)
            {
                ExecuteInsertTextCommand (ref iChar, (_cmd as InsertTextCommand).InsertedText);
            }
            else if (_cmd is InsertCharCommand && (_cmd as InsertCharCommand).c != '\x0' &&
                     (_cmd as InsertCharCommand).c != '\b') //if not DEL or BACKSPACE
            {
                ExecuteInsertTextCommand (ref iChar, (_cmd as InsertCharCommand).c.ToString());
            }
            else
            {
                ExecuteCommand (ref iChar);
            }
        }
        catch (ArgumentOutOfRangeException)
        {
        }
        finally
        {
            ts.CurrentTextBox.AllowInsertRemoveLines = true;
            ts.CurrentTextBox.EndUpdate();

            ts.CurrentTextBox.Selection = _range;
            if (iChar >= 0)
            {
                ts.CurrentTextBox.Selection.Start = new Place (iChar, iStartLine);
                ts.CurrentTextBox.Selection.End = new Place (iChar, iEndLine);
            }

            ts.CurrentTextBox.Selection.ColumnSelectionMode = true;
            ts.CurrentTextBox.Selection.EndUpdate();
        }
    }

    /// <inheritdoc cref="UndoableCommand.Undo"/>
    public override void Undo()
    {
        ts.CurrentTextBox.BeginUpdate();
        ts.CurrentTextBox.Selection.BeginUpdate();
        try
        {
            for (var i = _commandsByRanges.Count - 1; i >= 0; i--)
                _commandsByRanges[i].Undo();
        }
        finally
        {
            ts.CurrentTextBox.Selection.EndUpdate();
            ts.CurrentTextBox.EndUpdate();
        }

        ts.CurrentTextBox.Selection = _range.Clone();
        ts.CurrentTextBox.OnTextChanged (_range);
        ts.CurrentTextBox.OnSelectionChanged();
        ts.CurrentTextBox.Selection.ColumnSelectionMode = true;
    }

    /// <inheritdoc cref="UndoableCommand.Clone"/>
    public override UndoableCommand Clone()
    {
        throw new NotImplementedException();
    }

    #endregion
}
