// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ConsoleDebugger.cs -- консольный отладчик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

using AM.Kotik.Barsik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Diagnostics;

/*

    h[elp] - get help on commands
    h[elp] cmd  - get help on a specific command

    r[un] - run to next breakpoint or to end
    s[tep] - single-step, descending into functions
    n[ext] - single-step without descending into functions
    fin[ish] - finish current function, loop, etc.

    l[ist] - show lines of code surrounding the current point
    p[rint] name - print value of variable called `name`
    b[reak] name - set a breakpoint at function `name`
    b line - set breakpoint at `line`
    h[elp] b - documentation for setting breakpoints
    i[nfo] b - list breakpoints
    i - list all info commands
    dis[able] line - disable breakpoint at `line`
    en[able] line - enable breakpoint at `line`
    d[elete] line - delete breakpoint at `line`
    cond[ition] line expr - stop at breakpoint at `line` only if `expr` is true
    cond `line` - make breakpoint at `line` unconditional
    eval expr - evaluate expression
    exit - exit the debugger
    load file - load `file`

 */

/// <summary>
///
/// </summary>
public sealed class ConsoleDebugger
    : IBarsikDebugger
{
    #region Properties

    /// <inheritdoc cref="IBarsikDebugger.Breakpoints"/>
    public Dictionary<StatementBase, Breakpoint> Breakpoints { get; }

    #endregion

    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConsoleDebugger
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        _interpreter = interpreter;
        _interpreter.ScriptDebugger = this;
        Breakpoints = new ();
    }

    #endregion

    #region Private members

    private readonly Interpreter _interpreter;
    private ProgramNode? _program;
    private Thread? _scriptThread;
    private StatementBase? _currentStatement;
    private string[]? _sourceLines;

    private void RunProgram()
    {
        try
        {
            if (_program is not null)
            {
                _interpreter.Execute (_program, _interpreter.Context);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }

        Console.WriteLine ("Script execution completed");

        _scriptThread = null;
    }

    /// <summary>
    /// Задание точки останова.
    /// </summary>
    private void SetBreakpoint
        (
            string? where
        )
    {
        if (_program is null
            || string.IsNullOrWhiteSpace (where))
        {
            return;
        }

        if (int.TryParse (where, CultureInfo.InvariantCulture, out var lineNumber))
        {
            var statement = ((IStatementBlock) _program).FindStatementAt (lineNumber);
            if (statement is not null)
            {
                if (!Breakpoints.ContainsKey (statement))
                {
                    var breakpoint = new Breakpoint (statement)
                    {
                        Break = true
                    };
                    Breakpoints[statement] = breakpoint;
                }

                var sourceLine = _sourceLines.SafeAt (statement.Line - 1);
                Console.WriteLine ($"{statement}: {sourceLine}");
            }
            else
            {
                Console.WriteLine ($"Can't find statement at {where}");
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Цикл ввод-вычисление-печать.
    /// </summary>
    public void DoRepl()
    {
        while (true)
        {
            Console.Write ("! ");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace (line))
            {
                break;
            }

            var parts = line.Split (' ', 2);
            var command = parts[0];
            var other = parts.Length != 1 ? parts[1].Trim() : null;
            switch (command)
            {
                case "b":
                case "break":
                    SetBreakpoint (other);
                    break;

                case "eval":
                    Evaluate (other);
                    break;

                case "exit":
                    goto DONE;

                case "i":
                case "info":
                    if (other == "b")
                    {
                        ListBreakpoints();
                    }

                    break;

                case "load":
                    Load (other);
                    break;

                case "r":
                case "run":
                    Run();
                    break;

                default:
                    Console.WriteLine ($"Unknown command {command}");
                    break;
            }
        }

        DONE: ;
    }

    /// <summary>
    /// Получение списка точек останова.
    /// </summary>
    public void ListBreakpoints()
    {
        var breakpoints = Breakpoints.Values
            .OrderBy (it => it.Statement.Line)
            .ToArray();

        foreach (var breakpoint in breakpoints)
        {
            var statement = breakpoint.Statement;
            var sourceLine = _sourceLines.SafeAt(statement.Line - 1);
            Console.WriteLine ($"{statement}: {sourceLine}");
        }

        if (breakpoints.Length == 0)
        {
            Console.WriteLine ("(no breakpoints)");
        }
    }

    #endregion

    #region IBarsikDebugger members

    /// <inheritdoc cref="IBarsikDebugger.Evaluate"/>
    public void Evaluate
        (
            string? expression
        )
    {
        if (string.IsNullOrWhiteSpace (expression))
        {
            return;
        }

        try
        {
            var atom = _interpreter.EvaluateAtom (expression);
            Console.WriteLine (atom);
            var value = atom.Compute (_interpreter.Context);
            KotikUtility.PrintObject (Console.Out, value);
            Console.WriteLine();
        }
        catch (Exception exception)
        {
            Console.WriteLine ($"ERROR: {exception.Message}");
        }
    }

    /// <inheritdoc cref="IBarsikDebugger.ExecuteFile"/>
    public ExecutionResult ExecuteFile
        (
            string filename
        )
    {
        Sure.FileExists(filename);

        Load(filename);
        Run();

        return new ExecutionResult();
    }

    /// <inheritdoc cref="Load"/>
    public void Load
        (
            string? fileName
        )
    {
        if (string.IsNullOrWhiteSpace (fileName))
        {
            return;
        }

        Sure.FileExists (fileName);

        var sourceCode = File.ReadAllText (fileName);
        _sourceLines = sourceCode.SplitLines();
        _program = _interpreter.Settings.Grammar.ParseProgram (sourceCode, _interpreter.Settings.Tokenizer);
    }

    /// <inheritdoc cref="IBarsikDebugger.Next"/>
    public void Next()
    {
        Console.WriteLine ("Next");
    }

    /// <inheritdoc cref="IBarsikDebugger.PreTrace"/>
    public void PreTrace
        (
            Context context,
            StatementBase statement
        )
    {
        var sourceLine = _sourceLines.SafeAt (statement.Line - 1);
        Console.WriteLine ($"{statement}: {sourceLine}");
    }

    /// <inheritdoc cref="Print"/>
    public void Print
        (
            string? what
        )
    {
        Console.WriteLine ($"Print {what}");
    }

    /// <inheritdoc cref="IBarsikDebugger.Raise"/>
    public void Raise
        (
            Context context,
            StatementBase? statement
        )
    {
        _currentStatement = statement;
        context.Output.WriteLine ("Raise");
    }

    /// <inheritdoc cref="IBarsikDebugger.Run"/>
    public void Run()
    {
        if (_program is null)
        {
            Console.WriteLine ("No program");
            return;
        }

        if (_scriptThread is null)
        {
            _scriptThread = new Thread (RunProgram)
            {
                // IsBackground = true,
                Name = "ScriptExecution"
            };
            _scriptThread.Start();
        }
    }

    /// <inheritdoc cref="IBarsikDebugger.SetBreakPoint"/>
    public void SetBreakPoint
        (
            int line
        )
    {
        Console.WriteLine ($"Set breakpoint to {line}");
    }

    /// <inheritdoc cref="IBarsikDebugger.Step"/>
    public void Step()
    {
        Console.WriteLine ("Step");
    }

    /// <inheritdoc cref="IBarsikDebugger.Trace"/>
    public void Trace
        (
            Context context,
            StatementBase statement
        )
    {
        var sourceLine = _sourceLines.SafeAt (statement.Line);
        context.Output.WriteLine ($"{statement}: {sourceLine}");
    }

    #endregion
}
