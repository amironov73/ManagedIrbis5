// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxExecutive.cs -- исполняющая подсистема MX64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using AM;
using AM.Collections;
using AM.ConsoleIO;
using AM.Json;
using AM.Scripting.Barsik;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mx.Commands;
using ManagedIrbis.Mx.Handlers;
using ManagedIrbis.Mx.Infrastructrure;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Scripting.Barsik;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Mx;

/// <summary>
/// Исполняющая подсистема MX64.
/// </summary>
public sealed class MxExecutive
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Корневой контекст для PFT-форматтера.
    /// </summary>
    public PftContext Context { get; internal set; }

    /// <summary>
    /// Консольный ввод-вывод.
    /// </summary>
    public IMxConsole MxConsole { get; set; }

    /// <summary>
    /// Словарь алиасов.
    /// </summary>
    public IDictionary<string, string> Aliases { get; }

    /// <summary>
    /// Палитра цветов.
    /// </summary>
    public MxPalette Palette { get; set; }

    /// <summary>
    /// Синхронное подключение к серверу ИРБИС64.
    /// </summary>
    public ISyncProvider Provider { get; internal set; }

    /// <summary>
    /// Словарь известных команд.
    /// </summary>
    public NonNullCollection<MxCommand> Commands { get; }

    /// <summary>
    /// Формат для библиографического описания.
    /// </summary>
    public string? DescriptionFormat { get; set; }

    /// <summary>
    /// Order expression.
    /// </summary>
    public string? OrderFormat { get; set; }

    /// <summary>
    /// Search limit.
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// Records.
    /// </summary>
    public NonNullCollection<MxRecord> Records { get; private set; }

    /// <summary>
    /// Stop repl.
    /// </summary>
    public bool StopFlag { get; set; }

    /// <summary>
    /// Verbosity level.
    /// </summary>
    public int VerbosityLevel { get; set; }

    /// <summary>
    /// Search history.
    /// </summary>
    public Stack<string?> History { get; }

    /// <summary>
    /// Stack of databases.
    /// </summary>
    public Stack<string> Databases { get; }

    /// <summary>
    /// List of modules.
    /// </summary>
    public List<MxModule> Modules { get; }

    /// <summary>
    /// List of handlers.
    /// </summary>
    public List<MxHandler> Handlers { get; }

    /// <summary>
    /// Интерпретатор Барсика.
    /// </summary>
    public Interpreter Interpreter { get; }

    /// <summary>
    /// Get version of the executive.
    /// </summary>
    public static Version Version => ClientVersion.Version;

    /// <summary>
    /// Приглашение.
    /// </summary>
    public string? Prompt { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MxExecutive()
    {
        _output = new StringBuilder();

        VerbosityLevel = 3;
        DescriptionFormat = "@brief";

        Prompt = "$ ";
        Aliases = new CaseInsensitiveDictionary<string>();
        MxConsole = new MxConsole();
        Palette = MxPalette.GetDefaultPalette();
        Provider = new NullProvider();
        Context = new PftContext (null);
        Context.SetProvider (Provider);
        Commands = new ();
        Records = new ();
        History = new ();
        Databases = new Stack<string>();
        Modules = new List<MxModule>();
        Handlers = new List<MxHandler>();

        // инетрепретатор Барсика
        Interpreter = new Interpreter().WithStdLib();
        Interpreter.Context.AttachModule (new IrbisLib());
        Interpreter.Context.Defines.Add ("MX", this);
        // TODO добавить MxLib

        _CreateStandardCommands();
        _InitializeCommands();
        _CreateStandardHandlers();
    }

    #endregion

    #region Private members

    private readonly StringBuilder _output;

    private void _CancelKeyPress
        (
            object? sender,
            ConsoleCancelEventArgs e
        )
    {
        StopFlag = true;
    }

    private void _CreateStandardCommands()
    {
        Commands.AddRange
            (
                new MxCommand[]
                {
                    new AliasCommand(),
                    new BangCommand(),
                    new BarsikCommand(),
                    new ClsCommand(),
                    new ConnectCommand(),
                    new CsCommand(),
                    new CsFileCommand(),
                    new DbCommand(),
                    new DeleteCommand(),
                    new DirCommand(),
                    new DisconnectCommand(),
                    new ExitCommand(),
                    new ExportCommand(),
                    new FormatCommand(),
                    new HelpCommand(),
                    new HistoryCommand(),
                    new InfoCommand(),
                    new LimitCommand(),
                    new ListCommand(),
                    new ListDbCommand(),
                    new ListUsersCommand(),
                    new LockCommand(),
                    new ModuleCommand(),
                    new NopCommand(),
                    new PftCommand(),
                    new PingCommand(),
                    new PocketCommand(),
                    new PrintCommand(),
                    new ReconnectCommand(),
                    new RefineCommand(),
                    new RestartCommand(),
                    new SearchCommand(),
                    new SortCommand(),
                    new StatCommand(),
                    new StoreCommand(),
                    new TypeCommand(),
                    new VerCommand()
                }
            );
    }

    private void _CreateStandardHandlers()
    {
        Handlers.Add (new FileWriteHandler (this));
        Handlers.Add (new FileWriteHandler (this));
    }

    private void _DisposeCommands()
    {
        foreach (var command in Commands)
        {
            command.Dispose();
        }
    }

    private void _DisposeHandlers()
    {
        foreach (var handler in Handlers)
        {
            handler.Dispose();
        }
    }

    private void _DisposeModules()
    {
        foreach (var module in Modules)
        {
            module.Dispose();
        }
    }

    private bool _ExecuteLine
        (
            TextNavigator navigator
        )
    {
        navigator.SkipWhitespace();

        var line = navigator.ReadLine().ToString();
        if (string.IsNullOrEmpty (line))
        {
            return true;
        }

        if (line.StartsWith ("#"))
        {
            // Comment, ignore it
            return true;
        }

        var detectedHandlers = new List<MxHandler>();

        var prefixes = Handlers.Select (h => h.Prefix).ToArray();
        if (prefixes.Length != 0)
        {
            while (true)
            {
                var index = Utility.LastIndexOfAny (line, prefixes);
                if (index < 0)
                {
                    break;
                }

                var handlerCommand = line.Substring (index);
                foreach (var handler in Handlers)
                {
                    if (handlerCommand.StartsWith (handler.Prefix))
                    {
                        handlerCommand = handlerCommand
                            .Substring (handler.Prefix.Length).Trim();
                        handler.Parse (this, handlerCommand);
                        detectedHandlers.Add (handler);
                        break;
                    }
                }

                line = line.Substring (0, index);
            }
        }

        line = line.Trim();
        detectedHandlers.Reverse();

        var parts = line.Split (CommonSeparators.SpaceOrTab, 2);
        var commandName = parts[0];
        string? commandArgument = null;
        if (parts.Length != 1)
        {
            commandArgument = parts[1];
        }

        MxCommand? command = null;

        if (!string.IsNullOrEmpty (commandName))
        {
            command = _FindCommand (commandName);
            if (ReferenceEquals (command, null))
            {
                WriteError (string.Format
                    (
                        "Unknown command: '{0}'", commandName
                    ));
                return false;
            }
        }

        MxArgument[] arguments = commandArgument is null
            ? Array.Empty<MxArgument>()
            : new MxArgument[]
            {
                new MxArgument()
                {
                    Text = commandArgument
                }
            };
        var result = false;

        try
        {
            foreach (var handler in detectedHandlers)
            {
                handler.BeginOutput (this);
            }

            if (!ReferenceEquals (command, null))
            {
                result = command.Execute
                    (
                        this,
                        arguments
                    );
            }

            foreach (var handler in detectedHandlers)
            {
                handler.EndOutput (this);
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (MxExecutive) + "::" + nameof (_ExecuteLine)
                );

            WriteError ($"Exception: {exception}");

            return result;
        }

        return result;
    }

    private MxCommand? _FindCommand
        (
            string name
        )
    {
        return Commands.FirstOrDefault
            (
                cmd => cmd.Name.SameString (name)
            );
    }

    private void _InitializeCommands()
    {
        foreach (var command in Commands)
        {
            command.Initialize (this);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Print banner.
    /// </summary>
    public void Banner()
    {
        WriteMessage ($"mx64 version {Version}");
        WriteLine (string.Empty);
    }

    /// <summary>
    /// Clear the output.
    /// </summary>
    public void ClearOutput() => _output.Clear();

    /// <summary>
    /// Execute initialization script.
    /// </summary>
    public bool ExecuteInitScript()
    {
        // TODO implement

        return true;
    }

    /// <summary>
    /// Execute script file.
    /// </summary>
    public bool ExecuteFile
        (
            string fileName
        )
    {
        var text = File.ReadAllText (fileName, IrbisEncoding.Utf8);
        var result = ExecuteLine (text);

        return result;
    }

    /// <summary>
    /// Execute line of the script.
    /// </summary>
    public bool ExecuteLine
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return true;
        }

        var navigator = new TextNavigator (text);
        while (!navigator.IsEOF)
        {
            if (!_ExecuteLine (navigator))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Get collected output.
    /// </summary>
    public string GetOutput() => _output.ToString();

    /// <summary>
    /// Форматирование на сервере.
    /// </summary>
    public string FormatRemote
        (
            string source
        )
    {
        var record = new Record();
        var result = Provider.FormatRecord (source, record)
                     ?? string.Empty;

        return result;
    }

    /// <summary>
    /// Get specified command.
    /// </summary>
    public T GetCommand<T>()
        where T : MxCommand
    {
        var result = Commands.OfType<T>().FirstOrDefault();
        if (result is null)
        {
            throw new IrbisException ($"Command {typeof (T).Name} not found");
        }

        return result;
    }

    /// <summary>
    /// Get specified command.
    /// </summary>
    public MxCommand? GetCommand
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        return Commands.FirstOrDefault (c => c.Name.SameString (name));
    }

    /// <summary>
    /// Load the module.
    /// </summary>
    public void LoadModule
        (
            string modulePath
        )
    {
        var extension = Path.GetExtension (modulePath);
        if (string.IsNullOrEmpty (extension))
        {
            modulePath = modulePath + ".mxmodule";
        }

        if (!File.Exists (modulePath))
        {
            modulePath = Path.Combine ("modules", modulePath);
        }

        var definition = JsonUtility.ReadObjectFromFile<ModuleDefinition> (modulePath);

        // Must not load twice
        var found = Modules.FirstOrDefault
            (
                m => m.GetType().FullName == definition.ClassName
            );
        if (!ReferenceEquals (found, null))
        {
            return;
        }

        var assemblyPath = definition.AssemblyPath;
        var className = definition.ClassName;
        if (!string.IsNullOrEmpty (assemblyPath)
            && !string.IsNullOrEmpty (className))
        {
            var assembly = Assembly.LoadFile (assemblyPath);
            var type = assembly.GetType (className);
            if (type is not null)
            {
                var module = (MxModule?)Activator.CreateInstance (type);
                if (module is not null)
                {
                    module.Initialize (this);
                    Modules.Add (module);
                }
            }
        }
    }

    /// <summary>
    /// Разбор аргументов командной строки.
    /// </summary>
    public void ParseCommandLine
        (
            string[] args
        )

    {
        // TODO implement
    }

    /// <summary>
    /// Read one line.
    /// </summary>
    public string? ReadLine()
    {
        var saveColor = MxConsole.ForegroundColor;
        try
        {
            MxConsole.ForegroundColor = Palette.Command;
            return MxConsole.ReadLine();
        }
        finally
        {
            MxConsole.ForegroundColor = saveColor;
        }
    }

    /// <summary>
    /// REPL
    /// </summary>
    public void Repl()
    {
        Console.CancelKeyPress += _CancelKeyPress;
        Console.Title = $"mx64 v{Version}";

        while (!StopFlag)
        {
            Write (Prompt);
            var line = ReadLine();
            ExecuteLine (line);
            WriteLine (string.Empty);
        }

        if (Provider.IsConnected)
        {
            WriteMessage ("Disconnecting");
            Provider.Disconnect();
        }
    }

    /// <summary>
    /// Вывод в консоль сообщения об ошибке.
    /// </summary>
    public void WriteError
        (
            string text
        )
    {
        WriteLine (Palette.Error, text);
    }

    /// <summary>
    /// Write to console.
    /// </summary>
    public void WriteLine
        (
            string text
        )
    {
        MxConsole.Write (text);
        MxConsole.Write (Environment.NewLine);
    }

    /// <summary>
    /// Вывод простого текста.
    /// </summary>
    public void Write
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            MxConsole.Write (text);
        }
    }

    /// <summary>
    /// Write to console.
    /// </summary>
    public void WriteLine
        (
            ConsoleColor color,
            string text
        )
    {
        var saveColor = MxConsole.ForegroundColor;
        try
        {
            ConsoleInput.ForegroundColor = color;
            MxConsole.Write (text);
            MxConsole.Write (Environment.NewLine);
        }
        finally
        {
            MxConsole.ForegroundColor = saveColor;
        }
    }

    /// <summary>
    /// Write message
    /// </summary>
    public void WriteMessage
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return;
        }

        var saveColor = MxConsole.ForegroundColor;
        try
        {
            MxConsole.ForegroundColor = Palette.Message;
            WriteLine (text);
        }
        finally
        {
            MxConsole.ForegroundColor = saveColor;
        }
    }

    /// <summary>
    /// Write the text to the output.
    /// </summary>
    public void WriteOutput
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            _output.AppendLine (text);
            WriteLine (Palette.Foreground, text);
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _DisposeModules();
        _DisposeHandlers();
        _DisposeCommands();

        Provider.Dispose();
    }

    #endregion
}
