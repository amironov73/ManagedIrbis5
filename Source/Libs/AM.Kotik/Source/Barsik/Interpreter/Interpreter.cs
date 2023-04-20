// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Interpreter.cs -- интерпретатор Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using AM.IO;
using AM.Kotik.Barsik.Ast;
using AM.Kotik.Barsik.Diagnostics;
using AM.Kotik.Highlighting;
using AM.Kotik.Tokenizers;
using AM.Kotik.Types;
using AM.Results;
using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Интерпретатор Барсика.
/// </summary>
[PublicAPI]
public sealed class Interpreter
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Версия API.
    /// </summary>
    public static string AssemblyVersion = ThisAssembly.AssemblyVersion;

    /// <summary>
    /// Версия сборки.
    /// </summary>
    public static string FileVersion = ThisAssembly.AssemblyFileVersion;

    /// <summary>
    /// Корневой контекст исполнения программы.
    /// </summary>
    public Context Context { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Interpreter
        (
            TextReader? input = null,
            TextWriter? output = null,
            TextWriter? error = null,
            InterpreterSettings? settings = null
        )
    {
        var path = Environment.GetEnvironmentVariable ("BARSIK_PATH") ?? string.Empty;
        Context = new ()
        {
            Commmon =
            {
                Input = input ?? Console.In,
                Output = output ?? Console.Out,
                Error = error ?? Console.Error,
                Settings =  settings ?? InterpreterSettings.CreateDefault(),
                Resolver = new CachingResolver()
            }
        };

        Context.Commmon.Settings.Paths.AddRange (path.Split
            (
                Path.PathSeparator,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            ));

        // устанавливаем значения стандартных переменных
        Context.SetDefine ("__NAME__", string.Empty);
        Context.SetDefine ("__DIR__", string.Empty);
        Context.SetDefine ("__FILE__", string.Empty);
        Context.SetDefine ("__DOTNET__", Environment.Version);
        Context.SetDefine ("__ROOT__", AppContext.BaseDirectory);
        Context.SetDefine ("__VER__", Assembly.GetExecutingAssembly().GetName().Version);
    }

    #endregion

    #region Private members

    private bool _settinsWasApplied;

    private static void HighlightToConsole
        (
            string sourceCode,
            Tokenizer tokenizer
        )
    {
        var highlighter = new ScriptHighlighter<ConsoleColor>
        {
            Tokenizer = tokenizer,
            Colors = GetConsoleColors(),
            MainColor = ConsoleColor.Gray
        };
        var spans = highlighter.Highlight (sourceCode);
        var saveColor = Console.ForegroundColor;
        foreach (var textSpan in spans)
        {
            Console.ForegroundColor = textSpan.Highlight;
            Console.Write (textSpan.Fragment);
        }

        Console.ForegroundColor = saveColor;
    }

    private static void HighlightToHtml
        (
            string sourceCode,
            TextWriter output,
            Tokenizer tokenizer
        )
    {
        var highlighter = new ScriptHighlighter<string>
        {
            Tokenizer = tokenizer,
            Colors = GetHtmlColors(),
            MainColor = "#D3D3D3"
        };
        var spans = highlighter.Highlight (sourceCode);
        output.WriteLine ("<pre style=\"background: black;\">");
        foreach (var textSpan in spans)
        {
            output.Write ($"<span style=\"color:{textSpan.Highlight};\">");
            output.Write (HtmlText.Encode (textSpan.Fragment));
            // output.Write (textSpan.Fragment);
            output.Write ("</span>");
        }

        output.WriteLine ("</pre>");
    }

    /// <summary>
    /// Делаем контекст внимательным к выводу текста.
    /// </summary>
    internal void MakeAttentive()
    {
        if (Context.Commmon.Output is { } output and not AttentiveWriter)
        {
            Context.Commmon.Output = new AttentiveWriter (output);
        }
    }

    private OneOf<No, Yes<int>> _RunScriptFile
        (
            ConsoleDebugger? debugger,
            string scriptFile
        )
    {
        var executionResult = debugger is not null
            ? debugger.ExecuteFile (scriptFile)
            : ExecuteFile (scriptFile);
        if (executionResult.ExitRequested)
        {
            if (!string.IsNullOrEmpty (executionResult.Message))
            {
                Context.Commmon.Output?.WriteLine (executionResult.Message);
            }

            return new (new Yes<int>(executionResult.ExitCode));
        }

        return new (new No());
    }

    private int _Run
        (
            Action<Interpreter, Exception>? exceptionHandler
        )
    {
        var settings = Context.Commmon.Settings;

        ConsoleDebugger? debugger = null;
        if (settings.StartDebugger)
        {
            debugger = new ConsoleDebugger (this);
        }

        try
        {
            ApplySettings();
            foreach (var scriptFile in settings.ScriptFiles)
            {
                var result = _RunScriptFile (debugger, scriptFile);
                if (result.Is2)
                {
                    return result.As2().Value;
                }
            }

            if (settings.ReplMode)
            {
                if (debugger is not null)
                {
                    debugger.DoRepl();
                }
                else
                {
                    DoRepl();
                }
            }
        }
        catch (Exception exception)
        {
            if (Debugger.IsAttached)
            {
                throw;
            }

            exceptionHandler?.Invoke (this, exception);
            return 1;
        }

        return 0;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Применение настроек (которые не были применены ранее).
    /// Рекомендуется вызывать перед началом работы со скриптами.
    /// </summary>
    public void ApplySettings()
    {
        var settings = Context.Commmon.Settings;
        foreach (var assembly in settings.LoadAssemblies)
        {
            Context.LoadAssembly (assembly);
        }

        var paths = Context.Commmon.Settings.Paths;
        paths.Clear();
        paths.AddRange (settings.Paths);

        foreach (var ns in settings.UseNamespaces)
        {
            Context.Commmon.Resolver.Namespaces.Add (ns);
        }

        if (settings.DebugParser)
        {
            Context.Commmon.ParsingDebugOutput = Context.Commmon.Output;
        }

        settings.Grammar.Rebuild();
        _settinsWasApplied = true;
    }

    /// <summary>
    /// Создание интерпретатора согласно настройкам,
    /// заданным в командной строке.
    /// </summary>
    public static Interpreter CreateInterpreter
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        var settings = InterpreterSettings.CreateDefault();
        settings.FromCommandLine (args);

        if (!settings.DontLoadSettings)
        {
            var baseDirectory = AppContext.BaseDirectory;
            var fileName = Path.Combine (baseDirectory, "barsik");
            if (File.Exists (fileName))
            {
                var loaded = InterpreterSettings.FromFile
                    (
                        fileName,
                        withDefaults: false
                    );
                settings.Coalesce (loaded);
            }
        }

        return new Interpreter (settings: settings);
    }

    /// <summary>
    /// Запуск интерпретатора.
    /// </summary>
    public int Run
        (
            Action<Interpreter, Exception>? exceptionHandler = null
        )
    {
        var settings = Context.Commmon.Settings;
        if (settings.PrintVersion)
        {
            Context.Commmon.Output?.WriteLine (ThisAssembly.AssemblyFileVersion);
            return 0;
        }

        if (settings.BarsorMode)
        {
            return RunBarsorMode();
        }

        if (settings.Highlight is not null)
        {
            foreach (var file in settings.ScriptFiles)
            {
                HighlightFile (file, settings.Highlight);
            }

            return 0;
        }

        return _Run (exceptionHandler);
    }

    /// <summary>
    /// Запуск в режиме Barsor.
    /// </summary>
    public int RunBarsorMode()
    {
        ApplySettings();
        var barsor = new BarsorParser (this);
        foreach (var fileName in Context.Commmon.Settings.ScriptFiles)
        {
            var sourceCode = File.ReadAllText (fileName);
            var programNode = barsor.ParseTemplate (sourceCode);
            Execute (programNode);
        }

        return 0;
    }

    /// <summary>
    /// Создание и запуск интерпретатора.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <param name="configure">Опциональная конфигурация интерпретатора.</param>
    /// <param name="exceptionHandler">Опциональный обработчик исключений.</param>
    /// <returns>Код возврата.</returns>
    public static int CreateAndRunInterpreter
        (
            string[] args,
            Action<Interpreter>? configure = null,
            Action<Interpreter, Exception>? exceptionHandler = null
        )
    {
        Sure.NotNull (args);

        var interpreter = CreateInterpreter (args);
        configure?.Invoke (interpreter);

        return interpreter.Run (exceptionHandler);
    }

    /// <summary>
    /// Запуск REPL на указанном интерпретаторе.
    /// </summary>
    public ExecutionResult DoRepl()
    {
        if (Context.Commmon.Output is { } output)
        {
            output.WriteLine ($"Meow interpreter {FileVersion}");
            output.WriteLine ("Press ENTER twice to exit");
        }

        return new Repl (this).Loop();
    }

    /// <summary>
    /// Запуск скрипта на исполнение.
    /// </summary>
    public ExecutionResult Execute
        (
            string sourceCode,
            bool requireEnd = true
        )
    {
        Sure.NotNull (sourceCode);

        var settings = Context.Commmon.Settings;
        var program = settings.Grammar.ParseProgram
            (
                sourceCode,
                settings.Tokenizer,
                requireEnd,
                settings.DumpTokens,
                traceOutput: null,
                Context.Commmon.ParsingDebugOutput
            );
        if (settings.DumpAst && Context.Commmon.Output is { } output)
        {
            program.Dump (output);
            output.WriteLine (new string ('=', 60));
        }

        // отделяем отладочную печать парсеров от прочего вывода
        Context.Commmon.ParsingDebugOutput?.WriteLine (new string ('=', 60));

        var result = Context.Execute (program);

        return result;
    }

    /// <summary>
    /// Запуск скрипта на исполнение.
    /// </summary>
    public ExecutionResult Execute
        (
            ProgramNode program
        )
    {
        Sure.NotNull (program);

        if (!_settinsWasApplied)
        {
            ApplySettings();
        }

        return Context.Execute (program);
    }

    /// <summary>
    /// Выполнение файла <code>autoexec.barsik</code>,
    /// при условии, что он есть в директории приложения.
    /// </summary>
    public ExecutionResult? ExecuteAutoexec()
    {
        var executablePath = AppContext.BaseDirectory;

        var autoexec = Path.Combine (executablePath, "autoexec.meow");

        if (File.Exists (autoexec))
        {
            return ExecuteFile (autoexec);
        }

        return null;
    }

    /// <summary>
    /// Загрузка исходного кода из указанного файла
    /// с последующим исполнением.
    /// </summary>
    /// <param name="fileName">Имя файла скрипта.</param>
    /// <remarks>
    /// Скрипт-файл может начинаться с "#!", эта строка будет проигнорирована.
    /// Такой трюк позволяет сделать запускаемыми Barsuk-скрипты.
    /// </remarks>
    public ExecutionResult ExecuteFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        ExecutionResult result;
        var defines = Context.Commmon.Defines;
        try
        {
            if (!_settinsWasApplied)
            {
                ApplySettings();
            }

            var fullPath = Path.GetFullPath (fileName);
            defines["__NAME__"] = "__MAIN__";
            defines["__FILE__"] = fullPath;
            defines["__DIR__"] = Path.GetDirectoryName (fullPath);

            var sourceCode = File.ReadAllText (fileName);
            result = Execute (sourceCode);
        }
        catch (ReturnException exception)
        {
            result = new ExecutionResult
            {
                ExitRequested = true,
                ExitCode = KotikUtility.ToInt32 (exception.Value),
                Message = exception.Message
            };
        }
        catch (ExitException exception)
        {
            result = new ExecutionResult
            {
                ExitRequested = true,
                ExitCode = exception.ExitCode,
                Message = exception.Message
            };
        }
        finally
        {
            defines["__NAME__"] = string.Empty;
            defines["__FILE__"] = string.Empty;
            defines["__DIR__"] = string.Empty;
        }

        return result;
    }

    /// <summary>
    /// Получение цветовой схемы для подсвечивания исходного кода скриптов.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, ConsoleColor> GetConsoleColors() => new ()
    {
        [TokenKind.Char] = ConsoleColor.Green,
        [TokenKind.String] = ConsoleColor.Green,
        [TokenKind.RawString] = ConsoleColor.Green,
        [TokenKind.Format] = ConsoleColor.Green,
        [TokenKind.Comment] = ConsoleColor.DarkGreen,
        [TokenKind.Decimal] = ConsoleColor.Cyan,
        [TokenKind.Int32] = ConsoleColor.Cyan,
        [TokenKind.Int64] = ConsoleColor.Cyan,
        [TokenKind.UInt32] = ConsoleColor.Cyan,
        [TokenKind.UInt64] = ConsoleColor.Cyan,
        [TokenKind.Hex32] = ConsoleColor.Cyan,
        [TokenKind.Hex64] = ConsoleColor.Cyan,
        [TokenKind.BigInteger] = ConsoleColor.Cyan,
        [TokenKind.Single] = ConsoleColor.Cyan,
        [TokenKind.Double] = ConsoleColor.Cyan,
        [TokenKind.Identifier] = ConsoleColor.White,
        [TokenKind.ReservedWord] = ConsoleColor.Yellow,
        [TokenKind.External] = ConsoleColor.Magenta,
        [TokenKind.Directive] = ConsoleColor.Red,
    };

    /// <summary>
    /// Получение цветовой схемы для подсвечивания исходного кода скриптов.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, string> GetHtmlColors() => new ()
    {
        [TokenKind.Char] = "#00FF00",
        [TokenKind.String] = "#00FF00",
        [TokenKind.RawString] = "#00FF00",
        [TokenKind.Format] = "#00FF00",
        [TokenKind.Comment] = "#00C000",
        [TokenKind.Decimal] = "#00FFFF",
        [TokenKind.Int32] = "#00FFFF",
        [TokenKind.Int64] = "#00FFFF",
        [TokenKind.UInt32] = "#00FFFF",
        [TokenKind.UInt64] = "#00FFFF",
        [TokenKind.Hex32] = "#00FFFF",
        [TokenKind.Hex64] = "#00FFFF",
        [TokenKind.BigInteger] = "#00FFFF",
        [TokenKind.Single] = "#00FFFF",
        [TokenKind.Double] = "#00FFFF",
        [TokenKind.Identifier] = "#FFFFFF",
        [TokenKind.ReservedWord] = "#FFFF00",
        [TokenKind.External] = "#FF0000",
        [TokenKind.Directive] = "#FF0000",
    };

    /// <summary>
    /// Раскраска исходного кода из указанного файле.
    /// </summary>
    public static void HighlightFile
        (
            string fileName,
            string higlighterKind = "html",
            TextWriter? output = null
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNullNorEmpty (higlighterKind);

        var sourceCode = File.ReadAllText (fileName);
        HighlightSourceCode (sourceCode, higlighterKind, output);
    }

    /// <summary>
    /// Раскраска исходного кода.
    /// </summary>
    public static void HighlightSourceCode
        (
            string sourceCode,
            string higlighterKind = "html",
            TextWriter? output = null
        )
    {
        Sure.NotNull (sourceCode);
        Sure.NotNullNorEmpty (higlighterKind);
        output ??= Console.Out;

        var tokenizer = KotikUtility.CreateTokenizerForBarsik();
        CommentTokenizer.SwitchEat (tokenizer.Tokenizers, false);
        WhitespaceTokenizer.SwitchEat (tokenizer.Tokenizers, false);

        switch (higlighterKind)
        {
            case "html":
                HighlightToHtml (sourceCode, output, tokenizer);
                break;

            case "console":
                HighlightToConsole (sourceCode, tokenizer);
                break;
        }
    }


    /// <summary>
    /// Сброс состояния интерпретатора.
    /// </summary>
    public void Reset()
    {
        Context.Reset();
    }


    /// <summary>
    /// Подключение стандартной библиотеки.
    /// </summary>
    /// <returns><c>this</c>.</returns>
    public Interpreter WithStdLib()
    {
        foreach (var module in Context.Commmon.Modules)
        {
            if (module is StdLib)
            {
                return this;
            }
        }

        Context.AttachModule (new StdLib());

        return this;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // пустое тело метода
        // интерфейс IDisposable "на будущее"
    }

    #endregion
}
