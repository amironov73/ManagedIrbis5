﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

public class CommandManager
{
    #region Events

    public event EventHandler? RedoCompleted = delegate { };

    #endregion

    #region Properties

    public static int MaxHistoryLength = 200;

    LimitedStack<UndoableCommand> history;

    Stack<UndoableCommand> redoStack = new Stack<UndoableCommand>();

    public TextSource TextSource { get; private set; }

    public bool UndoRedoStackIsEnabled { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CommandManager
        (
            TextSource ts
        )
    {
        Sure.NotNull (ts);

        history = new LimitedStack<UndoableCommand> (MaxHistoryLength);
        TextSource = ts;
        UndoRedoStackIsEnabled = true;
    }

    #endregion

    #region Public methods

    public virtual void ExecuteCommand (Command cmd)
    {
        if (disabledCommands > 0)
            return;

        //multirange ?
        if (cmd.ts.CurrentTB.Selection.ColumnSelectionMode)
            if (cmd is UndoableCommand)

                //make wrapper
                cmd = new MultiRangeCommand ((UndoableCommand)cmd);


        if (cmd is UndoableCommand)
        {
            //if range is ColumnRange, then create wrapper
            (cmd as UndoableCommand).autoUndo = autoUndoCommands > 0;
            history.Push (cmd as UndoableCommand);
        }

        try
        {
            cmd.Execute();
        }
        catch (ArgumentOutOfRangeException)
        {
            //OnTextChanging cancels enter of the text
            if (cmd is UndoableCommand)
                history.Pop();
        }

        //
        if (!UndoRedoStackIsEnabled)
            ClearHistory();

        //
        redoStack.Clear();

        //
        TextSource.CurrentTB.OnUndoRedoStateChanged();
    }

    public void Undo()
    {
        if (history.Count > 0)
        {
            var cmd = history.Pop();

            //
            BeginDisableCommands(); //prevent text changing into handlers
            try
            {
                cmd.Undo();
            }
            finally
            {
                EndDisableCommands();
            }

            //
            redoStack.Push (cmd);
        }

        //undo next autoUndo command
        if (history.Count > 0)
        {
            if (history.Peek().autoUndo)
                Undo();
        }

        TextSource.CurrentTB.OnUndoRedoStateChanged();
    }

    protected int disabledCommands = 0;

    private void EndDisableCommands()
    {
        disabledCommands--;
    }

    private void BeginDisableCommands()
    {
        disabledCommands++;
    }

    int autoUndoCommands = 0;

    public void EndAutoUndoCommands()
    {
        autoUndoCommands--;
        if (autoUndoCommands == 0)
            if (history.Count > 0)
                history.Peek().autoUndo = false;
    }

    public void BeginAutoUndoCommands()
    {
        autoUndoCommands++;
    }

    internal void ClearHistory()
    {
        history.Clear();
        redoStack.Clear();
        TextSource.CurrentTB.OnUndoRedoStateChanged();
    }

    internal void Redo()
    {
        if (redoStack.Count == 0)
            return;
        UndoableCommand cmd;
        BeginDisableCommands(); //prevent text changing into handlers
        try
        {
            cmd = redoStack.Pop();
            if (TextSource.CurrentTB.Selection.ColumnSelectionMode)
                TextSource.CurrentTB.Selection.ColumnSelectionMode = false;
            TextSource.CurrentTB.Selection.Start = cmd.sel.Start;
            TextSource.CurrentTB.Selection.End = cmd.sel.End;
            cmd.Execute();
            history.Push (cmd);
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

        TextSource.CurrentTB.OnUndoRedoStateChanged();
    }

    public bool UndoEnabled => history.Count > 0;

    public bool RedoEnabled => redoStack.Count > 0;

    #endregion
}

internal class RangeInfo
{
    public Place Start { get; set; }
    public Place End { get; set; }

    public RangeInfo (TextRange r)
    {
        Start = r.Start;
        End = r.End;
    }

    internal int FromX
    {
        get
        {
            if (End.Line < Start.Line) return End.Column;
            if (End.Line > Start.Line) return Start.Column;
            return Math.Min (End.Column, Start.Column);
        }
    }
}
