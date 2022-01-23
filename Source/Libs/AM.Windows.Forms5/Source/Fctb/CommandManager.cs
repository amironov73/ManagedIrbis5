// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CommandManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
public class CommandManager
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    public event EventHandler? RedoCompleted = delegate { };

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    public static int maxHistoryLength = 200;

    /// <summary>
    ///
    /// </summary>
    public TextSource TextSource { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public bool UndoRedoStackIsEnabled { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool UndoEnabled => _history.Count > 0;

    /// <summary>
    ///
    /// </summary>
    public bool RedoEnabled => _redoStack.Count > 0;


    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CommandManager
        (
            TextSource textSource
        )
    {
        Sure.NotNull (textSource);

        _history = new LimitedStack<UndoableCommand> (maxHistoryLength);
        TextSource = textSource;
        UndoRedoStackIsEnabled = true;
    }

    #endregion

    #region Private members

    /// <summary>
    ///
    /// </summary>
    private int _autoUndoCommands;

    /// <summary>
    ///
    /// </summary>
    readonly LimitedStack<UndoableCommand> _history;

    /// <summary>
    ///
    /// </summary>
    private readonly Stack<UndoableCommand> _redoStack = new ();

    /// <summary>
    ///
    /// </summary>
    protected int disabledCommands;

    private void EndDisableCommands()
    {
        disabledCommands--;
    }

    private void BeginDisableCommands()
    {
        disabledCommands++;
    }

    internal void ClearHistory()
    {
        _history.Clear();
        _redoStack.Clear();
        TextSource.CurrentTextBox.OnUndoRedoStateChanged();
    }

    internal void Redo()
    {
        if (_redoStack.Count == 0)
            return;
        UndoableCommand cmd;
        BeginDisableCommands(); //prevent text changing into handlers
        try
        {
            cmd = _redoStack.Pop();
            if (TextSource.CurrentTextBox.Selection.ColumnSelectionMode)
                TextSource.CurrentTextBox.Selection.ColumnSelectionMode = false;
            TextSource.CurrentTextBox.Selection.Start = cmd.sel.Start;
            TextSource.CurrentTextBox.Selection.End = cmd.sel.End;
            cmd.Execute();
            _history.Push (cmd);
        }
        finally
        {
            EndDisableCommands();
        }

        //call event
        RedoCompleted?.Invoke (this, EventArgs.Empty);

        //redo command after autoUndoable command
        if (cmd.autoUndo)
        {
            Redo();
        }

        TextSource.CurrentTextBox.OnUndoRedoStateChanged();
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public virtual void ExecuteCommand
        (
            Command command
        )
    {
        Sure.NotNull (command);

        if (disabledCommands > 0)
        {
            return;
        }

        //multirange ?
        if (command.textSource.CurrentTextBox.Selection.ColumnSelectionMode)
        {
            if (command is UndoableCommand undoableCommand)
            {
                //make wrapper
                command = new MultiRangeCommand (undoableCommand);
            }
        }


        if (command is UndoableCommand undoableCommand1)
        {
            //if range is ColumnRange, then create wrapper
            undoableCommand1.autoUndo = _autoUndoCommands > 0;
            _history.Push (undoableCommand1);
        }

        try
        {
            command.Execute();
        }
        catch (ArgumentOutOfRangeException)
        {
            //OnTextChanging cancels enter of the text
            if (command is UndoableCommand)
            {
                _history.Pop();
            }
        }

        //
        if (!UndoRedoStackIsEnabled)
        {
            ClearHistory();
        }

        //
        _redoStack.Clear();

        //
        TextSource.CurrentTextBox.OnUndoRedoStateChanged();
    }

    /// <summary>
    ///
    /// </summary>
    public void Undo()
    {
        if (_history.Count > 0)
        {
            var command = _history.Pop();

            //
            BeginDisableCommands(); //prevent text changing into handlers
            try
            {
                if (command != null)
                {
                    command.Undo();
                }
            }
            finally
            {
                EndDisableCommands();
            }

            //
            if (command != null)
            {
                _redoStack.Push (command);
            }
        }

        //undo next autoUndo command
        if (_history.Count > 0)
        {
            if (_history.Peek()!.autoUndo)
            {
                Undo();
            }
        }

        TextSource.CurrentTextBox.OnUndoRedoStateChanged();
    }

    /// <summary>
    ///
    /// </summary>
    public void EndAutoUndoCommands()
    {
        _autoUndoCommands--;
        if (_autoUndoCommands == 0)
        {
            if (_history.Count > 0)
            {
                var command = _history.Peek();
                if (command != null)
                {
                    command.autoUndo = false;
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void BeginAutoUndoCommands()
    {
        _autoUndoCommands++;
    }

    #endregion
}
