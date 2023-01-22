// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Interpreter.cs -- интерпретатор Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
    /// Поток для отладочного вывода при парсинге скрипта.
    /// </summary>
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
    /// Вычисление значения переменной.
    /// </summary>
    public AtomNode Evaluate
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        var node = Grammar.ParseExpression (sourceCode, ParsingDebugOutput);

        return node;
    }

    /// <summary>
    /// Запуск скрипта на исполнение.
    /// </summary>
    public ExecutionResult Execute
        (
            string sourceCode,
            bool dumpAst = false
        )
    {
        Sure.NotNull (sourceCode);

        var program = Grammar.ParseProgram (sourceCode, ParsingDebugOutput);
        if (dumpAst)
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

        return result;
    }

    /// <summary>
    /// Выполнение файла <code>autoexec.barsik</code>,
    /// при условии, что он есть в директории приложения.
    /// </summary>
    public ExecutionResult? ExecuteAutoexec()
    {
        var executablePath = AppContext.BaseDirectory;

        // ReSharper disable StringLiteralTypo
        var autoexec = Path.Combine (executablePath, "autoexec.barsik");
        // ReSharper restore StringLiteralTypo

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
    /// <param name="dumpAst">Вывести дамп синтаксического дерева
    /// для контроля разбора.</param>
    /// <remarks>
    /// Скрипт-файл может начинаться с "#!", эта строка будет проигнорирована.
    /// Такой трюк позволяет сделать запускаемыми Barsuk-скрипты.
    /// </remarks>
    public ExecutionResult ExecuteFile
        (
            string fileName,
            bool dumpAst = false
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
            // удаляем shebanh
            sourceCode = KotikUtility.RemoveShebang (sourceCode);
            result = Execute (sourceCode, dumpAst);
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
    /// Разбор текста программы.
    /// </summary>
    public static ProgramNode ParseProgram
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        return Grammar.ParseProgram (sourceCode);
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
