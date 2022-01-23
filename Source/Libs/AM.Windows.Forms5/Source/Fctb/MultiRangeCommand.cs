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
        : base (command.textSource)
    {
        _cmd = command;
        _range = textSource.CurrentTextBox.Selection.Clone();
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
            var line = textSource.CurrentTextBox[r.Start.Line];
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

                textSource.CurrentTextBox.Selection = r;
                var c = new InsertTextCommand (textSource, insertedText);
                c.Execute();
                if (textSource.CurrentTextBox.Selection.End.Column > iChar)
                {
                    iChar = textSource.CurrentTextBox.Selection.End.Column;
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
            textSource.CurrentTextBox.Selection = r;
            var c = _cmd.Clone();
            c.Execute();
            if (textSource.CurrentTextBox.Selection.End.Column > iChar)
                iChar = textSource.CurrentTextBox.Selection.End.Column;
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
        textSource.CurrentTextBox.Selection.ColumnSelectionMode = false;
        textSource.CurrentTextBox.Selection.BeginUpdate();
        textSource.CurrentTextBox.BeginUpdate();
        textSource.CurrentTextBox.AllowInsertRemoveLines = false;
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
            textSource.CurrentTextBox.AllowInsertRemoveLines = true;
            textSource.CurrentTextBox.EndUpdate();

            textSource.CurrentTextBox.Selection = _range;
            if (iChar >= 0)
            {
                textSource.CurrentTextBox.Selection.Start = new Place (iChar, iStartLine);
                textSource.CurrentTextBox.Selection.End = new Place (iChar, iEndLine);
            }

            textSource.CurrentTextBox.Selection.ColumnSelectionMode = true;
            textSource.CurrentTextBox.Selection.EndUpdate();
        }
    }

    /// <inheritdoc cref="UndoableCommand.Undo"/>
    public override void Undo()
    {
        textSource.CurrentTextBox.BeginUpdate();
        textSource.CurrentTextBox.Selection.BeginUpdate();
        try
        {
            for (var i = _commandsByRanges.Count - 1; i >= 0; i--)
                _commandsByRanges[i].Undo();
        }
        finally
        {
            textSource.CurrentTextBox.Selection.EndUpdate();
            textSource.CurrentTextBox.EndUpdate();
        }

        textSource.CurrentTextBox.Selection = _range.Clone();
        textSource.CurrentTextBox.OnTextChanged (_range);
        textSource.CurrentTextBox.OnSelectionChanged();
        textSource.CurrentTextBox.Selection.ColumnSelectionMode = true;
    }

    /// <inheritdoc cref="UndoableCommand.Clone"/>
    public override UndoableCommand Clone()
    {
        throw new NotImplementedException();
    }

    #endregion
}
