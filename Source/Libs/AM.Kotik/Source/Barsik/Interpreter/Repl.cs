// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Local

/* Repl.cs -- цикл "ввод - вычисление - вывод"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.IO;

using RadLine;

using Spectre.Console;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/*
    https://ru.wikipedia.org/wiki/REPL

    REPL (от англ. read-eval-print loop — «цикл „чтение — вычисление — вывод“»)
    — форма организации простой интерактивной среды программирования в рамках
    средств интерфейса командной строки. Чаще всего этой аббревиатурой
    характеризуется интерактивная среда языка программирования Лисп, однако
    такая форма характерна и для интерактивных сред языков Erlang, Groovy,
    Haskell, Java, JavaScript, Perl, PHP, Python, Ruby, Scala, Smalltalk,
    Swift и других.

    В такой среде пользователь может вводить выражения, которые среда тут же будет
    вычислять, а результат вычисления отображать пользователю. Названия элементов
    цикла связаны с соответствующими примитивами Лиспа:

    * функция read читает одно выражение и преобразует его в соответствующую структуру данных в памяти;
    * функция eval принимает одну такую структуру данных и вычисляет соответствующее ей выражение;
    * функция print принимает результат вычисления выражения и печатает его пользователю.

    Чтобы реализовать REPL-среду для некоторого языка, достаточно реализовать три функции:
    чтения, вычисления и вывода, и объединить их в бесконечный цикл. REPL-среда очень
    удобна при изучении нового языка, так как предоставляет пользователю быструю обратную связь.

 */

/// <summary>
/// Цикл "чтение - вычисление - вывод".
/// </summary>
public sealed class Repl
{
    #region Properties

    /// <summary>
    /// Интерпретатор.
    /// </summary>
    public Interpreter Interpreter { get; }

    /// <summary>
    /// Выводить ли результат вычисления выражения?
    /// </summary>
    public bool Echo { get; set; }

    /// <summary>
    /// Контекст вычислений.
    /// </summary>
    public Context Context => Interpreter.Context;

    /// <summary>
    /// Абстракция стандартного вывода.
    /// </summary>
    public AttentiveWriter? Output => (AttentiveWriter?) Context.Commmon.Output;

    /// <summary>
    /// Абстракция потока ошибок.
    /// </summary>
    public TextWriter? Error => Context.Commmon.Error;

    /// <summary>
    /// Абстракция входного потока.
    /// </summary>
    public TextReader Input { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Repl
        (
            TextReader? input = null,
            TextWriter? output = null,
            TextWriter? error = null
        )
    {
        Input = input ?? Console.In;
        output = new AttentiveWriter (output ?? Console.Out);
        error ??= Console.Error;
        Interpreter = new Interpreter (Input, output, error);
        Interpreter.Context.UserData["repl"] = this;
        Echo = true;
        _input = new ReplInput (Input, output, Interpreter);
        if (LineEditor.IsSupported (AnsiConsole.Console))
        {
            _radInput = new RadInput();
        }
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Repl
        (
            Interpreter interpreter,
            TextReader? input = null
        )
    {
        interpreter.MakeAttentive();
        Interpreter = interpreter;
        Interpreter.Context.UserData["repl"] = this;
        Input = input ?? interpreter.Context.Commmon.Input ?? Console.In;
        Echo = true;
        _input = new ReplInput
            (
                Input,
                interpreter.Context.Commmon.Output ?? Console.Out,
                interpreter
            );
        if (LineEditor.IsSupported (AnsiConsole.Console))
        {
            _radInput = new RadInput();
        }
    }

    #endregion

    #region Private members

    private readonly ReplInput _input;
    private readonly RadInput? _radInput;

    #endregion

    #region Public methods

    /// <summary>
    /// Вычисление значения выражения.
    /// </summary>
    public bool Evaluate
        (
            string sourceCode,
            out dynamic? result
        )
    {
        try
        {
            var node = Interpreter.Context.EvaluateAtom (sourceCode);
            if (node != null!)
            {
                result = node.Compute (Context);
                return true;
            }
        }
        catch (ExitException)
        {
            throw;
        }
        catch
        {
            result = null;
            return false;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Выполнение указанного кода.
    /// </summary>
    public void Execute
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        try
        {
            Interpreter.Execute (sourceCode);
        }
        catch (Exception exception)
        {
            Error?.WriteLine ($"ERROR: {exception.Message}");
        }
    }

    /// <summary>
    /// Чтение одной строки пользовательского ввода.
    /// </summary>
    public string? ReadLine()
    {
        if (_radInput is not null)
        {
            return _radInput.ReadLine();
        }

        return _input.ReadLine();
    }

    /// <summary>
    /// Прокрутка цикла. Выход -- две пустые строки подряд.
    /// </summary>
    public ExecutionResult Loop()
    {
        var emptyLineCounter = 0;

        while (true)
        {
            var line = ReadLine();
            if (string.IsNullOrWhiteSpace (line))
            {
                if (++emptyLineCounter == 2)
                {
                    break;
                }
            }
            else
            {
                try
                {
                    emptyLineCounter = 0;
                    var suppressEcho = false;
                    if (line.StartsWith ('@'))
                    {
                        suppressEcho = true;
                        line = line.Substring (1);
                    }
                    if (Evaluate (line, out var result))
                    {
                        Context.Variables["$$"] = result;
                        if (Echo != suppressEcho)
                        {
                            KotikUtility.PrintObject (Output, result);
                            Output?.WriteLine();
                        }
                    }
                    else
                    {
                        Output?.ResetCounter();
                        Execute (line);
                        if (Output?.Counter != 0)
                        {
                            Output?.WriteLine();
                        }
                    }
                }
                catch (ExitException exception)
                {
                    return new ExecutionResult
                    {
                        ExitRequested = true,
                        ExitCode = exception.ExitCode,
                        Message = exception.Message
                    };
                }
            }
        }

        return new ExecutionResult();
    }

    #endregion
}
