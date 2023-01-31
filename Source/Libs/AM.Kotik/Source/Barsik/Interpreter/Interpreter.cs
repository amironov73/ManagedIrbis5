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
using System.Linq;
using System.Reflection;

using AM.Kotik.Barsik.Ast;
using AM.Kotik.Barsik.Diagnostics;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Интерпретатор Барсика.
/// </summary>
public sealed class Interpreter
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    public Dictionary<string, object?> UserData { get; }

    /// <summary>
    /// Отладчик скрипта.
    /// </summary>
    public IBarsikDebugger? ScriptDebugger { get; set; }

    /// <summary>
    /// Поток для отладочного вывода при парсинге скрипта.
    /// </summary>
    // TODO перенести в Settings
    public TextWriter? ParsingDebugOutput { get; set; }

    /// <summary>
    /// Версия API.
    /// </summary>
    public static string AssemblyVersion = ThisAssembly.AssemblyVersion;

    /// <summary>
    /// Версия сборки.
    /// </summary>
    public static string FileVersion = ThisAssembly.AssemblyFileVersion;

    /// <summary>
    /// Контекст исполнения программы.
    /// </summary>
    public Context Context { get; }

    /// <summary>
    /// Разрешение использовать оператор <c>new</c>.
    /// </summary>
    // TODO перенести в Settings
    public bool AllowNewOperator { get; set; }

    /// <summary>
    /// Загруженные модули.
    /// </summary>
    public List<IBarsikModule> Modules { get; }

    /// <summary>
    /// Загруженные сборки (чтобы не писать assembly-qualified type name).
    /// </summary>
    public Dictionary<string, Assembly> Assemblies { get; }

    /// <summary>
    /// Обработчик внешнего кода.
    /// </summary>
    public ExternalCodeHandler? ExternalCodeHandler { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные, свяазанные с данным интерпретатором.
    /// </summary>
    public BarsikDictionary Auxiliary { get; }

    /// <summary>
    /// Пути для поиска скриптов, модулей  и т. д.
    /// Инициализируется значением переменной окружения "BARSIK_PATH".
    /// </summary>
    // TODO перенести в Settings
    public List<string> Pathes { get; }

    /// <summary>
    /// Настройки интерпретатора.
    /// Применяются перед началом разбора и исполнения скрипта.
    /// </summary>
    public InterpreterSettings Settings { get; }

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
        input ??= Console.In;
        output ??= Console.Out;
        error ??= Console.Error;
        Settings =  settings ?? InterpreterSettings.CreateDefault();
        AllowNewOperator = true;
        Modules = new ();
        Assemblies = new ();
        Auxiliary = new ();
        UserData = new ();

        Context = new (input, output, error)
        {
            Interpreter = this
        };

        var path = Environment.GetEnvironmentVariable ("BARSIK_PATH") ?? string.Empty;
        Pathes = new (path.Split
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

    #region Public methods

    /// <summary>
    /// Применение настроек (которые не были применены ранее).
    /// Рекомендуется вызывать перед началом работы со скриптами.
    /// </summary>
    public void ApplySettings()
    {
        foreach (var assembly in Settings.LoadAssemblies)
        {
            Context.LoadAssembly (assembly);
        }

        foreach (var ns in Settings.UseNamespaces)
        {
            Context.Namespaces.Add (ns, null);
        }

        if (Settings.DebugParser)
        {
            ParsingDebugOutput = Context.Output;
        }

        Settings.Grammar.Rebuild();
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

        var settings = InterpreterSettings.FromCommandLine (args);
        return new Interpreter (settings: settings);
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
        ConsoleDebugger? debugger = null;
        if (interpreter.Settings.StartDebugger)
        {
            debugger = new ConsoleDebugger (interpreter);
        }

        try
        {
            interpreter.ApplySettings();
            foreach (var scriptFile in interpreter.Settings.ScriptFiles)
            {
                var executionResult = debugger is not null
                    ? debugger.ExecuteFile (scriptFile)
                    : interpreter.ExecuteFile (scriptFile);
                if (executionResult.ExitRequested)
                {
                    if (!string.IsNullOrEmpty (executionResult.Message))
                    {
                        interpreter.Context.Output.WriteLine (executionResult.Message);
                    }

                    return executionResult.ExitCode;
                }
            }

            if (interpreter.Settings.ReplMode)
            {
                if (debugger is not null)
                {
                    debugger.DoRepl();
                }
                else
                {
                    interpreter.DoRepl();
                }
            }
        }
        catch (Exception exception)
        {
            if (Debugger.IsAttached)
            {
                throw;
            }

            exceptionHandler?.Invoke (interpreter, exception);
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Запуск REPL на указанном интерпретаторе.
    /// </summary>
    public ExecutionResult DoRepl()
    {
        Context.Output.WriteLine ($"Meow interpreter {FileVersion}");
        Context.Output.WriteLine ("Press ENTER twice to exit");

        return new Repl (this).Loop();
    }

    /// <summary>
    /// Вычисление значения переменной.
    /// </summary>
    public AtomNode EvaluateAtom
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        var node = Settings.Grammar.ParseExpression
            (
                sourceCode,
                Settings.Tokenizer,
                ParsingDebugOutput
            );
        if (Settings.DumpAst)
        {
            Context.Output.WriteLine (new string ('=', 60));
            node.DumpHierarchyItem (null, 0, Context.Output);
            Context.Output.WriteLine (new string ('=', 60));
        }

        return node;
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

        var program = Settings.Grammar.ParseProgram
            (
                sourceCode,
                Settings.Tokenizer,
                requireEnd,
                ParsingDebugOutput
            );
        if (Settings.DumpAst)
        {
            program.Dump (Context.Output);
            Context.Output.WriteLine (new string ('=', 60));
        }

        // отделяем отладочную печать парсеров от прочего вывода
        ParsingDebugOutput?.WriteLine (new string ('=', 60));

        var result = Execute (program, Context);

        return result;
    }

    /// <summary>
    /// Запуск скрипта на исполнение.
    /// </summary>
    public ExecutionResult Execute
        (
            ProgramNode program,
            Context? context = null
        )
    {
        Sure.NotNull (program);

        context ??= Context;
        var haveDefinitions = false;
        foreach (var statement in program.Statements)
        {
            if (statement is FunctionDefinitionNode node)
            {
                haveDefinitions = true;
                var name = node.Name;
                if (Builtins.IsBuiltinFunction (name))
                {
                    throw new BarsikException ($"{name} used by builtin function");
                }

                var definition = new FunctionDefinition
                    (
                        name,
                        node._argumentNames,
                        node._body
                    );
                var descriptor = new FunctionDescriptor
                    (
                        name,
                        definition.CreateCallPoint()
                    );
                context.Functions[name] = descriptor;
            }
        }

        if (haveDefinitions)
        {
            program.Statements = program.Statements
                .Where (stmt => stmt is not PseudoNode)
                .ToList();
        }

        var result = new ExecutionResult();
        try
        {
            program.Execute (context);
        }
        catch (ReturnException exception)
        {
            result.ExitRequested = true;
            result.ExitCode = KotikUtility.ToInt32 (exception.Value);
            result.Message = exception.Message;
        }
        catch (ExitException exception)
        {
            result.ExitRequested = true;
            result.ExitCode = exception.ExitCode;
            result.Message = exception.Message;
        }

        Settings.VariableDumper?.DumpContext (Context);

        return result;
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
        try
        {
            var fullPath = Path.GetFullPath (fileName);
            Context.Defines["__NAME__"] = "__MAIN__";
            Context.Defines["__FILE__"] = fullPath;
            Context.Defines["__DIR__"] = Path.GetDirectoryName (fullPath);

            var sourceCode = File.ReadAllText (fileName);
            // удаляем shebang
            sourceCode = KotikUtility.RemoveShebang (sourceCode);
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
            Context.Defines["__NAME__"] = string.Empty;
            Context.Defines["__FILE__"] = string.Empty;
            Context.Defines["__DIR__"] = string.Empty;
        }

        return result;
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
